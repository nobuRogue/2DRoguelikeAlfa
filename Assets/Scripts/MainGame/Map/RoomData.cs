/**
 * @file RoomData.cs
 * @brief マップ上の1部屋の情報
 * @author yao
 * @date 2025/4/22
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	}

	/// <summary>
	/// 使用後の片付け
	/// </summary>
	public void Teardown() {
		roomID = -1;
		squareIDList = null;
	}

}
