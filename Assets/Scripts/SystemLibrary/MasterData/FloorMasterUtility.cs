/**
 * @file FloorMasterUtility.cs
 * @brief フロアマスターデータの実行処理
 * @author yao
 * @date 2025/5/20
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorMasterUtility {

	/// <summary>
	/// 階数指定のフロアマスター取得
	/// </summary>
	/// <param name="floorCount"></param>
	/// <returns></returns>
	public static Entity_FloorData.Param GetFloorMaster(int floorCount) {
		List<Entity_FloorData.Param> floorMasterList = MasterDataManager.floorData[0];
		for (int i = 0, max = floorMasterList.Count; i < max; i++) {
			if (floorMasterList[i].floorCount != floorCount) continue;
			// フロア数が一致したら返す
			return floorMasterList[i];
		}
		return null;
	}

}
