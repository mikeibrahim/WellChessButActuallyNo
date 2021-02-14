using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using System.IO;
using Photon.Realtime;

public class RoomManager : MonoBehaviourPunCallbacks {
	void Awake() {
		PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "ChessPlayer"), Vector3.zero, Quaternion.identity);
	}

	// void Start() {
	// 	// print("Number of Players: " + GameObject.FindObjectsOfType<ChessPlayer>().Length);
	// 	// if (GameObject.FindObjectsOfType<ChessPlayer>().Length >= 2) { // if there are less than two players, dont spawn a player
	// 	PhotonNetwork.Disconnect();
	// 	// }
	// }

	// public override void OnDisconnected(DisconnectCause cause) {
	// 	SceneManager.LoadScene(0);
	// }
}
