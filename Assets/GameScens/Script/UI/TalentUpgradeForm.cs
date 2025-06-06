using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

/// <summary>
/// 天赋升级界面逻辑
/// </summary>
public class TalentUpgradeForm : UIFormLogic
{
    private Button m_CloseButton;       // 关闭按钮
    private Button m_UpgradeButton;     // 升级按钮
    private Text m_CurrentLevelText;    // 当前等级文本
    private Text m_CostText;            // 升级消耗文本

    protected  override void OnOpen(object userData)
    {
        base.OnOpen(userData);
        InitUIComponents();
        BindButtonEvents();
        RefreshTalentInfo();  // 初始化时刷新天赋信息
    }

    protected  override void OnClose(bool isShutdown, object userData)
    {
        base.OnClose(isShutdown, userData);
        UnbindButtonEvents();
    }

    private void InitUIComponents()
    {
        m_CloseButton = transform.Find("CloseButton").GetComponent<Button>();
        m_UpgradeButton = transform.Find("UpgradeButton").GetComponent<Button>();
        m_CurrentLevelText = transform.Find("TalentInfo/Level").GetComponent<Text>();
        m_CostText = transform.Find("TalentInfo/Cost").GetComponent<Text>();
    }

    private void BindButtonEvents()
    {
        m_CloseButton.onClick.AddListener(OnCloseButtonClick);
        m_UpgradeButton.onClick.AddListener(OnUpgradeButtonClick);
    }

    private void UnbindButtonEvents()
    {
        m_CloseButton.onClick.RemoveListener(OnCloseButtonClick);
        m_UpgradeButton.onClick.RemoveListener(OnUpgradeButtonClick);
    }

    /// <summary>
    /// 关闭按钮点击事件
    /// </summary>
    private void OnCloseButtonClick()
    {
        GameEntry.UI.CloseUIForm(UIForm);  // 通过UI管理器关闭当前界面
    }

    /// <summary>
    /// 升级按钮点击事件
    /// </summary>
    private void OnUpgradeButtonClick()
    {
        
    }

    /// <summary>
    /// 刷新天赋信息显示
    /// </summary>
    private void RefreshTalentInfo()
    {
       
    }
}