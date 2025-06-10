/**
 * @file CharacterBase.cs
 * @brief キャラクター情報の基底
 * @author yao
 * @date 2025/5/8
 */

using UnityEngine;

using static MapSquareUtility;

public abstract class CharacterBase {
	public int ID { get; private set; } = -1;
	public int masterID { get; protected set; } = -1;
	public int posX { get; protected set; } = -1;
	public int posY { get; protected set; } = -1;

	// マスターデータ依存の変数
	public int nameID { get; protected set; } = -1;
	public int maxHP { get; protected set; } = -1;
	public int HP { get; protected set; } = -1;
	public int rawAttack { get; protected set; } = -1;
	public int rawDefense { get; protected set; } = -1;

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
	}

	/// <summary>
	/// マスターデータ関連の準備
	/// </summary>
	/// <param name="setMasterID"></param>
	protected virtual void SetupMaster(Entity_CharacterData.Param characterMaster) {
		if (characterMaster == null) return;

		nameID = characterMaster.nameID;
		maxHP = characterMaster.HP;
		HP = maxHP;
		rawAttack = characterMaster.Attack;
		rawDefense = characterMaster.Defense;
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
	public void SetPosition(Vector3 position) {
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
}
