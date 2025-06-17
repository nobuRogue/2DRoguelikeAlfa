/**
 * @file ActionMasterUtility.cs
 * @brief 行動のマスターデータ実行処理
 * @author yao
 * @date 2025/6/17
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionMasterUtility {

	/// <summary>
	/// ID指定の行動マスターデータ取得
	/// </summary>
	/// <param name="masterID"></param>
	/// <returns></returns>
	public static Entity_ActionData.Param GetActionMaster(int masterID) {
		var actionMasterList = MasterDataManager.actionData[0];
		for (int i = 0, max = actionMasterList.Count; i < max; i++) {
			if (actionMasterList[i].ID != masterID) continue;

			return actionMasterList[i];
		}
		return null;
	}

	/// <summary>
	/// ID指定の行動効果マスターデータ取得
	/// </summary>
	/// <param name="effectID"></param>
	/// <returns></returns>
	public static Entity_ActionEffectData.Param GetActionEffectMaster(int effectID) {
		var effectMasterList = MasterDataManager.effectData[0];
		for (int i = 0, max = effectMasterList.Count; i < max; i++) {
			if (effectMasterList[i].ID != effectID) continue;

			return effectMasterList[i];
		}
		return null;
	}

}
