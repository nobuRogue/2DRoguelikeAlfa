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

public class ActionEffect000_Attack : ActionEffectBase {
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
		int sourceAttack = sourceCharacter.GetAttack() * effectMaster.param[0];
		sourceAttack /= 100;
		// 対象ごとにダメージ付与
		for (int i = 0, max = targetList.Count; i < max; i++) {
			taskList.Add(ExecuteAttackDamage(sourceAttack, GetCharacterData(targetList[i])));
		}
		// 攻撃アニメーションの終了待ち
		while (sourceCharacter.GetCurrentAnimation() == eCharacterAnimation.Attack) {
			await UniTask.DelayFrame(1);
		}
		// タスクの終了待ち
		await WaitTask(taskList);
	}

	private async UniTask ExecuteAttackDamage(int sourceAttack, CharacterBase targetCharacter) {
		if (targetCharacter == null) return;
		// 対象の被ダメージモーション
		targetCharacter.SetAnimation(eCharacterAnimation.Damage);
		// ダメージ計算
		// 基本ダメージ : 攻撃力×(15/16)^防御力
		int targetDefense = targetCharacter.GetDefense();
		int damage = (int)(sourceAttack * Mathf.Pow(15.0f / 16.0f, targetDefense));
		// ログの追加
		MenuManager.instance.Get<MenuRogueLog>().AddLog(damage + " のダメージ");
		// 被ダメージモーションの終了待ち
		while (targetCharacter.GetCurrentAnimation() == eCharacterAnimation.Damage) {
			await UniTask.DelayFrame(1);
		}
		AddDamage(damage, targetCharacter);
	}

}
