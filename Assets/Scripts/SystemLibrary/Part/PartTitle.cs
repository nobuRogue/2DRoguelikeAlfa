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
	public override async UniTask Initialize() {
		await base.Initialize();
		// タイトルメニューの初期化
		await MenuManager.instance.Get<MenuTitle>("Prefabs/Menu/CanvasTitle").Initialize();
	}

	public override async UniTask Execute() {
		// タイトルメニュー表示
		await MenuManager.instance.Get<MenuTitle>().Open();
		// 初期ユーザデータ設定
		UserDataHolder.SetCurrentData(new UserData());
		// メインパートへ遷移
		UniTask task = PartManager.instance.TransitionPart(eGamePart.MainGame);
		await UniTask.CompletedTask;
	}
}
