/**
 * @file PutonAction.cs
 * @brief 床にアイテムを置くアクション
 * @author yao
 * @date 2025/7/17
 */

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static ItemUtility;
using static RogueLogUtility;

public class PutonAction {
	// ログメッセージID
	private static readonly int _PUTON_LOG_ID = 14030;
	private static readonly int _CANNOT_LOG_ID = 14031;

	/// <summary>
	/// マスにアイテムを置くアクション実行
	/// </summary>
	/// <param name="putonSquare"></param>
	/// <param name="itemID"></param>
	/// <returns></returns>
	public static async UniTask ExecutePuton(MapSquareData putonSquare, int itemID) {
		// アイテムのデータを取得
		ItemBase itemData = GetItemData(itemID);
		if (itemData == null || putonSquare == null) return;
		// マスにアイテムが置けるか判定
		if (putonSquare.existObject) {
			// マスにアイテムが置けない
			AddLog(string.Format(_CANNOT_LOG_ID.ToMessage(), itemData.GetName()));
			return;
		}
		// アイテムを置く
		itemData.SetSquare(putonSquare);
		// ログを表示
		AddLog(string.Format(_PUTON_LOG_ID.ToMessage(), itemData.GetName()));
		await UniTask.CompletedTask;
	}

}
