/**
 * @file CameraManager.cs
 * @brief カメラの管理
 * @author yao
 * @date 2025/6/26
 */

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : SystemObject {
	// 自身への参照
	public static CameraManager instance { get; private set; } = null;
	// 管理中のカメラ
	private Camera _camera = null;
	// カメラオブジェクトの名前
	private const string _CAMERA_NAME = "Main Camera";

	/// <summary>
	/// 初期化
	/// </summary>
	/// <returns></returns>
	public override async UniTask Initialize() {
		instance = this;
		// シーン上のカメラを探してキャッシュしておく
		_camera = GameObject.Find(_CAMERA_NAME).GetComponent<Camera>();
		await UniTask.CompletedTask;
	}

	/// <summary>
	/// カメラの移動
	/// </summary>
	/// <param name="movePos"></param>
	public void MoveCamera(Vector3 movePos) {
		_camera.transform.position = movePos;
	}
}
