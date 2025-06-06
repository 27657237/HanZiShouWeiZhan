using System;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityGameFramework.Runtime;
using UnityEngine;
using WeChatWASM;

/// <summary>
/// 玩家数据管理器（唯一可修改数据的子类，密封防止继承）
/// </summary>
public sealed class PlayerDataManager : Player
{
    private PlayerDataManager() { } // 私有化构造函数（单例模式要求）

    private void Start()
    {
        GameEntry.Event.Subscribe(EventEmum.WritePlayerDataEventID, OnWritePlayerData);
        GameEntry.Event.Subscribe(EventEmum.WritePlayerUserInfoEventID, OnWritePlayerUserInfo);
    }

    private void OnWritePlayerUserInfo(object sender, GameEventArgs e)
    {
        if (e is PlayerUserInfoEventArgs infoArgs && infoArgs.info != null)
        {
            // 创建新实例（值传递核心）
            UserInfo temp = new UserInfo();

            // 复制所有字段（处理可能的空值）
            temp.avatarUrl = infoArgs.info.avatarUrl ?? string.Empty;
            temp.city = infoArgs.info.city ?? string.Empty;
            temp.country = infoArgs.info.country ?? string.Empty;
            temp.gender = infoArgs.info.gender;
            temp.language = infoArgs.info.language ?? string.Empty;
            temp.nickName = infoArgs.info.nickName ?? string.Empty;
            temp.province = infoArgs.info.province ?? string.Empty;

            // 使用复制后的实例（避免直接操作原始数据）
            _userInfo = temp;
        }
    }

    // 私有写入逻辑（仅通过事件触发）
    private void OnWritePlayerData(object sender, GameEventArgs e)
    {
        // 从事件参数中获取数据（自定义参数需继承GameEventArgs）
        if (e is PlayerDataEventArgs dataArgs)
        {
            // 1. 创建新的PlayerData实例
            PlayerData newData = new PlayerData();

            // 2. 逐一复制字段值（值类型和字符串直接复制）
            newData.diamonds = dataArgs.data.diamonds;
            newData.gold = dataArgs.data.gold;
            newData.turret_talent_code = dataArgs.data.turret_talent_code;
            newData.wall_talent_code = dataArgs.data.wall_talent_code;
            newData.code = dataArgs.data.code;
            newData.openid = dataArgs.data.openid;

            // 3. 将副本赋值给成员变量
            _data = newData;

            Debug.Log("写入玩家数据成功");
        }

    }
}

// 自定义事件参数（继承GameEventArgs）
public class PlayerDataEventArgs : GameEventArgs
{
    public PlayerData data;
   
    public override int Id => EventEmum.WritePlayerDataEventID; // 关联事件ID

    public override void Clear()
    {
        data = null; // 清理数据引用
    }
}

// 自定义事件参数（继承GameEventArgs）
public class PlayerUserInfoEventArgs : GameEventArgs
{
    public UserInfo info;
   
    public override int Id => EventEmum.WritePlayerDataEventID; // 关联事件ID

    public override void Clear()
    {
        info = null; // 清理数据引用
    }
}