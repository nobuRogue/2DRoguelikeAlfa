/**
 * @file ChebyshevMoveData.cs
 * @brief 8方向移動の１マス分の移動データ
 * @author yao
 * @date 2025/5/13
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChebyshevMoveData {
	// 移動元のマスID
	public int sourceSquareID = -1;
	// 移動先のマスID
	public int targetSquareID = -1;
	// 移動した方向
	public eDirectionEight dir = eDirectionEight.Invalid;

	public ChebyshevMoveData(int setSourceID, int setTargetID, eDirectionEight setDir) {
		sourceSquareID = setSourceID;
		targetSquareID = setTargetID;
		dir = setDir;
	}

}
