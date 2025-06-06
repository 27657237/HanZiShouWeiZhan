using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventEmum
{
    public const int WritePlayerDataEventID = 1001;
    public const int WritePlayerUserInfoEventID = 1002;
   
}
/// <summary>
/// ����IDö�٣���UIConfigTable�е�������IDһһ��Ӧ��
/// </summary>
public enum UIFormId : byte
{
    Undefined = 0,        // δ������棨Ĭ��ֵ���������ڴ򿪽��棩
    DialogForm = 1,       // ͨ�öԻ�����棨��ȷ�Ͽ���ʾ��
    PlayGames = 100,      // ��ʼ��Ϸ���棨��ѡ��������Ŀ��������Ƿ񵥶�ʹ�ã�
    SettingForm = 101,    // ���ý��棨�������ڡ��������õȣ�
    AboutForm = 102,      // ���ڽ��棨��ʾ��Ϸ�汾����������Ϣ�ȣ�
    TalentForm = 103,     // �츳ϵͳ���棨��Ҽ�������������ǿ���ȣ�
    StoreForm = 104,
    LeaderboardForm = 105,
    MenuForm=106,//�����˵�����
}