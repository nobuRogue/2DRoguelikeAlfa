/**
 * @file PartStandby.cs
 * @brief 準備パート
 * @author yao
 * @date 2025/4/10
 */

using Cysharp.Threading.Tasks;

public class PartStandby : PartBase {

	public override async UniTask Execute() {
		// フェードアウト
		await FadeManager.instance.FadeOut();
		// マスターデータ読み込み
		MasterDataManager.LoadAllData();
		// タイトルパートへ遷移、終了待ちはしない
		UniTask task = PartManager.instance.TransitionPart(eGamePart.Title);
		await UniTask.CompletedTask;
	}

}
