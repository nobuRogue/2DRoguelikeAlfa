/**
 * @file DungeonProcessor.cs
 * @brief ダンジョン実行処理
 * @author yao
 * @date 2025/5/13
 */

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonProcessor {
	private FloorProcessor _floorProcessor = null;

	private eDungeonEndReason _endReason = eDungeonEndReason.Invalid;

	public void Initialize() {
		_floorProcessor = new FloorProcessor();
		_floorProcessor.Initialize();
	}

	public async UniTask<eDungeonEndReason> Execute() {
		_endReason = eDungeonEndReason.Invalid;
		// ダンジョンの実行
		while (_endReason == eDungeonEndReason.Invalid) {
			await _floorProcessor.Execute();
		}
		return _endReason;
	}

}
