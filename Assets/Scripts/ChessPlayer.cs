using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using System.IO;
using Photon.Realtime;

public class ChessPlayer : MonoBehaviourPunCallbacks {
	[SerializeField] private Piece piece;
	PhotonView PV;
	Board board;
	GameUI gameUI;
	boardPieceType[] myBoardPieces;
	List<Piece> myPieces = new List<Piece>();
	bool myTurn = true;
    
	void Start() {
		PV = GetComponent<PhotonView>();
		board = GameObject.FindObjectOfType<Board>();
		gameUI = GameObject.FindObjectOfType<GameUI>();

		if (!PhotonNetwork.IsMasterClient) {
			myTurn = false;
		}

		SetUpPieces();
	}

	#region Pieces
	public void SetUpPieces() {
		if (PV.IsMine) {
			myBoardPieces = board.GetPieces();

			foreach (boardPieceType boardPiece in myBoardPieces) { // Creates my pieces on my side
				Vector2 spawnPos = new Vector2(boardPiece.spawnPos.Item1, boardPiece.spawnPos.Item2); // This gets the spawn position from each board piece
				Piece p = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Piece"), spawnPos, Quaternion.identity).GetComponent<Piece>();
				p.SetUpPiece(this, GameConfiguration.Instance.pieces[boardPiece.pieceIndex]); // Sets up piece
				if (GameConfiguration.Instance.GetRule(GameConfiguration.Famine) && p.GetName() == GameConfiguration.PAWN) {
					PhotonNetwork.Destroy(p.gameObject);
				}
				myPieces.Add(p);
			}

			if (!PhotonNetwork.IsMasterClient) { // Moving pieces to other side of board in case of P2
				foreach (Piece p in myPieces) {
					if (p.GetName() == GameConfiguration.QUEEN) {
						p.transform.Translate(-Vector2.left);
					} else if (p.GetName() == GameConfiguration.KING) {
						p.transform.Translate(Vector2.left);
					}

					Vector2 cornerOfBoard = new Vector2(board.GetBoardSize().Item1-1, board.GetBoardSize().Item2-1);
					p.gameObject.transform.Translate(cornerOfBoard); // translates
					p.gameObject.transform.RotateAround(cornerOfBoard, Vector3.forward, 180); // rotate 190 degrees around corner
				}
			}
		}
	}

	public void MovePiece(Vector2 pos) {
		foreach (Piece p in myPieces) { // for every piece on my side
			if (p.GetIsActive()) { // if it is the active piece
				p.transform.position = pos; // Moving piece
				p.SetIsActive(false); // removing active variable
				board.ResetTileColors(); // getting rid of the green/red squares
				
				Collider2D[] colliders = Physics2D.OverlapCircleAll(p.gameObject.transform.position, 0.1f); // Finding piece to take
				foreach (Collider2D col in colliders) {
					if (col.gameObject.GetComponent<Piece>() && col.gameObject != p.gameObject) { // if it lands on enemy piece
						col.gameObject.GetComponent<Piece>().TakePiece(); // take the piece
					}
				}

				ChessPlayer[] chessPlayers = GameObject.FindObjectsOfType<ChessPlayer>(); // Setting the turns of each player
				foreach (ChessPlayer cp in chessPlayers) {
					if (cp.gameObject != gameObject) { // switching turns
						cp.SetTurn(true);
						myTurn = false;
					}
				}

				if (GameConfiguration.Instance.GetRule(GameConfiguration.Deadeye)) {
					print("You have deadeye");
					Deadeye();
				}
				break;
			}
		}
	}
	#endregion

	#region Turns
	public void SetTurn(bool b) {
		PV.RPC("RPC_SetTurn", RpcTarget.All, b);
	}

	[PunRPC]
	private void RPC_SetTurn(bool b) {
		if (!PV.IsMine) { return; } // if local
		myTurn = b;
	}

	public bool GetMyTurn() => myTurn;

	public void AddTurn() {
		PV.RPC("RPC_AddTurn", RpcTarget.All);
	}

	[PunRPC]
	private void RPC_AddTurn() {
		if (!PV.IsMine) { return; } // if local
		GameManager.Instance.AddTurn();
	}
	#endregion

	public void Deadeye() {
		foreach (Piece currPiece in myPieces) { // for each of my pieces
			foreach (Vector2 attVector in currPiece.GetAttackVectors()) { // for every attack vector{
				foreach (Vector2 possibleMove in currPiece.GetPossibleMoves(attVector)) { // for every attack vector 
					Collider2D[] colliders = Physics2D.OverlapCircleAll(possibleMove, 0.1f); // gets objs at that position
					foreach (Collider2D col in colliders) {
						if (col.gameObject.GetComponent<Piece>() && !col.gameObject.GetComponent<Piece>().GetIsMine() && col.gameObject.GetComponent<Piece>().GetName() == GameConfiguration.KING) {
							col.gameObject.GetComponent<Piece>().TakePiece();
						}
					}
				}
			}
		}
	}

	public void LoseGame() {
		ChessPlayer[] chessPlayers = GameObject.FindObjectsOfType<ChessPlayer>();
		foreach (ChessPlayer cp in chessPlayers) {
			if (cp.gameObject != gameObject) { // if opponent
				cp.WinGame();
			}
		}
		ShowEndPanel("You lost...");
	}

	public void WinGame() {
		PV.RPC("RPC_WinGame", RpcTarget.All);
	}

	[PunRPC]
	public void RPC_WinGame() {
		if (!PV.IsMine) { return; } // only run if me

		ShowEndPanel("You Won!");
	}

	public void ShowEndPanel(string text) {
		gameUI.SetEndScreenActive(true);
		gameUI.SetEndText(text);
	}

	public void KillPiece(GameObject go) => PhotonNetwork.Destroy(go);

	public void SetPiecesActive(bool b) {
		foreach (Piece p in myPieces) { p.SetIsActive(b); } // Setting all of the pieces to desired active state
	}

	public bool GetIsMine() => PV.IsMine; // Get local
}