/**
 * @file FloorProcessor.cs
 * @brief フロア実行処理
 * @author yao
 * @date 2025/5/13
 */

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static TerrainSpriteAssignor;
using static ItemUtility;
using static FloorMasterUtility;
using static MapSquareUtility;
using static CharacterUtility;
using static CommonModule;
using static GameConst;

public class FloorProcessor {
	private TurnProcessor _turnProcessor = null;
	// フロアの終了状態
	private eFloorEndReason _endReason = eFloorEndReason.Invalid;

	/// <summary>
	/// 初期化
	/// </summary>
	/// <param name="SetEndDungeon"></param>
	public void Initialize(System.Action<eDungeonEndReason> SetEndDungeon) {
		_turnProcessor = new TurnProcessor();
		_turnProcessor.Initialize(EndFloor, SetEndDungeon);
	}

	/// <summary>
	/// 1フロア実行処理
	/// </summary>
	/// <returns></returns>
	public async UniTask Execute() {
		// フロアの生成
		await SetupFloor();
		// フロアの実行
		while (_endReason == eFloorEndReason.Invalid) {
			await _turnProcessor.Execute();
		}
		// フロアの破棄
		await TeardownFloor();
	}

	/// <summary>
	/// フロア生成
	/// </summary>
	private async UniTask SetupFloor() {
		// マップ地形画像設定
		var floorMaster = GetCurrentFloorMaster();
		SetFloorSpriteTypeIndex(floorMaster.spriteIndex);
		// マップ生成
		MapCreater.CreateMap();
		// アイテムとキャラクターの設置候補マスを集約
		List<MapSquareData> roomSquareList = new List<MapSquareData>(MAP_SQUARE_COUNT);
		ExecuteAllSquare(mapSquare => {
			if (mapSquare.terrain != eTerrain.Room) return;

			roomSquareList.Add(mapSquare);
		});
		// キャラクター配置
		SetCharacter(floorMaster.enemyTableID, roomSquareList);
		// アイテム配置
		SetFloorItem(8, roomSquareList);
		// フロアを終了していない状態にする
		_endReason = eFloorEndReason.Invalid;
		// フェードイン
		await FadeManager.instance.FadeIn();
	}

	/// <summary>
	/// キャラクターの配置
	/// </summary>
	/// <param name="enemyTableID"></param>
	/// <param name="roomSquareList"></param>
	private void SetCharacter(int enemyTableID, List<MapSquareData> roomSquareList) {
		// プレイヤー取得
		CharacterBase player = GetPlayer();
		if (player == null) return;
		// プレイヤーを配置
		MapSquareData playerSquare = roomSquareList[Random.Range(0, roomSquareList.Count)];
		player.SetSquare(playerSquare);
		roomSquareList.Remove(playerSquare);
		// エネミーを生成配置
		SpawnEnemy(8, roomSquareList, enemyTableID);
	}

	/// <summary>
	/// エネミーの生成、配置
	/// </summary>
	/// <param name="spawnCount"></param>
	/// <param name="candidateSquareList"></param>
	private void SpawnEnemy(int spawnCount, List<MapSquareData> candidateSquareList, int enemyTableID) {
		// 現在フロアのエネミーテーブルを取得
		List<int> enemyTable = GetEnemyTable(enemyTableID);
		if (IsEmpty(enemyTable)) return;

		for (int i = 0; i < spawnCount; i++) {
			if (IsEmpty(candidateSquareList)) return;
			// 候補マスからランダムに取得
			MapSquareData spawnSquare = candidateSquareList[Random.Range(0, candidateSquareList.Count)];
			// テーブルからランダムに出現エネミーのマスターIDを取得
			int enemyID = enemyTable[Random.Range(0, enemyTable.Count)];
			// エネミー生成
			UseEnemy(spawnSquare, enemyID);
			candidateSquareList.Remove(spawnSquare);
		}
	}

	/// <summary>
	/// 床落ちアイテムの生成
	/// </summary>
	private void SetFloorItem(int createCount, List<MapSquareData> roomSquareList) {
		for (int i = 0; i < createCount; i++) {
			if (IsEmpty(roomSquareList)) break;
			// 候補マスからランダムに取得
			MapSquareData itemSquare = roomSquareList[Random.Range(0, roomSquareList.Count)];
			roomSquareList.Remove(itemSquare);
			CreateFloorItem(0, itemSquare);
		}
	}

	/// <summary>
	/// フロア終了時の処理
	/// </summary>
	/// <returns></returns>
	private async UniTask TeardownFloor() {
		// フェードアウト
		await FadeManager.instance.FadeOut();
		// 全キャラクターのフロア終了時処理
		ExecuteAllCharacter(OnEndFloorCharacter);
		// 床落ちアイテム全削除
		ExecuteAllItem(OnEndFloorItem);
	}

	/// <summary>
	/// フロア終了時のキャラクターの処理
	/// </summary>
	/// <param name="character"></param>
	private void OnEndFloorCharacter(CharacterBase character) {
		// エネミーなら削除
		var enemy = character as EnemyCharacter;
		if (enemy != null) {
			// エネミーなら削除
			UnuseEnemy(enemy);
		}
		else {
			// フロア終了時処理
			character.OnEndFloor();
		}
	}
	/// <summary>
	/// フロア終了時のアイテム処理
	/// </summary>
	/// <param name="item"></param>
	private void OnEndFloorItem(ItemBase item) {
		// 床落ちアイテムなら削除
		if (item.posX < 0 || item.posY < 0) return;

		RemoveItem(item);
	}

	/// <summary>
	/// フロア終了
	/// </summary>
	/// <param name="endReason"></param>
	private void EndFloor(eFloorEndReason endReason) {
		_endReason = endReason;
		switch (_endReason) {
			case eFloorEndReason.Dead:
				break;
			case eFloorEndReason.Stair:
				// 階段で次の階層へ（階数+1する）
				UserData currentData = UserDataHolder.currentData;
				currentData.SetFloorCount(currentData.floorCount + 1);
				break;
		}

	}
}
