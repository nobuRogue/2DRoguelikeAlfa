/**
 * @file CharacterManager.cs
 * @brief キャラクター管理
 * @author yao
 * @date 2025/5/8
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static GameConst;

public class CharacterManager : MonoBehaviour {
	/// 自身への参照
	public static CharacterManager instance { get; private set; } = null;

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

	}
}
