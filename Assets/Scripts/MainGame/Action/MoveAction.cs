/**
 * @file MoveAction.cs
 * @brief 移動アクション
 * @author yao
 * @date 2025/5/13
 */

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static CharacterUtility;
using static MapSquareUtility;

public class MoveAction {

	private int _moveCharacterID = -1;
	private ChebyshevMoveData _moveData = null;

	// フロアを終了させる処理
	private static System.Action<eFloorEndReason> _EndFloor = null;
	// ダンジョンを終了させる処理
	private static System.Action<eDungeonEndReason> _EndDungeon = null;

	/// <summary>
	/// フロア終了時処理の受取処理
	/// </summary>
	/// <param name="SetEndFloor"></param>
	public static void SetEndProcess(
		System.Action<eFloorEndReason> SetEndFloor,
		System.Action<eDungeonEndReason> SetEndDungeon) {
		_EndFloor = SetEndFloor;
		_EndDungeon = SetEndDungeon;
	}

	/// <summary>
	/// 階段に乗った時の処理
	/// </summary>
	/// <param name="goalSquare">移動先のマス</param>
	private async UniTask ProcessStair(MapSquareData goalSquare) {
		// 移動先が階段でなければ処理しない
		if (goalSquare.terrain != eTerrain.Stair) return;
		// 次の階層が存在するか判定
		int currentFloorCount = UserDataHolder.currentData.floorCount;
		var floorMaster = FloorMasterUtility.GetFloorMaster(currentFloorCount + 1);
		UniTask task = SoundManager.instance.PlaySE(5);
		if (floorMaster == null) {
			// 最後の階層なのでクリア（終了要因Clearでダンジョンを終了させる）
			_EndDungeon(eDungeonEndReason.Clear);
		} else {
			// 最後の階層でないのでフロア移動（終了要因Stairでフロアを終了させる）
			_EndFloor?.Invoke(eFloorEndReason.Stair);
		}
		await UniTask.CompletedTask;
	}

	/// <summary>
	/// 内部的な移動処理
	/// </summary>
	public void ExecuteData(CharacterBase moveCharacter, ChebyshevMoveData moveData) {
		_moveCharacterID = moveCharacter.ID;
		_moveData = moveData;

		moveCharacter.SetSquareData(GetSquareData(_moveData.targetSquareID));
	}

	/// <summary>
	/// 見た目上の移動
	/// </summary>
	/// <returns></returns>
	public async UniTask ExecuteObject(float duration) {
		// キャラクター、移動元、移動先の取得
		CharacterBase moveCharacter = GetCharacterData(_moveCharacterID);
		MapSquareData startSquare = GetSquareData(_moveData.sourceSquareID);
		MapSquareData goalSquare = GetSquareData(_moveData.targetSquareID);

		Vector3 startPos = startSquare.GetCharacterRoot().position;
		Vector3 goalPos = goalSquare.GetCharacterRoot().position;
		// 移動処理
		float elapsedTime = 0.0f;// 経過時間
		while (elapsedTime < duration) {
			// フレーム時間経過
			elapsedTime += Time.deltaTime;
			// 補間した座標取得、キャラに設定
			float t = elapsedTime / duration;
			Vector3 setPos = Vector3.Lerp(startPos, goalPos, t);
			moveCharacter.SetPosition(setPos);
			// 1フレーム待ち
			await UniTask.DelayFrame(1);
		}
		moveCharacter.SetPosition(goalPos);
		// 移動後処理
		await AfterMoveProcess(moveCharacter, goalSquare);
	}

	/// <summary>
	/// 移動後の処理
	/// </summary>
	/// <param name="moveCharacter"></param>
	/// <param name="goalSquare"></param>
	private async UniTask AfterMoveProcess(CharacterBase moveCharacter, MapSquareData goalSquare) {
		// プレイヤーでなければ移動後処理は行わない
		if (!moveCharacter.IsPlayer()) return;
		// 移動先に階段があったらフロア移動
		await ProcessStair(goalSquare);
	}



}
