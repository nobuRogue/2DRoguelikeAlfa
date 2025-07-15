/**
 * @file ActionEffect002_RecoveryStamina.cs
 * @brief 満腹度回復効果
 * @author yao
 * @date 2025/7/15
 */

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static CharacterUtility;
using static RogueLogUtility;

public class ActionEffect002_RecoveryStamina : ActionEffectBase {
	// マスターデータのパラメータのインデクスが何を表すか
	private enum eParamIndex {
		RecoveryValue,  // 回復量
	}
	// 満腹度回復のログメッセージID
	private const int _RECOVERY_LOG_ID = 14002;

	/// <summary>
	/// 満腹度回復効果実行
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
		// 全ての対象に満腹度回復
		List<int> targetList = range.targetList;
		for (int i = 0, max = targetList.Count; i < max; i++) {
			CharacterBase target = GetCharacterData(targetList[i]);
			if (target == null) continue;
			// 満腹度回復
			target.AddStamina(recoveryValue * 100);
			// ログ表示
			AddLog(string.Format(_RECOVERY_LOG_ID.ToMessage(), target.GetName(), recoveryValue));
		}
		// 適当に待つ
		await UniTask.Delay(500);
	}

}
