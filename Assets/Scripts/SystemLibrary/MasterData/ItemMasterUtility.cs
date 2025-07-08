/**
 * @file ItemMasterUtility.cs
 * @brief アイテムマスターデータの実行処理
 * @author yao
 * @date 2025/7/8
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMasterUtility {

	/// <summary>
	/// ID指定のアイテムマスター取得
	/// </summary>
	/// <param name="ID"></param>
	/// <returns></returns>
	public static Entity_ItemData.Param GetItemMaster(int ID) {
		var itemMasterList = MasterDataManager.itemData[0];
		for (int i = 0, max = itemMasterList.Count; i < max; i++) {
			if (itemMasterList[i].ID != ID) continue;

			return itemMasterList[i];
		}
		return null;
	}

}
