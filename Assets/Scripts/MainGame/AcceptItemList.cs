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
	// SEのID
	private const int _DECIDE_SE_ID = 12;
	private const int _CANCEL_SE_ID = 13;
	private const int _SORT_SE_ID = 12;

	public AcceptItemList() {
		_itemListCallbackFormat = new MenuListCallbackFormat();
		_itemListCallbackFormat.OnDecide = SetDecideItemID;// 決定時の処理
		_itemListCallbackFormat.OnCancel = EndAcceptItemList;// キャンセル時の処理
		_itemListCallbackFormat.FreeAccept = AcceptSortPlayerItem;// ソートの受付
	}

	/// <summary>
	/// 選択した項目のアイテムIDをキャッシュ
	/// </summary>
	/// <param name="currentItem"></param>
	/// <returns></returns>
	private async UniTask<bool> SetDecideItemID(ListItem currentItem) {
		var itemListItem = currentItem as ItemListItem;
		if (itemListItem == null) return true;
		// 選択したアイテムリスト項目のアイテムIDを取得
		UniTask task = SoundManager.instance.PlaySE(_DECIDE_SE_ID);
		_decideItemID = itemListItem.itemID;
		await UniTask.CompletedTask;
		return false;
	}

	/// <summary>
	/// アイテムリストでキャンセルされた際の処理
	/// </summary>
	/// <param name="currentItem"></param>
	/// <returns></returns>
	private async UniTask<bool> EndAcceptItemList(ListItem currentItem) {
		UniTask task = SoundManager.instance.PlaySE(_CANCEL_SE_ID);
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

	/// <summary>
	/// ソートの入力受付
	/// </summary>
	/// <param name="currentItem"></param>
	/// <returns></returns>
	private async UniTask<bool> AcceptSortPlayerItem(ListItem currentItem) {
		// キー入力の受付
		if (!Input.GetKeyDown(KeyCode.V)) return true;
		// プレイヤーのアイテムのソート
		UniTask task = SoundManager.instance.PlaySE(_SORT_SE_ID);
		CharacterBase player = GetPlayer();
		player.possessItemList.Sort(ItemSortMethod);
		// リストのセットアップ
		await MenuManager.instance.Get<MenuItemList>().Setup(player.possessItemList, _itemListCallbackFormat);
		return true;
	}

	/// <summary>
	/// ソート処理
	/// </summary>
	/// <param name="itemID_A"></param>
	/// <param name="itemID_B"></param>
	/// <returns></returns>
	private int ItemSortMethod(int itemID_A, int itemID_B) {
		ItemBase ItemA = GetItemData(itemID_A);
		ItemBase ItemB = GetItemData(itemID_B);
		return ItemA.masterID - ItemB.masterID;
	}

}
