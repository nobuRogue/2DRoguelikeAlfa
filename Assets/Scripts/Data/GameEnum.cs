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