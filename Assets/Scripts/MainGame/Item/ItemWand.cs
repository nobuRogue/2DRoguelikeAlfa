/**
 * @file ItemWand.cs
 * @brief 杖カテゴリアイテム
 * @author yao
 * @date 2025/7/3
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWand : ItemBase {
	// 残り使用回数
	private int _count = -1;
	// 杖の表示名メッセージID
	private const int _WAND_NAME_ID = 12299;

	/// <summary>
	/// カテゴリ取得
	/// </summary>
	/// <returns></returns>
	public override eItemCategory GetCategory() {
		return eItemCategory.Wand;
	}

	/// <summary>
	/// 使用前の準備、使用回数を設定
	/// </summary>
	/// <param name="setID"></param>
	/// <param name="masterData"></param>
	protected override void Setup(int setID, Entity_ItemData.Param masterData) {
		base.Setup(setID, masterData);
		_count = Random.Range(masterData.MinValue, masterData.MaxValue + 1);
	}

	/// <summary>
	/// 消費する
	/// </summary>
	public override void Consume() {
		// 回数を減らし0になったら消す
		_count--;
		if (_count <= 0) base.Consume();

	}

	/// <summary>
	/// 名前の取得、回数を付ける
	/// </summary>
	/// <returns></returns>
	public override string GetName() {
		return string.Format(_WAND_NAME_ID.ToMessage(), base.GetName(), _count);
	}

}
