// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using Photon.Pun;
// using System.IO;

// public struct playerType {
//     public string shipName;

//     public playerType (string n) {
//         this.shipName = n;
//     }
// }

// public static class Players {
// 	public static int EXPLORER = 0;

//     public static playerType[] players = new playerType[] {
//         //          shipName
//         new playerType("Explorer"),
//     };
// }

// public class PlayerController : MonoBehaviour {
// 	private string playerName = "Unknown";
// 	PhotonView PV;
// 	PlayerManager playerManager;
// 	Weapon weapon;
// 	GameUI gameUI;

// 	#region PlayerStats
// 	private int maxHealth = 100;
// 	int currentHealth;
// 	int currentWeapon = Weapons.RIFLE;
// 	#endregion

// 	#region Movement
// 	Rigidbody2D rb;
// 	private float maxSpeed = 20f;
// 	private float accelerationSpeed = 2;
// 	Vector2 smoothPosition;
// 	Vector2 velocity;
// 	private float turnSpeed = 150;
// 	#endregion

// 	private void Awake() {
//         rb = GetComponent<Rigidbody2D>();
// 		PV = GetComponent<PhotonView>();
// 		currentHealth = maxHealth;
// 		// weapon = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Weapon"), transform.position, transform.rotation).GetComponent<Weapon>();
// 		weapon = GetComponentInChildren<Weapon>();
// 		weapon.SetUpWeapon(this, currentWeapon);
// 	}

//     private void Start() {
// 		if (!PV.IsMine) {
// 			Destroy(rb);
// 			Destroy(weapon);
// 		} else {
// 			// print("I am calling SetPlayerName: " + gameObject);
// 			SetPlayerName();
// 			gameUI = GameObject.FindObjectOfType<GameUI>();
// 			gameUI.SetMaxHealth(maxHealth);
// 		}
//     }

// 	public void SetPlayerName() {
// 		PV.RPC("RPC_SetPlayerName", RpcTarget.All);
// 	}

// 	[PunRPC]
// 	void RPC_SetPlayerName() {
// 		this.playerName = this.PV.Owner.NickName;
// 		print("My Name: "+this.playerName);
// 		if (this.PV.IsMine) { 
// 			print("This is my playerview");
// 		} else {
// 			print("This is not my playerview");
// 		}
// 	}

// 	private void FixedUpdate() {
// 		if (!PV.IsMine) { return; }

// 		// Movement
// 		Turn();
// 		Move();
// 		rb.MovePosition(smoothPosition);
// 	}

// 	#region Movement
// 	private void Turn() {
// 		Vector3 rot = transform.eulerAngles;
// 		rot.z += -Input.GetAxisRaw("Horizontal") * turnSpeed * Time.deltaTime; // Turning the player with a & d keys
// 		transform.eulerAngles = rot;
// 	}

// 	private void Move() {
// 		Vector2 targetPosition = transform.TransformPoint(Vector3.up * (Input.GetKey(KeyCode.W) ? maxSpeed : 0)); // move certain amount if w key is pressed
// 		smoothPosition = Vector2.SmoothDamp(transform.position, targetPosition, ref velocity, accelerationSpeed); // smooth that distance
// 	}
// 	#endregion

// 	public void TakeDamge(int amount) {
// 		// PlayerController[] players = GameObject.FindObjectsOfType<PlayerController>();
// 		// foreach (PlayerController p in players) {
// 		// 	print("Player Loop CurrentHealth: " +  p.GetCurrentHealth());
// 		// }

// 		PV.RPC("RPC_TakeDamage", RpcTarget.All, amount);
// 	}

// 	// this is still running on my script, so it will only run if PV.IsMine returns true relative to me, which is my controller
// 	[PunRPC]
// 	void RPC_TakeDamage(int amount) {
// 		if (!PV.IsMine) { return; }

// 		this.currentHealth -= amount; // Taking damage
// 		gameUI.SetHealth(this.currentHealth);

// 		if (currentHealth <= 0) { Die(); }
// 	}

// 	public string GetPlayerName() => playerName;

// 	public int GetCurrentHealth() => currentHealth;

// 	private void Die() => playerManager.Die();

// 	public void SetPlayerManager(PlayerManager pm) => playerManager = pm;
// }
