/**
 * @file MenuListItem.cs
 * @brief リスト項目の基底クラス
 * @author yao
 * @date 2025/7/10
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class MenuListItem : MonoBehaviour {
	// 項目が選択されたときの表示画像
	[SerializeField]
	private Image _selectImage = null;

	/// <summary>
	/// 項目が選択されたときの画像を表示
	/// </summary>
	public virtual void Select() {
		if (_selectImage == null) return;

		_selectImage.enabled = true;
	}

	/// <summary>
	/// 項目が選択されたときの画像を非表示
	/// </summary>
	public virtual void Deselect() {
		if (_selectImage == null) return;

		_selectImage.enabled = false;
	}

}
