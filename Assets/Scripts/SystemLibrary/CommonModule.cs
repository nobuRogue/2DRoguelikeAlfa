/**
 * @file CommonModule.cs
 * @brief 共用処理クラス
 * @author yao
 * @date 2025/4/15
 */

using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;

public class CommonModule {

	/// <summary>
	/// リストが空か判定
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="list"></param>
	/// <returns></returns>
	public static bool IsEmpty<T>(List<T> list) {
		// 短絡評価なので大丈夫
		return list == null || list.Count <= 0;
	}

	public static bool IsEmpty<T>(T[] array) {
		return array == null || array.Length <= 0;
	}

	/// <summary>
	/// リストに対して有効なインデクスか判定
	/// </summary>
	/// <returns></returns>
	public static bool IsEnableIndex<T>(List<T> list, int index) {
		if (IsEmpty(list)) return false;

		return index >= 0 && list.Count > index;
	}

	public static bool IsEnableIndex<T>(T[] array, int index) {
		if (IsEmpty(array)) return false;

		return index >= 0 && array.Length > index;
	}

	/// <summary>
	/// リストを初期化する
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="list"></param>
	/// <param name="capacity"></param>
	public static void InitializeList<T>(ref List<T> list, int capacity = -1) {
		if (list == null) {
			if (capacity < 1) {
				list = new List<T>();
			} else {
				list = new List<T>(capacity);
			}
		} else {
			if (list.Capacity < capacity) list.Capacity = capacity;

			list.Clear();
		}
	}

	/// <summary>
	/// リストを重複なしでマージ
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="main"></param>
	/// <param name="sub"></param>
	public static void MeargeList<T>(ref List<T> main, List<T> sub) {
		if (IsEmpty(sub)) return;

		int meargeCount = sub.Count;
		if (main == null) main = new List<T>(meargeCount);

		for (int i = 0; i < meargeCount; i++) {
			// 重複した要素は追加しない
			if (main.Exists(mainElem => mainElem.Equals(sub[i]))) continue;

			main.Add(sub[i]);
		}
	}

	/// <summary>
	/// 複数のタスクの終了を待つ
	/// </summary>
	/// <param name="taskList"></param>
	/// <returns></returns>
	public static async UniTask WaitTask(List<UniTask> taskList) {
		// 終了したタスクをリストから除き、リストが空になるまで待つ
		while (!IsEmpty(taskList)) {
			// 途中で要素が抜ける可能性があるので末尾から走査
			for (int i = taskList.Count - 1; i >= 0; i--) {
				if (!taskList[i].Status.IsCompleted()) continue;
				// タスクが終了していたらリストから抜く
				taskList.RemoveAt(i);
			}
			await UniTask.DelayFrame(1);
		}
	}

	/// <summary>
	/// 複数のタスクの終了を待つ
	/// </summary>
	/// <param name="taskList"></param>
	/// <returns></returns>
	public static async UniTask WaitTask(List<UniTask> taskList, CancellationToken token) {
		// 終了したタスクをリストから除き、リストが空になるまで待つ
		while (!IsEmpty(taskList)) {
			// 途中で要素が抜ける可能性があるので末尾から走査
			for (int i = taskList.Count - 1; i >= 0; i--) {
				if (!taskList[i].Status.IsCompleted()) continue;
				// タスクが終了していたらリストから抜く
				taskList.RemoveAt(i);
			}
			await UniTask.DelayFrame(1, PlayerLoopTiming.Update, token);
		}
	}

}
