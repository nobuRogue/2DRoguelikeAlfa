/**
 * @file UserDataHolder.cs
 * @brief ユーザーデータ保持
 * @author yao
 * @date 2025/5/20
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserDataHolder {
	// 現在のユーザーデータ
	public static UserData currentData { get; private set; } = null;

	/// <summary>
	/// 現在のユーザデータ設定
	/// </summary>
	/// <param name="setData"></param>
	public static void SetCurrentData(UserData setData) {
		currentData = setData;
	}
}
