/**
 * @file ActionEffect006_Replace.cs
 * @brief 場所替え効果
 * @author yao
 * @date 2025/7/24
 */

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static CommonModule;
using static CharacterUtility;
using static MapSquareUtility;

public class ActionEffect006_Replace : ActionEffectBase {
	/// <summary>
	/// 場所替え効果実行
	/// </summary>
	/// <param name="sourceCharacter"></param>
	/// <param name="effectMaster"></param>
	/// <param name="range"></param>
	/// <returns></returns>
	public override async UniTask Execute(
		CharacterBase sourceCharacter,
		Entity_ActionEffectData.Param effectMaster,
		ActionRangeBase range) {
		if (IsEmpty(range.targetList)) return;
		// rangeから１体目の対象を取得
		CharacterBase target = GetCharacterData(range.targetList[0]);
		if (target == null) return;
		// 対象と行動者の位置を入れ替える
		MapSquareData sourceSquare = GetSquareData(sourceCharacter.posX, sourceCharacter.posY);
		MapSquareData targetSquare = GetSquareData(target.posX, target.posY);
		// 対象をマスから取り除く
		target.RemoveSquare();
		// 行動者を対象のマスに置く
		sourceCharacter.SetSquare(targetSquare);
		// 対象を行動者のマスに置く
		target.SetSquare(sourceSquare);
		// 適当に待つ
		await UniTask.Delay(500);
	}
}
