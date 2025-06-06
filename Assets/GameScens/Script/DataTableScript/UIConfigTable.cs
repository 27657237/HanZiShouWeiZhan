using UnityEngine;
using System.Collections.Generic;

// 单个界面配置项（必须可序列化才能在编辑器中显示）
[System.Serializable]
public class UIConfigItem
{
    [Header("基础配置")]
    public UIFormId Id;                     // 界面ID（对应你的 UIFormId 枚举值）
    public string AssetName;           // 预制体资源名称（如 "SettingForm"）
    public string UIGroupName;         // 所属界面组（如 "MainUIGroup"）
    //public GameObject Prefable;        //预制体
    [Header("行为控制")]
    public bool AllowMultiInstance;    // 是否允许多个实例（默认false）
    public bool PauseCoveredUIForm;    // 打开时是否暂停被覆盖的界面（默认true）
}

// 配置表（继承 ScriptableObject，可在编辑器创建）
[CreateAssetMenu(fileName = "UI", menuName = "UI/界面配置表")]
public class UIConfigTable : ScriptableObject
{
    [Header("所有界面配置")]
    [SerializeField] private List<UIConfigItem> m_UIConfigs = new List<UIConfigItem>();  // 使用 List 替代数组

    // 提供公共方法访问 List（保护数据封装）
    public List<UIConfigItem> GetAllConfigs() => m_UIConfigs;
    public UIConfigItem GetConfigById(UIFormId id) => m_UIConfigs.Find(item => item.Id == id);
}