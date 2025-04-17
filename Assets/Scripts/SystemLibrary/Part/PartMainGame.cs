/**
 * @file PartMainGame.cs
 * @brief メインゲームパート
 * @author yao
 * @date 2025/4/10
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class PartMainGame : PartBase {
	/// <summary>
	/// マスの管理クラス
	/// </summary>
	[SerializeField]
	private MapSquareManager _squareManager = null;

	public override async UniTask Initialize() {
		await base.Initialize();
		// マスの管理クラス初期化
		TerrainSpriteAssignor.Initialize();
		_squareManager?.Initialize();
	}

	public override async UniTask Execute() {
		// マップの表示
		MapCreater.CreateMap();
		await UniTask.CompletedTask;
	}

}
