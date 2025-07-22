/**
 * @file ActionEffect005_LotItem.cs
 * @brief 対象の所持アイテムを全て腐らせる
 * @author yao
 * @date 2025/7/22
 */

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static RogueLogUtility;
using static ItemMasterUtility;
using static ItemUtility;
using static CharacterUtility;
using static CommonModule;

public class ActionEffect005_LotItem : ActionEffectBase {
	// アイテムが腐ったときのログメッセージID
	private const int _LOT_ITEM_LOG_ID = 14004;

	public override async UniTask Execute(
		CharacterBase sourceCharacter,
		Entity_ActionEffectData.Param effectMaster,
		ActionRangeBase range) {
		List<int> targetIDList = range.targetList;
		for (int i = 0, max = targetIDList.Count; i < max; i++) {
			CharacterBase target = GetCharacterData(targetIDList[i]);
			if (target == null) continue;
			// 所持アイテムを全て腐らせる
			LotListItem(target.possessItemList);
		}
		// 適当に待つ
		await UniTask.Delay(500);
	}

	/// <summary>
	/// リストのIDのアイテムを全て腐らせる
	/// </summary>
	/// <param name="itemIDList"></param>
	private void LotListItem(List<int> itemIDList) {
		if (IsEmpty(itemIDList)) return;

		for (int i = 0, max = itemIDList.Count; i < max; i++) {
			// アイテムデータ取得
			ItemBase itemData = GetItemData(itemIDList[i]);
			if (itemData == null) continue;
			// アイテムマスター取得
			var itemMaster = GetItemMaster(itemData.masterID);
			if (itemMaster == null) continue;

			int lotID = itemMaster.lotID;
			if (lotID < 0) continue;
			// 腐る前のアイテム名をキャッシュ
			string beforeName = itemData.GetName();
			// アイテムを変化させる
			itemData.ChangeMasterID(lotID);
			// ログ表示
			AddLog(string.Format(_LOT_ITEM_LOG_ID.ToMessage(), beforeName, itemData.GetName()));
		}
	}
}
