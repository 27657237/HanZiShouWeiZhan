using System;
using System.Collections.Generic;
using GameFramework;
using UnityGameFramework.Runtime;
using WeChatWASM; // 引入GameFramework框架

/// <summary>
/// 玩家数据基类（只读接口）
/// </summary>
public abstract class Player : GameFrameworkComponent
{
    protected PlayerData _data; // 受保护的数据存储
    protected UserInfo _userInfo; 

    #region 只读属性（外部仅能访问数据）
    /// <summary> 钻石数量 </summary>
    public int Diamonds => _data.diamonds;
    /// <summary> 金币数量 </summary>
    public int Gold => _data.gold;
    /// <summary> 天赋系统 </summary>
    public int Turret_talent_code => _data.turret_talent_code;
    public int Wall_talent_code => _data.wall_talent_code;

    public string Code=>_data.code;
    public string OpenID=>_data.openid;

    public string playerName=>_userInfo.nickName;
    #endregion

}

/// <summary>
/// 微信小游戏用户核心数据结构体（可序列化，用于存储/传输）
/// </summary>
[Serializable]
public class PlayerData
{
    public int diamonds;          // 钻石
    public int gold;         // 金币
    public int turret_talent_code; // 天赋系统
    public int wall_talent_code; // 天赋系统
    public string code; // 玩家本次登陆凭证
    public string openid; // 玩家在小程序内唯一凭证
}




