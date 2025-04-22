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


}
