/**
 * @file CharacterBase.cs
 * @brief キャラクター情報の基底
 * @author yao
 * @date 2025/5/8
 */

using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

using static MapSquareUtility;

public abstract class CharacterBase {
	public int ID { get; private set; } = -1;
	public int masterID { get; protected set; } = -1;
	public int posX { get; protected set; } = -1;
	public int posY { get; protected set; } = -1;
	// キャラクターの向き
	public eDirectionEight direction { get; protected set; } = eDirectionEight.Invalid;
	// マスターデータ依存の変数
	public int nameID { get; protected set; } = -1;
	public int maxHP { get; protected set; } = -1;
	public int HP { get; protected set; } = -1;
	// 死亡しているか
	public bool isDead { get { return HP <= 0; } }
	public int rawAttack { get; protected set; } = -1;
	public int rawDefense { get; protected set; } = -1;
	// 所持アイテムIDリスト
	public List<int> possessItemList { get; private set; } = null;
	// 所持アイテムの最大数
	private static readonly int _POSSESS_ITEM_MAX = 10;

	/// <summary>
	/// 使用前の準備
	/// </summary>
	/// <param name="setID"></param>
	/// <param name="squareData"></param>
	/// <param name="setMasterID"></param>
	public virtual void Setup(int setID, MapSquareData squareData, int setMasterID) {
		ID = setID;
		masterID = setMasterID;
		var characterMaster = CharacterMasterUtility.GetCharacterMaster(masterID);
		SetupMaster(characterMaster);
		// キャラクターをマスに置く
		SetSquare(squareData);
		// オブジェクトの準備
		GetObject()?.Setup(characterMaster.spriteName);
		// とりあえず下を向かせる
		SetDirection(eDirectionEight.Down);
		// 所持アイテム初期化
		possessItemList = new List<int>(_POSSESS_ITEM_MAX);
	}

	/// <summary>
	/// マスターデータ関連の準備
	/// </summary>
	/// <param name="setMasterID"></param>
	protected virtual void SetupMaster(Entity_CharacterData.Param characterMaster) {
		if (characterMaster == null) return;

		nameID = characterMaster.nameID;
		SetMaxHP(characterMaster.HP);
		SetHP(maxHP);
		SetRawAttack(characterMaster.Attack);
		SetRawDefense(characterMaster.Defense);
	}

	/// <summary>
	/// 使用後の片付け
	/// </summary>
	public virtual void Teardown() {
		// 今いるマスから取り除く
		GetSquareData(posX, posY)?.RemoveCharacter();
		posX = -1;
		posY = -1;
		// オブジェクトの片付け
		GetObject()?.Teardown();
	}

	/// <summary>
	/// オブジェクトの取得
	/// </summary>
	/// <returns></returns>
	protected CharacterObject GetObject() {
		return CharacterManager.instance.GetCharacterObject(ID);
	}

	/// <summary>
	/// キャラクターを指定マスに設定
	/// 見た目と情報、両方の変更
	/// </summary>
	/// <param name="squareData"></param>
	public void SetSquare(MapSquareData squareData) {
		// 情報の変更
		SetSquareData(squareData);
		// 見た目の変更
		SetPosition(squareData.GetCharacterRoot().position);
	}

	/// <summary>
	/// キャラクターを指定マスに設定
	/// 情報のみの変更
	/// </summary>
	/// <param name="squareData"></param>
	public virtual void SetSquareData(MapSquareData squareData) {
		if (squareData == null) return;
		// 今いるマスから取り除く
		GetSquareData(posX, posY)?.RemoveCharacter();
		// マスに設定する
		squareData.SetCharacter(ID);
		posX = squareData.posX;
		posY = squareData.posY;
	}

	/// <summary>
	/// 見た目のみの位置変更
	/// </summary>
	/// <param name="position"></param>
	public virtual void SetPosition(Vector3 position) {
		// キャラクターオブジェクトを取得し位置変更する
		GetObject()?.SetPosition(position);
	}

	/// <summary>
	/// プレイヤーか否か
	/// </summary>
	/// <returns></returns>
	public abstract bool IsPlayer();

	/// <summary>
	/// 行動の思考
	/// </summary>
	public virtual void ThinkAction() {

	}

	/// <summary>
	/// フロア終了時処理
	/// </summary>
	public virtual void OnEndFloor() {

	}

	/// <summary>
	/// 移動の軌跡に含まれているか
	/// </summary>
	/// <param name="squareID"></param>
	/// <returns></returns>
	public virtual bool ExistMoveTrail(int squareID) {
		return false;
	}

