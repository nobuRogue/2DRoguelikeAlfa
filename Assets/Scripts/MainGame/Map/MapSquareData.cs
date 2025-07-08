/**
 * @file MapSquareData.cs
 * @brief 1マスの情報
 * @author yao
 * @date 2025/4/15
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSquareData {
	/// ユニークID
	public int ID { get; private set; } = -1;
	/// マス基準の座標
	public int posX { get; private set; } = -1;
	public int posY { get; private set; } = -1;
	/// 地形
	public eTerrain terrain { get; private set; } = eTerrain.Invalid;
	/// 部屋ID
	public int roomID { get; private set; } = -1;

	/// マスにいるキャラクターのID
	public int characterID { get; private set; } = -1;
	/// マスにキャラクターが存在するか
	public bool existCharacter { get { return characterID >= 0; } }
	/// マスにあるアイテムのID
	public int itemID { get; private set; } = -1;
	/// マスにアイテムがあるか
	public bool existItem { get { return itemID >= 0; } }

	/// <summary>
	/// 使用前の準備
	/// </summary>
	/// <param name="setID"></param>
	/// <param name="setX"></param>
	/// <param name="setY"></param>
	public void Setup(int setID, int setX, int setY) {
		ID = setID;
		posX = setX;
		posY = setY;
		// オブジェクトのセットアップ
		GetObject()?.Setup(posX, posY);
	}

	/// <summary>
	/// 地形の変更
	/// </summary>
	/// <param name="setTerrain"></param>

	public void SetTerrain(eTerrain setTerrain, int spriteIndex = -1) {
		terrain = setTerrain;
		// オブジェクトの地形変更
		GetObject()?.SetTerrain(terrain, spriteIndex);
	}

	/// <summary>
	/// 対応するオブジェクトの取得
	/// </summary>
	/// <returns></returns>
	private MapSquareObject GetObject() {
		return MapSquareManager.instance.GetSquareObject(ID);
	}

	/// <summary>
	/// キャラクター基準位置取得
	/// </summary>
	/// <returns></returns>
	public Transform GetCharacterRoot() {
		return GetObject()?.GetCharacterRoot();
	}
	/// <summary>
	/// アイテム基準位置取得
	/// </summary>
	/// <returns></returns>
	public Transform GetObjectRoot() {
		return GetObject()?.GetObjectRoot();
	}

	/// <summary>
	/// マスにキャラクターを設定する
	/// </summary>
	/// <param name="setCharacterID"></param>
	public void SetCharacter(int setCharacterID) {
		characterID = setCharacterID;
	}

	/// <summary>
	/// マスからキャラクターを取り除く
	/// </summary>
	public void RemoveCharacter() {
		characterID = -1;
	}

	/// <summary>
	/// マスにアイテム設定
	/// </summary>
	/// <param name="setItemID"></param>
	public void SetItem(int setItemID) {
		itemID = setItemID;
	}
	/// <summary>
	/// マスからアイテムを取り除く
	/// </summary>
	public void RemoveItem() {
		itemID = -1;
	}

	/// <summary>
	/// 部屋IDの設定
	/// </summary>
	/// <param name="setRoomID"></param>
	public void SetRoomID(int setRoomID) {
		roomID = setRoomID;
	}

	/// <summary>
	/// デバッグ用スプライト表示
	/// </summary>
	/// <param name="color"></param>
	public void ShowMark(Color color) {
		GetObject()?.ShowMark(color);
	}

	/// <summary>
	/// デバッグ用スプライト非表示
	/// </summary>
	public void HideMark() {
		GetObject()?.HideMark();
	}
}
