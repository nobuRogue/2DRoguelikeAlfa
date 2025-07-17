/**
 * @file DungeonProcessor.cs
 * @brief ダンジョン実行処理
 * @author yao
 * @date 2025/5/13
 */

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DungeonProcessor {
	private FloorProcessor _floorProcessor = null;

	private eDungeonEndReason _endReason = eDungeonEndReason.Invalid;

	/// <summary>
	/// 初期化
	/// </summary>
	public void Initialize() {
		_floorProcessor = new FloorProcessor();
		_floorProcessor.Initialize(EndDungeon);
	}

	public async UniTask<eDungeonEndReason> Execute() {
		_endReason = eDungeonEndReason.Invalid;
		// ダンジョンの実行
		while (_endReason == eDungeonEndReason.Invalid) {
			await _floorProcessor.Execute();
		}
		return _endReason;
	}

	/// <summary>
	/// ダンジョンを終了させる
	/// </summary>
	/// <param name="endReason"></param>
	private void EndDungeon(eDungeonEndReason endReason) {
		_endReason = endReason;
	}

}
