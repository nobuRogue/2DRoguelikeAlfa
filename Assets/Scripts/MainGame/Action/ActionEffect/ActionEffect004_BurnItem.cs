/**
 * @file ActionEffect004_BurnItem.cs
 * @brief 対象の所持アイテムを全て焼く
 * @author yao
 * @date 2025/7/17
 */

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionEffect004_BurnItem : ActionEffectBase {
	public override async UniTask Execute(
		CharacterBase sourceCharacter,
		Entity_ActionEffectData.Param effectMaster,
		ActionRangeBase range) {
		// 対象は range.targetList;
		//CharacterBase target = null;
		//target.possessItemList
	}
}
