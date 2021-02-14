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
        new pieceType(KING, 		(new(int, int)[]{ (0, 1), (1, 1) }, 	new(int, int)[]{ (0, 0), (0, 0) }), 	(false, true, 	false)),
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

	#region WD
	public string[] wd = new string[] { "abandoned labs" , "addison school" , "adobe creek boardwalk" , "advanced technology labs" , "aether ridge" , "alta Mesa" , "altaire block" , "alumni center" , "apartment complex" , "arastradero west" , "Arboretum" , "archery range" , "area 52" , "arrillaga dining" , "art center" , "art museums" , "barron park elementry" , "baseball field" , "baylands golf course" , "Bermuda triangle" , "bing concert hall" , "Bol park" , "boulware park" , "bowden park" , "bowman school" , "briones park" , "briones school" , "byxbee park" , "cactus land" , "cameron park" , "car depot" , "castilleja" , "challenger school" , "channing house" , "chareston gardens" , "chareston slough" , "chestnut residences" , "children's theatre" ,
										"city hall" , "cloverleaf" , "colorado park apartments" , "community center" , "company grounds" , "construction" , "contested territory" , "core barron park" , "corporations" , "crescent park quadrant 1" , "crescent park quadrant 2" , "crescent park quadrant 3" , "crescent park quadrant 4" , "cubberley center" , "curtner apartments" , "downtown 1" , "downtown 2" , "downtown 3" , "downtown 4" , "downtown 5" , "downtown university avenue" , "duck pond" , "duveneck school" , "east barron park" , "east charleston meadow" , "east downtown north" , "east duveneck" , "east fletcher" , "east meadow circle" , "east meadow creekside" , "east midtown" , "east palo verde" , "east triple el" , "edge of the gardens" , "egret pond" , 
										"el carmelo" , "electronics" , "embarcadero oaks" , "emerson school" , "emily renzel ponds" , "enchanted broccoli forest" , "energy center" , "engineering department" , "escondido" , "escondido village" , "escondido village 2" , "escondido village part B" , "evergreen park" , "fabian community" , "fairmeadow" , "fallen leaf" , "family day care" , "far north midtown" , "filtration system" , "fire station" , "fletcher" , "flight school" , "Frenchman's Park" , "galactic hq" , "gamble garden" , "gecko street" , "gerrymander island" , "gerrymander'd" , "golf course 1" , "golf course 2" , "goodwill" , "graduate residences" , "green acres" , "greendell  " , "greendell school" , "greene" , "greenmeadow" , "greer park" , "grocery outlet" ,
										"Gunn" , "henry from gunn.app park" , "heritage park" , "hoover" , "hoover park" , "hoover tower" , "hotel block" , "housing development" , "housing district" , "hunterton memorial grounds" , "hunterton sector 1" , "hunterton sector 2 " , "hunterton sector 3" , "hunterton sector 4" , "hunterton sector 5" , "hunterton sector 6" , "hunterton sector 7" , "I have no idea what to name this" , "infamous walgreens" , "island drive garden" , "jls" , "junior museum and zoo" , "keys school" , "lake lagunita" , "lakeside" , "land of sun and stores" , "lathrop park" , "leland manor" , "lucy evans nature preserve" , "manzanita field" , "mayfield" , "Mayfield park and library" , "mayfield slough" , "mears court" , "metro circle" ,
										"mid college terrace" , "mid-midtown" , "midtown  " , "midtown court apartments" , "mitchell park" , "mitchell park library" , "moffett circle" , "monroe park" , "monroe park 2" , "monroe park apartments" , "more labs" , "nevada" , "nixon" , "north barron park" , "north college terrace" , "north fairmeadow" , "north dish area" , "north midtown" , "north old palo alto" , "north palo verde" , "north professorville" , "north st. francis" , "north triple el" , "northwest midtown" , "norway" , "oak creek" , "ohlone school" , "old palo alto core 1" , "old palo alto core 2" , "old palo alto core 3" , "ortega court" , "outer charleston gardens" , "outer rim center" , "outskirts" , "palo alto airport" , "palo alto nursery school" , "palo alto orchards" ,
										"palo alto power center" , "palo alto square" , "palo alto VA" , "palo verde" , "paly and school district" , "paolo alto park" , "pardee park" , "park plaza" , "parkside" , "peers park" , "peter coutts" , "philz coffee" , "postal center" , "printer company grounds" , "professorville" , "railside housing" , "raimunod village" , "ramos park" , "red square" , "riconada park pool" , "rinconada park" , "roble athletics centers" , "robles park" , "robles stronghold" , "safeway" , "san alma" , "scouts base" , "seale forest" , "shell" , "shopping center" , "shoreline complex" , "silicon valley international school" , "soccer complex" , "software labs" , "solar observatories" , "sora international school" , "south barron park" , 
										"south charleston meadow" , "south college terrace" , "south dish area" , "south old palo alto" , "south palo verde" , "south palo verde" , "south st. francis" , "southgate" , "southwood apartments" , "spok tunnel" , "st. claires gardens quadrant 1" , "st. claires gardens quadrant 2" , "st. claires gardens quadrant 3" , "st. claires gardens quadrant 4" , "st. francis" , "stanford althetics" , "Stanford bio labs" , "Stanford Bus depot" , "stanford business" , "stanford center" , "stanford center court" , "stanford clinics" , "stanford eye institute" , "stanford golf driving range" , "Stanford hospital" , "stanford law school" , "stanford recreational association" , "stanford research departments" , "stanford stadium" , 
										"stanford villa" , "stanford weekend acres" , "stanford west" , "suburban 1" , "suburban 2" , "suburban 3" , "suburban 4" , "suburban 5" , "suburban 6" , "suburban 7" , "suburban 8" , "suburban 9" , "suburban 10" , "suburban 11" , "sutter heath" , "sutter park" , "teaspoon" , "tesla and volvo depot" , "tesla hq" , "the border" , "the crash site" , "the crescent" , "the dam" , "the deep core" , "the edge" , "the fields" , "the greenhouse" , "the hills" , "The Marc" , "The oval" , "the P " , "the place that no one ever goes" , "the place with circles" , "the sliver" , "the stanford dish" , "the triangle" , "tolman playgrounds" , "towle apartments" , "town and country village" , "tresidder" , "triple el" , "university club" , 
										"unnamed" , "van auken circle" , "ventura  " , "ventura apartments" , "ventura community center" , "Vi" , "VMware complex" , "walnut grove" , "walter hays" , "weisshaar park" , "wells fargo" , "wellsbury" , "werry park" , "west barron park" , "west community center" , "west crescent park" , "west downtown north" , "west duveneck" , "west midtown" , "west old palo alto" , "west palo verde" , "west st. francis" , "west triple el" , "where the wild things are" , "wilbur hall" , "winter lodge" , "YMCA" };
	#endregion

	#region Rules
	[SerializeField] private GameObject ruleGO;
	
	public List<int> activeRules = new List<int>();

	public static int  	Chaos = 			0,
						Communism = 		1,
						Deadeye = 			2,
						Famine = 			3,
						Gecko = 			4,
						HeroesNeverDie = 	5,
						Jack = 				6,
						Nigerundayo = 		7,
						QueenFabrication = 	8,
						WorldDomination = 	9;

	public static string[] descriptions = new string[] { 	"Chaos",
															"All Pieces are equal",
															"Check = Checkmate",
															"Pawns are gone",
															"Gecko Sprites",
															"Respawning Pieces",
															"JackSprites",
															"Hotfoot it outta there",
															"Promotions happen halfway",
															"PS, California" };
	#endregion

	void Awake() {
		Instance = this;
		DontDestroyOnLoad(gameObject);
		PV = GetComponent<PhotonView>();

		slider_boardSize.GetComponent<Slider>().onValueChanged.AddListener(delegate{  text_boardSize.text = slider_boardSize.GetValue().ToString();  });
		slider_boardSize.SetMaxValue(16); // Setting max range for board size
		slider_boardSize.SetMinValue(8);  ﬁ
	}

	void Start() {
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

	public bool GetRule(int index) => activeRules.Contains(index);

	public string GetWDName() {
		return wd[Random.Range(0, wd.Length)];
	}
}