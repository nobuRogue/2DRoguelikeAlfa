/**
 * @file ItemUtility.cs
 * @brief アイテム関連実行処理
 * @author yao
 * @date 2025/7/3
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUtility {

	/// <summary>
	/// ID指定のアイテムオブジェクト取得
	/// </summary>
	/// <param name="ID"></param>
	/// <returns></returns>
	public static ItemObject GetItemObject(int ID) {
		return ItemManager.instance.GetItemObject(ID);
	}

	/// <summary>
	/// ID指定のアイテムデータ取得
	/// </summary>
	/// <param name="ID"></param>
	/// <returns></returns>
	public static ItemBase GetItemData(int ID) {
		return ItemManager.instance.GetItemData(ID);
	}

	/// <summary>
	/// アイテムオブジェクトを使用状態にする
	/// </summary>
	/// <param name="useID"></param>
	/// <returns></returns>
	public static ItemObject UseItemObject(int useID) {
		return ItemManager.instance.UseItemObject(useID);
	}

	/// <summary>
	/// 床落ちアイテム生成
	/// </summary>
	/// <param name="masterID"></param>
	/// <param name="square"></param>
	public static void CreateFloorItem(int masterID, MapSquareData square) {
		ItemManager.instance.CreateFloorItem(masterID, square);
	}

	/// <summary>
	/// アイテム削除
	/// </summary>
	/// <param name="removeItem"></param>
	public static void RemoveItem(ItemBase removeItem) {
		ItemManager.instance.UnuseItem(removeItem);
	}

	/// <summary>
	/// アイテムオブジェクトを不可視化
	/// </summary>
	/// <param name="removeObject"></param>
	public static void RemoveItemObject(ItemObject removeObject) {
		ItemManager.instance.UnuseItemObject(removeObject);
	}

}
