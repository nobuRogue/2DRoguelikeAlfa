/**
 * @file ExpansionMethod.cs
 * @brief 拡張メソッド集
 * @author yao
 * @date 2025/4/22
 */

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

using static MessageMasterUtility;

public static class ExpansionMethod {

	/// <summary>
	/// 反対方向を取得
	/// </summary>
	/// <param name="dir"></param>
	/// <returns></returns>
	public static eDirectionFour ReverseDir(this eDirectionFour dir) {
		int result = (int)dir + 2;
		if (result >= (int)eDirectionFour.Max) result -= (int)eDirectionFour.Max;

		return (eDirectionFour)result;
	}

	/// <summary>
	/// 斜め方向か否か
	/// </summary>
	/// <param name="dir"></param>
	/// <returns></returns>
	public static bool IsSlant(this eDirectionEight dir) {
		return
			dir == eDirectionEight.UpRight ||
			dir == eDirectionEight.DownRight ||
			dir == eDirectionEight.DownLeft ||
			dir == eDirectionEight.UpLeft;
	}

	/// <summary>
	/// 斜め方向を2方向に分割
	/// </summary>
	/// <param name="dir"></param>
	/// <returns></returns>
	public static eDirectionFour[] Separate(this eDirectionEight dir) {
		eDirectionFour[] result = new eDirectionFour[2];
		switch (dir) {
			case eDirectionEight.UpRight:
			result[0] = eDirectionFour.Up;
			result[1] = eDirectionFour.Right;
			break;
			case eDirectionEight.DownRight:
			result[0] = eDirectionFour.Down;
			result[1] = eDirectionFour.Right;
			break;
			case eDirectionEight.DownLeft:
			result[0] = eDirectionFour.Down;
			result[1] = eDirectionFour.Left;
			break;
			case eDirectionEight.UpLeft:
			result[0] = eDirectionFour.Up;
			result[1] = eDirectionFour.Left;
			break;
		}
		return result;
	}

	/// <summary>
	/// ダンジョン終了要因からフロア終了要因を取得
	/// </summary>
	/// <param name="dEndReason"></param>
	/// <returns></returns>
	public static eFloorEndReason GetFloorEndReason(this eDungeonEndReason dEndReason) {
		switch (dEndReason) {
			case eDungeonEndReason.Dead:
			return eFloorEndReason.Dead;
			case eDungeonEndReason.Clear:
			return eFloorEndReason.Stair;
		}
		return eFloorEndReason.Invalid;
	}

	/// <summary>
	/// IDからメッセージ取得
	/// </summary>
	/// <param name="messageID"></param>
	/// <returns></returns>
	public static string ToMessage(this int messageID) {
		return GetMessageData(messageID, 0);
	}
}
