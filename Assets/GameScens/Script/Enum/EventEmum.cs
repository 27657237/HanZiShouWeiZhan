using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventEmum
{
    public const int WritePlayerDataEventID = 1001;
    public const int WritePlayerUserInfoEventID = 1002;
   
}
/// <summary>
/// 界面ID枚举（与UIConfigTable中的配置项ID一一对应）
/// </summary>
public enum UIFormId : byte
{
    Undefined = 0,        // 未定义界面（默认值，不可用于打开界面）
    DialogForm = 1,       // 通用对话框界面（如确认框、提示框）
    PlayGames = 100,      // 开始游戏界面（可选，根据项目需求决定是否单独使用）
    SettingForm = 101,    // 设置界面（音量调节、画质设置等）
    AboutForm = 102,      // 关于界面（显示游戏版本、开发者信息等）
    TalentForm = 103,     // 天赋系统界面（玩家技能升级、属性强化等）
    StoreForm = 104,
    LeaderboardForm = 105,
    MenuForm=106,//大厅菜单界面
}