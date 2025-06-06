using GameFramework.Event;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

/// <summary>
/// 用户信息界面逻辑（继承框架UI逻辑基类）
/// </summary>
public class SetUserInfoForm : UIFormLogic
{
    private Text m_PlayerName;    // 玩家昵称文本
    private Text m_Gold;          // 金币文本
    private Text m_Diamonds;      // 钻石文本

 
    // 界面可用时触发（框架生命周期方法）
    protected  override void OnOpen(object userData)
    {
        base.OnOpen(userData);
        InitUIComponents();       // 初始化UI组件
        
    }

    // 界面关闭时触发（框架生命周期方法）
    protected override void OnClose(bool isShutdown, object userData)
    {
        base.OnClose(isShutdown, userData);
    }

    /// <summary>
    /// 初始化UI组件（通过transform查找）
    /// </summary>
    private void InitUIComponents()
    {
        m_PlayerName = transform.Find("Name").GetComponent<Text>();
        m_Gold = transform.Find("Glod").GetComponent<Text>();    // 注意：原代码可能拼写错误"Glod"应为"Gold"
        m_Diamonds = transform.Find("Diamonds").GetComponent<Text>();
    }

   
    /// <summary>
    /// 处理玩家数据更新事件
    /// </summary>
    public void OnWritePlayerData()
    {
        m_Diamonds.text = "钻石："+GameEntry.PlayerData.Diamonds.ToString();
        m_Gold.text = "金币：" + GameEntry.PlayerData.Gold.ToString();
        m_PlayerName.text = GameEntry.PlayerData.playerName;
       
    }

  
}