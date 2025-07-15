/**
 * @file ActionManager.cs
 * @brief 行動と効果の管理
 * @author yao
 * @date 2025/6/12
 */

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static ItemMasterUtility;
using static ActionMasterUtility;
using static ActionRangeManager;
using static RogueLogUtility;
using static CommonModule;


public class ActionManager {
	// 効果のリスト
	private static List<ActionEffectBase> _effectList = null;
	// 行動ログのメッセージID
	private static readonly int _USE_ACTION_LOG_ID = 14010;
	private static readonly int _USE_ITEM_LOG_ID = 14011;

	public static void Initialize() {
		_effectList = new List<ActionEffectBase>();
		_effectList.Add(new ActionEffect000_Attack());
		_effectList.Add(new ActionEffect001_RecoveryHP());
		_effectList.Add(new ActionEffect002_RecoveryStamina());

	}

	/// <summary>
	/// アイテムの使用効果発動
	/// </summary>
	/// <param name="sourceCharacter"></param>
	/// <param name="useItem"></param>
	/// <returns></returns>
	public static async UniTask UseItem(CharacterBase sourceCharacter, ItemBase useItem) {
		// アイテムマスターから行動のマスタ－取得
		var itemMaster = GetItemMaster(useItem.masterID);
		if (itemMaster == null) return;

		var actionMaster = GetActionMaster(itemMaster.actionID);
		if (actionMaster == null) return;
		// ログ追加
		string characterName = sourceCharacter.GetName();
		string itemName = useItem.GetName();
		AddLog(string.Format(_USE_ITEM_LOG_ID.ToMessage(), characterName, itemName));
		await ExecuteAction(sourceCharacter, actionMaster);
	}

	/// <summary>
	/// 行動の実行
	/// </summary>
	/// <param name="sourceCharacter"></param>
	/// <param name="actionID"></param>
	/// <returns></returns>
	public static async UniTask UseAction(CharacterBase sourceCharacter, int actionID) {
		// 行動のマスター取得
		Entity_ActionData.Param actionMaster = GetActionMaster(actionID);
		if (actionMaster == null) return;
		// ログ追加
		string characterName = sourceCharacter.GetName();
		string actionName = actionMaster.nameID.ToMessage();
		AddLog(string.Format(_USE_ACTION_LOG_ID.ToMessage(), characterName, actionName));
		await ExecuteAction(sourceCharacter, actionMaster);
	}

	/// <summary>
	/// アクションの実行
	/// </summary>
	/// <param name="sourceCharacter"></param>
	/// <param name="actionMaster"></param>
	/// <returns></returns>
	private static async UniTask ExecuteAction(
		CharacterBase sourceCharacter,
		Entity_ActionData.Param actionMaster) {
		// 射程クラス取得、実行
		ActionRangeBase range = GetRange(actionMaster.rangeType);
		range.Execute(sourceCharacter);
		// アクションの効果処理
		int[] effectIDList = actionMaster.effectID;
		for (int i = 0, max = effectIDList.Length; i < max; i++) {
			if (effectIDList[i] < 0) continue;

			await ExecuteActionEffect(effectIDList[i], sourceCharacter, range);
		}
		await UniTask.DelayFrame(1);
	}

	/// <summary>
	/// 1効果の実行
	/// </summary>
	/// <param name="effectID"></param>
	/// <param name="sourceCharacter"></param>
	/// <param name="range"></param>
	/// <returns></returns>
	private static async UniTask ExecuteActionEffect(int effectID, CharacterBase sourceCharacter, ActionRangeBase range) {
		// 効果のマスター取得
		Entity_ActionEffectData.Param effectMaster = GetActionEffectMaster(effectID);
		if (effectMaster == null) return;

		int effectType = effectMaster.effectType;
		if (!IsEnableIndex(_effectList, effectType)) return;
		// 効果実行
		await _effectList[effectType].Execute(sourceCharacter, effectMaster, range);
	}

}
