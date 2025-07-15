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

/// ダンジョン終了要因
public enum eDungeonEndReason {
	Invalid = -1,    // 終了していない
	Dead,           // プレイヤー死亡
	Clear,          // クリア
}

/// キャラのアニメーションの種類
public enum eCharacterAnimation {
	Invalid = -1,   // 不正値
	Wait,
	Walk,
	Attack,
	Damage,
	Max
}

public enum eItemCategory {
	Potion,     // 薬
	Food,       // 食べ物
	Wand,       // 杖
	Scroll,     // 巻物
	Bag,        // バッグ
	Throwing,   // 投げモノ
	Weapon,     // 武器
	Armor,      // 防具
	Max,
}

// アイテムのコマンド
public enum eItemCommand {
	Invalid = -1,
	Use,        // 使う
	Puton,      // 置く
	SetEquip,   // 装備
	RemoveEquip,// 外す
}