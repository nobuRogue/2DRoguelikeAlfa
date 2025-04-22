/**
 * @file MapCreater.cs
 * @brief ランダムマップ生成
 * @author yao
 * @date 2025/4/17
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static MapSquareUtility;
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
		DevideAreaFixCount();
		// 部屋を置く

		// 部屋を繋げる

		// 階段を置く

	}

	/// <summary>
	/// 最初のエリア生成
	/// </summary>
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

	/// <summary>
	/// エリアを一定回数分割する
	/// </summary>
	private static void DevideAreaFixCount() {
		for (int i = 0; i < AREA_DEVIDE_COUNT; i++) {
			// 幅最大のエリアを取得
			AreaData maxSizeArea = GetMaxSizeArea(out int maxSize, out bool isVertical);
			// 取得したエリアが分割不可能なら終了
			if (maxSize < (MIN_ROOM_SIZE + 2) * 2 + 1) return;
			// 取得したエリアを分割
			DevideArea(maxSizeArea, isVertical);
		}
	}

	/// <summary>
	/// 最大幅のエリア取得
	/// </summary>
	/// <param name="maxSize"></param>
	/// <param name="isVertical"></param>
	/// <returns></returns>
	private static AreaData GetMaxSizeArea(out int maxSize, out bool isVertical) {
		maxSize = -1;
		isVertical = false;
		AreaData result = null;
		for (int i = 0, max = _areaList.Count; i < max; i++) {
			AreaData area = _areaList[i];
			// 横幅の確認
			if (area.width > maxSize) {
				maxSize = area.width;
				isVertical = false;
				result = area;
			}
			// 高さの確認
			if (area.height > maxSize) {
				maxSize = area.height;
				isVertical = true;
				result = area;
			}
		}
		return result;
	}

	/// <summary>
	/// エリアの分割
	/// </summary>
	/// <param name="devideArea"></param>
	/// <param name="isVertical"></param>
	private static void DevideArea(AreaData devideArea, bool isVertical) {
		if (isVertical) {
			// 水平方向に線を引いて縦に分割
			DevideAreaVertical(devideArea);
		} else {
			// 垂直方向に線を引いて横に分割
			DevideAreaHorizontal(devideArea);
		}
	}

	/// <summary>
	/// 水平方向に線を引いてエリアを縦に分割
	/// </summary>
	/// <param name="devideArea"></param>
	private static void DevideAreaVertical(AreaData devideArea) {
		// 分割位置の決定
		int randomMax = devideArea.height - (MIN_ROOM_SIZE + 2) * 2;
		int devidePos = Random.Range(0, randomMax);
		devidePos += MIN_ROOM_SIZE + 2 + devideArea.startY;
		// 新しいエリアの生成
		int newAreaHeight = devideArea.startY + devideArea.height - devidePos - 1;
		_areaList.Add(new AreaData(devideArea.startX, devidePos + 1, devideArea.width, newAreaHeight));
		// 既存エリアの修整
		devideArea.height = devidePos - devideArea.startY;
		// 分割線マスの追加
		for (int x = 0, max = devideArea.width; x < max; x++) {
			AddDevideLine(GetSquareData(devideArea.startX + x, devidePos));
		}
	}

	/// <summary>
	/// 垂直方向に線を引いてエリアを横に分割
	/// </summary>
	/// <param name="devideArea"></param>
	private static void DevideAreaHorizontal(AreaData devideArea) {
		// 分割位置の決定
		int randomMax = devideArea.width - (MIN_ROOM_SIZE + 2) * 2;
		int devidePos = Random.Range(0, randomMax);
		devidePos += MIN_ROOM_SIZE + 2 + devideArea.startX;
		// 新しいエリアの生成
		int newAreaWidth = devideArea.startX + devideArea.width - devidePos - 1;
		_areaList.Add(new AreaData(devidePos + 1, devideArea.startY, newAreaWidth, devideArea.height));
		// 既存エリアの修正
		devideArea.width = devidePos - devideArea.startX;
		// 分割線マスの追加
		for (int y = 0, max = devideArea.height; y < max; y++) {
			AddDevideLine(GetSquareData(devidePos, devideArea.startY + y));
		}
	}

}
