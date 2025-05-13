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

using static MapSquareUtility;
using static CharacterUtility;
using static CommonModule;

public class FloorProcessor {
	private TurnProcessor _turnProcessor = null;

	private eFloorEndReason _endReason = eFloorEndReason.Invalid;

	public void Initialize() {
		_turnProcessor = new TurnProcessor();
		_turnProcessor.Initialize();
	}

	/// <summary>
	/// フロア実行処理
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
		// マップ生成
		MapCreater.CreateMap();
		// プレイヤー配置
		SetPlayer();
		// フロアを終了していない状態にする
		_endReason = eFloorEndReason.Invalid;
	}

	private void SetPlayer() {
		// プレイヤー取得
		CharacterBase player = GetPlayer();
		if (player == null) return;
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

	}
}
