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
		// プレイヤー配置
		SetCharacter();
		// フロアを終了していない状態にする
		_endReason = eFloorEndReason.Invalid;
		// フェードイン
		await FadeManager.instance.FadeIn();
	}

	private void SetCharacter() {
		// プレイヤー取得
		CharacterBase player = GetPlayer();
		if (player == null) return;
		// 全ての部屋マスを集約（配置の候補マスリスト）
		List<MapSquareData> roomSquareList = new List<MapSquareData>(MAP_SQUARE_COUNT);

		// プレイヤーを配置

		// エネミーを配置

		//------------------------------------------------
		// ランダムな部屋取得
		RoomData roomData = GetRandomRoom();
		if (roomData == null ||
			IsEmpty(roomData.squareIDList)) return;
		// 部屋のランダムなマス取得
		List<int> squareList = roomData.squareIDList;
		int squareID = squareList[Random.Range(0, squareList.Count)];
		MapSquareData playerSquare = GetSquareData(squareID);
		// プレイヤー配置
		player.SetSquare(playerSquare);
	}

	private async UniTask TeardownFloor() {
		// フェードアウト
		await FadeManager.instance.FadeOut();
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
