/**
 * @file RouteSearcher.cs
 * @brief 経路探索
 * @author yao
 * @date 2025/4/24
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static MapSquareUtility;
using static GameConst;
using static CommonModule;

public class RouteSearcher {

	private abstract class DistanceNode {
		// 実コスト（スタート地点から何マス離れているか）
		public int distance { get; private set; } = -1;
		// マスID
		public int squareID { get; private set; } = -1;

		public DistanceNode(int setDistance, int setSquareID) {
			distance = setDistance;
			squareID = setSquareID;
		}

		public abstract int GetScore(int goalX, int goalY);
	}

	/// <summary>
	/// 4方向経路探索のノード
	/// </summary>
	private class DistanceNodeManhattan : DistanceNode {
		public eDirectionFour dir { get; private set; } = eDirectionFour.Invalid;
		// 親ノードへの参照
		public DistanceNodeManhattan prevNode { get; private set; } = null;

		public DistanceNodeManhattan(eDirectionFour setDir, DistanceNodeManhattan setPrevNode, int setDistance, int setSquareID) : base(setDistance, setSquareID) {
			dir = setDir;
			prevNode = setPrevNode;
		}

		public override int GetScore(int goalX, int goalY) {
			MapSquareData square = GetSquareData(squareID);
			int diffX = Mathf.Abs(goalX - square.posX);
			int diffY = Mathf.Abs(goalY - square.posY);
			return diffX + diffY;
		}
	}

	/// <summary>
	/// 4方向経路探索のノード管理クラス
	/// </summary>
	private class DistanceNodeTableManahattan {
		public DistanceNodeManhattan goalNode = null;
		/// <summary>
		/// 全てのノード
		/// </summary>
		public List<DistanceNodeManhattan> nodeList = null;

		public DistanceNodeTableManahattan() {
			nodeList = new List<DistanceNodeManhattan>(MAP_SQUARE_COUNT);
		}
		/// <summary>
		/// 初期化
		/// </summary>
		public void Clear() {
			goalNode = null;
			nodeList.Clear();
		}
	}

	private static DistanceNodeTableManahattan _nodeTableManhattan = null;
	/// <summary>
	/// 次にオープンする候補のリスト
	/// </summary>
	private static List<DistanceNodeManhattan> _manhattanOpenList = null;

	/// <summary>
	/// 4方向の経路探索
	/// </summary>
	/// <param name="startSquareID"></param>
	/// <param name="goalSquareID"></param>
	/// <param name="CanPass">通行可否判定</param>
	/// <returns></returns>
	public static List<ManhattanMoveData> RouteSearchManhattan(int startSquareID, int goalSquareID,
		System.Func<MapSquareData, eDirectionFour, int, bool> CanPass) {
		// ゴールノードにたどり着くまでノードをオープンする
		OpenNodeToGoalManhattan(startSquareID, goalSquareID, CanPass);
		// ゴールノードからスタートまで遡って経路を生成
		return CreateRouteManhattan();
	}

	/// <summary>
	/// スタートからゴールにたどり着くまでノードをオープンする
	/// </summary>
	/// <param name="startSquareID"></param>
	/// <param name="goalSquareID"></param>
	/// <param name="CanPass"></param>
	private static void OpenNodeToGoalManhattan(int startSquareID, int goalSquareID,
		System.Func<MapSquareData, eDirectionFour, int, bool> CanPass) {
		// 経路探索に使うメンバ変数の初期化
		if (_nodeTableManhattan == null) {
			_nodeTableManhattan = new DistanceNodeTableManahattan();
		} else {
			_nodeTableManhattan.Clear();
		}
		InitializeList(ref _manhattanOpenList, MAP_SQUARE_COUNT);
		// スタートのノードを生成する
		_manhattanOpenList.Add(new DistanceNodeManhattan(eDirectionFour.Invalid, null, 0, startSquareID));
		// ゴールマスの位置を取得しておく
		MapSquareData goalSquare = GetSquareData(goalSquareID);
		int goalX = goalSquare.posX, goalY = goalSquare.posY;
		// ゴールノードが見つかるまでループ
		while (_nodeTableManhattan.goalNode == null) {
			// スコア最小のノードを取得
			DistanceNodeManhattan minScoreNode = GetMinScoreNodeManhattan(goalX, goalY);
			// スコア最小のノードがなければ終わり（ゴールにたどり着けない）
			if (minScoreNode == null) break;
			// スコア最小のノードの周辺をオープンする
			OpenNodeAroundManhattan(minScoreNode, goalSquareID, CanPass);
			_manhattanOpenList.Remove(minScoreNode);
		}
	}

	/// <summary>
	/// 最少スコアのノードを取得
	/// </summary>
	/// <param name="goalX"></param>
	/// <param name="goalY"></param>
	/// <returns></returns>
	private static DistanceNodeManhattan GetMinScoreNodeManhattan(int goalX, int goalY) {
		if (IsEmpty(_manhattanOpenList)) return null;

		DistanceNodeManhattan result = null;
		int minScore = -1;
		for (int i = 0, max = _manhattanOpenList.Count; i < max; i++) {
			DistanceNodeManhattan node = _manhattanOpenList[i];
			if (node == null) continue;

			int score = node.GetScore(goalX, goalY);
			// スコア最小のノードか判定
			if (result != null && score >= minScore) continue;

			result = node;
			minScore = score;
		}
		return result;
	}

	/// <summary>
	/// 基準ノードの周囲4マスをオープンする
	/// </summary>
	/// <param name="baseNode"></param>
	/// <param name="goalSquareID"></param>
	/// <param name="CanPass"></param>
	private static void OpenNodeAroundManhattan(DistanceNodeManhattan baseNode, int goalSquareID,
		System.Func<MapSquareData, eDirectionFour, int, bool> CanPass) {
		if (baseNode == null) return;

		MapSquareData baseSquare = GetSquareData(baseNode.squareID);
		int baseX = baseSquare.posX, baseY = baseSquare.posY;
		// これからオープンするノードの実コスト
		int distance = baseNode.distance + 1;
		// 周囲4マスをオープンする
		for (int i = 0, max = (int)eDirectionFour.Max; i < max; i++) {
			// インデクスを方向にキャスト
			eDirectionFour dir = (eDirectionFour)i;
			MapSquareData openSquare = GetToDirSquare(baseX, baseY, dir);
			if (openSquare == null) continue;
			// 既にオープンされたノードならオープンしない
			if (_nodeTableManhattan.nodeList.Exists(node => node.squareID == openSquare.ID)) continue;
			// 通行不可のマスならオープンしない
			if (!CanPass(openSquare, dir, distance)) continue;
			// ノードのオープン
			DistanceNodeManhattan addNode = new DistanceNodeManhattan(dir, baseNode, distance, openSquare.ID);
			_nodeTableManhattan.nodeList.Add(addNode);
			_manhattanOpenList.Add(addNode);
			// ゴール判定
			if (openSquare.ID != goalSquareID) continue;
			// ゴールをオープンしたので終わり
			_nodeTableManhattan.goalNode = addNode;
			return;
		}
	}

	/// <summary>
	/// 4方向の経路生成
	/// </summary>
	/// <returns></returns>
	private static List<ManhattanMoveData> CreateRouteManhattan() {
		// ゴールにたどり着いていないならnullを返す
		if (_nodeTableManhattan == null || _nodeTableManhattan.goalNode == null) return null;
		// あらかじめ経路のリストをキャッシュする
		int routeCount = _nodeTableManhattan.goalNode.distance;
		List<ManhattanMoveData> result = new List<ManhattanMoveData>(routeCount);
		for (int i = 0; i < routeCount; i++) {
			result.Add(null);
		}
		// ゴールから遡って経路生成
		DistanceNodeManhattan currentNode = _nodeTableManhattan.goalNode;
		for (int i = routeCount - 1; i >= 0; i--) {
			ManhattanMoveData moveData = new ManhattanMoveData(currentNode.prevNode.squareID, currentNode.squareID, currentNode.dir);
			result[i] = moveData;
			// 親ノードを現在のノードにする
			currentNode = currentNode.prevNode;
		}
		return result;
	}
}
