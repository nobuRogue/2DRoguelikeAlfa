/**
 * @file ActionManager.cs
 * @brief s“®‚ÆŒø‰Ê‚ÌŠÇ—
 * @author yao
 * @date 2025/6/12
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager {
	// Œø‰Ê‚ÌƒŠƒXƒg
	private static List<ActionEffectBase> _effectList = null;

	public static void Initialize() {
		_effectList = new List<ActionEffectBase>();

	}

}
