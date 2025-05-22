/**
 * @file MenuTitle.cs
 * @brief タイトルメニュー
 * @author yao
 * @date 2025/5/22
 */

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTitle : MenuBase {

	public override async UniTask Open() {
		await base.Open();
		// Zキーが押されるまで待つ
		while (true) {
			if (Input.GetKeyDown(KeyCode.Z)) break;

			await UniTask.DelayFrame(1);
		}
		await Close();
	}

}
