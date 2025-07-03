/**
 * @file ItemBase.cs
 * @brief アイテムデータの基底
 * @author yao
 * @date 2025/7/3
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static ItemUtility;

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
		SetSquare(square);
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

	/// <summary>
	/// 使用後の片付け
	/// </summary>
	public void Teardown() {
		RemoveCurrentPlace();
		ID = -1;
		masterID = -1;
		nameID = -1;
	}

	/// <summary>
	/// アイテムをマスに置く
	/// </summary>
	/// <param name="square"></param>
	public void SetSquare(MapSquareData square) {
		if (square == null) return;
		// 現在の場所から取り除く
		RemoveCurrentPlace();
		// 座標の設定
		posX = square.posX;
		posY = square.posY;
		// オブジェクトの処理
		ItemObject itemObject = GetItemObject(ID);
		if (itemObject == null) {
			// オブジェクトを生成する
			itemObject = UseItemObject(ID);
		}
		itemObject.SetSquare(square);
	}

	/// <summary>
	/// アイテムを現在の場所から取り除く
	/// </summary>
	private void RemoveCurrentPlace() {
		posX = -1;
		posY = -1;
		possessCharacterID = -1;
	}

}
