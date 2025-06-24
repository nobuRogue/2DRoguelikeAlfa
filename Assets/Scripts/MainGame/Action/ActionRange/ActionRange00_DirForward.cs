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
		if (targetSquare == null || !targetSquare.existCharacter) return;
		// 攻撃可否判定
		if (!CanAttack(sourceX, sourceY, targetSquare, sourceDir)) return;
		// 相対敵なら対象にとる
		CharacterBase targetCharacter = GetCharacterData(targetSquare.characterID);
		if (IsRelativeEnemy(sourceCharacter, targetCharacter)) targetList.Add(targetSquare.characterID);

	}

	/// <summary>
	/// 使用可否判定
	/// </summary>
	/// <param name="sourceCharacter"></param>
	/// <param name="dir"></param>
	/// <returns></returns>
	public override bool CanUse(CharacterBase sourceCharacter, ref eDirectionEight dir) {
		int sourceX = sourceCharacter.posX, sourceY = sourceCharacter.posY;
		// 8方向の前方1マスで判定
		for (int i = 0, max = (int)eDirectionEight.Max; i < max; i++) {
			eDirectionEight checkDir = (eDirectionEight)i;
			MapSquareData targetSquare = GetToDirSquare(sourceX, sourceY, checkDir);
			if (targetSquare == null || !targetSquare.existCharacter) continue;
			// 攻撃可否判定
			if (!CanAttack(sourceX, sourceY, targetSquare, checkDir)) continue;
			// 相対敵でなければ使用不可能
			CharacterBase targetCharacter = GetCharacterData(targetSquare.characterID);
			if (!IsRelativeEnemy(sourceCharacter, targetCharacter)) continue;
			// 使用可能な向きを設定して使用可で返す
			dir = checkDir;
			return true;
		}
		// 使用不可能
		return false;
	}

}
