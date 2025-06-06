using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using System.Collections.Generic;

/// <summary>
/// �������а�����߼�
/// </summary>
public class FriendRankingForm : UIFormLogic
{
    private Button m_CloseButton;        // �رհ�ť
    private Transform m_RankingItemRoot; // ���а���Ŀ���ڵ�
    private GameObject m_RankingItemPrefab; // ���а���ĿԤ����

    protected override void OnOpen(object userData)
    {
        base.OnOpen(userData);
        InitUIComponents();
        BindButtonEvents();
        LoadRankingData();  // �������а�����
    }

    protected override void OnClose(bool isShutdown, object userData)
    {
        base.OnClose(isShutdown, userData);
        UnbindButtonEvents();
        ClearRankingItems(); // ������Ŀ
    }

    private void InitUIComponents()
    {
        m_CloseButton = transform.Find("CloseButton").GetComponent<Button>();
        m_RankingItemRoot = transform.Find("ScrollView/Viewport/Content");
        m_RankingItemPrefab = Resources.Load<GameObject>("UI/RankingItem"); // ����Ԥ����·��
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
    /// �رհ�ť����¼�
    /// </summary>
    private void OnCloseButtonClick()
    {
        GameEntry.UI.CloseUIForm(UIForm);
    }

    /// <summary>
    /// �������а����ݣ�ʾ����ģ����������
    /// </summary>
    private void LoadRankingData()
    {
        // ʾ�����ӷ�������ȡ�������а����ݣ�ʵ�����滻Ϊ��������
        var mockData = new List<FriendRankingData>
        {
            new FriendRankingData("���A", 1000),
            new FriendRankingData("���B", 900),
            new FriendRankingData("�Լ�", 850)
        };

        UpdateRankingList(mockData);
    }

    /// <summary>
    /// �������а��б���ʾ
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
    /// �����������а���Ŀ
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
/// ���а���Ŀ���ݽṹ
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
/// ���а���Ŀ���������ص�Ԥ���壩
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