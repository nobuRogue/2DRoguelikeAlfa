/**
 * @file MapSquareObject.cs
 * @brief 1マスのオブジェクト
 * @author yao
 * @date 2025/4/15
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSquareObject : MonoBehaviour {
	private static readonly float _SQUARE_SIZE_RARIO = 0.32f;

	/// <summary>
	/// 地形画像
	/// </summary>
	[SerializeField]
	private SpriteRenderer _terrainSprite = null;

	public void Setup(int setX, int setY) {
		Vector3 position = transform.position;
		position.x = setX * _SQUARE_SIZE_RARIO;
		position.y = setY * _SQUARE_SIZE_RARIO;
		position.z = setY * 0.1f;
		transform.position = position;
	}

	/// <summary>
	/// 地形の変更
	/// </summary>
	/// <param name="setTerrain"></param>
	public void SetTerrain(eTerrain setTerrain) {
		// 地形に対応したスプライト画像を取得して設定
		_terrainSprite.sprite = TerrainSpriteAssignor.GetTerrainSprite(setTerrain);
	}
}
