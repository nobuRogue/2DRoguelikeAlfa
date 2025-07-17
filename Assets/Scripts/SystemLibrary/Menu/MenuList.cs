/**
 * @file MenuList.cs
 * @brief リストメニューの基底
 * @author yao
 * @date 2025/7/10
 */

using Cysharp.Threading.Tasks;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using static CommonModule;

public abstract class MenuList : MenuBase {
	// リスト項目のオリジナル
	[SerializeField]
	private ListItem _itemOrigin = null;

	// 表示項目の親オブジェクト
	[SerializeField]
	private Transform _contentRoot = null;

	// 未使用項目の親オブジェクト
	[SerializeField]
	private Transform _unuseRoot = null;

	// 使用、未使用項目のリスト
	private List<ListItem> _useList = null;
	private List<ListItem> _unuseList = null;
	// 現在の選択項目インデックス
	private int _currentIndex = -1;

	// リストメニューで決定、キャンセル等が行われたときの処理は
	// 呼び出し側が設定できるようにしなければならない

	/// <summary>
	/// リストメニューのコールバック集クラス
	/// </summary>
	public class MenuListCallbackFormat {
		// 決定された際の処理
		public System.Func<ListItem, CancellationToken, UniTask<bool>> OnDecide = null;
		// キャンセルされた際の処理
		public System.Func<ListItem, CancellationToken, UniTask<bool>> OnCancel = null;
		// カーソルが移動した際の処理
		public System.Func<ListItem, ListItem, CancellationToken, UniTask> OnMoveCursor = null;
		// 自由な受付処理
		public System.Func<ListItem, CancellationToken, UniTask<bool>> FreeAccept = null;
	}
	// 現在のコールバックフォーマット
	private MenuListCallbackFormat _currentFormat = null;
	// 入力受付タスク中断用トークン
	private CancellationToken _token;

	/// <summary>
	/// 初期化
	/// </summary>
	/// <returns></returns>
	public override async UniTask Initialize() {
		await base.Initialize();
		_useList = new List<ListItem>();
		_unuseList = new List<ListItem>();

		// オブジェクト破棄時に処理されるタスク中断用トークンを取得
		_token = this.GetCancellationTokenOnDestroy();
	}

	/// <summary>
	/// コールバック集クラスの設定
	/// </summary>
	/// <param name="setFortmat"></param>
	public void SetCallbackFortmat(MenuListCallbackFormat setFortmat) {
		_currentFormat = setFortmat;
	}

	// 三項演算子
	// [判定] ? [trueのときの返り値] : [false のときの返り値]

	/// <summary>
	/// リスト項目の生成
	/// </summary>
	/// <returns></returns>
	public ListItem AddListItem() {
		ListItem addItem;
		// 未使用リストが空なら生成、空でなければそこから使う
		if (IsEmpty(_unuseList)) {
			addItem = Instantiate(_itemOrigin, _contentRoot);
		}
		else {
			addItem = _unuseList[0];
			_unuseList.RemoveAt(0);
			addItem.transform.SetParent(_contentRoot);
		}
		// 使用リストに追加
		_useList.Add(addItem);
		addItem.Deselect();
		return addItem;
	}

	/// <summary>
	/// リスト項目の削除
	/// </summary>
	/// <param name="removeIndex"></param>
	public void RemoveListItem(int removeIndex) {
		if (!IsEnableIndex(_useList, removeIndex)) return;
		// 使用リストから取り除く
		ListItem removeItem = _useList[removeIndex];
		_useList.RemoveAt(removeIndex);
		// 未使用リストへ追加
		_unuseList.Add(removeItem);
		removeItem.transform.SetParent(_unuseRoot);
		removeItem.Deselect();
	}

	/// <summary>
	/// 全てのリスト項目削除
	/// </summary>
	public void RemoveAllItem() {
		// 使用リストが空になるまで0番目の要素を削除
		while (!IsEmpty(_useList)) RemoveListItem(0);

	}

	/// <summary>
	/// リストの入力受付タスク
	/// </summary>
	/// <returns></returns>
	public async UniTask AcceptInput() {
		while (true) {
			// カーソル移動の受付
			await AcceptMoveCursor();
			// 決定入力の受付
			if (!await AcceptDecide()) break;
			// キャンセル入力の受付
			if (!await AcceptCancel()) break;
			// 自由な入力受付
			if (!await AcceptFree()) break;

			await UniTask.DelayFrame(1, PlayerLoopTiming.Update, _token);
		}
	}

