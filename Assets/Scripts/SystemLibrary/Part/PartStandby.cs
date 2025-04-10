/**
 * @file PartStandby.cs
 * @brief 準備パート
 * @author yao
 * @date 2025/4/10
 */

using Cysharp.Threading.Tasks;

public class PartStandby : PartBase {

	public override async UniTask Execute() {
		// タイトルパートへ遷移
		await UniTask.CompletedTask;
	}

}
