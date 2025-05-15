/**
 * @file PartTitle.cs
 * @brief ゲームパートの基底
 * @author yao
 * @date 2025/4/10
 */

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartTitle : PartBase {
	public override async UniTask Execute() {
		// フェードアウト
		await FadeManager.instance.FadeOut();
		// メインパートへ遷移
		UniTask task = PartManager.instance.TransitionPart(eGamePart.MainGame);
		await UniTask.CompletedTask;
	}
}
