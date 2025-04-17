/**
 * @file TerrainSpriteAssignor.cs
 * @brief 地形に対応したスプライトの割り当て
 * @author yao
 * @date 2025/4/17
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static CommonModule;

public class TerrainSpriteAssignor {
	// 地形スプライト画像のパス
	private static readonly string _MAP_SPRITE_PATH = "Design/Sprites/Map/";
	// 地形スプライト画像のファイル名
	private static readonly string[][] _MAP_SPRITE_NAME_LIST = new string[][] {
		new string[] { "rogue_map_sand_floor","rogue_map_sand_wall","rogue_map_sand_stair" },
		new string[] { "rogue_map_snow_floor","rogue_map_snow_wall","rogue_map_snow_stair" },
		new string[] { "rogue_map_urban_floor","rogue_map_urban_wall","rogue_map_urban_stair"}};

	// 読み込んだスプライト画像
	private static List<List<Sprite[]>> _terrainSpriteList = null;
	private static int _floorTypeIndex = -1;

	public static void Initialize() {
		_floorTypeIndex = 0;
		// 地形スプライト画像の読み込み
		int mapTypeMax = _MAP_SPRITE_NAME_LIST.Length;
		int terrainSpriteMax = _MAP_SPRITE_NAME_LIST[0].Length;
		_terrainSpriteList = new List<List<Sprite[]>>(mapTypeMax);
		// マップのタイプで回す
		for (int mapType = 0; mapType < mapTypeMax; mapType++) {
			_terrainSpriteList.Add(new List<Sprite[]>(terrainSpriteMax));
			// 地形毎のスプライト数で回す
			for (int i = 0; i < terrainSpriteMax; i++) {
				Sprite[] loadSprite = Resources.LoadAll<Sprite>(_MAP_SPRITE_PATH + _MAP_SPRITE_NAME_LIST[mapType][i]);
				_terrainSpriteList[mapType].Add(loadSprite);
			}
		}
	}

	/// <summary>
	/// 地形に対応したスプライトを返す
	/// </summary>
	/// <param name="terrain"></param>
	/// <returns></returns>
	public static Sprite GetTerrainSprite(eTerrain terrain) {
		if (!IsEnableIndex(_terrainSpriteList, _floorTypeIndex)) return null;

		Sprite[] spriteList = _terrainSpriteList[_floorTypeIndex][GetSpriteIndex(terrain)];
		return spriteList[Random.Range(0, spriteList.Length)];
	}

	/// <summary>
	/// 地形からスプライトのインデクス取得
	/// </summary>
	/// <param name="terrain"></param>
	/// <returns></returns>
	private static int GetSpriteIndex(eTerrain terrain) {
		switch (terrain) {
			case eTerrain.Passage:
			case eTerrain.Room:
			return 0;
			case eTerrain.Wall:
			return 1;
			case eTerrain.Stair:
			return 2;
		}
		return 0;
	}

}
