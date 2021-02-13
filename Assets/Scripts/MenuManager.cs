using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {
	public static MenuManager Instance;
	[SerializeField] private Menu[] menus;

	public static string LoadingMenu = "Menu_Loading";
	public static string TitleMenu = "Menu_Title";
	public static string CreateRoomMenu = "Menu_CreateRoom";
	public static string RoomMenu = "Menu_Room";

	private void Awake() {
		Instance = this;
	}

	public void SetMenu(string name) {
		foreach (Menu menu in menus) {
			menu.gameObject.SetActive(menu.gameObject.name == name); // If menu has the same name -> activate 
		}
	}

	public void SetMenu(Menu targetMenu) {
		foreach (Menu menu in menus) {
			menu.gameObject.SetActive(menu == targetMenu);
		}
	}
}
