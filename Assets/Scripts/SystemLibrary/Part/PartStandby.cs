/**
 * @file PartStandby.cs
 * @brief �����p�[�g
 * @author yao
 * @date 2025/4/10
 */

using Cysharp.Threading.Tasks;

public class PartStandby : PartBase {

	public override async UniTask Execute() {
		// �^�C�g���p�[�g�֑J��
		await UniTask.CompletedTask;
	}

}
