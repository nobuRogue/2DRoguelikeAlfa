/**
 * @file AcceptPlayerAction.cs
 * @brief プレイヤー入力受付、実行
 * @author yao
 * @date 2025/5/13
 */

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static CharacterUtility;
using static UnityEngine.Input;

public class AcceptPlayerAction {

	public async UniTask AcceptInput() {

	}

	/// <summary>
	/// 移動の受付、内部処理
	/// </summary>
	/// <returns>移動したらTrue</returns>
	public bool AcceptMove() {
		// 8方向の入力を受け付ける
		eDirectionEight inputDir = AcceptDirInput();
		if (inputDir == eDirectionEight.Invalid) return false;
		// 移動可否の判定
		CharacterBase player = GetPlayer();

		// 受け付けた入力に応じて移動
		return true;
	}

	private eDirectionEight AcceptDirInput() {
		if (GetKey(KeyCode.UpArrow)) {
			if (GetKey(KeyCode.RightArrow)) {
				return eDirectionEight.UpRight; // 右上
			} else if (GetKey(KeyCode.LeftArrow)) {
				return eDirectionEight.UpLeft;  // 左上
			} else {
				return eDirectionEight.Up;      // 上
			}
		} else if (GetKey(KeyCode.DownArrow)) {
			if (GetKey(KeyCode.RightArrow)) {
				return eDirectionEight.DownRight;// 右下
			} else if (GetKey(KeyCode.LeftArrow)) {
				return eDirectionEight.DownLeft;// 左下
			} else {
				return eDirectionEight.Down;    // 下
			}
		} else {
			if (GetKey(KeyCode.RightArrow)) {
				return eDirectionEight.Right;   // 右
			} else if (GetKey(KeyCode.LeftArrow)) {
				return eDirectionEight.Left;    // 左
			}
		}
		return eDirectionEight.Invalid;
	}

}
