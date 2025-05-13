/**
 * @file PartMainGame.cs
 * @brief メインゲームパート
 * @author yao
 * @date 2025/4/10
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

using static MapSquareUtility;

public class PartMainGame : PartBase {
	// マスの管理クラス
	[SerializeField]
	private MapSquareManager _squareManager = null;
	// キャラクターの管理クラス
	[SerializeField]
	private CharacterManager _characterManager = null;

	// ダンジョン実行クラス
	private DungeonProcessor _dungeonProcessor = null;

	public override async UniTask Initialize() {
		await base.Initialize();
		// ダンジョン実行クラス初期化
		_dungeonProcessor = new DungeonProcessor();
		_dungeonProcessor.Initialize();
		// マスの管理クラス初期化
		TerrainSpriteAssignor.Initialize();
		_squareManager?.Initialize();
		// キャラクター管理クラス初期化
		_characterManager?.Initialize();
	}

	public override async UniTask Setup() {
		await base.Setup();
		// プレイヤーを生成
		_characterManager.UsePlayer(GetSquareData(0, 0), 0);
	}

	public override async UniTask Execute() {
		// ダンジョンの実行
		eDungeonEndReason endReason = await _dungeonProcessor.Execute();
		// ダンジョン終了結果の処理
		switch (endReason) {
			case eDungeonEndReason.Dead:
			// ゲームオーバーの処理
			break;
			case eDungeonEndReason.Clear:
			// ゲームクリアの処理
			break;
		}
	}

}
