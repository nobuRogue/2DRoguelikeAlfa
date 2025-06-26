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

using static CharacterUtility;

using static CommonModule;
using static GameConst;

public class TurnProcessor {
	// プレイヤー入力の受付
	private AcceptPlayerAction _acceptPlayerAction = null;
	// 移動アクションリスト
	private List<MoveAction> _moveActionList = null;
	// ターン継続フラグ
	private bool _isContinueTurn = true;
	// フロア終了処理
	private System.Action<eFloorEndReason> _EndFloor = null;
	// ダンジョン終了処理
	private System.Action<eDungeonEndReason> _EndDungeon = null;

	/// <summary>
	/// 初期化
	/// </summary>
	/// <param name="SetEndFloor"></param>
	/// <param name="SetEndDungeon"></param>
	public void Initialize(
		System.Action<eFloorEndReason> SetEndFloor,
		System.Action<eDungeonEndReason> SetEndDungeon) {
		_moveActionList = new List<MoveAction>(FLOOR_ENEMY_MAX + 1);

		_acceptPlayerAction = new AcceptPlayerAction();
		_acceptPlayerAction.Initialize(moveAction => _moveActionList.Add(moveAction));

		_EndFloor = SetEndFloor;
		_EndDungeon = SetEndDungeon;
		// 移動アクションにフロア、ダンジョン終了処理を渡す
		MoveAction.SetEndProcess(EndFloor, EndDungeon);
		// プレイヤーにダンジョン終了処理を渡す
		PlayerCharacter.SetEndDungeonProcess(EndDungeon);
		// AIに移動の追加処理を渡す
		CharacterAIBase.SetAddMoveCallback(moveAction => _moveActionList.Add(moveAction));
	}

	/// <summary>
	/// 1ターンの実行処理
	/// </summary>
	/// <returns></returns>
	public async UniTask Execute() {
		_isContinueTurn = true;
		// プレイヤーの入力受付、移動以外の行動実行
		await AcceptPlayerAction();
		// 全エネミーの思考
		ExecuteAllCharacter(character => character.ThinkAction());
		// 全キャラクターの移動
		await MoveAllCharacter();
		// 全エネミーの移動以外の行動
		await ActionAllCharacter();
		// ターン終了時の処理
		await OnEndTurn();
	}

	/// <summary>
	/// プレイヤーの入力受付、行動実行
	/// </summary>
	/// <returns></returns>
	private async UniTask AcceptPlayerAction() {
		// 継続移動があるか判定
		if (_acceptPlayerAction.AcceptMove()) return;
		// 全てのキャラクター全てのキャラクターを待機アニメーションにする(ラムダ式を使う)
		ExecuteAllCharacter(character => character.SetAnimation(eCharacterAnimation.Wait));
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

	/// <summary>
	/// 全てのキャラクターの予定行動実行
	/// </summary>
	/// <returns></returns>
	private async UniTask ActionAllCharacter() {
		await ExecuteTaskAllCharacter(ExecuteScheduleAction);
	}

	/// <summary>
	/// ターンが継続中なら予定行動を実行
	/// </summary>
	/// <param name="character"></param>
	/// <returns></returns>
	private async UniTask ExecuteScheduleAction(CharacterBase character) {
		if (_isContinueTurn) {
			await character.ExecuteScheduleAction();
		} else {
			character.ResetScheduleAction();
		}
	}

	/// <summary>
	/// ターン終了時処理
	/// </summary>
	/// <returns></returns>
	private async UniTask OnEndTurn() {
		// 全てのキャラクターにターン終了時処理を行わせる
		ExecuteAllCharacter(character => character.OnEndTurn());
		await UniTask.CompletedTask;
	}

	/// <summary>
	/// ターンを終了させる
	/// </summary>
	private void EndTurn() {
		_isContinueTurn = false;
	}

	/// <summary>
	/// フロアを終了させる
	/// </summary>
	/// <param name="endReason"></param>
	private void EndFloor(eFloorEndReason endReason) {
		_EndFloor?.Invoke(endReason);
		EndTurn();
	}

	private void EndDungeon(eDungeonEndReason endReason) {
		// ダンジョンを終了させる
		_EndDungeon?.Invoke(endReason);
		// フロアとターンを終了させる
		EndFloor(endReason.GetFloorEndReason());
	}

}
