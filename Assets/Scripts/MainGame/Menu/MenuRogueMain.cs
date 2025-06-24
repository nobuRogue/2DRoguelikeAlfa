/**
 * @file MenuRogueMain.cs
 * @brief ローグ画面のメインUI
 * @author yao
 * @date 2025/6/24
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;
using Cysharp.Threading.Tasks;

public class MenuRogueMain : MenuBase {

	// 現在のフロア数
	[SerializeField]
	private TextMeshProUGUI _floorCountText = null;
	// HPの表示テキスト
	[SerializeField]
	private TextMeshProUGUI _HPText = null;
	// 満腹度の表示テキスト
	[SerializeField]
	private TextMeshProUGUI _staminaText = null;
	// 攻撃力の表示テキスト
	[SerializeField]
	private TextMeshProUGUI _attackText = null;
	// 防御力の表示テキスト
	[SerializeField]
	private TextMeshProUGUI _defenseText = null;

	// 表示用メッセージID
	private const int _FLOOR_COUNT_MESSAGE_ID = 0;
	private const int _HP_MESSAGE_ID = 1;
	private const int _STAMINA_MESSAGE_ID = 2;

	/// <summary>
	/// フロア数表示更新
	/// </summary>
	/// <param name="floorCount"></param>
	public void SetFloorCount(int floorCount) {
		_floorCountText.text = string.Format(_FLOOR_COUNT_MESSAGE_ID.ToMessage(), floorCount);
	}

	/// <summary>
	/// HP表示更新
	/// </summary>
	/// <param name="currentHP"></param>
	/// <param name="maxHP"></param>
	public void SetHP(int currentHP, int maxHP) {
		_HPText.text = string.Format(_HP_MESSAGE_ID.ToMessage(), currentHP, maxHP);
	}

	/// <summary>
	/// 満腹度表示更新
	/// </summary>
	/// <param name="showStamina"></param>
	public void SetStamina(int showStamina) {
		_staminaText.text = string.Format(_STAMINA_MESSAGE_ID.ToMessage(), showStamina);
	}

	/// <summary>
	/// 攻撃力表示更新
	/// </summary>
	/// <param name="attack"></param>
	public void SetAttack(int attack) {
		_attackText.text = attack.ToString();
	}

	/// <summary>
	/// 防御力表示更新
	/// </summary>
	/// <param name="defense"></param>
	public void SetDefense(int defense) {
		_defenseText.text = defense.ToString();
	}
}
