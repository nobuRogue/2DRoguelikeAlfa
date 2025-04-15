/**
 * @file CommonModule.cs
 * @brief 共用処理クラス
 * @author yao
 * @date 2025/4/15
 */

using Cysharp.Threading.Tasks;
using System.Collections.Generic;

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

	/// <summary>
	/// 複数のタスクの終了を待つ
	/// </summary>
	/// <param name="taskList"></param>
	/// <returns></returns>
	public static async UniTask WaitTask(List<UniTask> taskList) {
		// 終了したタスクをリストから除き、リストが空になるまで待つ
		while (true) {
			// 途中で要素が抜ける可能性があるので末尾から走査
			for (int i = taskList.Count - 1; i >= 0; i--) {
				if (!taskList[i].Status.IsCompleted()) continue;
				// タスクが終了していたらリストから抜く
				taskList.RemoveAt(i);
			}
			// リストが空ならループを抜ける
			if (IsEmpty(taskList)) break;

			await UniTask.DelayFrame(1);
		}
	}


}
