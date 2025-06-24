/**
 * @file CharacterManager.cs
 * @brief キャラクター管理
 * @author yao
 * @date 2025/5/8
 */

using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static CommonModule;
using static GameConst;

public class CharacterManager : MonoBehaviour {
	/// 自身への参照
	public static CharacterManager instance { get; private set; } = null;

	// 使用中キャラクターオブジェクトの親オブジェクト
	[SerializeField]
	private Transform _useObjectRoot = null;
	// 未使用キャラクターオブジェクトの親オブジェクト
	[SerializeField]
	private Transform _unuseObjectRoot = null;
	/// キャラクターオブジェクトのオリジナル
	[SerializeField]
	private CharacterObject _originObject = null;

	// 使用中のキャラクターリスト
	private List<CharacterBase> _useList = null;
	// 未使用状態のプレイヤーリスト
	private List<PlayerCharacter> _unusePlayerList = null;
	// 未使用状態のエネミーリスト
	private List<EnemyCharacter> _unuseEnemyList = null;

	// 使用中のキャラクターオブジェクトリスト
	private List<CharacterObject> _useObjectList = null;
	// 未使用状態のキャラクターオブジェクトリスト
	private List<CharacterObject> _unuseObjectList = null;

	public void Initialize() {
		instance = this;
		// キャラクター情報を必要数生成して未使用状態にしておく
		_useList = new List<CharacterBase>(FLOOR_ENEMY_MAX + 1);

		_unusePlayerList = new List<PlayerCharacter>(1);
		_unusePlayerList.Add(new PlayerCharacter());

		_unuseEnemyList = new List<EnemyCharacter>(FLOOR_ENEMY_MAX);
		for (int i = 0; i < FLOOR_ENEMY_MAX; i++) {
			_unuseEnemyList.Add(new EnemyCharacter());
		}
		// キャラクターオブジェクトを必要数生成して未使用状態にしておく
		_useObjectList = new List<CharacterObject>(FLOOR_ENEMY_MAX + 1);
		_unuseObjectList = new List<CharacterObject>(FLOOR_ENEMY_MAX + 1);
		for (int i = 0; i < FLOOR_ENEMY_MAX + 1; i++) {
			_unuseObjectList.Add(Instantiate(_originObject, _unuseObjectRoot));
		}
	}

	/// <summary>
	/// プレイヤーキャラクター生成
	/// </summary>
	/// <param name="squareData"></param>
	/// <param name="masterID"></param>
	public void UsePlayer(MapSquareData squareData, int masterID) {
		// キャラクター情報のインスタンスの取得
		PlayerCharacter usePlayer;
		if (IsEmpty(_unusePlayerList)) {
			// 未使用がないので生成
			usePlayer = new PlayerCharacter();
		} else {
			// 未使用リストから使う
			usePlayer = _unusePlayerList[0];
			_unusePlayerList.RemoveAt(0);
		}
		// 使用可能なIDを割り当て使用状態にする
		UseCharacter(usePlayer, squareData, masterID);
	}

	/// <summary>
	/// エネミーキャラクターの生成
	/// </summary>
	/// <param name="squareData"></param>
	/// <param name="masterID"></param>
	public void UseEnemy(MapSquareData squareData, int masterID) {
		// エネミー情報のインスタンスの取得
		EnemyCharacter useEnemy;
		if (IsEmpty(_unuseEnemyList)) {
			// 未使用がないので生成
			useEnemy = new EnemyCharacter();
		} else {
			// 未使用リストから使う
			useEnemy = _unuseEnemyList[0];
			_unuseEnemyList.RemoveAt(0);
		}
		UseCharacter(useEnemy, squareData, masterID);
	}

