/**
 * @file SystemObject.cs
 * @brief �Q�[���S�̂Ŏg�p����@�\�̊��
 * @author yao
 * @date 2025/4/10
 */
using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class SystemObject : MonoBehaviour {
	/// <summary>
	/// ����������
	/// </summary>
	/// <returns></returns>
	public abstract UniTask Initialize();
}
