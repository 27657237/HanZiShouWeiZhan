using GameFramework.Event;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

/// <summary>
/// �û���Ϣ�����߼����̳п��UI�߼����ࣩ
/// </summary>
public class SetUserInfoForm : UIFormLogic
{
    private Text m_PlayerName;    // ����ǳ��ı�
    private Text m_Gold;          // ����ı�
    private Text m_Diamonds;      // ��ʯ�ı�

 
    // �������ʱ����������������ڷ�����
    protected  override void OnOpen(object userData)
    {
        base.OnOpen(userData);
        InitUIComponents();       // ��ʼ��UI���
        
    }

    // ����ر�ʱ����������������ڷ�����
    protected override void OnClose(bool isShutdown, object userData)
    {
        base.OnClose(isShutdown, userData);
    }

    /// <summary>
    /// ��ʼ��UI�����ͨ��transform���ң�
    /// </summary>
    private void InitUIComponents()
    {
        m_PlayerName = transform.Find("Name").GetComponent<Text>();
        m_Gold = transform.Find("Glod").GetComponent<Text>();    // ע�⣺ԭ�������ƴд����"Glod"ӦΪ"Gold"
        m_Diamonds = transform.Find("Diamonds").GetComponent<Text>();
    }

   
    /// <summary>
    /// ����������ݸ����¼�
    /// </summary>
    public void OnWritePlayerData()
    {
        m_Diamonds.text = "��ʯ��"+GameEntry.PlayerData.Diamonds.ToString();
        m_Gold.text = "��ң�" + GameEntry.PlayerData.Gold.ToString();
        m_PlayerName.text = GameEntry.PlayerData.playerName;
       
    }

  
}