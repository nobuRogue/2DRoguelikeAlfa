/**
 * @file ActionEffect000_Attack.cs
 * @brief 通常攻撃
 * @author yao
 * @date 2025/6/17
 */

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CharacterUtility;
using static CommonModule;
using static MessageMasterUtility;
using static RogueLogUtility;
using static UnityEngine.GraphicsBuffer;

public class ActionEffect000_Attack : ActionEffectBase {
	private enum eParamIndex {
		Percentage, // 攻撃力割合
	}

	/// <summary>
	/// 効果処理実行
	/// </summary>
	/// <param name="sourceCharacter"></param>
	/// <param name="effectMaster"></param>
	/// <param name="range"></param>
	/// <returns></returns>
	public override async UniTask Execute(
		CharacterBase sourceCharacter,
		Entity_ActionEffectData.Param effectMaster,
		ActionRangeBase range) {

		List<int> targetList = range.targetList;
		int targetCount = targetList.Count;
		List<UniTask> taskList = new List<UniTask>(targetCount);
		// 行動者の攻撃アニメーション再生
		sourceCharacter.SetAnimation(eCharacterAnimation.Attack);
		// 攻撃力の取得
		int sourceAttack = sourceCharacter.GetAttack() * effectMaster.param[(int)eParamIndex.Percentage];
		sourceAttack /= 100;
		// 対象ごとにダメージ付与
		for (int i = 0, max = targetList.Count; i < max; i++) {
			CharacterBase target = GetCharacterData(targetList[i]);
			if (target == null) continue;
			// ダメージ計算
			// 基本ダメージ : 攻撃力×(15/16)^防御力
			int targetDefense = target.GetDefense();
			int damage = (int)(sourceAttack * Mathf.Pow(15.0f / 16.0f, targetDefense));
			// ダメージ付与
			taskList.Add(ExecuteDamage(damage, target));
		}
		// 攻撃アニメーションの終了待ち
		while (sourceCharacter.GetCurrentAnimation() == eCharacterAnimation.Attack) {
			await UniTask.DelayFrame(1);
		}
		// タスクの終了待ち
		await WaitTask(taskList);
	}

}
