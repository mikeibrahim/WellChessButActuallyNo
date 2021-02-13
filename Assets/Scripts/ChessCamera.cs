using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ChessCamera : MonoBehaviour {
	Camera cam;
	Board board;
	int buffer = 1;

    void Start() {
		cam = GetComponent<Camera>();
        board = GameObject.FindObjectOfType<Board>();
		(int, int) boardSize = board.GetBoardSize();

		cam.orthographicSize = boardSize.Item1 / 2.0f + buffer; // procedually setting camera size

		transform.position = new Vector3((boardSize.Item1 - 1) / 2.0f, (boardSize.Item2 - 1) / 2.0f, -1); // set to center of screen
		if (!PhotonNetwork.IsMasterClient) {
			Vector3 rot = transform.eulerAngles;
			rot.z += 180;
			transform.eulerAngles = rot;
		}
    }
}
