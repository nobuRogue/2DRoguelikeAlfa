/**
 * @file ItemBase.cs
 * @brief アイテム情報の基底
 * @author yao
 * @date 2025/7/3
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemBase {
	// ユニークID
	public int ID { get; private set; } = -1;
	// マスターID
	public int masterID { get; private set; } = -1;
	// 置かれいる座標
	public int posX { get; private set; } = -1;
	public int posY { get; private set; } = -1;
	// 所持キャラクターID
	public int possessCharacterID { get; private set; } = -1;
	// 名前ID
	public int nameID { get; private set; } = -1;
	// アイテムカテゴリ取得
	public abstract eItemCategory GetCategory();

	/// <summary>
	/// マスに置くアイテムの使用前準備
	/// </summary>
	/// <param name="setID"></param>
	/// <param name="setMasterID"></param>
	/// <param name="square"></param>
	public void SetupSquare(int setID, int setMasterID, MapSquareData square) {
		Setup(setID, setMasterID);
		// マスに置く

	}

	/// <summary>
	/// アイテムの使用前準備
	/// </summary>
	/// <param name="setID"></param>
	/// <param name="setMasterID"></param>
	private void Setup(int setID, int setMasterID) {
		ID = setID;
		masterID = setMasterID;
		// マスターデータ関連のセットアップ

	}

}
