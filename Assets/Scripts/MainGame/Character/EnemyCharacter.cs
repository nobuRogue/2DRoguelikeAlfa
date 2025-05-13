/**
 * @file EnemyCharacter.cs
 * @brief エネミーキャラクター情報
 * @author yao
 * @date 2025/5/8
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : CharacterBase {
	public override bool IsPlayer() {
		return false;
	}
}
