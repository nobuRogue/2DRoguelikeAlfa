/**
 * @file ItemManager.cs
 * @brief アイテム管理
 * @author yao
 * @date 2025/7/3
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour {
	// 自身への参照
	public static ItemManager instance { get; private set; } = null;

	// 使用中キャラクターオブジェクトの親オブジェクト
	[SerializeField]
	private Transform _useObjectRoot = null;
	// 未使用キャラクターオブジェクトの親オブジェクト
	[SerializeField]
	private Transform _unuseObjectRoot = null;
	/// キャラクターオブジェクトのオリジナル
	[SerializeField]
	private ItemObject _originObject = null;

	// 使用中のアイテムリスト
	private List<ItemBase> _useList = null;
	// 未使用状態のアイテムリスト
	private List<List<ItemBase>> _unuseList = null;

	// 使用中のアイテムオブジェクト
	private List<ItemObject> _useObject = null;
	// 未使用状態のアイテムオブジェクト
	private List<ItemObject> _unuseObject = null;

	private const int _ITEM_MAX = 256;

	public void Initialize() {
		instance = this;
		// アイテム情報をある程度生成して未使用状態にしておく
		_useList = new List<ItemBase>(_ITEM_MAX);

		int itemCategoryMax = (int)eItemCategory.Max;
		_unuseList = new List<List<ItemBase>>(itemCategoryMax);
		for (int i = 0; i < itemCategoryMax; i++) {
			_unuseList.Add(new List<ItemBase>(_ITEM_MAX));
			for (int itemCount = 0; itemCount < _ITEM_MAX; itemCount++) {
				// カテゴリごとの派生クラスを生成してリストに積む
				_unuseList[i].Add(CreateCategoryItem((eItemCategory)i));
			}
		}

		// アイテムオブジェクトをある程度生成して未使用状態にしておく

	}

	private ItemBase CreateCategoryItem(eItemCategory category) {
		switch (category) {
			case eItemCategory.Potion:
			break;
			case eItemCategory.Food:
			break;
			case eItemCategory.Wand:
			break;
			case eItemCategory.Scroll:
			break;
			case eItemCategory.Bag:
			break;
			case eItemCategory.Throwing:
			break;
			case eItemCategory.Weapon:
			break;
			case eItemCategory.Armor:
			break;
		}
		return null;
	}

}
