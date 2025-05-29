/**
 * @file CharacterAIBase.cs
 * @brief キャラクターAIの基底
 * @author yao
 * @date 2025/5/29
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterAIBase {
	// 移動アクションの追加
	protected static System.Action<MoveAction> _AddMove = null;

	// 持ち主のキャラクターのID
	protected int _sourceCharacterID = -1;

	/// <summary>
	/// 移動アクション追加処理の設定
	/// </summary>
	/// <param name="setProcess"></param>
	public static void SetAddMoveCallback(System.Action<MoveAction> setProcess) {
		_AddMove = setProcess;
	}

	/// <summary>
	/// コンストラクタ
	/// </summary>
	/// <param name="sourceCharacterID"></param>
	public CharacterAIBase(int sourceCharacterID) {
		_sourceCharacterID = sourceCharacterID;
	}

	/// <summary>
	/// 行動の思考
	/// </summary>
	public abstract void ThinkAction();

}
