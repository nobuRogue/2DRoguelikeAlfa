/**
 * @file ActionRange02_Shoot.cs
 * @brief キャラの向き前方10マス（射撃用）
 * @author yao
 * @date 2025/7/24
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static MapSquareUtility;
using static CharacterUtility;
using static CommonModule;

public class ActionRange02_Shoot : ActionRangeBase {
	// 射程のマス数
	private const int _RANGE_COUNT = 10;

	/// <summary>
	/// 対象取得の実行処理
	/// </summary>
	/// <param name="sourceCharacter"></param>
	public override void Execute(CharacterBase sourceCharacter) {
		InitializeList(ref targetList);
		// 行動者のいる座標を取得
		int sourceX = sourceCharacter.posX, sourceY = sourceCharacter.posY;
		eDirectionEight sourceDir = sourceCharacter.direction;
		for (int i = 0; i < _RANGE_COUNT; i++) {
			// 行動者の向きの隣接マスを取得
			MapSquareData targetSquare = GetToDirSquare(sourceX, sourceY, sourceDir);
			// 壁なら終了
			if (targetSquare == null ||
				targetSquare.terrain == eTerrain.Wall) break;
			// 対象マスにキャラがいなければ継続
			sourceX = targetSquare.posX;
			sourceY = targetSquare.posY;
			if (!targetSquare.existCharacter) continue;
			// 対象マスのキャラを対象にして終了
			targetList.Add(targetSquare.characterID);
			break;
		}
	}

	/// <summary>
	/// AI用使用可否判定
	/// </summary>
	/// <param name="sourceCharacter"></param>
	/// <param name="dir"></param>
	/// <returns></returns>
	public override bool CanUse(CharacterBase sourceCharacter, ref eDirectionEight dir) {
		// 全ての向きで判定する
		for (int dirIndex = 0, max = (int)eDirectionEight.Max; dirIndex < max; dirIndex++) {
			// 相対敵がみつからなかったら継続
			eDirectionEight checkDir = dirIndex.ToDirEight();
			if (!ExistRelativeEnemy(sourceCharacter, checkDir)) continue;
			// 相対敵が見つかったのでtrueを返す
			dir = checkDir;
			return true;
		}
		return false;
	}

	/// <summary>
	/// 指定の向きで射程に相対敵が含まれるか否か判定
	/// </summary>
	/// <param name="sourceCharacter"></param>
	/// <param name="dir"></param>
	/// <returns></returns>
	private bool ExistRelativeEnemy(CharacterBase sourceCharacter, eDirectionEight dir) {
		int sourceX = sourceCharacter.posX, sourceY = sourceCharacter.posY;
		for (int i = 0; i < _RANGE_COUNT; i++) {
			MapSquareData targetSquare = GetToDirSquare(sourceX, sourceY, dir);
			// 壁なら終了
			if (targetSquare == null ||
				targetSquare.terrain == eTerrain.Wall) return false;
			// 対象マスにキャラがいなければ継続
			if (!targetSquare.existCharacter) continue;
			// 対象マスのキャラが相対敵か否かを返す
			return IsRelativeEnemy(sourceCharacter, GetCharacterData(targetSquare.characterID));
		}
		return false;
	}

}
