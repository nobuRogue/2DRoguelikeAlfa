/**
 * @file ItemBase.cs
 * @brief アイテムデータの基底
 * @author yao
 * @date 2025/7/3
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static CharacterUtility;
using static MapSquareUtility;
using static ItemUtility;
using static ItemMasterUtility;

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
	private int _nameID = -1;
	// アイテムカテゴリ取得
	public abstract eItemCategory GetCategory();

	/// <summary>
	/// マスに置くアイテムの使用前準備
	/// </summary>
	/// <param name="setID"></param>
	/// <param name="masterData"></param>
	/// <param name="square"></param>
	public void SetupSquare(int setID, Entity_ItemData.Param masterData, MapSquareData square) {
		Setup(setID, masterData);
		// マスに置く
		SetSquare(square);
	}

	/// <summary>
	/// アイテムの使用前準備
	/// </summary>
	/// <param name="setID"></param>
	/// <param name="masterData"></param>
	protected virtual void Setup(int setID, Entity_ItemData.Param masterData) {
		ID = setID;
		masterID = masterData.ID;
		// マスターデータ関連のセットアップ
		_nameID = masterData.nameID;
	}

	/// <summary>
	/// 使用後の片付け
	/// </summary>
	public void Teardown() {
		RemoveCurrentPlace();
		ID = -1;
		masterID = -1;
		_nameID = -1;
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
		square.SetItem(ID);
		// オブジェクトの処理
		ItemObject itemObject = GetItemObject(ID);
		if (itemObject == null) {
			// オブジェクトを生成する
			itemObject = UseItemObject(ID);
		}
		itemObject.SetSquare(square);
	}

	/// <summary>
	/// キャラクターの手持ちに追加
	/// </summary>
	/// <param name="character"></param>
	public void AddCharacter(CharacterBase character) {
		if (character == null) return;
		// 現在の場所から取り除く
		RemoveCurrentPlace();
		character.AddItem(ID);
		possessCharacterID = character.ID;
	}

	/// <summary>
	/// アイテムを現在の場所から取り除く
	/// </summary>
	private void RemoveCurrentPlace() {
		// 床落ちアイテム除去
		MapSquareData itemSuqare = GetSquareData(posX, posY);
		if (itemSuqare != null) {
			itemSuqare.RemoveObject();
			posX = -1;
			posY = -1;
			// オブジェクト非表示
			GetItemObject(ID).UnuseSelf();
		}
		// キャラ手持ちアイテム除去
		GetCharacterData(possessCharacterID)?.RemoveItem(ID);
		possessCharacterID = -1;
	}

	/// <summary>
	/// 名前の取得
	/// </summary>
	/// <returns></returns>
	public virtual string GetName() {
		return _nameID.ToMessage();
	}

	/// <summary>
	/// マスターID変更
	/// </summary>
	/// <param name="changeID"></param>
	public void ChangeMasterID(int changeID) {
		masterID = changeID;
		var itemMaster = GetItemMaster(masterID);
		_nameID = itemMaster.nameID;
	}

	/// <summary>
	/// 消費する
	/// </summary>
	public virtual void Consume() {
		// 自身を削除
		RemoveItem(this);
	}

}
