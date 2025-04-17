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
}
