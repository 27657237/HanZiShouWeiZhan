using System;
using System.Collections.Generic;
using GameFramework;
using UnityGameFramework.Runtime;
using WeChatWASM; // ����GameFramework���

/// <summary>
/// ������ݻ��ֻࣨ���ӿڣ�
/// </summary>
public abstract class Player : GameFrameworkComponent
{
    protected PlayerData _data; // �ܱ��������ݴ洢
    protected UserInfo _userInfo; 

    #region ֻ�����ԣ��ⲿ���ܷ������ݣ�
    /// <summary> ��ʯ���� </summary>
    public int Diamonds => _data.diamonds;
    /// <summary> ������� </summary>
    public int Gold => _data.gold;
    /// <summary> �츳ϵͳ </summary>
    public int Turret_talent_code => _data.turret_talent_code;
    public int Wall_talent_code => _data.wall_talent_code;

    public string Code=>_data.code;
    public string OpenID=>_data.openid;

    public string playerName=>_userInfo.nickName;
    #endregion

}

/// <summary>
/// ΢��С��Ϸ�û��������ݽṹ�壨�����л������ڴ洢/���䣩
/// </summary>
[Serializable]
public class PlayerData
{
    public int diamonds;          // ��ʯ
    public int gold;         // ���
    public int turret_talent_code; // �츳ϵͳ
    public int wall_talent_code; // �츳ϵͳ
    public string code; // ��ұ��ε�½ƾ֤
    public string openid; // �����С������Ψһƾ֤
}




