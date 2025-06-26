/**
 * @file PlayerCharacter.cs
 * @brief プレイヤーキャラクター情報
 * @author yao
 * @date 2025/5/8
 */

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

using static MapSquareUtility;

public class PlayerCharacter : CharacterBase {
	// ダンジョン終了処理
	private static System.Action<eDungeonEndReason> _EndDungeon = null;
	private const int _PLAYER_MOVE_TRAIL_COUNT = 3;

	// 移動の軌跡のマスIDリスト
	private List<int> _moveTrailList = null;

	// 満腹度関連の定数
	// readonly:コンストラクタの終了まで変更可
	// const:コンパイル時に確定
	private const int _MAX_STAMINA = 10000;
	private const int _SHOW_STAMINA_RATIO = 100;
	private const int _TURN_DECREASE_STAMINA = 10;
	// 満腹度
	private int _stamina = 0;

	/// <summary>
	/// ダンジョン終了処理の受取
	/// </summary>
	/// <param name="setProcess"></param>
	public static void SetEndDungeonProcess(System.Action<eDungeonEndReason> setProcess) {
		_EndDungeon = setProcess;
	}

	public override void Setup(int setID, MapSquareData squareData, int setMasterID) {
		_moveTrailList = new List<int>(_PLAYER_MOVE_TRAIL_COUNT);
		SetStamina(_MAX_STAMINA);
		base.Setup(setID, squareData, setMasterID);
	}

	public override void SetSquareData(MapSquareData squareData) {
		if (squareData == null) return;

		base.SetSquareData(squareData);
		// 移動の軌跡に追加
		AddMoveTrail(squareData);
	}

	/// <summary>
	/// フロア終了時処理
	/// </summary>
	public override void OnEndFloor() {
		base.OnEndFloor();
		ClearMoveTrail();
	}

	/// <summary>
	/// 移動の軌跡に含まれているか
	/// </summary>
	/// <param name="squareID"></param>
	/// <returns></returns>
	public override bool ExistMoveTrail(int squareID) {
		return _moveTrailList.Contains(squareID);
	}

	/// <summary>
	/// 移動の軌跡を追加
	/// </summary>
	/// <param name="square"></param>
	private void AddMoveTrail(MapSquareData square) {
		// 既に軌跡に存在していたら処理しない
		if (_moveTrailList.Contains(square.ID)) return;
		// 軌跡が3マス分あったら最初の要素を取り除く
		if (_moveTrailList.Count >= _PLAYER_MOVE_TRAIL_COUNT) {
			GetSquareData(_moveTrailList[0])?.HideMark();
			_moveTrailList.RemoveAt(0);
		}
		// 軌跡に追加
		_moveTrailList.Add(square.ID);
		square.ShowMark(Color.red);
	}

	/// <summary>
	/// 移動の軌跡をクリア
	/// </summary>
	private void ClearMoveTrail() {
		for (int i = 0, max = _moveTrailList.Count; i < max; i++) {
			GetSquareData(_moveTrailList[i])?.HideMark();
		}
		_moveTrailList.Clear();
	}

	public override bool IsPlayer() {
		return true;
	}

	/// <summary>
	/// 素の攻撃力設定
	/// </summary>
	/// <param name="setValue"></param>
	public override void SetRawAttack(int setValue) {
		base.SetRawAttack(setValue);
		// UI更新
		MenuManager.instance.Get<MenuRogueMain>().SetAttack(GetAttack());
	}

	public override void SetRawDefense(int setValue) {
		base.SetRawDefense(setValue);
		// UI更新
		MenuManager.instance.Get<MenuRogueMain>().SetDefense(GetDefense());
	}

	/// <summary>
	/// 最大HP設定
	/// </summary>
	/// <param name="setValue"></param>
	public override void SetMaxHP(int setValue) {
		base.SetMaxHP(setValue);
		// UI更新
		MenuManager.instance.Get<MenuRogueMain>().SetHP(HP, maxHP);
	}

	/// <summary>
	/// HP更新
	/// </summary>
	/// <param name="setValue"></param>
	public override void SetHP(int setValue) {
		base.SetHP(setValue);
		// UI更新
		MenuManager.instance.Get<MenuRogueMain>().SetHP(HP, maxHP);
	}

	/// <summary>
	/// 死亡時処理
	/// </summary>
	public override void Dead() {
		// プレイヤー死亡でダンジョン終了
		_EndDungeon?.Invoke(eDungeonEndReason.Dead);
	}

	/// <summary>
	/// 表示満腹度取得
	/// </summary>
	/// <returns></returns>
	public override int GetShowStamina() {
		return (_stamina + _SHOW_STAMINA_RATIO - 1) / _SHOW_STAMINA_RATIO;
	}

	/// <summary>
	/// 満腹度取得
	/// </summary>
	/// <returns></returns>
	public override int GetStamina() {
		return _stamina;
	}

	/// <summary>
	/// 満腹度設定
	/// </summary>
	/// <param name="setValue"></param>
	public override void SetStamina(int setValue) {
		// 0～最大値に丸める
		_stamina = Mathf.Clamp(setValue, 0, _MAX_STAMINA);
		// UI
		MenuManager.instance.Get<MenuRogueMain>().SetStamina(GetShowStamina());
	}
}
