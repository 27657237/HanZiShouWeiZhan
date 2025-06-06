using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using System.Collections.Generic;

/// <summary>
/// 好友排行榜界面逻辑
/// </summary>
public class FriendRankingForm : UIFormLogic
{
    private Button m_CloseButton;        // 关闭按钮
    private Transform m_RankingItemRoot; // 排行榜条目父节点
    private GameObject m_RankingItemPrefab; // 排行榜条目预制体

    protected override void OnOpen(object userData)
    {
        base.OnOpen(userData);
        InitUIComponents();
        BindButtonEvents();
        LoadRankingData();  // 加载排行榜数据
    }

    protected override void OnClose(bool isShutdown, object userData)
    {
        base.OnClose(isShutdown, userData);
        UnbindButtonEvents();
        ClearRankingItems(); // 清理条目
    }

    private void InitUIComponents()
    {
        m_CloseButton = transform.Find("CloseButton").GetComponent<Button>();
        m_RankingItemRoot = transform.Find("ScrollView/Viewport/Content");
        m_RankingItemPrefab = Resources.Load<GameObject>("UI/RankingItem"); // 假设预制体路径
    }

    private void BindButtonEvents()
    {
        m_CloseButton.onClick.AddListener(OnCloseButtonClick);
    }

    private void UnbindButtonEvents()
    {
        m_CloseButton.onClick.RemoveListener(OnCloseButtonClick);
    }

    /// <summary>
    /// 关闭按钮点击事件
    /// </summary>
    private void OnCloseButtonClick()
    {
        GameEntry.UI.CloseUIForm(UIForm);
    }

    /// <summary>
    /// 加载排行榜数据（示例：模拟网络请求）
    /// </summary>
    private void LoadRankingData()
    {
        // 示例：从服务器获取好友排行榜数据（实际需替换为网络请求）
        var mockData = new List<FriendRankingData>
        {
            new FriendRankingData("玩家A", 1000),
            new FriendRankingData("玩家B", 900),
            new FriendRankingData("自己", 850)
        };

        UpdateRankingList(mockData);
    }

    /// <summary>
    /// 更新排行榜列表显示
    /// </summary>
    private void UpdateRankingList(List<FriendRankingData> dataList)
    {
        ClearRankingItems();
        foreach (var data in dataList)
        {
            GameObject item = Instantiate(m_RankingItemPrefab, m_RankingItemRoot);
            item.GetComponent<RankingItem>().SetData(data.Name, data.Score);
        }
    }

    /// <summary>
    /// 清理现有排行榜条目
    /// </summary>
    private void ClearRankingItems()
    {
        foreach (Transform child in m_RankingItemRoot)
        {
            Destroy(child.gameObject);
        }
    }
}

/// <summary>
/// 排行榜条目数据结构
/// </summary>
public class FriendRankingData
{
    public string Name { get; }
    public int Score { get; }

    public FriendRankingData(string name, int score)
    {
        Name = name;
        Score = score;
    }
}

/// <summary>
/// 排行榜条目组件（需挂载到预制体）
/// </summary>
public class RankingItem : MonoBehaviour
{
    private Text m_NameText;
    private Text m_ScoreText;

    private void Awake()
    {
        m_NameText = transform.Find("Name").GetComponent<Text>();
        m_ScoreText = transform.Find("Score").GetComponent<Text>();
    }

    public void SetData(string name, int score)
    {
        m_NameText.text = name;
        m_ScoreText.text = score.ToString();
    }
}