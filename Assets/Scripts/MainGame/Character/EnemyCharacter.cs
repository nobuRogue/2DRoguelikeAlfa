/**
 * @file EnemyCharacter.cs
 * @brief エネミーキャラクター情報
 * @author yao
 * @date 2025/5/8
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static CharacterUtility;

public class EnemyCharacter : CharacterBase {
	/// <summary>
	/// 行動AI
	/// </summary>
	private CharacterAIBase _actionAI = null;

	public override void Setup(int setID, MapSquareData squareData, int setMasterID) {
		base.Setup(setID, squareData, setMasterID);
		_actionAI = new CharacterAI00_Normal(ID);
	}

	public override bool IsPlayer() {
		return false;
	}

	/// <summary>
	/// 行動の思考
	/// </summary>
	public override void ThinkAction() {
		_actionAI.ThinkAction();
	}

	/// <summary>
	/// 死亡時処理
	/// </summary>
	/// <exception cref="System.NotImplementedException"></exception>
	public override void Dead() {
		// 自身を削除
		UnuseEnemy(this);
	}
}
