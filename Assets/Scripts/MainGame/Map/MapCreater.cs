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
using static CommonModule;
using static GameConst;
using Cysharp.Threading.Tasks;

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
		CreateAllRoom();
		// 部屋を繋げる
		ConnectAllRoom();
		// 階段を置く
		CreateStair();
	}

	/// <summary>
	/// 最初のエリア生成
	/// </summary>
	private static void CreateFirstArea() {
		// 部屋情報のクリア
		RemoveAllRoom();
		_areaList = new List<AreaData>();
		_devideLineList = new List<int>();
		// マップを全て壁で埋める、ラムダ式も使えるが使わない
		ExecuteAllSquare(SetFirstWall);
		// 最初のエリア生成
		_areaList.Add(new AreaData(2, 2, MAP_SQUARE_WIDTH_COUNT - 4, MAP_SQUARE_HEIGHT_COUNT - 4));
	}

	/// <summary>
	/// 地形を壁にする
	/// </summary>
	/// <param name="squareData"></param>
	private static void SetFirstWall(MapSquareData squareData) {
		// 地形を壁にする
		squareData.SetTerrain(eTerrain.Wall);
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

	/// <summary>
	/// 各エリアに部屋を生成
	/// </summary>
	private static void CreateAllRoom() {
		for (int i = 0, max = _areaList.Count; i < max; i++) {
			// 部屋の生成
			CreateRoom(_areaList[i]);
		}
	}

	/// <summary>
	/// 指定エリアに部屋を生成
	/// </summary>
	/// <param name="area"></param>
	private static void CreateRoom(AreaData area) {
		if (area == null) return;
		// 部屋のサイズ決定
		int roomWidth = Random.Range(MIN_ROOM_SIZE, area.width - 1);
		int roomHeight = Random.Range(MIN_ROOM_SIZE, area.height - 1);
		// 部屋の生成位置決定
		int xRandomRange = area.width - roomWidth - 1;
		int yRandomRange = area.height - roomHeight - 1;
		int roomStartX = area.startX + Random.Range(0, xRandomRange) + 1;
		int roomStartY = area.startY + Random.Range(0, yRandomRange) + 1;
		// 部屋の生成
		List<int> roomIDList = new List<int>(roomWidth * roomHeight);
		for (int y = 0; y < roomHeight; y++) {
			for (int x = 0; x < roomWidth; x++) {
				MapSquareData roomSquare = GetSquareData(roomStartX + x, roomStartY + y);
				if (roomSquare == null) continue;
				// マスを部屋地形に変更
				roomSquare.SetTerrain(eTerrain.Room);
				roomIDList.Add(roomSquare.ID);
			}
		}
		AddRoom(roomIDList);
	}

	/// <summary>
	/// 全てに部屋を通路で連結
	/// </summary>
	private static void ConnectAllRoom() {
		eDirectionFour digDir = (eDirectionFour)Random.Range(0, (int)eDirectionFour.Max);
		for (int i = 0, max = _areaList.Count - 1; i < max; i++) {
			// エリア1を分割線まで掘る
			AreaData area1 = _areaList[i];
			MapSquareData startSquare = DigToDevideLine(area1, digDir);
			digDir = (eDirectionFour)Random.Range(0, (int)eDirectionFour.Max);
			// エリア2を分割線まで掘る
			AreaData area2 = _areaList[i + 1];
			MapSquareData goalSquare = DigToDevideLine(area2, digDir);

			int dirIndex = (int)digDir + Random.Range(1, (int)eDirectionFour.Max);
			if (dirIndex >= (int)eDirectionFour.Max) dirIndex -= (int)eDirectionFour.Max;

			digDir = (eDirectionFour)dirIndex;
			// 分割線内で繋げる
			ConnetInDevideLine(startSquare.ID, goalSquare.ID);
		}
	}

	private static void ConnetInDevideLine(int startID, int goalID) {
		List<ManhattanMoveData> route = RouteSearcher.RouteSearchManhattan(startID, goalID, IsDivideLine);
		//List<ManhattanMoveData> route = RouteSearcher.RouteSearchManhattan(startSquare.ID, goalSquare.ID,
		//	(square, dir, distance) => _devideLineList.Exists(squareID => squareID == square.ID));
		for (int i = 0, max = route.Count; i < max; i++) {
			MapSquareData moveSquare = GetSquareData(route[i].targetSquareID);
			if (moveSquare == null) continue;

			moveSquare.SetTerrain(eTerrain.Passage);
		}
	}

	/// <summary>
	/// マスが分割線リストに含まれているか
	/// </summary>
	/// <param name="square"></param>
	/// <param name="dir"></param>
	/// <param name="distance"></param>
	/// <returns></returns>
	private static bool IsDivideLine(MapSquareData square, eDirectionFour dir, int distance) {
		return _devideLineList.Exists(squareID => squareID == square.ID);
	}

	/// <summary>
	/// 部屋からエリア分割線まで掘る
	/// </summary>
	/// <param name="area"></param>
	/// <param name="dir"></param>
	/// <returns></returns>
	private static MapSquareData DigToDevideLine(AreaData area, eDirectionFour dir) {
		// 掘削開始マスの決定
		eDirectionFour reverseDir = dir.ReverseDir();
		List<MapSquareData> targetList = new List<MapSquareData>(16);
		int startX = area.startX;
		int startY = area.startY;
		for (int y = 0, height = area.height; y < height; y++) {
			for (int x = 0, width = area.width; x < width; x++) {
				// 壁地形でかつ、掘削方向と反対のマスが部屋地形のマスを集約
				MapSquareData square = GetSquareData(startX + x, startY + y);
				if (square == null ||
					square.terrain != eTerrain.Wall) continue;
				// squareから掘削方向の反対の隣接マスを取得
				MapSquareData toDirSquare = GetToDirSquare(square.posX, square.posY, reverseDir);
				if (toDirSquare == null ||
					toDirSquare.terrain != eTerrain.Room) continue;

				targetList.Add(square);
			}
		}
		if (IsEmpty(targetList)) return null;

		MapSquareData currentSquare = targetList[Random.Range(0, targetList.Count)];
		// 分割線まで掘る
		while (true) {
			currentSquare.SetTerrain(eTerrain.Passage);
			// 分割線リストに含まれていたら終了
			if (_devideLineList.Exists(squareID => squareID == currentSquare.ID)) break;

			currentSquare = GetToDirSquare(currentSquare.posX, currentSquare.posY, dir);
		}
		return currentSquare;
	}

	/// <summary>
	/// 階段マスの生成
	/// </summary>
	private static void CreateStair() {
		// ランダムな部屋の取得
		RoomData targetRoom = GetRandomRoom();
		if (targetRoom == null) return;
		// 部屋内のランダムな1マスを階段地形にする
		List<int> roomSquareList = targetRoom.squareIDList;
		if (IsEmpty(roomSquareList)) return;

		int targetSquareID = roomSquareList[Random.Range(0, roomSquareList.Count)];
		GetSquareData(targetSquareID)?.SetTerrain(eTerrain.Stair);
	}


}
