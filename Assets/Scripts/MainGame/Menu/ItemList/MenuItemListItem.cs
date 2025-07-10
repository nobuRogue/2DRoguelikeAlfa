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

public class MenuItemListItem : MenuListItem {
	// アイテムのアイコン画像
	[SerializeField]
	private Image _itemIconImage = null;
	// アイテム名テキスト
	[SerializeField]
	private TextMeshProUGUI _itemNameText = null;

}
