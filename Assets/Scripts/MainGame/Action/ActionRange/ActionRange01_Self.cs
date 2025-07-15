/**
 * @file ActionRange01_Self.cs
 * @brief s“®Ò‚ğ‘ÎÛ‚Æ‚·‚éË’ö
 * @author yao
 * @date 2025/7/15
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static CommonModule;

public class ActionRange01_Self : ActionRangeBase {

	/// <summary>
	/// s“®Ò‚ğ‘ÎÛ‚É“ü‚ê‚é
	/// </summary>
	/// <param name="sourceCharacter"></param>
	public override void Execute(CharacterBase sourceCharacter) {
		InitializeList(ref targetList, 1);
		targetList.Add(sourceCharacter.ID);
	}

}
