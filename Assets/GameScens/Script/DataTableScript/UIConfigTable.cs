using UnityEngine;
using System.Collections.Generic;

// ���������������������л������ڱ༭������ʾ��
[System.Serializable]
public class UIConfigItem
{
    [Header("��������")]
    public UIFormId Id;                     // ����ID����Ӧ��� UIFormId ö��ֵ��
    public string AssetName;           // Ԥ������Դ���ƣ��� "SettingForm"��
    public string UIGroupName;         // ���������飨�� "MainUIGroup"��
    //public GameObject Prefable;        //Ԥ����
    [Header("��Ϊ����")]
    public bool AllowMultiInstance;    // �Ƿ�������ʵ����Ĭ��false��
    public bool PauseCoveredUIForm;    // ��ʱ�Ƿ���ͣ�����ǵĽ��棨Ĭ��true��
}

// ���ñ��̳� ScriptableObject�����ڱ༭��������
[CreateAssetMenu(fileName = "UI", menuName = "UI/�������ñ�")]
public class UIConfigTable : ScriptableObject
{
    [Header("���н�������")]
    [SerializeField] private List<UIConfigItem> m_UIConfigs = new List<UIConfigItem>();  // ʹ�� List �������

    // �ṩ������������ List���������ݷ�װ��
    public List<UIConfigItem> GetAllConfigs() => m_UIConfigs;
    public UIConfigItem GetConfigById(UIFormId id) => m_UIConfigs.Find(item => item.Id == id);
}