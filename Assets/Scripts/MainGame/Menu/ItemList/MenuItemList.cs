/**
 * @file MenuItemList.cs
 * @brief アイテムリストメニュー
 * @author yao
 * @date 2025/7/10
 */

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static CommonModule;

public class MenuItemList : MenuList {

	/// <summary>
	/// アイテムリストを開く前の準備
	/// </summary>
	public async UniTask Setup(List<int> itemIDList, MenuListCallbackFormat setFortmat) {
		// コールバックの設定
		SetCallbackFortmat(setFortmat);
		// 全ての項目削除
		RemoveAllItem();
		await SetIndex(-1);
		// アイテムリストが空なら終了
		if (IsEmpty(itemIDList)) return;
		// 項目の生成
		for (int i = 0, max = itemIDList.Count; i < max; i++) {
			var createItem = AddListItem() as ItemListItem;
			if (createItem == null) continue;

			createItem.Setup(itemIDList[i], false);
		}
		// 0番目の項目を選択
		await SetIndex(0);
	}

}
