using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

public class MenuForm : UIFormLogic
{

    public UIConfigTable UIConfigTable;
    // ������Щ��ť��Unity��Inspector������Ѿ�����ȷ��ֵ
    public Button settingButton;
    public Button startGameButton;
    public Button leaderboardButton;
    public Button storeButton;
    public Button TalentButton;
    public Button quitButton;
    

    void Start()
    {
        // Ϊÿ����ť��ӵ���¼�������
        settingButton.onClick.AddListener(OnSettingButtonClick);
        startGameButton.onClick.AddListener(OnStartGameButtonClick);
        leaderboardButton.onClick.AddListener(OnLeaderboardButtonClick);
        storeButton.onClick.AddListener(OnStoreButtonClick);
        quitButton.onClick.AddListener(OnQuitButtonClick);
        TalentButton.onClick.AddListener(OnTalentButtonClick); 
    }

    private void OnTalentButtonClick()
    {

        UIConfigItem configItem = UIConfigTable.GetConfigById(UIFormId.TalentForm);
        if (configItem != null)
        {
            GameEntry.UI.OpenUIForm(configItem.AssetName, configItem.UIGroupName);
        }
        else
        {
            Debug.LogError($"δ�ҵ� UIFormId.TalentForm ��Ӧ�� UIConfigItem��");
        }
    }

    void Update()
    {
        // ����������Update�߼�������еĻ�
    }

    // ���ð�ť����¼�
    private void OnSettingButtonClick()
    {
      
        UIConfigItem configItem = UIConfigTable.GetConfigById(UIFormId.SettingForm);
        if (configItem != null)
        {
            GameEntry.UI.OpenUIForm(configItem.AssetName, configItem.UIGroupName);
        }
        else
        {
            Debug.LogError($"δ�ҵ� UIFormId.SettingForm ��Ӧ�� UIConfigItem��");
        }
    }

    // ��ʼ��Ϸ��ť����¼�
    private void OnStartGameButtonClick()
    {
      
        UIConfigItem configItem = UIConfigTable.GetConfigById(UIFormId.PlayGames);
        if (configItem != null)
        {
            GameEntry.UI.OpenUIForm(configItem.AssetName, configItem.UIGroupName);
        }
        else
        {
            Debug.LogError($"δ�ҵ� UIFormId.PlayGames ��Ӧ�� UIConfigItem��");
        }
    }

    // ���а�ť����¼�
    private void OnLeaderboardButtonClick()
    {
        UIConfigItem configItem = UIConfigTable.GetConfigById(UIFormId.LeaderboardForm);
        if (configItem != null)
        {
            GameEntry.UI.OpenUIForm(configItem.AssetName, configItem.UIGroupName);
        }
        else
        {
            Debug.LogError($"δ�ҵ� UIFormId.LeaderboardForm ��Ӧ�� UIConfigItem��");
        }
    }

    // �̵갴ť����¼�
    private void OnStoreButtonClick()
    {
        UIConfigItem configItem = UIConfigTable.GetConfigById(UIFormId.StoreForm);
        if (configItem != null)
        {
            GameEntry.UI.OpenUIForm(configItem.AssetName, configItem.UIGroupName);
        }
        else
        {
            Debug.LogError($"δ�ҵ� UIFormId.StoreForm ��Ӧ�� UIConfigItem��");
        }
    }

    // �˳���ť����¼�
    private void OnQuitButtonClick()
    {
        
        UIConfigItem configItem = UIConfigTable.GetConfigById(UIFormId.DialogForm); // ����DialogForm���˳�ȷ�϶Ի���
        if (configItem != null)
        {
            GameEntry.UI.OpenUIForm(configItem.AssetName, configItem.UIGroupName);
        }
        else
        {
            Debug.LogError($"δ�ҵ� UIFormId.DialogForm ��Ӧ�� UIConfigItem��");
        }
    }
}