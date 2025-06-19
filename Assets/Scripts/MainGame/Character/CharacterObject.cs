/**
 * @file CharacterObject.cs
 * @brief キャラクターオブジェクト
 * @author yao
 * @date 2025/5/8
 */

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

using static CommonModule;

public class CharacterObject : MonoBehaviour {
	// 画像読み込み用の定数
	private static StringBuilder _spriteNameBuilder = new StringBuilder();
	private static readonly string _CHARACTER_SPRITE_PATH = "Design/Sprites/Character/";
	private static readonly string[] _ANIMATION_SPRITE_NAME =
		new string[] { "wait", "walk", "attack", "damage" };
	// アニメーションで画像が切り替わる時間[ミリ秒]
	private static readonly int _ANIMATION_DELAY_MILLI_SEC = 150;

	[SerializeField]
	private SpriteRenderer _characterSprite = null;

	// 使用予定のアニメーション毎の画像リスト
	private Sprite[][] _animationSpriteList = null;
	// アニメーション再生タスク
	private UniTask _animTask;
	// 現在再生中のアニメーション
	public eCharacterAnimation currentAnim { get; private set; } = eCharacterAnimation.Invalid;
	// 現在の画像のインデクス
	private int _animIndex = -1;

	/// <summary>
	/// 使用前の準備
	/// </summary>
	public void Setup(string spriteName) {
		// アニメーション画像の読み込み
		int animMax = (int)eCharacterAnimation.Max;
		_animationSpriteList = new Sprite[animMax][];
		for (int i = 0; i < animMax; i++) {
			_spriteNameBuilder.Append(_CHARACTER_SPRITE_PATH);
			_spriteNameBuilder.Append(spriteName);
			_spriteNameBuilder.Append(_ANIMATION_SPRITE_NAME[i]);
			_animationSpriteList[i] = Resources.LoadAll<Sprite>(_spriteNameBuilder.ToString());
			_spriteNameBuilder.Clear();
		}
		// 待機アニメーションを設定
		SetAnimation(eCharacterAnimation.Wait);
		// アニメーション再生タスクを実行(既に実行中ならしない)
		if (_animTask.Status.IsCompleted()) _animTask = PlayAnimationTask();

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

	/// <summary>
	/// 画像の向き変更
	/// </summary>
	/// <param name="setDir"></param>
	public void SetDirection(eDirectionEight setDir) {
		switch (setDir) {
			case eDirectionEight.UpRight:
			case eDirectionEight.Right:
			case eDirectionEight.DownRight:
			// 画像を右に向かせる
			Vector3 scale = _characterSprite.transform.localScale;
			scale.x = 1.0f;
			_characterSprite.transform.localScale = scale;
			break;
			case eDirectionEight.DownLeft:
			case eDirectionEight.Left:
			case eDirectionEight.UpLeft:
			// 画像を左に向かせる
			scale = _characterSprite.transform.localScale;
			scale.x = -1.0f;
			_characterSprite.transform.localScale = scale;
			break;
		}
	}

	/// <summary>
	/// アニメーション用の画像切り替えタスク
	/// </summary>
	/// <returns></returns>
	private async UniTask PlayAnimationTask() {
		while (true) {
			// 現在のアニメーション取得
			int currentAnimIndex = (int)currentAnim;
			if (!IsEnableIndex(_animationSpriteList, currentAnimIndex)) {
				// 無効なアニメーションなら終わり
				await UniTask.DelayFrame(1);
				return;
			}
			Sprite[] currentAnimSpriteList = _animationSpriteList[currentAnimIndex];
			// ループ判定、処理
			if (!IsEnableIndex(currentAnimSpriteList, _animIndex)) AnimationLoopProcess();
			// 画像の設定
			_characterSprite.sprite = currentAnimSpriteList[_animIndex];
			// 規定ミリ秒待ち、インデックス増加
			await UniTask.Delay(_ANIMATION_DELAY_MILLI_SEC);
			_animIndex++;
		}
	}

	/// <summary>
	/// アニメーションのループ処理
	/// </summary>
	private void AnimationLoopProcess() {
		if (currentAnim == eCharacterAnimation.Attack ||
			currentAnim == eCharacterAnimation.Damage) {
			// 攻撃か被ダメージなら待機に戻す
			SetAnimation(eCharacterAnimation.Wait);
		} else {
			// 待機と歩行は_animIndexを0にするだけ
			_animIndex = 0;
		}
	}

	/// <summary>
	/// アニメーションの再生
	/// </summary>
	/// <param name="setAnim"></param>
	public void SetAnimation(eCharacterAnimation setAnim) {
		// 現在と同じアニメーションなら処理しない
		if (currentAnim == setAnim) return;

		currentAnim = setAnim;
		_animIndex = 0;
	}
}
