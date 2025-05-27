/**
 * @file CharacterObject.cs
 * @brief キャラクターオブジェクト
 * @author yao
 * @date 2025/5/8
 */

using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class CharacterObject : MonoBehaviour {
	private static StringBuilder _spriteNameBuilder = new StringBuilder();
	private static readonly string _CHARACTER_SPRITE_PATH = "Design/Sprites/Character/";

	[SerializeField]
	private SpriteRenderer _characterSprite = null;

	/// <summary>
	/// 使用前の準備
	/// </summary>
	public void Setup(string spriteName) {
		_spriteNameBuilder.Append(_CHARACTER_SPRITE_PATH);
		_spriteNameBuilder.Append(spriteName);
		_spriteNameBuilder.Append("wait");
		Sprite[] characterSprite = Resources.LoadAll<Sprite>(_spriteNameBuilder.ToString());
		_spriteNameBuilder.Clear();

		_characterSprite.sprite = characterSprite[0];
	}

	/// <summary>
	/// 使用後の片付け
	/// </summary>
	public void Teardown() {
		_characterSprite.sprite = null;
	}

	/// <summary>
	/// 位置の設定
	/// </summary>
	/// <param name="position"></param>
	public void SetPosition(Vector3 position) {
		transform.position = position;
	}
}
