/**
 * @file ItemManager.cs
 * @brief アイテム管理
 * @author yao
 * @date 2025/7/3
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static CommonModule;

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
	private List<ItemObject> _useObjectList = null;
	// 未使用状態のアイテムオブジェクト
	private List<ItemObject> _unuseObjectList = null;

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
		_useObjectList = new List<ItemObject>(_ITEM_MAX);
		_unuseObjectList = new List<ItemObject>(_ITEM_MAX);
		for (int i = 0; i < _ITEM_MAX; i++) {
			_unuseObjectList.Add(Instantiate(_originObject, _unuseObjectRoot));
		}
	}

	/// <summary>
	/// アイテムカテゴリに対応したクラスのインスタンスを返す
	/// </summary>
	/// <param name="category"></param>
	/// <returns></returns>
	private ItemBase CreateCategoryItem(eItemCategory category) {
		switch (category) {
			case eItemCategory.Potion:
			return new ItemPotion();
			case eItemCategory.Food:
			return new ItemFood();
			case eItemCategory.Wand:
			return new ItemWand();
			case eItemCategory.Scroll:
			return new ItemScroll();
			case eItemCategory.Bag:
			return new ItemBag();
			case eItemCategory.Throwing:
			return new ItemThrowing();
			case eItemCategory.Weapon:
			return new ItemWeapon();
			case eItemCategory.Armor:
			return new ItemArmor();
		}
		return null;
	}

	/// <summary>
	/// ID指定のアイテムオブジェクト取得
	/// </summary>
	/// <param name="ID"></param>
	/// <returns></returns>
	public ItemObject GetItemObject(int ID) {
		if (!IsEnableIndex(_useObjectList, ID)) return null;

		return _useObjectList[ID];
	}

	/// <summary>
	/// ID指定のアイテムデータ取得
	/// </summary>
	/// <param name="ID"></param>
	/// <returns></returns>
	public ItemBase GetItemData(int ID) {
		if (!IsEnableIndex(_useList, ID)) return null;

		return _useList[ID];
	}

	/// <summary>
	/// 床落ちアイテム生成
	/// </summary>
	/// <param name="masterID"></param>
	/// <param name="square"></param>
	public void CreateFloorItem(int masterID, MapSquareData square) {
		// 使用可能なインスタンス取得
		eItemCategory createItemCategory = eItemCategory.Potion;
		// データを使用状態にする
		int useID = UseItemData(createItemCategory);
		GetItemData(useID)?.SetupSquare(useID, masterID, square);
	}

	/// <summary>
	/// アイテムを使用状態にする
	/// </summary>
	/// <param name="useItemCategory"></param>
	/// <returns></returns>
	private int UseItemData(eItemCategory useItemCategory) {
		// 使用可能なアイテムデータのインスタンス取得
		ItemBase useItem = GetUsableItemData((int)useItemCategory);
		// 使用可能なIDを取得して使用リストに追加
		int useID = -1;
		for (int i = 0, max = _useList.Count; i < max; i++) {
			if (_useList[i] != null) continue;
			// 使用可能な場所が見つかった
			useID = i;
			_useList[i] = useItem;
			break;
		}
		// リストに使用可能な場所が見つからなかったので末尾に追加
		if (useID < 0) {
			useID = _useList.Count;
			_useList.Add(useItem);
		}
		return useID;
	}

	/// <summary>
	/// 使用可能なアイテムデータのインスタンスを返す
	/// </summary>
	/// <param name="categoryIndex"></param>
	/// <returns></returns>
	private ItemBase GetUsableItemData(int categoryIndex) {
		// 未使用状態のインスタンスがあれば返す、無ければ生成して返す
		List<ItemBase> targetList = _unuseList[categoryIndex];
		if (IsEmpty(targetList)) return CreateCategoryItem((eItemCategory)categoryIndex);

		ItemBase result = targetList[0];
		targetList.RemoveAt(0);
		return result;
	}

	/// <summary>
	/// アイテムオブジェクトを使用状態にする
	/// </summary>
	/// <param name="useID"></param>
	/// <returns></returns>
	public ItemObject UseItemObject(int useID) {
		// 使用可能なアイテムオブジェクトのインスタンスを取得
		ItemObject useObject = GetUsableItemObject();
		// useIDが有効になるように使用リストの要素を追加する
		while (!IsEnableIndex(_useObjectList, useID)) _useObjectList.Add(null);
		// 使用リストへの追加
		_useObjectList[useID] = useObject;
		useObject.transform.SetParent(_useObjectRoot);
		ItemBase itemData = GetItemData(useID);
		useObject.Setup(useID, itemData.GetCategory());
		return useObject;
	}

	/// <summary>
	/// 未使用状態のアイテムオブジェクト取得
	/// </summary>
	/// <returns></returns>
	private ItemObject GetUsableItemObject() {
		if (IsEmpty(_unuseObjectList)) return Instantiate(_originObject);

		ItemObject result = _unuseObjectList[0];
		_unuseObjectList.RemoveAt(0);
		return result;
	}

	/// <summary>
	/// アイテムを未使用状態にする
	/// </summary>
	/// <param name="unuseItem"></param>
	public void UnuseItem(ItemBase unuseItem) {
		if (unuseItem == null) return;
		// データの未使用化
		int unuseID = unuseItem.ID;
		_useList[unuseID] = null;
		unuseItem.Teardown();
		_unuseList[(int)unuseItem.GetCategory()].Add(unuseItem);
		// オブジェクトの未使用化
		UnuseItemObject(GetItemObject(unuseID));
	}

	/// <summary>
	/// アイテムオブジェクトを未使用状態にする
	/// </summary>
	/// <param name="unuseObject"></param>
	public void UnuseItemObject(ItemObject unuseObject) {
		if (unuseObject == null) return;
		// 未使用状態にする
		_useObjectList[unuseObject.ID] = null;
		unuseObject.Teardown();
		_unuseObjectList.Add(unuseObject);
		unuseObject.transform.SetParent(_unuseObjectRoot);
	}

}