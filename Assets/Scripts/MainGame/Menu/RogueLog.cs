/**
 * @file RogueLog.cs
 * @brief 1つのログメッセージ
 * @author yao
 * @date 2025/6/19
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using System.Threading;
using UnityEditor;

public class RogueLog : MonoBehaviour {
	// ログ１行分の移動にかかる時間[秒]
	private static readonly float _FLOW_DURATION_SEC = 0.5f;

	// 表示されるログのテキスト
	[SerializeField]
	private TextMeshProUGUI _logText = null;
	// 自身の矩形
	[SerializeField]
	private RectTransform _rectTransform = null;

	CancellationToken _token;

	/// <summary>
	/// 使用前の準備
	/// </summary>
	/// <param name="showText"></param>
	public void Setup(string showText) {
		_logText.text = showText;
	}
	/// <summary>
	/// 使用後の片付け
	/// </summary>
	public void Teardown() {
		_logText.text = string.Empty;
	}

	/// <summary>
	/// 自身をログ１行分上に流す
	/// </summary>
	/// <returns></returns>
	public async UniTask FlowLog() {
		_token = this.GetCancellationTokenOnDestroy();
		// スタートと目的地の決定
		float flowValue = _rectTransform.sizeDelta.y;
		Vector3 startPos = transform.position;
		Vector3 goalPos = startPos;
		goalPos.y += flowValue;
		// 規定の秒数をかけて移動
		float elapsedTime = 0.0f;
		while (elapsedTime < _FLOW_DURATION_SEC) {
			elapsedTime += Time.deltaTime;
			float t = elapsedTime / _FLOW_DURATION_SEC;
			transform.position = Vector3.Lerp(startPos, goalPos, t);
			await UniTask.DelayFrame(1, PlayerLoopTiming.Update, _token);
		}
		transform.position = goalPos;
	}
}
