/**
 * @file MenuManager.cs
 * @brief メニュー管理
 * @author yao
 * @date 2025/4/22
 */

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : SystemObject {

	public static MenuManager instance { get; private set; } = null;
	// 管理しているメニューのリスト
	private List<MenuBase> _menuList = null;

	public override async UniTask Initialize() {
		instance = this;
		_menuList = new List<MenuBase>(256);
		await UniTask.CompletedTask;
	}

	/// <summary>
	/// メニューの取得
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="name">プレハブのパスと名前</param>
	/// <returns></returns>
	public T Get<T>(string name = null) where T : MenuBase {
		// キャッシュしたメニューオブジェクトから探す
		for (int i = 0, max = _menuList.Count; i < max; i++) {
			T menu = _menuList[i] as T;
			if (menu == null) continue;

			return menu;
		}
		// みつからなければ生成する
		return Load<T>(name);
	}

	/// <summary>
	/// メニューの読み込み
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="name">プレハブのパスと名前</param>
	/// <returns></returns>
	private T Load<T>(string name) where T : MenuBase {
		// メニューの読み込み
		T menu = Resources.Load<T>(name);
		if (menu == null) return null;
		// メニューの生成
		T createMenu = Instantiate(menu, transform);
		if (createMenu == null) return null;

		_menuList.Add(createMenu);
		return createMenu;
	}
}
