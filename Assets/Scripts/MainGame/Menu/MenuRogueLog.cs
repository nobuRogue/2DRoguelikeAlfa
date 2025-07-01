/**
 * @file MenuRogueLog.cs
 * @brief ログ表示メニュー
 * @author yao
 * @date 2025/6/19
 */

using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

using static CommonModule;

public class MenuRogueLog : MenuBase {

	// ログ単体のプレハブの参照
	[SerializeField]
	private RogueLog _originLogObject = null;
	// 未使用ログのルート
	[SerializeField]
	private Transform _unuseRoot = null;
	// 使用中ログのルート
	[SerializeField]
	private Transform _useRoot = null;

	// １画面に表示されるログの最大数
	private readonly int _SHOW_LOG_COUNT = 4;
	// 使用中のログオブジェクト
	private List<RogueLog> _useList = null;
	// 未使用のログオブジェクト
	private List<RogueLog> _unuseList = null;
	// 表示待機中のテキストリスト
	private List<string> _standbyTextList = null;
	// 表示待機ログリストの初期化数
	private readonly int _STANDBY_LOG_COUNT = 256;
	// ログ移動タスクのリスト
	private List<UniTask> _taskList = null;

	/// <summary>
	/// 初期化
	/// </summary>
	/// <returns></returns>
	public override async UniTask Initialize() {
		await base.Initialize();
		_standbyTextList = new List<string>(_STANDBY_LOG_COUNT);
		_useList = new List<RogueLog>(_SHOW_LOG_COUNT);
		_unuseList = new List<RogueLog>(_SHOW_LOG_COUNT);
		// ログオブジェクトを使用分生成して未使用状態にしておく
		for (int i = 0; i < _SHOW_LOG_COUNT; i++) {
			RogueLog createObject = Instantiate(_originLogObject, _unuseRoot);
			_unuseList.Add(createObject);
		}
		UniTask task = ShowLogTask();
	}

	/// <summary>
	/// ログの追加
	/// </summary>
	/// <param name="addLog"></param>
	public void AddLog(string addLog) {
		_standbyTextList.Add(addLog);
	}

	private async UniTask ShowLogTask() {
		while (true) {
			// 待機中のログメッセージがあり、使用可能なログオブジェクトがあるか判定
			while (IsEmpty(_standbyTextList) || IsEmpty(_unuseList)) await UniTask.DelayFrame(1);
			// スタンバイリストの要素0をオブジェクトとして生成
			string showText = _standbyTextList[0];
			_standbyTextList.RemoveAt(0);
			UseLogObject(showText);
			// 表示中のログオブジェクトを全て１行分移動させる
			int showLogCount = _useList.Count;
			InitializeList(ref _taskList, showLogCount);
			for (int i = 0; i < showLogCount; i++) {
				_taskList.Add(_useList[i].FlowLog());
			}
			await WaitTask(_taskList);
			// 表示範囲外のログオブジェクトを未使用状態にする
			while (_useList.Count >= _SHOW_LOG_COUNT) UnuseLogObject(_useList[0]);

		}
	}

	/// <summary>
	/// ログオブジェクトを表示する
	/// </summary>
	/// <param name="logMessage"></param>
	/// <returns></returns>
	private void UseLogObject(string logMessage) {
		if (IsEmpty(_unuseList)) return;

		RogueLog useLogObject = _unuseList[0];
		_unuseList.RemoveAt(0);
		useLogObject.Setup(logMessage);
		_useList.Add(useLogObject);
		useLogObject.transform.SetParent(_useRoot);
		useLogObject.transform.localPosition = Vector3.zero;
	}

	/// <summary>
	/// ログオブジェクトを非表示にする
	/// </summary>
	/// <param name="unuseLog"></param>
	private void UnuseLogObject(RogueLog unuseLog) {
		if (unuseLog == null) return;

		_useList.Remove(unuseLog);
		unuseLog.Teardown();
		_unuseList.Add(unuseLog);
		unuseLog.transform.SetParent(_unuseRoot);
	}
}

public class RogueLogUtility {
	/// <summary>
	/// ログの追加
	/// </summary>
	/// <param name="addLog"></param>
	public static void AddLog(string addLog) {
		MenuManager.instance.Get<MenuRogueLog>().AddLog(addLog);
	}
}
