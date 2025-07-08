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
	/// <summary>
	/// マスにキャラクターを置く際の位置
	/// </summary>
	[SerializeField]
	private Transform _characterRoot = null;
	/// <summary>
	/// マスにアイテム等を置く際の位置
	/// </summary>
	[SerializeField]
	private Transform _objectRoot = null;
	/// <summary>
	/// デバッグ用スプライト
	/// </summary>
	[SerializeField]
	private SpriteRenderer _markSprite = null;

	public void Setup(int setX, int setY) {
		Vector3 position = transform.position;
		position.x = setX * _SQUARE_SIZE_RARIO;
		position.y = setY * _SQUARE_SIZE_RARIO;
		position.z = setY * 0.1f;
		transform.position = position;
		HideMark();
	}

	/// <summary>
	/// 地形の変更
	/// </summary>
	/// <param name="setTerrain"></param>
	public void SetTerrain(eTerrain setTerrain, int spriteIndex = -1) {
		// 地形に対応したスプライト画像を取得して設定
		_terrainSprite.sprite = TerrainSpriteAssignor.GetTerrainSprite(setTerrain, spriteIndex);
	}

	/// <summary>
	/// キャラクター基準位置取得
	/// </summary>
	/// <returns></returns>
	public Transform GetCharacterRoot() { return _characterRoot; }
	/// <summary>
	/// アイテム基準位置取得
	/// </summary>
	/// <returns></returns>
	public Transform GetObjectRoot() { return _objectRoot; }

	/// <summary>
	/// デバッグ用スプライト表示
	/// </summary>
	/// <param name="color"></param>
	public void ShowMark(Color color) {
		_markSprite.color = color;
		_markSprite.enabled = true;
	}

	/// <summary>
	/// デバッグ用スプライト非表示
	/// </summary>
	public void HideMark() {
		_markSprite.enabled = false;
	}
}
