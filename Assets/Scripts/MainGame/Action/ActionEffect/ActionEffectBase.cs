/**
 * @file ActionEffectBase.cs
 * @brief s“®‚ÌŒø‰Ê‚ÌŠî’ê
 * @author yao
 * @date 2025/6/12
 */

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionEffectBase {

	/// <summary>
	/// Œø‰Ê‚ÌÀsˆ—
	/// </summary>
	/// <returns></returns>
	public abstract UniTask Execute(CharacterBase sourceCharacter, List<int> targetList);

}
