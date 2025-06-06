using System;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityGameFramework.Runtime;
using UnityEngine;
using WeChatWASM;

/// <summary>
/// ������ݹ�������Ψһ���޸����ݵ����࣬�ܷ��ֹ�̳У�
/// </summary>
public sealed class PlayerDataManager : Player
{
    private PlayerDataManager() { } // ˽�л����캯��������ģʽҪ��

    private void Start()
    {
        GameEntry.Event.Subscribe(EventEmum.WritePlayerDataEventID, OnWritePlayerData);
        GameEntry.Event.Subscribe(EventEmum.WritePlayerUserInfoEventID, OnWritePlayerUserInfo);
    }

    private void OnWritePlayerUserInfo(object sender, GameEventArgs e)
    {
        if (e is PlayerUserInfoEventArgs infoArgs && infoArgs.info != null)
        {
            // ������ʵ����ֵ���ݺ��ģ�
            UserInfo temp = new UserInfo();

            // ���������ֶΣ�������ܵĿ�ֵ��
            temp.avatarUrl = infoArgs.info.avatarUrl ?? string.Empty;
            temp.city = infoArgs.info.city ?? string.Empty;
            temp.country = infoArgs.info.country ?? string.Empty;
            temp.gender = infoArgs.info.gender;
            temp.language = infoArgs.info.language ?? string.Empty;
            temp.nickName = infoArgs.info.nickName ?? string.Empty;
            temp.province = infoArgs.info.province ?? string.Empty;

            // ʹ�ø��ƺ��ʵ��������ֱ�Ӳ���ԭʼ���ݣ�
            _userInfo = temp;
        }
    }

    // ˽��д���߼�����ͨ���¼�������
    private void OnWritePlayerData(object sender, GameEventArgs e)
    {
        // ���¼������л�ȡ���ݣ��Զ��������̳�GameEventArgs��
        if (e is PlayerDataEventArgs dataArgs)
        {
            // 1. �����µ�PlayerDataʵ��
            PlayerData newData = new PlayerData();

            // 2. ��һ�����ֶ�ֵ��ֵ���ͺ��ַ���ֱ�Ӹ��ƣ�
            newData.diamonds = dataArgs.data.diamonds;
            newData.gold = dataArgs.data.gold;
            newData.turret_talent_code = dataArgs.data.turret_talent_code;
            newData.wall_talent_code = dataArgs.data.wall_talent_code;
            newData.code = dataArgs.data.code;
            newData.openid = dataArgs.data.openid;

            // 3. ��������ֵ����Ա����
            _data = newData;

            Debug.Log("д��������ݳɹ�");
        }

    }
}

// �Զ����¼��������̳�GameEventArgs��
public class PlayerDataEventArgs : GameEventArgs
{
    public PlayerData data;
   
    public override int Id => EventEmum.WritePlayerDataEventID; // �����¼�ID

    public override void Clear()
    {
        data = null; // ������������
    }
}

// �Զ����¼��������̳�GameEventArgs��
public class PlayerUserInfoEventArgs : GameEventArgs
{
    public UserInfo info;
   
    public override int Id => EventEmum.WritePlayerDataEventID; // �����¼�ID

    public override void Clear()
    {
        info = null; // ������������
    }
}