	/// <summary>
	/// キャラの向き設定
	/// </summary>
	/// <param name="setDir"></param>
	public void SetDirection(eDirectionEight setDir) {
		if (direction == setDir) return;

		direction = setDir;
		GetObject()?.SetDirection(direction);
	}

	/// <summary>
	/// 攻撃力取得
	/// </summary>
	/// <returns></returns>
	public int GetAttack() {
		return rawAttack;
	}

	/// <summary>
	/// 素の攻撃力設定
	/// </summary>
	/// <param name="setValue"></param>
	public virtual void SetRawAttack(int setValue) {
		rawAttack = setValue;
	}

	/// <summary>
	/// 防御力取得
	/// </summary>
	/// <returns></returns>
	public int GetDefense() {
		return rawDefense;
	}

	/// <summary>
	/// 素の防御力設定
	/// </summary>
	/// <param name="setValue"></param>
	public virtual void SetRawDefense(int setValue) {
		rawDefense = setValue;
	}

	/// <summary>
	/// 最大HP設定
	/// </summary>
	/// <param name="setValue"></param>
	public virtual void SetMaxHP(int setValue) {
		maxHP = setValue;
	}

	/// <summary>
	/// 現在HP設定
	/// </summary>
	/// <param name="setValue"></param>
	public virtual void SetHP(int setValue) {
		// 0～最大値に丸める
		HP = Mathf.Clamp(setValue, 0, maxHP);
	}

	/// <summary>
	/// HP回復
	/// </summary>
	/// <param name="addValue"></param>
	public void AddHP(int addValue) {
		SetHP(HP + addValue);
	}

	/// <summary>
	/// HP減少
	/// </summary>
	/// <param name="removeValue"></param>
	public void RemoveHP(int removeValue) {
		SetHP(HP - removeValue);
	}

	/// <summary>
	/// キャラクターの死亡
	/// </summary>
	public abstract void Dead();

	/// <summary>
	/// 表示用満腹度取得
	/// </summary>
	/// <returns></returns>
	public virtual int GetShowStamina() {
		return 0;
	}

	/// <summary>
	/// 満腹度取得
	/// </summary>
	/// <returns></returns>
	public virtual int GetStamina() {
		return 0;
	}

	/// <summary>
	/// 満腹度設定
	/// </summary>
	/// <param name="setValue"></param>
	public virtual void SetStamina(int setValue) {

	}

	/// <summary>
	/// 満腹度増加
	/// </summary>
	/// <param name="addValue"></param>
	public void AddStamina(int addValue) {
		SetStamina(GetStamina() + addValue);
	}

	/// <summary>
	/// 満腹度減少
	/// </summary>
	/// <param name="removeValue"></param>
	public void RemoveStamina(int removeValue) {
		SetStamina(GetStamina() - removeValue);
	}

	/// <summary>
	/// アニメーションの再生
	/// </summary>
	/// <param name="setAnim"></param>
	public void SetAnimation(eCharacterAnimation setAnim) {
		GetObject()?.SetAnimation(setAnim);
	}

	/// <summary>
	/// 再生中のアニメーション取得
	/// </summary>
	/// <returns></returns>
	public eCharacterAnimation GetCurrentAnimation() {
		CharacterObject characterObject = GetObject();
		if (characterObject == null) return eCharacterAnimation.Invalid;

		return characterObject.currentAnim;
	}

	/// <summary>
	/// 予定行動の実行
	/// </summary>
	/// <returns></returns>
	public virtual async UniTask ExecuteScheduleAction() {
		await UniTask.CompletedTask;
	}

	/// <summary>
	/// 予定行動のリセット
	/// </summary>
	public virtual void ResetScheduleAction() { }

	/// <summary>
	/// ターン終了時の処理
	/// </summary>
	public virtual void OnEndTurn() {

	}

	/// <summary>
	/// アイテムが入手可能か否か
	/// </summary>
	/// <returns></returns>
	public bool CanAddItem() {
		return possessItemList.Count < _POSSESS_ITEM_MAX;
	}
	/// <summary>
	/// アイテム入手
	/// </summary>
	/// <param name="addItemID"></param>
	public void AddItem(int addItemID) {
		possessItemList.Add(addItemID);
	}
	/// <summary>
	/// アイテム除去
	/// </summary>
	/// <param name="removeItemID"></param>
	public void RemoveItem(int removeItemID) {
		possessItemList.Remove(removeItemID);
	}

	/// <summary>
	/// 名前の取得
	/// </summary>
	/// <returns></returns>
	public string GetName() {
		return nameID.ToMessage();
	}
}
