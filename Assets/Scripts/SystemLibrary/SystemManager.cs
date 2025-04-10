/**
 * @file SystemManager.cs
 * @brief ゲーム全体で使用する機能の管理
 * @author yao
 * @date 2025/4/10
 */

using Cysharp.Threading.Tasks;
using UnityEngine;

public class SystemManager : MonoBehaviour {
	/// <summary>
	/// 管理するシステムオブジェクトのリスト
	/// </summary>
	[SerializeField]
	private SystemObject[] _systemObjectList = null;

	private void Start() {
		UniTask task = Initialize();
	}

	/// <summary>
	/// 初期化処理
	/// </summary>
	/// <returns></returns>
	private async UniTask Initialize() {
		// 全システムオブジェクトの生成、初期化
		for (int i = 0, max = _systemObjectList.Length; i < max; i++) {
			SystemObject origin = _systemObjectList[i];
			if (origin == null) continue;
			// システムオブジェクト生成
			SystemObject createObject = Instantiate(origin);
			// 初期化
			await createObject.Initialize();
		}
		// 初期パートの実行

	}

}
