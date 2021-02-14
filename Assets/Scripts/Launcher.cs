using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System.Linq;

public class Launcher : MonoBehaviourPunCallbacks  {
	public static Launcher Instance;

	[SerializeField] private TMP_InputField inputField_roomName;
	[SerializeField] private TMP_Text text_roomName;
	
	[SerializeField] private Transform panel_roomListHolder; // Showing the Rooms
	[SerializeField] private GameObject prefab_roomListItem;

	[SerializeField] private Transform panel_playerListHolder; // Showing the Players
	[SerializeField] private GameObject prefab_playerListItem;

	[SerializeField] private GameObject button_startGame;
	[SerializeField] private GameObject panel_rules;

	void Awake()  {
		Instance = this;
	}

    void Start()  {
		// Debug.Log("Connectingto Master");
		PhotonNetwork.ConnectUsingSettings();
		MenuManager.Instance.SetMenu(MenuManager.LoadingMenu);
    }

	public override void OnConnectedToMaster()  {
		// Debug.Log("Connected to Master");
		PhotonNetwork.JoinLobby();
		PhotonNetwork.AutomaticallySyncScene = true;
	}

	public override void OnJoinedLobby()  {
		MenuManager.Instance.SetMenu(MenuManager.TitleMenu);
		// Debug.Log("Joined Lobby");
		PhotonNetwork.NickName = "Player " + Random.Range(0, 1000).ToString("0000"); 
	}

	public void CreateRoom()  {
		if (string.IsNullOrEmpty(inputField_roomName.text))  {
			return;
		}
		PhotonNetwork.CreateRoom(inputField_roomName.text);
		MenuManager.Instance.SetMenu(MenuManager.LoadingMenu);
	}

	public override void OnJoinedRoom()  {
		button_startGame.GetComponent<Button>().interactable = PhotonNetwork.PlayerList.Length == 2;
		MenuManager.Instance.SetMenu(MenuManager.RoomMenu);
		text_roomName.text = PhotonNetwork.CurrentRoom.Name;

		Player[] players = PhotonNetwork.PlayerList;

		foreach (Transform trans in panel_playerListHolder) {
			Destroy(trans.gameObject);
		}

		for (int i = 0; i < players.Count(); i++) {
			Instantiate(prefab_playerListItem, panel_playerListHolder).GetComponent<PlayerListItem>().SetUp(players[i]);
		}

		button_startGame.SetActive(PhotonNetwork.IsMasterClient);
		panel_rules.SetActive(PhotonNetwork.IsMasterClient);
	}

	public override void OnMasterClientSwitched(Player newMasterClient) {
		button_startGame.SetActive(PhotonNetwork.IsMasterClient);
		panel_rules.SetActive(PhotonNetwork.IsMasterClient);
	}

	public void StartGame() {
		PhotonNetwork.LoadLevel(1);
	}

	public void LeaveRoom()  {
		PhotonNetwork.LeaveRoom();
		MenuManager.Instance.SetMenu(MenuManager.LoadingMenu);
	}

	public void JoinRoom(RoomInfo info) {
		PhotonNetwork.JoinRoom(info.Name);
		MenuManager.Instance.SetMenu(MenuManager.LoadingMenu);
	}

	public override void OnLeftRoom() {
		MenuManager.Instance.SetMenu(MenuManager.TitleMenu);
	}

	public override void OnRoomListUpdate(List<RoomInfo> roomList) {
		foreach (Transform trans in panel_roomListHolder) {
			Destroy(trans.gameObject);
		}
		
		for (int i = 0; i < roomList.Count; i++) {
			if (roomList[i].RemovedFromList)  {  continue;  }
			Instantiate(prefab_roomListItem, panel_roomListHolder).GetComponent<RoomListItem>().SetUp(roomList[i]);
		}
	}

	public override void OnPlayerEnteredRoom(Player newPlayer) {
		Instantiate(prefab_playerListItem, panel_playerListHolder).GetComponent<PlayerListItem>().SetUp(newPlayer);
		button_startGame.GetComponent<Button>().interactable = PhotonNetwork.PlayerList.Length == 2;
	}

	public override void OnPlayerLeftRoom(Player otherPlayer) {
		button_startGame.GetComponent<Button>().interactable = PhotonNetwork.PlayerList.Length == 2;
	}
}
