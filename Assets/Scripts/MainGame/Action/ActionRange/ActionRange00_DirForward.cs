/**
 * @file ActionRange00_DirForward.cs
 * @brief キャラの向き前方1マス（通常攻撃用）
 * @author yao
 * @date 2025/6/12
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static MapSquareUtility;

using static CommonModule;

public class ActionRange00_DirForward : ActionRangeBase {

	/// <summary>
	/// 
	/// </summary>
	/// <param name="sourceCharacter"></param>
	public override void Execute(CharacterBase sourceCharacter) {
		InitializeList(ref targetList);
		// キャラの居るマスから、キャラの向き前方1マスを取得
		int sourceX = sourceCharacter.posX, sourceY = sourceCharacter.posY;
		eDirectionEight sourceDir = sourceCharacter.direction;
		MapSquareData targetSquare = GetToDirSquare(sourceX, sourceY, sourceDir);
		// ↑のマスにキャラがいたら追加
		if (!targetSquare.existCharacter) return;
		// 攻撃可否判定
		if (!CanAttack(sourceX, sourceY, targetSquare, sourceDir)) return;
		// プレイヤーはエネミーだけを対象に、エネミーはプレイヤーだけを対象にとるようにする。
		// →相対的な敵か否かの判定
		targetList.Add(targetSquare.characterID);
	}
}
