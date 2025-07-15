/**
 * @file CharacterAIBase.cs
 * @brief キャラクターAIの基底
 * @author yao
 * @date 2025/5/29
 */

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static ActionManager;
using static CharacterUtility;
using static ActionRangeManager;
using static ActionMasterUtility;

public abstract class CharacterAIBase {
	// 移動アクションの追加
	protected static System.Action<MoveAction> _AddMove = null;

	// 持ち主のキャラクターのID
	protected int _sourceCharacterID = -1;

	// 予定行動のID
	protected int _scheduleActionID { get; private set; } = -1;

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

	/// <summary>
	/// 持ち主のキャラクターデータ取得
	/// </summary>
	/// <returns></returns>
	protected CharacterBase GetSourceCharacter() {
		return GetCharacterData(_sourceCharacterID);
	}

	/// <summary>
	/// 予定行動の実行
	/// </summary>
	/// <returns></returns>
	public async UniTask ExecuteScheduleAction() {
		if (_scheduleActionID < 0) return;
		// 行動のマスター取得
		Entity_ActionData.Param actionMaster = GetActionMaster(_scheduleActionID);
		if (actionMaster == null) return;
		// 使用可否の判定
		CharacterBase sourceCharacter = GetSourceCharacter();
		ActionRangeBase range = GetRange(actionMaster.rangeType);
		eDirectionEight canUseDir = eDirectionEight.Invalid;
		if (range == null || !range.CanUse(sourceCharacter, ref canUseDir)) return;
		// 使用可能な向きに設定
		if (canUseDir != eDirectionEight.Invalid) sourceCharacter.SetDirection(canUseDir);
		// アクション実行
		await UseAction(sourceCharacter, _scheduleActionID);
	}

	/// <summary>
	/// 予定行動の設定
	/// </summary>
	/// <param name="setID"></param>
	protected void SetScheduleAction(int setID) {
		_scheduleActionID = setID;
	}

	/// <summary>
	/// 予定行動のクリア
	/// </summary>
	public void ResetScheduleAction() {
		_scheduleActionID = -1;
	}

}
