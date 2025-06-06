using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

/// <summary>
/// �츳���������߼�
/// </summary>
public class TalentUpgradeForm : UIFormLogic
{
    private Button m_CloseButton;       // �رհ�ť
    private Button m_UpgradeButton;     // ������ť
    private Text m_CurrentLevelText;    // ��ǰ�ȼ��ı�
    private Text m_CostText;            // ���������ı�

    protected  override void OnOpen(object userData)
    {
        base.OnOpen(userData);
        InitUIComponents();
        BindButtonEvents();
        RefreshTalentInfo();  // ��ʼ��ʱˢ���츳��Ϣ
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
    /// �رհ�ť����¼�
    /// </summary>
    private void OnCloseButtonClick()
    {
        GameEntry.UI.CloseUIForm(UIForm);  // ͨ��UI�������رյ�ǰ����
    }

    /// <summary>
    /// ������ť����¼�
    /// </summary>
    private void OnUpgradeButtonClick()
    {
        
    }

    /// <summary>
    /// ˢ���츳��Ϣ��ʾ
    /// </summary>
    private void RefreshTalentInfo()
    {
       
    }
}