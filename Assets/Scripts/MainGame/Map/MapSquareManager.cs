/**
 * @file MapSquareManager.cs
 * @brief マスデータ管理
 * @author yao
 * @date 2025/4/15
 */

using System.Collections.Generic;
using UnityEngine;

using static GameConst;
using static CommonModule;

public class MapSquareManager : MonoBehaviour {
	public static MapSquareManager instance { get; private set; } = null;

	/// <summary>
	/// マスオブジェクトのオリジナル
	/// </summary>
	[SerializeField]
	private MapSquareObject _originObject = null;

	private List<MapSquareData> _squareDataList = null;
	private List<MapSquareObject> _squareObjectList = null;

	// 使用中の部屋情報リスト
	private List<RoomData> _roomDataList = null;
	// 未使用状態の部屋情報リスト
	private List<RoomData> _unuseRoomDataList = null;

	public void Initialize() {
		instance = this;
		// マスを必要な数だけ生成
		int squareCount = MAP_SQUARE_HEIGHT_COUNT * MAP_SQUARE_WIDTH_COUNT;
		_squareDataList = new List<MapSquareData>(squareCount);
		_squareObjectList = new List<MapSquareObject>(squareCount);
		// マスの生成
		for (int i = 0; i < squareCount; i++) {
			// オブジェクト生成
			_squareObjectList.Add(Instantiate(_originObject, transform));
			// データを生成
			MapSquareData createSquare = new MapSquareData();
			_squareDataList.Add(createSquare);
			// セットアップ
			int x, y;
			GetSquarePosition(i, out x, out y);
			createSquare.Setup(i, x, y);
			// とりあえず壁地形を設定
			createSquare.SetTerrain(eTerrain.Wall);
		}
		// 部屋リストの初期化
		int roomCount = AREA_DEVIDE_COUNT + 1;
		_roomDataList = new List<RoomData>(roomCount);
		_unuseRoomDataList = new List<RoomData>(roomCount);
		// 部屋を未使用状態で追加
		for (int i = 0; i < roomCount; i++) {
			_unuseRoomDataList.Add(new RoomData());
		}
	}

	/// <summary>
	/// IDから2次元座標に変換
	/// </summary>
	/// <param name="ID"></param>
	/// <param name="x"></param>
	/// <param name="y"></param>
	private void GetSquarePosition(int ID, out int x, out int y) {
		x = ID % MAP_SQUARE_WIDTH_COUNT;
		y = ID / MAP_SQUARE_WIDTH_COUNT;
	}

	/// <summary>
	/// 2次元座標からIDに変換
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <returns></returns>
	private int GetSquareID(int x, int y) {
		// マップの範囲から外れた座標は不正値を返す
		if (x < 0 || x >= MAP_SQUARE_WIDTH_COUNT ||
			y < 0 || y >= MAP_SQUARE_HEIGHT_COUNT) return -1;

		return y * MAP_SQUARE_WIDTH_COUNT + x;
	}

	/// <summary>
	/// ID指定のマスオブジェクト取得
	/// </summary>
	/// <param name="ID"></param>
	/// <returns></returns>
	public MapSquareObject GetSquareObject(int ID) {
		// IDがリストに対して無効なインデクスならnullを返す
		if (!IsEnableIndex(_squareObjectList, ID)) return null;

		return _squareObjectList[ID];
	}

	/// <summary>
	/// ID指定のマス情報取得
	/// </summary>
	/// <param name="ID"></param>
	/// <returns></returns>
	public MapSquareData GetSquareData(int ID) {
		if (!IsEnableIndex(_squareDataList, ID)) return null;

		return _squareDataList[ID];
	}

	/// <summary>
	/// 座標指定のマス情報取得
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <returns></returns>
	public MapSquareData GetSquareData(int x, int y) {
		return GetSquareData(GetSquareID(x, y));
	}

	/// <summary>
	/// 全てのマスに指定処理実行
	/// </summary>
	/// <param name="action"></param>
	public void ExecuteAllSquare(System.Action<MapSquareData> action) {
		if (action == null || IsEmpty(_squareDataList)) return;

		for (int i = 0, max = _squareDataList.Count; i < max; i++) {
			if (_squareDataList[i] == null) continue;

			action(_squareDataList[i]);
		}

	}

	/// <summary>
	/// 部屋情報追加
	/// </summary>
	/// <param name="roomSquareList"></param>
	public void AddRoom(List<int> roomSquareList) {
		// 使用可能な部屋情報を取得
		RoomData addRoom = GetUsableRoomData();
		// 使用リストに追加
		addRoom.Setup(_roomDataList.Count, roomSquareList);
		_roomDataList.Add(addRoom);
	}

	/// <summary>
	/// 使用可能な部屋情報取得
	/// </summary>
	/// <returns></returns>
	private RoomData GetUsableRoomData() {
		// 未使用リストが空なら新たに生成
		if (IsEmpty(_unuseRoomDataList)) return new RoomData();
		// 未使用リストが空でなければ要素0番を返す
		RoomData result = _unuseRoomDataList[0];
		_unuseRoomDataList.RemoveAt(0);
		return result;
	}

	/// <summary>
	/// 全ての部屋情報の削除
	/// </summary>
	public void RemoveAllRoom() {
		if (IsEmpty(_roomDataList)) return;

		for (int i = 0, max = _roomDataList.Count; i < max; i++) {
			RoomData removeRoom = _roomDataList[i];
			removeRoom.Teardown();
			// 未使用リストに追加
			_unuseRoomDataList.Add(removeRoom);
		}
		_roomDataList.Clear();
	}

}
