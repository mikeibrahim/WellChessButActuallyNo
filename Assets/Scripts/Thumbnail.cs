using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thumbnail : MonoBehaviour {
	public static Thumbnail Instance;
	[SerializeField] private Sprite[] whiteThumbnails;
	[SerializeField] private Sprite[] blackThumbnails;

	void Awake() {
		Instance = this;
	}

	public Sprite GetWhiteThumbnail(int index) => whiteThumbnails[index];

	public Sprite GetBlackThumbnail(int index) => blackThumbnails[index];
}
