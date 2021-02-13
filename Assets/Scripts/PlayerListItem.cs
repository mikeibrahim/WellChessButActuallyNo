using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PlayerListItem : MonoBehaviourPunCallbacks {
	[SerializeField] private TMP_Text text_playerName;
	Player player;

    public void SetUp(Player player) {
		this.player = player;
		text_playerName.text = player.NickName;
	}

	public override void OnPlayerLeftRoom(Player otherPlayer) {
		if (player == otherPlayer) {
			Destroy(gameObject);
		}
	}

	public override void OnLeftRoom() {
		Destroy(gameObject);
	}
}
