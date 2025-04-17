/**
 * @file MapCreater.cs
 * @brief ランダムマップ生成
 * @author yao
 * @date 2025/4/17
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

		// エリアを分割する

		// 部屋を置く

		// 部屋を繋げる

		// 階段を置く

	}

	private void CreateFirstArea() {
		// マップを全て壁で埋める、ラムダ式
		MapSquareUtility.ExecuteAllSquare(squareData => squareData.SetTerrain(eTerrain.Wall));
		MapSquareUtility.ExecuteAllSquare(SetWall);
		// 最初のエリア生成

	}

	/// <summary>
	/// 地形を壁にする
	/// </summary>
	/// <param name="squareData"></param>
	private void SetWall(MapSquareData squareData) {
		squareData.SetTerrain(eTerrain.Wall);
	}

}
