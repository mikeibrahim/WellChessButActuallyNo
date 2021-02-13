using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;

public class RoomListItem : MonoBehaviour {
	[SerializeField] private TMP_Text text_roomName;

	public RoomInfo info;

	public void SetUp(RoomInfo info) {
		this.info = info;
		text_roomName.text = info.Name;
	}

	public void OnClick() {
		Launcher.Instance.JoinRoom(info);
	}
}
