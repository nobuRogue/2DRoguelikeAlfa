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
	/// プレイヤー生成
	/// </summary>
	/// <param name="squareData"></param>
	/// <param name="masterID"></param>
	public static void UsePlayer(MapSquareData squareData, int masterID) {
		CharacterManager.instance.UsePlayer(squareData, masterID);
	}

	/// <summary>
	/// エネミー生成
	/// </summary>
	/// <param name="squareData"></param>
	/// <param name="masterID"></param>
	public static void UseEnemy(MapSquareData squareData, int masterID) {
		CharacterManager.instance.UseEnemy(squareData, masterID);
	}

	/// <summary>
	/// プレイヤー削除
	/// </summary>
	/// <param name="unusePlayer"></param>
	public static void UnusePlayer(PlayerCharacter unusePlayer) {
		CharacterManager.instance.UnusePlayer(unusePlayer);
	}

	/// <summary>
	/// エネミー削除
	/// </summary>
	/// <param name="unuseEnemy"></param>
	public static void UnuseEnemy(EnemyCharacter unuseEnemy) {
		CharacterManager.instance.UnuseEnemy(unuseEnemy);
	}

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

	/// <summary>
	/// 全てのキャラクターに指定処理実行
	/// </summary>
	/// <param name="action"></param>
	public static void ExecuteAllCharacter(System.Action<CharacterBase> action) {
		CharacterManager.instance.ExecuteAllCharacter(action);
	}

}
