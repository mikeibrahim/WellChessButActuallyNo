using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	public static GameManager Instance;
	private int numberOfTurns;

	private void Awake() {
		Instance = this;
	}

	public void AddTurn() {
		numberOfTurns++;
		print("Number of turns: "+numberOfTurns);
	}

	public int GetTurns() => numberOfTurns;
}
