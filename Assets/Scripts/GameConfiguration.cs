using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class GameConfiguration : MonoBehaviour {
	public static GameConfiguration Instance;
	[SerializeField] private GameObject panel_ruleHolder;
	ChessPlayer player;
	PhotonView PV;

	#region Pieces
	public (int, int) pawnMove = (0, 2);

	public static int PAWN = 	0;
	public static int ROOK = 	1;
	public static int KNIGHT = 	2;
	public static int BISHOP = 	3;
	public static int QUEEN = 	4;
	public static int KING = 	5;

    public pieceType[] pieces = new pieceType[] {
        //         	   Name			 				   Move	 							  	  Att					 Infin   Any   Ghost
        new pieceType(PAWN, 		(new(int, int)[]{ (0, 1) }, 			new(int, int)[]{ (1, 1) } ), 			(false, false, 	false)),
        new pieceType(ROOK, 		(new(int, int)[]{ (0, 1) }, 			new(int, int)[]{ (0, 0) }), 			(true, 	true, 	false)),
        new pieceType(KNIGHT, 		(new(int, int)[]{ (1, 2) }, 			new(int, int)[]{ (0, 0) }), 			(false, true, 	false)),
        new pieceType(BISHOP, 		(new(int, int)[]{ (1, 1) }, 			new(int, int)[]{ (0, 0) }), 			(true, 	true, 	false)),
        new pieceType(QUEEN, 		(new(int, int)[]{ (0, 1), (1, 1) }, 	new(int, int)[]{ (0, 0), (0, 0) }), 	(true, 	true, 	false)),
        new pieceType(KING, 		(new(int, int)[]{ (0, 1), (1, 1)  }, 	new(int, int)[]{ (0, 0), (0, 0) }), 	(false, true, 	false)),
    };
	#endregion

	#region Board
	[SerializeField] private SlideBar slider_boardSize;
	[SerializeField] private TMP_Text text_boardSize;

	public boardType board = new boardType("Default", (8, 8),
								new boardPieceType[] {
									new boardPieceType(PAWN, 	(0, 1)),
									new boardPieceType(PAWN, 	(1, 1)),
									new boardPieceType(PAWN, 	(2, 1)),
									new boardPieceType(PAWN, 	(3, 1)),
									new boardPieceType(PAWN, 	(4, 1)),
									new boardPieceType(PAWN, 	(5, 1)),
									new boardPieceType(PAWN, 	(6, 1)),
									new boardPieceType(PAWN, 	(7, 1)),
									new boardPieceType(ROOK, 	(0, 0)),
									new boardPieceType(ROOK, 	(7, 0)),
									new boardPieceType(KNIGHT, 	(1, 0)),
									new boardPieceType(KNIGHT, 	(6, 0)),
									new boardPieceType(BISHOP, 	(2, 0)),
									new boardPieceType(BISHOP, 	(5, 0)),
									new boardPieceType(QUEEN, 	(3, 0)),
									new boardPieceType(KING, 	(4, 0)),
								});
	#endregion

	#region Rules
	[SerializeField] private GameObject ruleGO;
	
	public List<int> activeRules = new List<int>();
	#endregion

	void Awake() {
		Instance = this;
		DontDestroyOnLoad(gameObject);
		PV = GetComponent<PhotonView>();

		slider_boardSize.GetComponent<Slider>().onValueChanged.AddListener(delegate{  text_boardSize.text = slider_boardSize.GetValue().ToString();  });
		slider_boardSize.SetMaxValue(16); // Setting max range for board size
		slider_boardSize.SetMinValue(8);

		for (int i = 0; i < Thumbnail.Instance.GetRuleThumbnails().Length; i++) {
			Rule r = Instantiate(ruleGO, panel_ruleHolder.transform).GetComponent<Rule>();
			r.SetRuleIndex(i);
			r.SetRuleName(Thumbnail.Instance.GetRuleName(i));
		}
	}

	public void ApplySettings() {
		PV.RPC("RPC_ApplySettings", RpcTarget.All, slider_boardSize.GetValue(), activeRules.ToArray());
	}

	public void SetBoardSize(int size) => board.boardSize = (size, size);

	public void AddRule(int index) { activeRules.Add(index); }

	public void RemoveRule(int index) { activeRules.Remove(index); }

	[PunRPC]
	public void RPC_ApplySettings(int boardSize, int[] rules) {
		SetBoardSize(boardSize);
		activeRules = new List<int>(rules);
	}
}