	/// <summary>
	/// カーソル移動の入力受付
	/// </summary>
	/// <returns></returns>
	private async UniTask AcceptMoveCursor() {
		// 四方向の入力受付
		eDirectionFour inputDir = GetDirInput();
		if (inputDir == eDirectionFour.Invalid) return;
		// 入力に応じたインデクスの変更
		int moveIndex = _currentIndex;
		switch (inputDir) {
			case eDirectionFour.Up:
				moveIndex--;
				break;
			case eDirectionFour.Down:
				moveIndex++;
				break;
		}
		// 移動後のインデクスがリスト項目に収まるように修正
		if (moveIndex < 0) moveIndex = _useList.Count - 1;

		if (moveIndex >= _useList.Count) moveIndex = 0;
		// カーソル移動の処理
		await SetIndex(moveIndex);
	}

	/// <summary>
	/// 選択項目の設定
	/// </summary>
	/// <returns></returns>
	public async UniTask SetIndex(int setIndex) {
		if (_currentIndex == setIndex) return;
		// 現在の項目を未選択状態にする
		ListItem prevItem = null;
		if (IsEnableIndex(_useList, _currentIndex)) {
			prevItem = _useList[_currentIndex];
			prevItem.Deselect();
		}
		_currentIndex = setIndex;
		// 移動後の項目を選択状態にする
		ListItem currentItem = null;
		if (IsEnableIndex(_useList, _currentIndex)) {
			currentItem = _useList[_currentIndex];
			currentItem.Select();
		}
		// カーソル移動コールバックの実行
		if (_currentFormat == null ||
			_currentFormat.OnMoveCursor == null) return;

		await _currentFormat.OnMoveCursor(prevItem, currentItem, _token);
	}

	/// <summary>
	/// 4方向の入力受付
	/// </summary>
	/// <returns></returns>
	private eDirectionFour GetDirInput() {
		if (Input.GetKeyDown(KeyCode.UpArrow)) return eDirectionFour.Up;

		if (Input.GetKeyDown(KeyCode.RightArrow)) return eDirectionFour.Right;

		if (Input.GetKeyDown(KeyCode.DownArrow)) return eDirectionFour.Down;

		if (Input.GetKeyDown(KeyCode.LeftArrow)) return eDirectionFour.Left;

		return eDirectionFour.Invalid;
	}

	/// <summary>
	/// 決定入力の受付
	/// </summary>
	/// <returns></returns>
	private async UniTask<bool> AcceptDecide() {
		if (!Input.GetKeyDown(KeyCode.Z)) return true;
		// 決定処理のコールバックが無ければ終了
		if (_currentFormat == null ||
			_currentFormat.OnDecide == null) return true;
		// 決定処理のコールバック実行
		return await _currentFormat.OnDecide(GetCurrentItem(), _token);
	}

	/// <summary>
	/// キャンセル処理の受付
	/// </summary>
	/// <returns></returns>
	private async UniTask<bool> AcceptCancel() {
		if (!Input.GetKeyDown(KeyCode.X)) return true;
		// キャンセル処理のコールバックが無ければ終了
		if (_currentFormat == null ||
			_currentFormat.OnCancel == null) return true;
		// キャンセル処理のコールバック実行
		return await _currentFormat.OnCancel(GetCurrentItem(), _token);
	}

	/// <summary>
	/// 自由な入力受付処理の実行
	/// </summary>
	/// <returns></returns>
	private async UniTask<bool> AcceptFree() {
		// 自由受付のコールバックが無ければ終了
		if (_currentFormat == null ||
			_currentFormat.FreeAccept == null) return true;
		// 自由受付のコールバック実行
		return await _currentFormat.FreeAccept(GetCurrentItem(), _token);
	}

	/// <summary>
	/// 現在選択中の項目を取得
	/// </summary>
	/// <returns></returns>
	private ListItem GetCurrentItem() {
		return IsEnableIndex(_useList, _currentIndex) ? _useList[_currentIndex] : null;
	}

}
