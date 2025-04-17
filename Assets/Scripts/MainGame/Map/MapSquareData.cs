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
	/// <summary>
	/// ユニークID
	/// </summary>
	public int ID { get; private set; } = -1;
	/// <summary>
	/// マス基準の座標
	/// </summary>
	public int posX { get; private set; } = -1;
	public int posY { get; private set; } = -1;
	/// <summary>
	/// 地形
	/// </summary>
	public eTerrain terrain { get; private set; } = eTerrain.Invalid;

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
}
