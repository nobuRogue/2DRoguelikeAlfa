/**
 * @file MessageMasterUtility.cs
 * @brief メッセージマスターデータの実行処理
 * @author yao
 * @date 2025/6/24
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static CommonModule;

public class MessageMasterUtility {

	/// <summary>
	/// ID指定のメッセージ取得
	/// </summary>
	/// <param name="ID"></param>
	/// <param name="index"></param>
	/// <returns></returns>
	public static string GetMessageData(int ID, int index) {
		var messageMaster = MasterDataManager.messageData;
		// シートごとに回す
		for (int sheetIndex = 0, sheetMax = messageMaster.Count; sheetIndex < sheetMax; sheetIndex++) {
			var messageMasterSheet = messageMaster[sheetIndex];
			// IDが一致するものを探して返す
			for (int i = 0, max = messageMasterSheet.Count; i < max; i++) {
				if (messageMasterSheet[i].ID != ID) continue;

				if (!IsEnableIndex(messageMasterSheet[i].text, index)) return string.Empty;

				return messageMasterSheet[i].text[index];
			}
		}
		return string.Empty;
	}

}
