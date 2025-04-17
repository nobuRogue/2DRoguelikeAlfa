/**
 * @file MapSquareUtility.cs
 * @brief マス関連実行処理
 * @author yao
 * @date 2025/4/17
 */

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	/// 全てのマスに指定処理実行
	/// </summary>
	/// <param name="action"></param>
	public static void ExecuteAllSquare(System.Action<MapSquareData> action) {
		MapSquareManager.instance.ExecuteAllSquare(action);
	}

}
