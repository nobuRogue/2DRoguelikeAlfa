/**
 * @file ActionEffect003_FixDamage.cs
 * @brief 固定ダメージ付与
 * @author yao
 * @date 2025/7/22
 */

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static CharacterUtility;
using static CommonModule;

public class ActionEffect003_FixDamage : ActionEffectBase {
	private enum eParamIndex {
		DamageValue,    // ダメージ値
	}

	public override async UniTask Execute(
		CharacterBase sourceCharacter,
		Entity_ActionEffectData.Param effectMaster,
		ActionRangeBase range) {
		// ダメージ量取得
		int damageValue = effectMaster.param[(int)eParamIndex.DamageValue];
		// 対象ごとにダメージの付与
		List<int> targetList = range.targetList;
		int targetCount = targetList.Count;
		List<UniTask> taskList = new List<UniTask>(targetCount);
		for (int i = 0; i < targetCount; i++) {
			CharacterBase target = GetCharacterData(targetList[i]);
			if (target == null) continue;

			taskList.Add(ExecuteDamage(damageValue, target));
		}
		// 終了待ち
		await WaitTask(taskList);
	}

}
