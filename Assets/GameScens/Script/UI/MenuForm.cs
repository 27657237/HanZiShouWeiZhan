using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

public class MenuForm : UIFormLogic
{

    public UIConfigTable UIConfigTable;
    // 假设这些按钮在Unity的Inspector面板中已经被正确赋值
    public Button settingButton;
    public Button startGameButton;
    public Button leaderboardButton;
    public Button storeButton;
    public Button TalentButton;
    public Button quitButton;
    

    void Start()
    {
        // 为每个按钮添加点击事件监听器
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
            Debug.LogError($"未找到 UIFormId.TalentForm 对应的 UIConfigItem！");
        }
    }

    void Update()
    {
        // 这里可以添加Update逻辑，如果有的话
    }

    // 设置按钮点击事件
    private void OnSettingButtonClick()
    {
      
        UIConfigItem configItem = UIConfigTable.GetConfigById(UIFormId.SettingForm);
        if (configItem != null)
        {
            GameEntry.UI.OpenUIForm(configItem.AssetName, configItem.UIGroupName);
        }
        else
        {
            Debug.LogError($"未找到 UIFormId.SettingForm 对应的 UIConfigItem！");
        }
    }

    // 开始游戏按钮点击事件
    private void OnStartGameButtonClick()
    {
      
        UIConfigItem configItem = UIConfigTable.GetConfigById(UIFormId.PlayGames);
        if (configItem != null)
        {
            GameEntry.UI.OpenUIForm(configItem.AssetName, configItem.UIGroupName);
        }
        else
        {
            Debug.LogError($"未找到 UIFormId.PlayGames 对应的 UIConfigItem！");
        }
    }

    // 排行榜按钮点击事件
    private void OnLeaderboardButtonClick()
    {
        UIConfigItem configItem = UIConfigTable.GetConfigById(UIFormId.LeaderboardForm);
        if (configItem != null)
        {
            GameEntry.UI.OpenUIForm(configItem.AssetName, configItem.UIGroupName);
        }
        else
        {
            Debug.LogError($"未找到 UIFormId.LeaderboardForm 对应的 UIConfigItem！");
        }
    }

    // 商店按钮点击事件
    private void OnStoreButtonClick()
    {
        UIConfigItem configItem = UIConfigTable.GetConfigById(UIFormId.StoreForm);
        if (configItem != null)
        {
            GameEntry.UI.OpenUIForm(configItem.AssetName, configItem.UIGroupName);
        }
        else
        {
            Debug.LogError($"未找到 UIFormId.StoreForm 对应的 UIConfigItem！");
        }
    }

    // 退出按钮点击事件
    private void OnQuitButtonClick()
    {
        
        UIConfigItem configItem = UIConfigTable.GetConfigById(UIFormId.DialogForm); // 假设DialogForm是退出确认对话框
        if (configItem != null)
        {
            GameEntry.UI.OpenUIForm(configItem.AssetName, configItem.UIGroupName);
        }
        else
        {
            Debug.LogError($"未找到 UIFormId.DialogForm 对应的 UIConfigItem！");
        }
    }
}