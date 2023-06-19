using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �v���C���[�̏�ԁi�X�e�[�^�X�j���Ǘ�����N���X
/// </summary>
public class PlayerStater : MonoBehaviour
{
    //���
    public Dictionary<string, bool> State { get => state; }

    private Dictionary<string, bool> state = new Dictionary<string, bool>()
    { { "Movable", true }, { "Shootable", true }, { "Damageable", true }, { "Smashable", false } };

    /// <summary>
    /// �X�e�[�^�X�̗L��������ύX
    /// </summary>
    /// <param name="key">�Ώۂ̃X�e�[�^�X</param>
    /// <param name="param">true=>�L���@false=>����</param>
    public void TransferState(string key, bool param)
    {
        state[key] = param;
    }

    /// <summary>
    /// ��莞�Ԏw�肳�ꂽ�X�e�[�^�X�𖳌��ɂ���
    /// </summary>
    /// <param name="key">�Ώۂ̃X�e�[�^�X</param>
    /// <param name="time">����(�b��)</param>
    /// <returns></returns>
    public IEnumerator WaitForStatusTransition(string key, float time)
    {
        state[key] = false;
        yield return new WaitForSeconds(time);
        state[key] = true;
    }
}
