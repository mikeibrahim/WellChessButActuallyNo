using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class GameUI : MonoBehaviourPunCallbacks {
	[SerializeField] private GameObject panel_endScreen;
	[SerializeField] private TMP_Text text_endText;
	PhotonView PV;
	// ChessPlayer player;
	// [SerializeField] private GameObject button_rematch;

	void Awake() {
		PV = GetComponent<PhotonView>();
		panel_endScreen.SetActive(false); // turn it off before
		// if (!PhotonNetwork.IsMasterClient) {
		// 	button_rematch.SetActive(false);
		// }
	}

	public void SetEndScreenActive(bool b) {
		panel_endScreen.SetActive(b);
	}

	public void SetEndText(string text) => text_endText.text = text;

	// public void SetPlayer(ChessPlayer p) => player = p;

	public void BackToMainMenu() {
		GameConfiguration gameConfig = GameObject.FindObjectOfType<GameConfiguration>();
		Destroy(gameConfig);
		PhotonNetwork.Disconnect();
	}

	public override void OnDisconnected(DisconnectCause cause) {
		SceneManager.LoadScene(0);
	}
}
