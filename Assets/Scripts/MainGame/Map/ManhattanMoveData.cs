/**
 * @file ManhattanMoveData.cs
 * @brief 4方向移動の１マス分の移動データ
 * @author yao
 * @date 2025/4/24
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManhattanMoveData {
	// 移動元のマスID
	public int sourceSquareID = -1;
	// 移動先のマスID
	public int targetSquareID = -1;
	// 移動した方向
	public eDirectionFour dir = eDirectionFour.Invalid;

	public ManhattanMoveData(int setSourceID, int setTargetID, eDirectionFour setDir) {
		sourceSquareID = setSourceID;
		targetSquareID = setTargetID;
		dir = setDir;
	}

}
