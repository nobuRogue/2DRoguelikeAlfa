/**
 * @file PartBase.cs
 * @brief �Q�[���p�[�g�̊��
 * @author yao
 * @date 2025/4/10
 */

using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class PartBase : MonoBehaviour {
	/// <summary>
	/// ����������
	/// �Q�[���J�n����1�x�����Ă΂��
	/// </summary>
	/// <returns></returns>
	public virtual async UniTask Initialize() {
		await UniTask.CompletedTask;
	}

	/// <summary>
	/// �p�[�g���s�O�ɂ�����
	/// �p�[�g�ɑJ�ڂ���O�ɌĂ΂��
	/// </summary>
	/// <returns></returns>
	public virtual async UniTask Setup() {
		await UniTask.CompletedTask;
	}

	/// <summary>
	/// �p�[�g�̎��s
	/// </summary>
	/// <returns></returns>
	public abstract UniTask Execute();

	/// <summary>
	/// �p�[�g�I�����̕Еt��
	/// </summary>
	/// <returns></returns>
	public virtual async UniTask Teardown() {
		await UniTask.CompletedTask;
	}
}
