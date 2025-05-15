/**
 * @file FadeManager.cs
 * @brief フェードの管理クラス
 * @author yao
 * @date 2025/5/15
 */

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : SystemObject {
	// フェード用黒画像
	[SerializeField]
	private Image _fadeImage = null;

	public static FadeManager instance { get; private set; } = null;
	// デフォルトのフェード時間
	private const float _DEFAULT_FADE_DURAITION = 0.3f;

	/// <summary>
	/// 初期化
	/// </summary>
	/// <returns></returns>
	public override async UniTask Initialize() {
		instance = this;
		await UniTask.CompletedTask;
	}

	/// <summary>
	/// フェードアウト、暗くする
	/// </summary>
	/// <param name="duration"></param>
	/// <returns></returns>
	public async UniTask FadeOut(float duration = _DEFAULT_FADE_DURAITION) {
		float elapsedTime = 0.0f;// 経過時間
		float startAlpha = _fadeImage.color.a;
		float targetAlpha = 1.0f;
		while (elapsedTime < duration) {
			// フレーム時間経過
			elapsedTime += Time.deltaTime;
			// 補間した不透明度をフェード画像に設定
			float t = elapsedTime / duration;
			Color setColor = _fadeImage.color;
			setColor.a = Mathf.Lerp(startAlpha, targetAlpha, t);
			_fadeImage.color = setColor;
			// 1フレーム待ち
			await UniTask.DelayFrame(1);
		}

	}

}
