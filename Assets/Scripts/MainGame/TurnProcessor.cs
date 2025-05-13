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

using static CommonModule;
using static GameConst;

public class TurnProcessor {
	// プレイヤー入力の受付
	private AcceptPlayerAction _acceptPlayerAction = null;
	// 移動アクションリスト
	private List<MoveAction> _moveActionList = null;

	/// <summary>
	/// 初期化
	/// </summary>
	public void Initialize() {
		_moveActionList = new List<MoveAction>(FLOOR_ENEMY_MAX + 1);

		_acceptPlayerAction = new AcceptPlayerAction();
		_acceptPlayerAction.Initialize(moveAction => _moveActionList.Add(moveAction));
	}

	public async UniTask Execute() {
		// プレイヤーの入力受付、移動以外の行動実行
		await AcceptPlayerAction();
		// エネミーの思考
		// 全キャラクターの移動
		await MoveAllCharacter();
		// 全エネミーの移動以外の行動

		// ターン終了時の処理

	}

	/// <summary>
	/// プレイヤーの入力受付、行動実行
	/// </summary>
	/// <returns></returns>
	private async UniTask AcceptPlayerAction() {
		await _acceptPlayerAction.AcceptInput();
	}

	/// <summary>
	/// 全キャラクターの見た目上の移動
	/// </summary>
	/// <returns></returns>
	private async UniTask MoveAllCharacter() {
		int moveCount = _moveActionList.Count;
		List<UniTask> taskList = new List<UniTask>(moveCount);
		for (int i = 0; i < moveCount; i++) {
			taskList.Add(_moveActionList[i].ExecuteObject(0.2f));
		}
		// 終了待ち
		await WaitTask(taskList);
		_moveActionList.Clear();
	}



}
