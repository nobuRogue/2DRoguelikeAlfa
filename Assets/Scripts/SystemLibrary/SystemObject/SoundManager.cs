/**
 * @file SoundManager.cs
 * @brief サウンドの管理
 * @author yao
 * @date 2025/5/15
 */

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static CommonModule;

public class SoundManager : SystemObject {
	// BGM再生用コンポーネント
	[SerializeField]
	private AudioSource _bgmAudioSource = null;
	// SE再生用コンポーネント
	[SerializeField]
	private AudioSource[] _seAudioSouce = null;

	// BGMのリスト
	[SerializeField]
	private BGMAssign _bgmAssign = null;
	// SEのリスト
	[SerializeField]
	private SEAssign _seAssign = null;

	public static SoundManager instance { get; private set; } = null;

	public override async UniTask Initialize() {
		instance = this;
		await UniTask.CompletedTask;
	}

	/// <summary>
	/// BGM再生
	/// </summary>
	/// <param name="bgmID"></param>
	public void PlayBGM(int bgmID) {
		if (!IsEnableIndex(_bgmAssign.bgmArray, bgmID)) return;

		_bgmAudioSource.clip = _bgmAssign.bgmArray[bgmID];
		_bgmAudioSource.Play();
	}

	/// <summary>
	/// BGM停止
	/// </summary>
	public void StopBGM() {
		_bgmAudioSource.Stop();
	}

	/// <summary>
	/// SE再生
	/// </summary>
	/// <param name="seID"></param>
	public async UniTask PlaySE(int seID) {
		if (!IsEnableIndex(_seAssign.seArray, seID)) return;
		// 再生中でないオーディオソースを探してそれで再生
		for (int i = 0, max = _seAudioSouce.Length; i < max; i++) {
			AudioSource audioSource = _seAudioSouce[i];
			if (audioSource == null ||
				audioSource.isPlaying) continue;
			// 再生中でないオーディオソースが見つかったので再生
			audioSource.clip = _seAssign.seArray[seID];
			audioSource.Play();
			// SEの終了待ち
			while (audioSource.isPlaying) await UniTask.DelayFrame(1);

			return;
		}
	}

}
