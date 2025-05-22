/**
 * @file PartMainGame.cs
 * @brief メインゲームパート
 * @author yao
 * @date 2025/1/9
 */

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartMainGame : PartBase {
	[SerializeField]
	private MapSquareManager _squareManager = null;

	[SerializeField]
	private CharacterManager _characterManager = null;

	private DungeonProcessor _dungeonProcessor = null;

	private const int _MAIN_BGM_ID = 0;

	public override async UniTask Initialize() {
		TerrainSpriteAssignor.Initialize();

		_dungeonProcessor = new DungeonProcessor();
		_dungeonProcessor.Initialize();

		_squareManager.Initialize();
		_characterManager.Initialize();
		await UniTask.CompletedTask;
	}

	public override async UniTask Setup() {
		// プレイヤー生成
		CharacterManager.instance.UsePlayer(MapSquareUtility.GetSquareData(0, 0), 0);
		await UniTask.CompletedTask;
	}

	public override async UniTask Execute() {
		// BGM再生
		SoundManager.instance.PlayBGM(_MAIN_BGM_ID);
		// ダンジョンの実行
		eDungeonEndReason endReason = await _dungeonProcessor.Execute();
		// BGM止める
		SoundManager.instance.StopBGM();
		// ダンジョン終了結果の処理
		switch (endReason) {
			case eDungeonEndReason.Dead:
			break;
			case eDungeonEndReason.Clear:
			break;
		}

	}

	public override async UniTask Teardown() {
		await UniTask.CompletedTask;
	}
}
