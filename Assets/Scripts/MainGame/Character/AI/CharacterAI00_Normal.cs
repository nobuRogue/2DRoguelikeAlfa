/**
 * @file CharacterAI00_Normal.cs
 * @brief プレイヤーが視界に居るなら近づくキャラクターAI
 * @author yao
 * @date 2025/5/29
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static RouteSearcher;
using static CharacterUtility;
using static MapSquareUtility;
using static ActionRangeManager;
using static ActionMasterUtility;

using static GameConst;
using static CommonModule;
using static Unity.VisualScripting.Member;

public class CharacterAI00_Normal : CharacterAIBase {
	/// <summary>
	/// コンストラクタ
	/// </summary>
	/// <param name="sourceCharacter"></param>		↓基底クラス（CharacterAIBase）のコンストラクタを呼び出す
	public CharacterAI00_Normal(CharacterBase sourceCharacter) : base(sourceCharacter) {

	}

	/// <summary>
	/// 行動の思考
	/// </summary>
	public override void ThinkAction() {
		// 視界にプレイヤーが居るか判定
		CharacterBase sourceCharacter = GetSourceCharacter();
		MapSquareData sourceSquare = GetSquareData(sourceCharacter.posX, sourceCharacter.posY);
		List<int> visibleArea = null;
		GetVisbleArea(ref visibleArea, sourceSquare);
		CharacterBase player = GetPlayer();
		bool visiblePlayer = visibleArea.Exists(player.ExistMoveTrail);
		if (visiblePlayer) {
			// 視界にプレイヤーが居るので可能な行動を探す
			CheckCanUseAction(sourceCharacter);
			if (_scheduleActionID >= 0) return;
			// 可能な行動が無ければプレイヤーに近づく
			CloseMoveToPlayer(player, sourceSquare, sourceCharacter);
		}
		else {
			// 視界にプレイヤーが居ないのでランダム移動
			RandomMove();
		}
	}

	/// <summary>
	/// 使用可能な行動があれば予定行動に設定する
	/// </summary>
	private void CheckCanUseAction(CharacterBase sourceCharacter) {
		if (IsEmpty(_actionList)) return;
		// 使用する行動をリストからランダムに決定
		int useActionID = _actionList[Random.Range(0, _actionList.Count)];
		// 使用する行動の使用可否判定
		Entity_ActionData.Param actionMaster = GetActionMaster(useActionID);
		if (actionMaster == null) return;

		ActionRangeBase range = GetRange(actionMaster.rangeType);
		eDirectionEight canUseDir = eDirectionEight.Invalid;
		if (range == null || !range.CanUse(sourceCharacter, ref canUseDir)) return;
		// 予定行動に設定
		SetScheduleAction(useActionID);
	}

	/// <summary>
	/// プレイヤーに近づく移動
	/// </summary>
	/// <param name="player"></param>
	/// <param name="sourceSquare"></param>
	/// <param name="sourceCharacter"></param>
	private void CloseMoveToPlayer(CharacterBase player, MapSquareData sourceSquare, CharacterBase sourceCharacter) {
		MapSquareData playerSquare = GetSquareData(player.posX, player.posY);
		List<ChebyshevMoveData> toPlayerRoute = RouteSearchChebyshev(sourceSquare.ID, playerSquare.ID, CanPassCharacter);
		// 経路（toPlayerRoute）の要素1が有効なら（プレイヤーと隣接していなければ)プレイヤーに近づく移動を行う
		if (!IsEnableIndex(toPlayerRoute, 1)) return;
		// プレイヤーに近づく移動を行う
		MoveAction moveAction = new MoveAction();
		moveAction.ExecuteData(sourceCharacter, toPlayerRoute[0]);
		_AddMove?.Invoke(moveAction);
	}

	/// <summary>
	/// キャラクターの通行可否判定
	/// </summary>
	/// <param name="moveSquare"></param>
	/// <param name="dir"></param>
	/// <param name="distance"></param>
	/// <returns></returns>
	private bool CanPassCharacter(MapSquareData baseSquare, MapSquareData moveSquare, eDirectionEight dir, int distance) {
		// 移動先のキャラクターが取得
		CharacterBase squareCharacter = GetCharacterData(moveSquare.characterID);
		// キャラクターがいなければ通常の通行可否判定
		if (squareCharacter == null) return CanMove(baseSquare.posX, baseSquare.posY, moveSquare, dir);
		// プレイヤーなら地形のみの通行可否判定
		return squareCharacter.IsPlayer() && CanMoveTerrain(baseSquare.posX, baseSquare.posY, moveSquare, dir);
	}

	/// <summary>
	/// ランダム移動処理
	/// </summary>
	private void RandomMove() {
		// 移動可能な方向を全て取得
		CharacterBase sourceCharacter = GetCharacterData(_sourceCharacterID);
		int sourceX = sourceCharacter.posX, sourceY = sourceCharacter.posY;
		int dirMax = (int)eDirectionEight.Max;
		// 移動可能な方向のリスト
		List<eDirectionEight> canMoveDirList = new List<eDirectionEight>(dirMax);
		// キャラクターの周囲8マスに、移動可否判定を行い全ての移動可能な方向を取得
		for (int i = 0; i < dirMax; i++) {
			eDirectionEight dir = (eDirectionEight)i;
			MapSquareData square = GetToDirSquare(sourceX, sourceY, dir);
			// 移動可否判定
			if (!CanMove(sourceX, sourceY, square, dir)) continue;

			canMoveDirList.Add(dir);
		}
		// 移動可能な方向が一つもないので終わり
		if (IsEmpty(canMoveDirList)) return;
		// ランダムな方向に移動
		eDirectionEight moveDir = canMoveDirList[Random.Range(0, canMoveDirList.Count)];
		MoveAction moveAction = new MoveAction();
		MapSquareData sourceSquare = GetSquareData(sourceX, sourceY);
		MapSquareData moveSquare = GetToDirSquare(sourceX, sourceY, moveDir);
		ChebyshevMoveData moveData = new ChebyshevMoveData(sourceSquare.ID, moveSquare.ID, moveDir);
		// 内部的な移動
		moveAction.ExecuteData(sourceCharacter, moveData);
		_AddMove?.Invoke(moveAction);
	}
}
