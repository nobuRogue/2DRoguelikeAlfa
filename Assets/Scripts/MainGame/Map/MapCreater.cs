/**
 * @file MapCreater.cs
 * @brief ランダムマップ生成
 * @author yao
 * @date 2025/4/17
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static GameConst;

public class MapCreater {
	/// <summary>
	/// マップ生成時のエリア情報
	/// </summary>
	private class AreaData {
		// スタート位置
		public int startX = -1;
		public int startY = -1;
		// サイズ
		public int width = -1;
		public int height = -1;

		public AreaData(int setX, int setY, int setWidth, int setHeight) {
			startX = setX;
			startY = setY;
			width = setWidth;
			height = setHeight;
		}
	}
	// エリアのリスト
	private static List<AreaData> _areaList = null;
	// 分割線のマスIDリスト
	private static List<int> _devideLineList = null;

	public static void CreateMap() {
		// 最初のエリアの生成
		CreateFirstArea();
		// エリアを分割する

		// 部屋を置く

		// 部屋を繋げる

		// 階段を置く

	}

	private static void CreateFirstArea() {
		_areaList = new List<AreaData>();
		_devideLineList = new List<int>();
		// マップを全て壁で埋める、ラムダ式も使えるが使わない
		MapSquareUtility.ExecuteAllSquare(SetFirstWall);
		// 最初のエリア生成
		_areaList.Add(new AreaData(2, 2, MAP_SQUARE_WIDTH_COUNT - 4, MAP_SQUARE_HEIGHT_COUNT - 4));
	}

	/// <summary>
	/// 地形を壁にする
	/// </summary>
	/// <param name="squareData"></param>
	private static void SetFirstWall(MapSquareData squareData) {
		// 地形を壁にする
		squareData.SetTerrain(eTerrain.Wall, 0);
		// 最初のエリアの分割線なら分割線に加える
		int x = squareData.posX;
		int y = squareData.posY;
		if (x == 0 || x == MAP_SQUARE_WIDTH_COUNT - 1 ||
			y == 0 || y == MAP_SQUARE_HEIGHT_COUNT - 1) return;

		if (x != 1 && x != MAP_SQUARE_WIDTH_COUNT - 2 &&
			y != 1 && y != MAP_SQUARE_HEIGHT_COUNT - 2) return;
		// 分割線に追加
		AddDevideLine(squareData);
	}

	/// <summary>
	/// 分割線リストへ追加
	/// </summary>
	/// <param name="squareData"></param>
	private static void AddDevideLine(MapSquareData squareData) {
		squareData.SetTerrain(eTerrain.Wall, 2);
		_devideLineList.Add(squareData.ID);
	}


}
