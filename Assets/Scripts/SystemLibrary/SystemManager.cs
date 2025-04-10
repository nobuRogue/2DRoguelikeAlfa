/**
 * @file SystemManager.cs
 * @brief �Q�[���S�̂Ŏg�p����@�\�̊Ǘ�
 * @author yao
 * @date 2025/4/10
 */

using Cysharp.Threading.Tasks;
using UnityEngine;

public class SystemManager : MonoBehaviour {
	/// <summary>
	/// �Ǘ�����V�X�e���I�u�W�F�N�g�̃��X�g
	/// </summary>
	[SerializeField]
	private SystemObject[] _systemObjectList = null;

	private void Start() {
		UniTask task = Initialize();
	}

	/// <summary>
	/// ����������
	/// </summary>
	/// <returns></returns>
	private async UniTask Initialize() {
		// �S�V�X�e���I�u�W�F�N�g�̐����A������
		for (int i = 0, max = _systemObjectList.Length; i < max; i++) {
			SystemObject origin = _systemObjectList[i];
			if (origin == null) continue;
			// �V�X�e���I�u�W�F�N�g����
			SystemObject createObject = Instantiate(origin);
			// ������
			await createObject.Initialize();
		}
		// �����p�[�g�̎��s

	}

}
