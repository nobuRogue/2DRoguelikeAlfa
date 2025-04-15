/**
 * @file PartManager.cs
 * @brief パート管理
 * @author yao
 * @date 2025/4/15
 */

using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class PartManager : SystemObject {
	/// <summary>
	/// 自身への参照
	/// </summary>
	public static PartManager instance { get; private set; } = null;

	/// <summary>
	/// パートオブジェクトのオリジナル
	/// </summary>
	[SerializeField]
	private PartBase[] _partOriginList = null;
	/// <summary>
	/// 管理しているパートオブジェクト
	/// </summary>
	private PartBase[] _partList = null;
	/// <summary>
	/// 現在のパート
	/// </summary>
	private PartBase _currentPart = null;

	/// <summary>
	/// 初期化処理
	/// </summary>
	/// <returns></returns>
	/// <exception cref="System.NotImplementedException"></exception>
	public override async UniTask Initialize() {
		instance = this;
		// パートオブジェクトの生成、初期化
		int partMax = (int)eGamePart.Max;
		_partList = new PartBase[partMax];

		List<UniTask> taskList = new List<UniTask>(partMax);
		for (int i = 0; i < partMax; i++) {
			// パートオブジェクトの生成
			_partList[i] = Instantiate(_partOriginList[i], transform);
			taskList.Add(_partList[i].Initialize());
		}
		// 全てのパートの初期化終了を待つ
		await CommonModule.WaitTask(taskList);
	}

	/// <summary>
	/// パートの切り替え
	/// </summary>
	/// <param name="nextPart"></param>
	/// <returns></returns>
	public async UniTask TransitionPart(eGamePart nextPart) {
		// 現在のパートの片付け
		if (_currentPart != null) await _currentPart.Teardown();
		// パートの切り替え
		_currentPart = _partList[(int)nextPart];
		await _currentPart.Setup();
		// 次のパートの実行
		UniTask task = _currentPart.Execute();
	}

}
