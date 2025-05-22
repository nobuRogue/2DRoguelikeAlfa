/**
 * @file PartEnding.cs
 * @brief エンディングパート
 * @author yao
 * @date 2025/4/10
 */

using Cysharp.Threading.Tasks;

public class PartEnding : PartBase {

	public override async UniTask Execute() {
		UniTask task = PartManager.instance.TransitionPart(eGamePart.Title);
		await UniTask.CompletedTask;
	}

}
