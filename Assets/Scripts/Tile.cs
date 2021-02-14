using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour {
	[SerializeField] private Color whiteTileColor;
	[SerializeField] private Color blackTileColor;
	[SerializeField] private Image image;
	Color defaultColor;
	ChessPlayer player;
	Collider2D col;

	public void SetUp() {
		col = GetComponent<Collider2D>();
		transform.SetParent(GameObject.Find("World").transform); // Sets the parent to the World canvas
		ChessPlayer[] players = GameObject.FindObjectsOfType<ChessPlayer>();
		foreach (ChessPlayer p in players) {
			print("Player: " + p);
			if (p && p.GetIsMine()) { player = p; } // Gets the local player
		}
	}
	
	public void SetCheckered(bool b) {
		defaultColor = b ? whiteTileColor : blackTileColor;
		image.color = defaultColor;
	}

	public void SetColor(Color c, bool b) {
		// if (player.GetMyTurn()) {
			image.color = b ? c : defaultColor;
		// }
	}

	public void SetTIleCollider(bool b) {
		col.enabled = b;
	}

	public bool GetTileCollidre() => col.enabled;

	public Color GetColor() => image.color;

	public void OnClick() {
		if (image.color == Color.green || image.color == Color.red) { // if Tile is ready to be moved on
			player.MovePiece(transform.position);
		// } else if (image.color == Color.red) {
		// 	player.AttackPieceAt(transform.position);
		}
	}
}
