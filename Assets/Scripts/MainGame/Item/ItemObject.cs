/**
 * @file ItemObject.cs
 * @brief アイテムオブジェクト
 * @author yao
 * @date 2025/7/3
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static GameConst;

public class ItemObject : MonoBehaviour {
	[SerializeField]
	private SpriteRenderer _itemSprite = null;

	// ユニークID
	public int ID { get; private set; } = -1;

	/// <summary>
	/// 使用前の準備
	/// </summary>
	/// <param name="setID"></param>
	/// <param name="category"></param>
	public void Setup(int setID, eItemCategory category) {
		ID = setID;
		// カテゴリから見た目を設定
		_itemSprite.sprite = Resources.LoadAll<Sprite>(ITEM_SPRITE_FILE_NAME)[(int)category];
	}

	/// <summary>
	/// 使用後の片付け
	/// </summary>
	public void Teardown() {
		ID = -1;
		_itemSprite.sprite = null;
	}

	/// <summary>
	/// アイテムオブジェクトをマスに設定
	/// </summary>
	/// <param name="square"></param>
	public void SetSquare(MapSquareData square) {
		transform.position = square.GetObjectRoot().position;
	}

	/// <summary>
	/// 自身を未使用状態にする
	/// </summary>
	public void UnuseSelf() {

	}

}
