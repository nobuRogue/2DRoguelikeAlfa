/**
 * @file MapSquareUtility.cs
 * @brief マス関連実行処理
 * @author yao
 * @date 2025/4/17
 */

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

using static CommonModule;

public class MapSquareUtility {

	/// <summary>
	/// ID指定のマス情報取得
	/// </summary>
	/// <param name="ID"></param>
	/// <returns></returns>
	public static MapSquareData GetSquareData(int ID) {
		return MapSquareManager.instance.GetSquareData(ID);
	}
	/// <summary>
	/// 座標指定のマス情報取得
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <returns></returns>
	public static MapSquareData GetSquareData(int x, int y) {
		return MapSquareManager.instance.GetSquareData(x, y);
	}

	/// <summary>
	/// 指定方向に隣接した座標のマスを取得
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <param name="dir"></param>
	/// <returns></returns>
	public static MapSquareData GetToDirSquare(int x, int y, eDirectionFour dir) {
		return MapSquareManager.instance.GetToDirSquare(x, y, dir);
	}

	/// <summary>
	/// 指定方向に隣接した座標のマスを取得
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <param name="dir"></param>
	/// <returns></returns>
	public static MapSquareData GetToDirSquare(int x, int y, eDirectionEight dir) {
		return MapSquareManager.instance.GetToDirSquare(x, y, dir);
	}

	/// <summary>
	/// 全てのマスに指定処理実行
	/// </summary>
	/// <param name="action"></param>
	public static void ExecuteAllSquare(System.Action<MapSquareData> action) {
		MapSquareManager.instance.ExecuteAllSquare(action);
	}

	/// <summary>
	/// 部屋情報追加
	/// </summary>
	/// <param name="roomSquareList"></param>
	public static void AddRoom(List<int> roomSquareList) {
		MapSquareManager.instance.AddRoom(roomSquareList);
	}
	/// <summary>
	/// 全部屋情報削除
	/// </summary>
	public static void RemoveAllRoom() {
		MapSquareManager.instance.RemoveAllRoom();
	}

	/// <summary>
	/// ランダムな部屋情報取得
	/// </summary>
	/// <returns></returns>
	public static RoomData GetRandomRoom() {
		return MapSquareManager.instance.GetRandomRoom();
	}

	/// <summary>
	/// 移動可否判定
	/// </summary>
	/// <param name="startX"></param>
	/// <param name="startY"></param>
	/// <param name="moveSquare"></param>
	/// <param name="dir"></param>
	/// <returns></returns>
	public static bool CanMove(int startX, int startY, MapSquareData moveSquare, eDirectionEight dir) {
		// 移動可能な地形かつキャラクターが居なければ移動可能
		return CanMoveTerrain(startX, startY, moveSquare, dir) && !moveSquare.existCharacter;
	}

	/// <summary>
	/// 地形のみの移動可否判定
	/// </summary>
	/// <param name="startX"></param>
	/// <param name="startY"></param>
	/// <param name="moveSquare"></param>
	/// <param name="dir"></param>
	/// <returns></returns>
	public static bool CanMoveTerrain(int startX, int startY, MapSquareData moveSquare, eDirectionEight dir) {
		// 移動先の地形判定
		if (moveSquare == null ||
			moveSquare.terrain == eTerrain.Wall) return false;
		// 斜め移動でなければ終わり
		if (!dir.IsSlant()) return true;
		// 斜め移動なら、方向を分割し各方向のマスの地形を判定
		eDirectionFour[] separateDir = dir.Separate();
		for (int i = 0, max = separateDir.Length; i < max; i++) {
			// 分割した方向の隣接マスを取得
			MapSquareData checkSquare = GetToDirSquare(startX, startY, separateDir[i]);
			if (checkSquare == null ||
				checkSquare.terrain == eTerrain.Wall) return false;
		}
		return true;
	}

	/// <summary>
	/// キャラクターの視界マスリスト取得
	/// </summary>
	public static void GetVisbleArea(ref List<int> visibleArea, MapSquareData sourceSquare) {
		InitializeList(ref visibleArea);
		if (sourceSquare == null) return;
		// 起点の周囲8マスを追加


		// 周囲8マスに部屋があればキャッシュ


		// キャッシュされた部屋マスの部屋全てのマスを追加

	}

	/// <summary>
	/// 等チェビシェフ距離のマスを全て取得
	/// </summary>
	/// <param name="result"></param>
	/// <param name="sourceSquare"></param>
	/// <param name="distance"></param>
	public static void GetChebyshevAroundSquare(ref List<int> result, MapSquareData sourceSquare, int distance = 1) {
		InitializeList(ref result, distance * 8);
		if (sourceSquare == null) return;

		int countMax = distance * 2;
		int sourceX = sourceSquare.posX, sourceY = sourceSquare.posY;
		for (int count = 0; count < countMax; count++) {
			MapSquareData 
			targetSquare = GetSquareData(sourceX - distance + count, sourceY - distance);
			if (targetSquare != null) result.Add(targetSquare.ID);

			targetSquare = GetSquareData(sourceX + distance, sourceY - distance + count);
			if (targetSquare != null) result.Add(targetSquare.ID);

			targetSquare = GetSquareData(sourceX + distance - count, sourceY + distance);
			if (targetSquare != null) result.Add(targetSquare.ID);

			targetSquare = GetSquareData(sourceX - distance, sourceY + distance - count);
			if (targetSquare != null) result.Add(targetSquare.ID);

		}
	}

}