	/// <summary>
	/// キャラクターを使用状態にする
	/// </summary>
	/// <param name="useCharacter"></param>
	/// <param name="squareData"></param>
	/// <param name="masterID"></param>
	private void UseCharacter(CharacterBase useCharacter, MapSquareData squareData, int masterID) {
		// 使用可能なIDを取得して使用リストに追加
		int useID = -1;
		for (int i = 0, max = _useList.Count; i < max; i++) {
			if (_useList[i] != null) continue;
			// 使用リストに使える場所が見つかった
			useID = i;
			_useList[i] = useCharacter;
			break;
		}
		if (useID < 0) {
			// 使用リストに使える場所がなかったので末尾に追加
			useID = _useList.Count;
			_useList.Add(useCharacter);
		}
		// オブジェクトの取得
		CharacterObject useObject = null;
		if (IsEmpty(_unuseObjectList)) {
			// 未使用がないので生成
			useObject = Instantiate(_originObject);
		} else {
			// 未使用リストから使う
			useObject = _unuseObjectList[0];
			_unuseObjectList.RemoveAt(0);
		}
		// オブジェクトの使用リストへの追加
		while (!IsEnableIndex(_useObjectList, useID)) _useObjectList.Add(null);

		_useObjectList[useID] = useObject;
		useObject.transform.SetParent(_useObjectRoot);

		// インスタンスの使用準備をする
		useCharacter.Setup(useID, squareData, masterID);
	}

	/// <summary>
	/// ID指定のキャラクターデータ取得
	/// </summary>
	/// <param name="ID"></param>
	/// <returns></returns>
	public CharacterBase GetCharacterData(int ID) {
		if (!IsEnableIndex(_useList, ID)) return null;

		return _useList[ID];
	}

	/// <summary>
	/// ID指定のキャラクターオブジェクト取得
	/// </summary>
	/// <param name="ID"></param>
	/// <returns></returns>
	public CharacterObject GetCharacterObject(int ID) {
		if (!IsEnableIndex(_useObjectList, ID)) return null;

		return _useObjectList[ID];
	}

	/// <summary>
	/// プレイヤーを未使用状態にする
	/// </summary>
	/// <param name="unusePlayer"></param>
	public void UnusePlayer(PlayerCharacter unusePlayer) {
		if (unusePlayer == null) return;
		// 未使用リストに加える
		_unusePlayerList.Add(unusePlayer);
		UnuseCharacter(unusePlayer);
	}

	/// <summary>
	/// エネミーを未使用状態にする
	/// </summary>
	/// <param name="unuseEnemy"></param>
	public void UnuseEnemy(EnemyCharacter unuseEnemy) {
		if (unuseEnemy == null) return;
		// 未使用リストに加える
		_unuseEnemyList.Add(unuseEnemy);
		UnuseCharacter(unuseEnemy);
	}

	/// <summary>
	/// キャラクターを未使用状態にする
	/// </summary>
	/// <param name="unuseCharacter"></param>
	private void UnuseCharacter(CharacterBase unuseCharacter) {
		// 使用リストから取り除く
		int unuseID = unuseCharacter.ID;
		_useList[unuseID] = null;
		// 片付け処理を呼ぶ
		unuseCharacter.Teardown();
		// オブジェクトを未使用にする
		CharacterObject unuseObject = GetCharacterObject(unuseID);
		_useObjectList[unuseID] = null;
		_unuseObjectList.Add(unuseObject);
		unuseObject.transform.SetParent(_unuseObjectRoot);
	}

	/// <summary>
	/// プレイヤー取得
	/// </summary>
	/// <returns></returns>
	public CharacterBase GetPlayer() {
		if (IsEmpty(_useList)) return null;

		for (int i = 0, max = _useList.Count; i < max; i++) {
			// プレイヤーか否か判定
			if (_useList[i] == null ||
				!_useList[i].IsPlayer()) continue;

			return _useList[i];
		}
		return null;
	}

	/// <summary>
	/// 全てのキャラクターに指定処理実行
	/// </summary>
	/// <param name="action"></param>
	public void ExecuteAllCharacter(System.Action<CharacterBase> action) {
		if (action == null || IsEmpty(_useList)) return;

		for (int i = 0, max = _useList.Count; i < max; i++) {
			if (_useList[i] == null) continue;

			action(_useList[i]);
		}
	}

	/// <summary>
	/// 全てのキャラクターに指定タスク実行
	/// </summary>
	/// <param name="task"></param>
	/// <returns></returns>
	public async UniTask ExecuteTaskAllCharacter(System.Func<CharacterBase, UniTask> task) {
		if (task == null || IsEmpty(_useList)) return;

		for (int i = 0, max = _useList.Count; i < max; i++) {
			if (_useList[i] == null) continue;

			await task(_useList[i]);
		}
	}

}
