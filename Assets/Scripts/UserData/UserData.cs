/**
 * @file UserData.cs
 * @brief ユーザが持つデータ
 * @author yao
 * @date 2025/5/20
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData {
	// 現在の階数
	public int floorCount { get; private set; } = -1;

	/// <summary>
	/// コンストラクタ
	/// </summary>
	public UserData() {
		SetFloorCount(1);
	}

	/// <summary>
	/// 階数設定
	/// </summary>
	/// <param name="setCount"></param>
	public void SetFloorCount(int setCount) {
		floorCount = setCount;
	}

}
