/**
 * @file ActionRangeBase.cs
 * @brief 行動の射程の基底
 * @author yao
 * @date 2025/6/12
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionRangeBase {
	// 射程の対象になるキャラクター
	public List<int> targetList = null;

	/// <summary>
	/// 対象取得の実行処理
	/// </summary>
	public abstract void Execute(CharacterBase sourceCharacter);

	/// <summary>
	/// 射程が使用可能か（AI用）
	/// </summary>
	/// <param name="sourceCharacter"></param>
	/// <param name="dir">使用可能な向き</param>
	/// <returns></returns>
	public virtual bool CanUse(CharacterBase sourceCharacter, ref eDirectionEight dir) {
		return true;
	}
}
