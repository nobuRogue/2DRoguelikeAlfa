/**
 * @file MenuBase.cs
 * @brief メニューの基底
 * @author yao
 * @date 2025/5/22
 */

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBase : MonoBehaviour {
	[SerializeField]
	private GameObject _menuRoot = null;

	// 初期化
	public virtual async UniTask Initialize() {
		await UniTask.CompletedTask;
	}

	// 開く
	public virtual async UniTask Open() {
		// メニューを表示する
		_menuRoot?.SetActive(true);
		await UniTask.CompletedTask;

	}

	// 閉じる
	public virtual async UniTask Close() {
		// メニュ－を非表示にする
		_menuRoot?.SetActive(false);
		await UniTask.CompletedTask;
	}

}
