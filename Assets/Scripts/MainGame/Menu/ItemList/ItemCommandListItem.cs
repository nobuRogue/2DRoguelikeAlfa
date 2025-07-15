/**
 * @file ItemCommandListItem.cs
 * @brief アイテムコマンドリストの項目
 * @author yao
 * @date 2025/7/10
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemCommandListItem : ListItem {
	[SerializeField]
	private TextMeshProUGUI _commandNameText = null;

	// この項目のコマンド
	public eItemCommand command { get; private set; } = eItemCommand.Invalid;
	// コマンド名のメッセージIDのオフセット
	private static readonly int _COMMAND_MESSAGE_ID_OFFSET = 20;

	public void Setup(eItemCommand setCommand) {
		command = setCommand;
		// テキストの設定
		_commandNameText.text = (_COMMAND_MESSAGE_ID_OFFSET + (int)command).ToMessage();
	}

}
