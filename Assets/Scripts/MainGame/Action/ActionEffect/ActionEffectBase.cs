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

public abstract class ActionEffectBase {

	/// <summary>
	/// 効果の実行処理
	/// </summary>
	/// <returns></returns>
	public abstract UniTask Execute(
		CharacterBase sourceCharacter,
		Entity_ActionEffectData.Param effectMaster,
		ActionRangeBase range);

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
