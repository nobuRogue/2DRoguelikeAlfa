/**
 * @file AcceptItemList.cs
 * @brief アイテムリストの受付処理
 * @author yao
 * @date 2025/7/15
 */

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static ActionManager;
using static ItemUtility;
using static CharacterUtility;
using static MenuList;

public class AcceptItemList {
	// アイテムリスト用コールバック集
	private MenuListCallbackFormat _itemListCallbackFormat = null;
	// 選択したアイテムのID
	private int _decideItemID = -1;

	public AcceptItemList() {
		_itemListCallbackFormat = new MenuListCallbackFormat();
		_itemListCallbackFormat.OnDecide = SetDecideItemID;// 決定時の処理
		_itemListCallbackFormat.OnCancel = EndAcceptItemList;// キャンセル時の処理
	}

	/// <summary>
	/// 選択した項目のアイテムIDをキャッシュ
	/// </summary>
	/// <param name="currentItem"></param>
	/// <returns></returns>
	private async UniTask<bool> SetDecideItemID(MenuListItem currentItem) {
		var itemListItem = currentItem as MenuItemListItem;
		if (itemListItem == null) return true;
		// 選択したアイテムリスト項目のアイテムIDを取得
		_decideItemID = itemListItem.itemID;
		await UniTask.CompletedTask;
		return false;
	}

	/// <summary>
	/// アイテムリストでキャンセルされた際の処理
	/// </summary>
	/// <param name="currentItem"></param>
	/// <returns></returns>
	private async UniTask<bool> EndAcceptItemList(MenuListItem currentItem) {
		await UniTask.CompletedTask;
		return false;
	}

	/// <summary>
	/// アイテムリストの受付
	/// </summary>
	/// <returns></returns>
	public async UniTask<bool> Accept() {
		// アイテムリストの表示、入力受付
		var itemList = MenuManager.instance.Get<MenuItemList>();
		CharacterBase player = GetPlayer();
		await itemList.Setup(player.possessItemList, _itemListCallbackFormat);
		await itemList.Open();
		await itemList.AcceptInput();
		await itemList.Close();
		// 選択結果の処理
		return await ProcessItemListResult(player);
	}

	/// <summary>
	/// アイテムリスト選択の結果を処理する
	/// </summary>
	/// <returns></returns>
	private async UniTask<bool> ProcessItemListResult(CharacterBase useItemCharacter) {
		// アイテムリストのキャンセル判定
		if (_decideItemID < 0) return false;
		// 選択したアイテムの使用効果を実行
		ItemBase itemData = GetItemData(_decideItemID);
		_decideItemID = -1;
		if (itemData == null) return false;
		// アクションマネージャーからアクション実行
		await UseItem(useItemCharacter, itemData);
		// アイテムを消す
		RemoveItem(itemData);
		return true;
	}

}
