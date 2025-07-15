/**
 * @file MenuItemListItem.cs
 * @brief アイテムリストの項目クラス
 * @author yao
 * @date 2025/7/10
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using static ItemUtility;
using static GameConst;

public class ItemListItem : ListItem {
	// アイテムのアイコン画像
	[SerializeField]
	private Image _itemIconImage = null;
	// アイテム名テキスト
	[SerializeField]
	private TextMeshProUGUI _itemNameText = null;
	// 装備アイコン
	[SerializeField]
	private GameObject _equipIcon = null;

	// アイテムID
	public int itemID { get; private set; } = -1;

	/// <summary>
	/// 使用前の準備
	/// </summary>
	public void Setup(int setItemID, bool isEquip) {
		itemID = setItemID;
		// アイテムアイコンの設定
		ItemBase itemData = GetItemData(itemID);
		_itemIconImage.sprite = Resources.LoadAll<Sprite>(ITEM_SPRITE_FILE_NAME)[(int)itemData.GetCategory()];
		// アイテム名の設定
		_itemNameText.text = itemData.GetName();
		// 装備アイコンの設定
		_equipIcon.SetActive(isEquip);
	}

}
