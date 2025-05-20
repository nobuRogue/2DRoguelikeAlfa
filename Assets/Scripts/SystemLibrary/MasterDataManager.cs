/**
 * @file MasterDataManager.cs
 * @brief マスターデータ管理
 * @author yao
 * @date 2025/5/20
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterDataManager {
	// マスターデータのファイルパス
	private static readonly string _DATA_PATH = "MasterData/";
	// 読み込んだマスターデータ
	public static List<List<Entity_FloorData>> floorData = null;

	/// <summary>
	/// 全てのマスターデータを読み込む
	/// </summary>
	public static void LoadAllData() {

	}

	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T1"></typeparam>
	/// <typeparam name="T2"></typeparam>
	/// <typeparam name="T3"></typeparam>
	/// <param name="dataName">ScriptableObjectファイル名</param>
	/// <returns></returns>											↓ジェネリッククラス T1 はScriptableObject を継承したクラスに限られる
	private static List<List<T3>> Load<T1, T2, T3>(string dataName) where T1 : ScriptableObject {
		// ファイルを読み込む
		T1 sourceData = Resources.Load<T1>(_DATA_PATH + dataName);
		// 名称指定でシートを取得
		System.Reflection.FieldInfo sheetField = typeof(T1).GetField("sheets");

		// 名称指定でフィールドを取得

		return null;
	}

}
