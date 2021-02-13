using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;


public struct pieceType {
	public int pieceName;
//			MoveVector	AttackVector
	public ((int, int)[], (int, int)[]) moveVectors;
//		   Infin  Any Ghost
    public (bool, bool, bool) moveStyles;

    public pieceType (int n, ((int, int)[], (int, int)[]) mv, (bool, bool, bool) ms) {
		this.pieceName = n;
        this.moveVectors = mv;
        this.moveStyles = ms;
    }
}

public class Piece : MonoBehaviour {
	[SerializeField] private Image spriteRenderer;
	PhotonView PV;
	Board board;
	GameConfiguration gc;
	
	ChessPlayer player;
	int pieceName; //  name of piece
	bool infiniteMove; // can move one direction infinite
	bool anyMove; // moveVector can go up/down
	bool ghostMove; // move through your own pieces
	List<Vector2> moveVectors = new List<Vector2>();// how the piece moves
	List<Vector2> attackVectors = new List<Vector2>(); // how the piece attacks

	bool isActivated;

	private void Awake() {
		PV = GetComponent<PhotonView>();
		board = GameObject.FindObjectOfType<Board>();
		gc = GameObject.FindObjectOfType<GameConfiguration>();

		if (!PV.IsMine) {
			Destroy(GetComponent<Button>());
			spriteRenderer.raycastTarget = false;
			if (!PhotonNetwork.IsMasterClient) { // if it is opponent and the opposite side
				Vector3 rot = transform.eulerAngles; // rotate upside down
				rot.z += 180;
				transform.eulerAngles = rot;
			}
		}
	}

	private void Start() {
		transform.SetParent(GameObject.FindObjectOfType<Canvas>().transform); // Sets the parent to the World canvas
	}

	public void SetUpPiece(ChessPlayer player, pieceType type) {
		this.player = player;

		pieceName = type.pieceName;
		infiniteMove = type.moveStyles.Item1;
		anyMove = type.moveStyles.Item2;
		ghostMove = type.moveStyles.Item3;
		foreach ((int, int) mv in type.moveVectors.Item1) {
			moveVectors.Add(new Vector2(mv.Item1, mv.Item2));
		}
		foreach ((int ,int) av in type.moveVectors.Item2) {
			if (av == (0, 0)) {
				attackVectors = moveVectors;
			} else {
				attackVectors.Add(new Vector2(av.Item1, av.Item2));
			}
		}

		SetSprite(pieceName);
	}

	public void OnClick() {
		if (player.GetMyTurn()) {
			Activate(isActivated);
		}
	}

	public void Activate(bool b) { // On clicked
		board.ResetTileColors();
		if (b) { // if the piece is already active
			board.ResetTileColors(); // then reset everything
		} else {
			foreach (Vector2 v in moveVectors) {
				board.ShowPossibleMoves(GetPossibleMoves(v)); // show where you can go
			}
			foreach (Vector2 v in attackVectors) {
				board.ShowPossibleAttacks(GetPossibleMoves(v)); // show where you can go
			}
			player.SetPiecesActive(false); // Deactivate other pieces
		}

		isActivated = !isActivated; // Toggle
	}

	public void SetIsActive(bool b) { isActivated = b; }

	public bool GetIsActive() => isActivated;

	private List<Vector2> GetPossibleMoves(Vector2 pos) { // Where the piece can move on the board
		List<Vector2> possibleMoves = new List<Vector2>(); // empty list to add positions
		for (int x = 0; x < 2; x++) { // positive and negative x values
			for (int y = 0; y < 2; y++) { // positive and negative y values
				for (int z = 0; z < 2; z++) { // switching the position of x and y within the vector
					float xPos = x == 0 ? pos.x : -pos.x; // positive and negative x
					float yPos = y == 0 ? pos.y : -pos.y; // positive and negative y
					Vector2 XY = z == 0 ? new Vector2(xPos, yPos) : new Vector2(yPos, xPos); // final X and Y values for points
					
					if (XY.y <= 0 && !anyMove) {  continue;  } // Can only move forward
					
					if (infiniteMove) {
						foreach (Vector2 v in GetOneDirectionPoints(XY)) {
							// print("Cycled through one point");
							possibleMoves.Add(v);
						}
					} else {
						Vector2 final = transform.TransformPoint(XY); // final position that is possible
						possibleMoves.Add(final); // original vector
					}
				}
			}
		}
		return possibleMoves;
	}

	public List<Vector2> GetOneDirectionPoints(Vector2 start) { // For infinite moves
		List<Vector2> vectors = new List<Vector2>();
		for (int i = 1; i < board.GetBoardSize().Item1 + 1; i++) {
			Collider2D[] hits = Physics2D.OverlapCircleAll(transform.TransformPoint(start * i), 0.1f); // hits anything
			bool hitSomething = hits != null; // did you hit anything
			bool hitPiece = false;

			foreach (Collider2D col in hits) { // checking if you hit a piece
				if (col.GetComponent<Piece>()) { // if hit piece
					hitPiece = true;
					if (!col.GetComponent<Piece>().GetIsMine()) { // checking if you hit an enemy
						vectors.Add(transform.TransformPoint(start * i)); // Add vector to enemy spot
					}
				}
			}

			if (hitSomething && !hitPiece) { // if still on board, and didnt hit a piece
				vectors.Add(transform.TransformPoint(start * i));
			} else { break; }
		}
		return vectors;
	}

	public void TakePiece() {
		PV.RPC("RPC_TakePiece", RpcTarget.All);
	}

	[PunRPC]
	private void RPC_TakePiece() {
		if (!PV.IsMine) { return; }

		if (pieceName == GameConfiguration.Instance.pieces[GameConfiguration.KING].pieceName) { // if king
			player.LoseGame(); // lose
		}
		player.KillPiece(this.gameObject);
	}

	public bool GetIsMine() {
		return PV.IsMine;
	}


	public void SetSprite(int index) {
		PV.RPC("RPC_SetSprite", RpcTarget.All, index);
	}

	[PunRPC]
	private void RPC_SetSprite(int index) {
		print("Set up the sprite with: " + index);
		bool isWhite = (PhotonNetwork.IsMasterClient && PV.IsMine) || (!PhotonNetwork.IsMasterClient && !PV.IsMine);
		spriteRenderer.sprite = isWhite ? Thumbnail.Instance.GetWhiteThumbnail(index) : Thumbnail.Instance.GetBlackThumbnail(index);
	}

}
