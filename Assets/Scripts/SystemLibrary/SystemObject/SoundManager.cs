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

}
