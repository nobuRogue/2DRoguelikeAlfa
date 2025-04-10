/**
 * @file SystemObject.cs
 * @brief ゲーム全体で使用する機能の基底
 * @author yao
 * @date 2025/4/10
 */
using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class SystemObject : MonoBehaviour {
	/// <summary>
	/// 初期化処理
	/// </summary>
	/// <returns></returns>
	public abstract UniTask Initialize();
}
