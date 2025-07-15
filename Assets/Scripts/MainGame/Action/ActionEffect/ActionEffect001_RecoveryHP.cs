/**
 * @file ActionEffect001_RecoveryHP.cs
 * @brief HP回復効果
 * @author yao
 * @date 2025/7/15
 */

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static CharacterUtility;
using static RogueLogUtility;

public class ActionEffect001_RecoveryHP : ActionEffectBase {
	// マスターデータのパラメータのインデクスが何を表すか
	private enum eParamIndex {
		RecoveryValue,  // 回復量
	}
	// HP回復のログメッセージID
	private const int _RECOVERY_LOG_ID = 14001;

	/// <summary>
	/// HP回復効果実行
	/// </summary>
	/// <param name="sourceCharacter"></param>
	/// <param name="effectMaster"></param>
	/// <param name="range"></param>
	/// <returns></returns>
	public override async UniTask Execute(
		CharacterBase sourceCharacter,
		Entity_ActionEffectData.Param effectMaster,
		ActionRangeBase range) {
		// 回復量取得
		int recoveryValue = effectMaster.param[(int)eParamIndex.RecoveryValue];
		// 全ての対象にHP回復
		List<int> targetList = range.targetList;
		for (int i = 0, max = targetList.Count; i < max; i++) {
			CharacterBase target = GetCharacterData(targetList[i]);
			if (target == null) continue;
			// HP回復
			target.AddHP(recoveryValue);
			// ログ表示
			AddLog(string.Format(_RECOVERY_LOG_ID.ToMessage(), target.GetName(), recoveryValue));
		}
		// 適当に待つ
		await UniTask.Delay(500);
	}

}
