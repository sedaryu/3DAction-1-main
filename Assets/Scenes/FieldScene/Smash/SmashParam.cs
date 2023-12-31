using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SmashParam", menuName = "Custom/SmashParam")]
public class SmashParam : ScriptableObject
{
    public float SmashTime //演出時間
    {
        get => _smashTime;
    }
    [SerializeField] private float _smashTime;

    public float DestroyTime //SmashColliderが破壊されるまでの時間
    {
        get => _destroyTime;
    }
    [SerializeField] private float _destroyTime;

    public float Attack //攻撃力
    {
        get => _attack;
    }
    [SerializeField] private float _attack;

    public float Knockback //ノックバック距離
    {
        get => _knockback;
    }
    [SerializeField] private float _knockback;

    public Smash SmashCollider //コリダーオブジェクト
    {
        get => _smashCollider;
    }
    [SerializeField] private Smash _smashCollider;

    public GameObject SmashEffect //スマッシュ攻撃時のエフェクト
    {
        get => _smashEffect;
    }
    [SerializeField] private GameObject _smashEffect;
}
