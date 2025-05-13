/**
 * @file CharacterObject.cs
 * @brief キャラクターオブジェクト
 * @author yao
 * @date 2025/5/8
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterObject : MonoBehaviour {
	[SerializeField]
	private SpriteRenderer _characterSprite = null;

	/// <summary>
	/// 使用前の準備
	/// </summary>
	public void Setup() {

	}

	/// <summary>
	/// 使用後の片付け
	/// </summary>
	public void Teardown() {

	}

	/// <summary>
	/// 位置の設定
	/// </summary>
	/// <param name="position"></param>
	public void SetPosition(Vector3 position) {
		transform.position = position;
	}
}
