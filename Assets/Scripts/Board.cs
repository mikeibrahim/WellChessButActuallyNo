using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public struct boardPieceType { // to spawn a piece on the board
	public int pieceIndex;
    public (int, int) spawnPos;

    public boardPieceType (int n, (int, int) b) {
		this.pieceIndex = n;
        this.spawnPos = b;
    }
}

public struct boardType { // board settings
	public string boardName;
    public (int, int) boardSize;
	public boardPieceType[] pieces;

    public boardType (string n, (int, int) b, boardPieceType[] p) {
		this.boardName = n;
        this.boardSize = b;
		this.pieces = p;
    }
}

public class Board : MonoBehaviour {
	[SerializeField] private Tile tile = null;
	PhotonView PV;

	boardType currentBoard;
	string boardName;
	(int, int) boardSize;
	bool altColor;
	boardPieceType[] boardPieces;
	Tile[] tiles;
	List<Vector2> tilesForSpawn = new List<Vector2>();

	private void Awake() {
		PV = GetComponent<PhotonView>();
		currentBoard = GameConfiguration.Instance.board;
		boardName = currentBoard.boardName;
		boardSize = currentBoard.boardSize;
		boardPieces = currentBoard.pieces;
	}

	private void Start() {
		CreateBoard();
		tiles = GameObject.FindObjectsOfType<Tile>();
		if (GameConfiguration.Instance.GetRule(GameConfiguration.Chaos)) {
			foreach (Tile t in tiles) {
				tilesForSpawn.Add(t.transform.position);
			}
		}
	}

	public void CreateBoard() {
		for (int x = 0; x < boardSize.Item1; x++) {
			for (int y = 0; y < boardSize.Item2; y++) {
				Vector2 spawnPos = new Vector2(x, y);
				Tile t = Instantiate(tile, spawnPos, Quaternion.identity);
				t.SetUp();
				t.SetCheckered(altColor);
				altColor = !altColor; // Alternating colors between rows
			}
			altColor = !altColor; // to Switch which color goes first 
		}
	}

	public void ShowPossibleMoves(List<Vector2> positions) { // Actually reveals the tiles showing where you can move
		foreach (Vector2 p in positions) {
			Collider2D[] colliders = Physics2D.OverlapCircleAll(p, 0.1f);
			bool hitPiece = false;
			foreach (Collider2D col in colliders) {
				if (col.gameObject.GetComponent<Piece>()) {
					hitPiece = true;
				}
			}

			foreach (Collider2D col in colliders) {
				if (!hitPiece) { // if not moving into piece
					col.gameObject.GetComponent<Tile>()?.SetColor(Color.green, true);
				}
			}
		}
	}

	public void ShowPossibleAttacks(List<Vector2> positions) { // Actually reveals the tiles showing where you can attack
		foreach (Vector2 p in positions) {
			Collider2D[] colliders = Physics2D.OverlapCircleAll(p, 0.1f);
			bool hitPiece = false;
			foreach (Collider2D col in colliders) {
				if (col.gameObject.GetComponent<Piece>() && !col.gameObject.GetComponent<Piece>().GetIsMine()) {
					hitPiece = true;
				}
			}

			foreach (Collider2D col in colliders) {
				if (hitPiece) { // if not moving into piece
					col.gameObject.GetComponent<Tile>()?.SetColor(Color.red, true);
				}
			}
		}
	}
	

	public void ResetTileColors() {
		foreach (Tile t in tiles) {
			if (!t.GetTileCollidre()) { continue; }
			t.SetColor(Color.green, false);
		}
	}

	public boardPieceType[] GetPieces() => boardPieces;

	public (int, int) GetBoardSize() => boardSize;

	// public Vector2 GetSpawn() {
	// 	Vector2 pos = tilesForSpawn[Random.Range(0, tilesForSpawn.Count)];
	// 	PV.RPC("RPC_GetSpawn", RpcTarget.All, pos);
	// 	return pos;
	// }

	// [PunRPC]
	// public void RPC_GetSpawn(Vector2 pos) {
	// 	tilesForSpawn.Remove(pos);
	// }


	// public void NewBlackVoid() {
	// 	for (int x = 0; x < 2; x++) {
	// 		for (int y = 0; y < 2; y++) {
	// 			Vector2 pos = new Vector2(boardSize.Item1 / 2 + x - xOffset, boardSize.Item1 / 2 + y - 1); // center 4 squares
	// 			Collider2D[] colliders = Physics2D.OverlapCircleAll(pos, 0.1f); // find colliders
	// 			foreach (Collider2D col in colliders) { 
	// 				col.GetComponent<Tile>()?.SetColor(Color.black, true);
	// 				col.GetComponent<Tile>()?.SetTIleCollider(false);
	// 			}
	// 		}
	// 	}
	// }

	// public void BlackVoid() {
	// 	if (GameManager.Instance.GetTurns() % 4 == 0) {
	// 		foreach (Tile t in tiles) {
	// 			t.SetTIleCollider(true);
	// 		}
	// 		ResetTileColors();
	// 		xOffset += 4;
	// 		NewBlackVoid();
	// 	}
	// }
}
