/**
 * @file ActionRangeManager.cs
 * @brief s“®‚ÌË’ö‚ÌŠÇ—
 * @author yao
 * @date 2025/6/12
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static CommonModule;

public class ActionRangeManager {
	// Ë’ö‚ÌƒŠƒXƒg
	private static List<ActionRangeBase> _rangeList = null;

	/// <summary>
	/// ‰Šú‰»
	/// </summary>
	public static void Initialize() {
		_rangeList = new List<ActionRangeBase>();
		_rangeList.Add(new ActionRange00_DirForward());
		_rangeList.Add(new ActionRange01_Self());
		_rangeList.Add(new ActionRange02_Shoot());
	}

	/// <summary>
	/// Ë’ö‚Ìæ“¾
	/// </summary>
	/// <param name="rangeType"></param>
	/// <returns></returns>
	public static ActionRangeBase GetRange(int rangeType) {
		if (!IsEnableIndex(_rangeList, rangeType)) return null;

		return _rangeList[rangeType];
	}

}
