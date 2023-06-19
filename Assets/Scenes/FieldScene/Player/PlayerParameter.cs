using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Events;

/// <Summary>
/// PlayerParamを取得し格納する、
/// また取得したParamの値を増減させる目的のクラス
/// </Summary>
public class PlayerParameter : MonoBehaviour
{
    public UnityAction onGameOver;

    private PlayerParam param;

    //スマッシュ
    public float SmashTime { get => param.Smash.Param.SmashTime; }
    public Smash Smash { get => param.Smash; }

    //シューズ
    public Shoes Shoes { get => param.Shoes; }

    //ガン
    public float Range { get => param.Gun.Param.Range; }
    public float Reach { get => param.Gun.Param.Reach; }
    public Gun Gun { get => param.Gun; }
    //ガンエフェクト
    public ParticleSystem GunEffect { get; private set; }

    public float Parameter(string key)
    {
        if (!parameter.ContainsKey(key)) throw new NullReferenceException();
        return parameter[key];
    }
    public bool IsAdrenalinable => parameter["Adrenaline"] > 0;
    private Dictionary<string, float> parameter;

    private void Awake()
    {

    }

    private void Start()
    {
        //PlayerParamを取得
        param = GameObject.Find("ParamReceiver").GetComponent<ParamReceiver>().PlayerParam;
        //パラメーターをディクショナリーに設定
        SettingParameter();
        SettingGunPrefab(); //銃のオブジェクトを生成し、位置を調整する
    }

    private void SettingParameter()
    {
        parameter = new Dictionary<string, float>()
        { 
          {"Life", param.Life}, {"LifeMax", Shoes.Param.Life}, 
          {"MoveSpeed", Shoes.Param.MoveSpeed}, {"MoveSpeedMax", Shoes.Param.MoveSpeed},
          {"Adrenaline", 0}, {"AdrenalineMax", 1},
          {"AdrenalineTank", 0}, {"AdrenalineTankMax", 3},
          {"AdrenalineSpeed", Shoes.Param.AdrenalineSpeed}, {"AdrenalineSpeedMax", Shoes.Param.AdrenalineSpeed},
          {"Attack", Gun.Param.Attack}, {"AttackMax", Gun.Param.Attack},
          {"Knockback", Gun.Param.Knockback}, {"KnockbackMax", Gun.Param.Knockback},
          {"Critical", Gun.Param.CriticalMin}, {"CriticalMin", Gun.Param.CriticalMin}, {"CriticalAdd", Gun.Param.CriticalAdd},
          {"Bullet", Gun.Param.Bullet}, {"BulletMax", Gun.Param.Bullet},
          {"ReloadSpeed", Gun.Param.ReloadSpeed}, {"ReloadSpeedMax", Gun.Param.ReloadSpeed}
        };
    }

    /// <summary>
    /// 銃のオブジェクトを生成し、位置を調整する目的のメソッド
    /// </summary>
    private void SettingGunPrefab()
    {
        Gun gunObject = Instantiate(Gun); //オブジェクトを生成
        string path = "Armature | Humanoid/Hips/Spine/Spine1/Spine2/RightShoulder/RightArm/RightForeArm/RightHand";
        gunObject.transform.parent = GameObject.Find("Player").transform.Find(path); //親オブジェクトのトランスフォームを取得
        //位置・角度・大きさを調整
        gunObject.transform.localPosition = new Vector3(0, 0.25f, 0);
        gunObject.transform.localRotation = Quaternion.Euler(-90, 180, -90);
        gunObject.transform.localScale = new Vector3(3, 3, 3);
        GunEffect = gunObject.transform.Find("ShotEffect").GetComponent<ParticleSystem>(); //銃固有のパーティクルを取得
    }

    /// <summary>
    /// パラメーターを増減させるメソッド
    /// </summary>
    /// <param name="key">増減させたいパラメーターのKey</param>
    /// <param name="param">増減の値</param>
    /// <exception cref="NullReferenceException"></exception>
    public void ChangeParameter(string key, float param)
    {
        if (!parameter.ContainsKey(key)) throw new NullReferenceException(); //存在しないKeyならばnullを返す
        if (parameter[key] + param < 0) parameter[key] = 0; //増減の結果パラメーターの値がマイナスになるのを防ぐ
        else if (parameter[key + "Max"] < parameter[key] + param) parameter[key] = parameter[key + "Max"]; //値が規定値以上になるのを防ぐ
        else parameter[key] += param; //0以上規定値以下ならば

        EffectOfParameterChange(key); //増減の結果発生するそれぞれのパラメーター固有の処理を実行
    }

    public void SetParameter(string key, float param)
    {
        if (!parameter.ContainsKey(key)) throw new NullReferenceException();
        if (param < 0) parameter[key] = 0;
        else if (parameter[key + "Max"] < param) parameter[key] = parameter[key + "Max"];
        else parameter[key] = param;

        EffectOfParameterChange(key);
    }

    public void EffectOfParameterChange(string key)
    {
        switch (key)
        {
            case "Life":
                if (parameter["Life"] == 0) //ライフが0になった場合
                {
                    onGameOver?.Invoke(); //ゲームオーバー時の処理を実行
                    Destroy(gameObject); //プレイヤーのオブジェクトを破壊
                }
                break;

            case "MoveSpeed":
                if (parameter["MoveSpeed"] <= parameter["MoveSpeedMax"] * 0.15f) parameter["MoveSpeed"] = parameter["MoveSpeedMax"] * 0.15f;
                break;

            case "Adrenaline":
                if (parameter["Adrenaline"] == 1)
                { 
                    parameter["Adrenaline"] = 0.5f; 
                    parameter["AdrenalineTank"] = 
                    parameter["AdrenalineTank"] + 1 >= parameter["AdrenalineTankMax"] ? parameter["AdrenalineTankMax"] : parameter["AdrenalineTank"] + 1;
                }
                parameter["Critical"] = parameter["CriticalMin"] + (parameter["CriticalAdd"] * parameter["Adrenaline"]);
                break;

            default:
                break;
        }
    }

    public void RevertParameter(string key)
    {
        parameter[key] = parameter[key + "Max"];
    }
}
