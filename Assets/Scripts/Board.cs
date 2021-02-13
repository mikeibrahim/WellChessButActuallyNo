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

	boardType currentBoard;
	string boardName;
	(int, int) boardSize;
	bool altColor;
	boardPieceType[] boardPieces;
	Tile[] tiles;

	private void Awake() {
		currentBoard = GameConfiguration.Instance.board;
		boardName = currentBoard.boardName;
		boardSize = currentBoard.boardSize;
		boardPieces = currentBoard.pieces;
	}

	private void Start() {
		CreateBoard();
		tiles = GameObject.FindObjectsOfType<Tile>();
	}

	public void CreateBoard() {
		for (int x = 0; x < boardSize.Item1; x++) {
			for (int y = 0; y < boardSize.Item2; y++) {
				Vector2 spawnPos = new Vector2(x, y);
				Tile t = Instantiate(tile, spawnPos, Quaternion.identity);
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
					col.gameObject.GetComponent<Tile>()?.SetGreen(true);
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
					col.gameObject.GetComponent<Tile>()?.SetRed(true);
				}
			}
		}
	}
	

	public void ResetTileColors() {
		foreach (Tile t in tiles) {
			t.SetGreen(false);
		}
	}

	public boardPieceType[] GetPieces() => boardPieces;

	public (int, int) GetBoardSize() => boardSize;
}
