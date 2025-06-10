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
	private readonly int _PLAYER_MOVE_TRAIL_COUNT = 3;

	// 移動の軌跡のマスIDリスト
	private List<int> _moveTrailList = null;

	public override void Setup(int setID, MapSquareData squareData, int setMasterID) {
		_moveTrailList = new List<int>(_PLAYER_MOVE_TRAIL_COUNT);
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
}
