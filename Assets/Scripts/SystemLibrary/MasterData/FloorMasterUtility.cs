/**
 * @file FloorMasterUtility.cs
 * @brief フロアマスターデータの実行処理
 * @author yao
 * @date 2025/5/20
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static CommonModule;

public class FloorMasterUtility {

	/// <summary>
	/// 階数指定のフロアマスター取得
	/// </summary>
	/// <param name="floorCount"></param>
	/// <returns></returns>
	public static Entity_FloorData.Param GetFloorMaster(int floorCount) {
		// フロアマスターデータ取得
		var floorMasterList = MasterDataManager.floorData[0];
		for (int i = 0, max = floorMasterList.Count; i < max; i++) {
			if (floorMasterList[i].floorCount != floorCount) continue;
			// フロア数が一致したら返す
			return floorMasterList[i];
		}
		return null;
	}

	/// <summary>
	/// 現在の階数のフロアマスター取得
	/// </summary>
	/// <returns></returns>
	public static Entity_FloorData.Param GetCurrentFloorMaster() {
		int currentFloorCount = UserDataHolder.currentData.floorCount;
		return GetFloorMaster(currentFloorCount);
	}

	/// <summary>
	/// エネミーテーブル取得
	/// </summary>
	/// <param name="ID"></param>
	/// <returns></returns>
	public static List<int> GetEnemyTable(int ID) {
		var enemyTableMasterList = MasterDataManager.enemyTableData[0];
		for (int i = 0, max = enemyTableMasterList.Count; i < max; i++) {
			if (enemyTableMasterList[i].ID != ID) continue;
			// 指定IDのデータが見つかったので-1を取り除いて返す
			return CreateEnableEnemyTable(enemyTableMasterList[i].enemyID);
		}
		return null;
	}

	/// <summary>
	/// 使用可能なエネミーテーブルを生成
	/// </summary>
	/// <returns></returns>
	private static List<int> CreateEnableEnemyTable(int[] origin) {
		if (IsEmpty(origin)) return null;

		int tableCount = origin.Length;
		List<int> result = new List<int>(tableCount);
		for (int i = 0; i < tableCount; i++) {
			if (origin[i] < 0) continue;

			result.Add(origin[i]);
		}
		return result;
	}

}
