/**
 * @file ActionRange00_DirForward.cs
 * @brief キャラの向き前方1マス（通常攻撃用）
 * @author yao
 * @date 2025/6/12
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static CharacterUtility;
using static MapSquareUtility;
using static CommonModule;

public class ActionRange00_DirForward : ActionRangeBase {

	/// <summary>
	/// 対象取得の実行処理
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
		// 相対敵なら対象にとる
		CharacterBase targetCharacter = GetCharacterData(targetSquare.characterID);
		if (IsRelativeEnemy(sourceCharacter, targetCharacter)) targetList.Add(targetSquare.characterID);

	}


}
