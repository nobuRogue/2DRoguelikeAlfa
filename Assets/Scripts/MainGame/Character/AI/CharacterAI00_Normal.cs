/**
 * @file CharacterAI00_Normal.cs
 * @brief プレイヤーが視界に居るなら近づくキャラクターAI
 * @author yao
 * @date 2025/5/29
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static CharacterUtility;
using static MapSquareUtility;
using static CommonModule;

public class CharacterAI00_Normal : CharacterAIBase {
	/// <summary>
	/// コンストラクタ
	/// </summary>
	/// <param name="sourceCharacterID"></param>		↓基底クラス（CharacterAIBase）のコンストラクタを呼び出す
	public CharacterAI00_Normal(int sourceCharacterID) : base(sourceCharacterID) {

	}

	/// <summary>
	/// 行動の思考
	/// </summary>
	public override void ThinkAction() {
		// 視界にプレイヤーが居るか判定
		CharacterBase sourceCharacter = GetCharacterData(_sourceCharacterID);
		MapSquareData sourceSquare = GetSquareData(sourceCharacter.posX, sourceCharacter.posY);
		List<int> visibleArea = null;
		GetVisbleArea(ref visibleArea, sourceSquare);
		CharacterBase player = GetPlayer();
		bool visiblePlayer = visibleArea.Exists(player.ExistMoveTrail);
		if (visiblePlayer) {
			// 視界にプレイヤーが居るので可能な行動を探す

			// 可能な行動が無ければプレイヤーに近づく


		} else {
			// 視界にプレイヤーが居ないのでランダム移動
			RandomMove();
		}
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
