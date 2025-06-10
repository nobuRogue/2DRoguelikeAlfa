/**
 * @file RoomData.cs
 * @brief マップ上の1部屋の情報
 * @author yao
 * @date 2025/4/22
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static MapSquareUtility;

public class RoomData {
	/// <summary>
	/// 識別ID
	/// </summary>
	public int roomID { get; private set; } = -1;
	/// <summary>
	/// 部屋のマスのリスト
	/// </summary>
	public List<int> squareIDList { get; private set; } = null;

	/// <summary>
	/// 使用前の準備
	/// </summary>
	/// <param name="setID"></param>
	/// <param name="setSquareIDList"></param>
	public void Setup(int setID, List<int> setSquareIDList) {
		roomID = setID;
		squareIDList = setSquareIDList;
		// マス情報に所属する部屋IDを設定
		for (int i = 0, max = squareIDList.Count; i < max; i++) {
			MapSquareData square = GetSquareData(squareIDList[i]);
			if (square == null) continue;

			square.SetRoomID(roomID);
		}
	}

	/// <summary>
	/// 使用後の片付け
	/// </summary>
	public void Teardown() {
		// マス情報の部屋IDを初期化
		for (int i = 0, max = squareIDList.Count; i < max; i++) {
			MapSquareData square = GetSquareData(squareIDList[i]);
			if (square == null) continue;

			square.SetRoomID(-1);
		}
		roomID = -1;
		squareIDList = null;
	}

}
