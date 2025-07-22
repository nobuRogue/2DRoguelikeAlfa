/**
 * @file ActionEffectBase.cs
 * @brief 行動の効果の基底
 * @author yao
 * @date 2025/6/12
 */

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static RogueLogUtility;

public abstract class ActionEffectBase {
	// ダメージを与えるログメッセージのID
	private const int _DAMAGE_LOG_ID = 14000;

	/// <summary>
	/// 効果の実行処理
	/// </summary>
	/// <returns></returns>
	public abstract UniTask Execute(
		CharacterBase sourceCharacter,
		Entity_ActionEffectData.Param effectMaster,
		ActionRangeBase range);

	/// <summary>
	/// アニメーションをログを表示するダメージ付与
	/// </summary>
	/// <param name="damage"></param>
	/// <param name="target"></param>
	/// <returns></returns>
	protected async UniTask ExecuteDamage(int damage, CharacterBase target) {
		if (target == null) return;
		// 対象の被ダメージモーション
		target.SetAnimation(eCharacterAnimation.Damage);
		// ログの追加
		AddLog(string.Format(_DAMAGE_LOG_ID.ToMessage(), target.GetName(), damage));
		// 被ダメージモーションの終了待ち
		while (target.GetCurrentAnimation() == eCharacterAnimation.Damage) {
			await UniTask.DelayFrame(1);
		}
		AddDamage(damage, target);
	}

	/// <summary>
	/// 死亡判定付きのダメージ付与
	/// </summary>
	/// <param name="gamageValue"></param>
	/// <param name="targetCharacter"></param>
	protected void AddDamage(int gamageValue, CharacterBase targetCharacter) {
		if (targetCharacter == null) return;
		// ダメージの付与
		targetCharacter.RemoveHP(gamageValue);
		// 死亡の判定
		if (!targetCharacter.isDead) return;
		// 死亡処理
		targetCharacter.Dead();
	}

}
