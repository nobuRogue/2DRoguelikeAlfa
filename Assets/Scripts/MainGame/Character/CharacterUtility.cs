/**
 * @file CharacterUtility.cs
 * @brief キャラクター関連実行処理
 * @author yao
 * @date 2025/5/13
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterUtility {
	/// <summary>
	/// ID指定のキャラクターデータ取得
	/// </summary>
	/// <param name="ID"></param>
	/// <returns></returns>
	public static CharacterBase GetCharacterData(int ID) {
		return CharacterManager.instance.GetCharacterData(ID);
	}

	/// <summary>
	/// プレイヤー取得
	/// </summary>
	/// <returns></returns>
	public static CharacterBase GetPlayer() {
		return CharacterManager.instance.GetPlayer();
	}

}
