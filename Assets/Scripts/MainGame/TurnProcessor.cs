/**
 * @file TurnProcessor.cs
 * @brief ターン実行処理
 * @author yao
 * @date 2025/5/13
 */

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnProcessor {



	/// <summary>
	/// 初期化
	/// </summary>
	public void Initialize() {

	}

	public async UniTask Execute() {
		// プレイヤーの入力受付、移動以外の行動実行
		await AcceptPlayerAction();
		// エネミーの思考
		// 全キャラクターの移動

		// 全エネミーの移動以外の行動

		// ターン終了時の処理

	}

	/// <summary>
	/// プレイヤーの入力受付、行動実行
	/// </summary>
	/// <returns></returns>
	private async UniTask AcceptPlayerAction() {

	}

}
