/**
 * @file GameEnum.cs
 * @brief 列挙体定義
 * @author yao
 * @date 2025/4/10
 */

public enum eGamePart {
	Invalid = -1,
	Standby,    // 準備パート
	Title,      // タイトルパート
	MainGame,   // メインパート
	Ending,     // エンディングパート
	Max,
}

/// <summary>
/// マスの地形
/// </summary>
public enum eTerrain {
	Invalid = -1,
	Passage,    // 通路
	Room,       // 部屋
	Wall,       // 壁
	Stair,      // 階段
	Max,
}

/// <summary>
/// 4方向
/// </summary>
public enum eDirectionFour {
	Invalid = -1,
	Up,
	Right,
	Down,
	Left,
	Max,
}

/// <summary>
/// 8方向
/// </summary>
public enum eDirectionEight {
	Invalid = -1,
	Up,
	UpRight,
	Right,
	DownRight,
	Down,
	DownLeft,
	Left,
	UpLeft,
	Max,
}

/// <summary>
/// フロアの終了要因
/// </summary>
public enum eFloorEndReason {
	Invalid = -1,   // フロアは終了していない
	Dead,           // プレイヤー死亡
	Stair,          // 階段で移動
}

/// <summary>
/// ダンジョン終了要因
/// </summary>
public enum eDungeonEndReason {
	Invalid,    // 終了していない
	Dead,       // プレイヤー死亡
	Clear,      // クリア
}

/// <summary>
/// キャラのアニメーションの種類
/// </summary>
public enum eCharacterAnimation {
	Invalid,
	Wait,
	Walk,
	Attack,
	Damage,
	Max
}