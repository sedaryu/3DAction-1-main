using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの状態（ステータス）を管理するクラス
/// </summary>
public class PlayerStater : MonoBehaviour
{
    //状態
    public Dictionary<string, bool> State { get => state; }

    private Dictionary<string, bool> state = new Dictionary<string, bool>()
    { { "Movable", true }, { "Shootable", true }, { "Damageable", true }, { "Smashable", false } };

    /// <summary>
    /// ステータスの有効無効を変更
    /// </summary>
    /// <param name="key">対象のステータス</param>
    /// <param name="param">true=>有効　false=>無効</param>
    public void TransferState(string key, bool param)
    {
        state[key] = param;
    }

    /// <summary>
    /// 一定時間指定されたステータスを無効にする
    /// </summary>
    /// <param name="key">対象のステータス</param>
    /// <param name="time">時間(秒数)</param>
    /// <returns></returns>
    public IEnumerator WaitForStatusTransition(string key, float time)
    {
        state[key] = false;
        yield return new WaitForSeconds(time);
        state[key] = true;
    }
}
