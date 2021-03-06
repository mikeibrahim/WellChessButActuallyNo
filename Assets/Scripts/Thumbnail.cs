using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thumbnail : MonoBehaviour {
	public static Thumbnail Instance;
	[SerializeField] private Sprite[] whiteThumbnails;
	[SerializeField] private Sprite[] blackThumbnails;
	[SerializeField] private Sprite[] ruleThumbnails;
	[SerializeField] private Sprite[] jackThumbnails;
	[SerializeField] private Sprite[] geckoThumbnails;

	void Awake() {
		Instance = this;
	}

	public Sprite GetWhiteThumbnail(int index) => whiteThumbnails[index];

	public Sprite GetBlackThumbnail(int index) => blackThumbnails[index];

	public Sprite GetJackThumbnail(int index) => jackThumbnails[index];

	public Sprite GetGeckoThumbnail(int index) => geckoThumbnails[index];

	public Sprite[] GetRuleThumbnails() => ruleThumbnails;
	public Sprite GetRuleThumbnail(int index) => ruleThumbnails[index];

	public string GetRuleName(int index) => ruleThumbnails[index].name;
}
