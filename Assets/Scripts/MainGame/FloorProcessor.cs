/**
 * @file FloorProcessor.cs
 * @brief フロア実行処理
 * @author yao
 * @date 2025/5/13
 */

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorProcessor {
	private TurnProcessor _turnProcessor = null;

	private eFloorEndReason _endReason = eFloorEndReason.Invalid;

	public void Initialize() {
		_turnProcessor = new TurnProcessor();
		_turnProcessor.Initialize();
	}

	/// <summary>
	/// フロア実行処理
	/// </summary>
	/// <returns></returns>
	public async UniTask Execute() {
		// フロアの生成
		await SetupFloor();
		// フロアの実行
		while (_endReason == eFloorEndReason.Invalid) {
			await _turnProcessor.Execute();
		}
		// フロアの破棄
		await TeardownFloor();
	}

	/// <summary>
	/// フロア生成
	/// </summary>
	private async UniTask SetupFloor() {
		// マップ生成
		MapCreater.CreateMap();
		// フロアを終了していない状態にする
		_endReason = eFloorEndReason.Invalid;
	}

	private async UniTask TeardownFloor() {

	}
}
