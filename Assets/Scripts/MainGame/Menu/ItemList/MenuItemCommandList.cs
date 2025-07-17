/**
 * @file MenuItemCommandList.cs
 * @brief アイテムコマンドリストメニュー
 * @author yao
 * @date 2025/7/17
 */

using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

using static ItemUtility;

public class MenuItemCommandList : MenuList {
	[SerializeField]
	private Transform _listRoot = null;


	/// <summary>
	/// リスト項目とコールバックの設定
	/// </summary>
	/// <param name="itemID"></param>
	/// <param name="setFormat"></param>
	/// <returns></returns>
	public async Task Setup(int itemID, MenuListCallbackFormat setFormat, Vector3 position) {
		_listRoot.position = position;
		// コールバックの設定
		SetCallbackFortmat(setFormat);
		// 全ての項目削除
		RemoveAllItem();
		await SetIndex(-1);
		// アイテムカテゴリから表示するコマンド項目を生成
		AddItemCommand(GetItemData(itemID).GetCategory());
		// 0番目の項目を選択
		await SetIndex(0);
	}

	/// <summary>
	/// アイテムカテゴリからアイテムコマンドリスト項目生成
	/// </summary>
	/// <param name="itemCategory"></param>
	private void AddItemCommand(eItemCategory itemCategory) {
		ItemCommandListItem createItem;
		switch (itemCategory) {
			case eItemCategory.Potion:
			case eItemCategory.Food:
			case eItemCategory.Wand:
			case eItemCategory.Scroll:
				// 「使う」コマンド項目の追加
				createItem = AddListItem() as ItemCommandListItem;
				createItem.Setup(eItemCommand.Use);
				break;
			case eItemCategory.Bag:
				break;
			case eItemCategory.Throwing:
				break;
			case eItemCategory.Weapon:
			case eItemCategory.Armor:
				// TODO:プレイヤーの装備品なら「外す」
				// そうでなければ「装備」コマンド項目の追加
				createItem = AddListItem() as ItemCommandListItem;
				createItem.Setup(eItemCommand.SetEquip);
				break;
		}
		// 「置く」コマンド項目の追加
		createItem = AddListItem() as ItemCommandListItem;
		createItem.Setup(eItemCommand.Puton);
	}

}
