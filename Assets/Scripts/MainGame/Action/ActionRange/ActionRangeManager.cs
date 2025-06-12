/**
 * @file ActionRangeManager.cs
 * @brief s“®‚ÌË’ö‚ÌŠî’ê
 * @author yao
 * @date 2025/6/12
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static CommonModule;

public class ActionRangeManager {
	// Ë’ö‚ÌƒŠƒXƒg
	private static List<ActionRangeBase> _actionRangeList = null;

	/// <summary>
	/// ‰Šú‰»
	/// </summary>
	public static void Initialize() {
		_actionRangeList = new List<ActionRangeBase>();

	}

	/// <summary>
	/// Ë’ö‚Ìæ“¾
	/// </summary>
	/// <param name="rangeType"></param>
	/// <returns></returns>
	public static ActionRangeBase GetRange(int rangeType) {
		if (!IsEnableIndex(_actionRangeList, rangeType)) return null;

		return _actionRangeList[rangeType];
	}

}
