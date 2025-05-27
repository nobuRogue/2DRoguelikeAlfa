/**
 * @file CharacterMasterUtility.cs
 * @brief キャラクターマスターデータの実行処理
 * @author yao
 * @date 2025/5/27
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMasterUtility {
	/// <summary>
	/// ID指定のキャラクターマスター取得
	/// </summary>
	/// <param name="masterID"></param>
	/// <returns></returns>
	public static Entity_CharacterData.Param GetCharacterMaster(int masterID) {
		// キャラクターマスターデータ取得
		var characterMasterList = MasterDataManager.characterData[0];
		// IDが一致するものを返す
		for (int i = 0, max = characterMasterList.Count; i < max; i++) {
			if (characterMasterList[i].ID != masterID) continue;

			return characterMasterList[i];
		}
		return null;
	}

}
