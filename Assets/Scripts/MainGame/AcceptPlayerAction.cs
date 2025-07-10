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

using static MenuList;
using static GameConst;
using static ActionManager;
using static MapSquareUtility;
using static CharacterUtility;
using static UnityEngine.Input;

public class AcceptPlayerAction {

	private System.Action<MoveAction> _AddMove = null;

	// アイテムリスト用コールバック集
	private MenuListCallbackFormat _itemListCallbackFormat = null;

	public void Initialize(System.Action<MoveAction> SetAddMove) {
		// MoveActionをターン処理に積むコールバックをキャッシュしておく
		_AddMove = SetAddMove;

		_itemListCallbackFormat = new MenuListCallbackFormat();
		_itemListCallbackFormat.OnCancel = OnItemListCancel;
	}

	/// <summary>
	/// アイテムリストでキャンセルされた際の処理
	/// </summary>
	/// <param name="currentItem"></param>
	/// <returns></returns>
	private async UniTask<bool> OnItemListCancel(MenuListItem currentItem) {
		await UniTask.CompletedTask;
		return false;
	}

	public async UniTask AcceptInput() {
		while (true) {
			// 移動の受付
			if (AcceptMove()) break;
			// 攻撃の受付
			if (await AcceptAttack()) break;
			// アイテムリストの入力受付
			await AcceptItemList();
			// 方向転換入力の受付処理
			await AcceptDirChange();
			await UniTask.DelayFrame(1);
		}
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
		MapSquareData moveSquare = GetToDirSquare(player.posX, player.posY, inputDir);
		if (!CanMove(player.posX, player.posY, moveSquare, inputDir)) {
			player.SetDirection(inputDir);
			return false;
		}
		// 受け付けた入力に応じて移動
		MoveAction moveAction = new MoveAction();
		MapSquareData sourceSquare = GetSquareData(player.posX, player.posY);
		ChebyshevMoveData moveData = new ChebyshevMoveData(sourceSquare.ID, moveSquare.ID, inputDir);
		// 内部的な移動
		moveAction.ExecuteData(player, moveData);
		_AddMove?.Invoke(moveAction);
		return true;
	}

	private eDirectionEight AcceptDirInput() {
		if (GetKey(KeyCode.UpArrow)) {
			if (GetKey(KeyCode.RightArrow)) {
				return eDirectionEight.UpRight; // 右上
			}
			else if (GetKey(KeyCode.LeftArrow)) {
				return eDirectionEight.UpLeft;  // 左上
			}
			else {
				return eDirectionEight.Up;      // 上
			}
		}
		else if (GetKey(KeyCode.DownArrow)) {
			if (GetKey(KeyCode.RightArrow)) {
				return eDirectionEight.DownRight;// 右下
			}
			else if (GetKey(KeyCode.LeftArrow)) {
				return eDirectionEight.DownLeft;// 左下
			}
			else {
				return eDirectionEight.Down;    // 下
			}
		}
		else {
			if (GetKey(KeyCode.RightArrow)) {
				return eDirectionEight.Right;   // 右
			}
			else if (GetKey(KeyCode.LeftArrow)) {
				return eDirectionEight.Left;    // 左
			}
		}
		return eDirectionEight.Invalid;
	}

	/// <summary>
	/// 通常攻撃入力受付、処理
	/// </summary>
	/// <returns></returns>
	private async UniTask<bool> AcceptAttack() {
		if (!GetKeyDown(KeyCode.Z)) return false;

		await ExecuteAction(GetPlayer(), NORMAL_ATTACK_ACTION_ID);
		return true;
	}

	/// <summary>
	/// 方向転換入力の受付、処理
	/// </summary>
	/// <returns></returns>
	private async UniTask AcceptDirChange() {
		if (!GetKey(KeyCode.LeftShift)) return;

		CharacterBase player = GetPlayer();
		// 方向転換キー入力のトリガーを受け付け、隣接エネミーの方を自動的に向く
		if (GetKeyDown(KeyCode.LeftShift)) ChangeDirToEnemy(player);
		// 方向転換キー入力が離されるまでループ
		while (GetKey(KeyCode.LeftShift)) {
			// 8方向の入力を受け付ける
			eDirectionEight inputDir = AcceptDirInput();
			// 入力に応じて向きを変え、向いている方向のマスの色を変える
			ChangeCharacterDir(player, inputDir);
			await UniTask.DelayFrame(1);
		}
		// 向いている方向のマスの色を消す
		GetToDirSquare(player.posX, player.posY, player.direction)?.HideMark();
	}

	/// <summary>
	/// キャラクターの向きを変え、前方のマスに色を付ける
	/// </summary>
	/// <param name="character"></param>
	/// <param name="inputDir"></param>
	private void ChangeCharacterDir(CharacterBase character, eDirectionEight inputDir) {
		if (inputDir == eDirectionEight.Invalid) return;
		// 今向いているマスの色を消す
		GetToDirSquare(character.posX, character.posY, character.direction)?.HideMark();
		// キャラクターの向きを変える
		character.SetDirection(inputDir);
		// キャラクターが向いている先1マスを取得して色を点ける
		GetToDirSquare(character.posX, character.posY, character.direction)?.ShowMark(Color.red);
	}

	/// <summary>
	/// 隣接エネミーにキャラクターを向かせる
	/// </summary>
	/// <param name="character"></param>
	private void ChangeDirToEnemy(CharacterBase character) {
		// スタート方向を決める
		int startIndex = (int)character.direction + 1;
		int basePosX = character.posX, basePosY = character.posY;
		// 8方向の隣接マスで走査、エネミーを探し、向きを変えマスの色を変える
		for (int i = 0, max = (int)eDirectionEight.Max; i < max; i++) {
			eDirectionEight dir = (startIndex + i).ToDirEight();
			MapSquareData square = GetToDirSquare(basePosX, basePosY, dir);
			if (square == null || !square.existCharacter) continue;

			ChangeCharacterDir(character, dir);
			return;
		}
		// エネミーが見つからなかったら、現在の向きのマスの色を変える
		MapSquareData dirSquare = GetToDirSquare(basePosX, basePosY, character.direction);
		if (dirSquare == null) return;

		dirSquare.ShowMark(Color.red);
	}

	/// <summary>
	/// アイテムリスト入力の受付
	/// </summary>
	/// <returns></returns>
	private async UniTask AcceptItemList() {
		if (!GetKeyDown(KeyCode.C)) return;
		// アイテムリストを開く
		var itemList = MenuManager.instance.Get<MenuItemList>();
		await itemList.Setup(GetPlayer().possessItemList, _itemListCallbackFormat);
		await itemList.Open();
		await itemList.AcceptInput();
		await itemList.Close();
	}

}
