using GameFramework;
using GameFramework.Event;
using GameFramework.Resource;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

/// <summary>
/// 预加载流程，负责加载游戏启动所需的基础资源
/// </summary>
public class ProcedurePreload : ProcedureBase
{
    public static readonly string[] DataTableNames = new string[]
    {
      
        "UIForm",
        
    };
    // 预加载资源计数器和总数
    private int m_PreloadCount = 0;
    private int m_PreloadTotal = 0;

    // 配置和数据表加载列表
    private readonly List<string> m_LoadedConfigs = new List<string>();
    private readonly List<string> m_LoadedDataTables = new List<string>();

    /// <summary>
    /// 是否使用原生对话框
    /// </summary>
    public override bool UseNativeDialog
    {
        get
        {
            return true;
        }
    }

    /// <summary>
    /// 进入流程时调用
    /// </summary>
    /// <param name="procedureOwner">流程持有者</param>
    protected override void OnEnter(ProcedureOwner procedureOwner)
    {
        base.OnEnter(procedureOwner);

        // 初始化计数器
        m_PreloadCount = 0;
        m_PreloadTotal = 0;

        // 开始预加载资源
        PreloadResources();
    }

    /// <summary>
    /// 流程更新时调用
    /// </summary>
    /// <param name="procedureOwner">流程持有者</param>
    /// <param name="elapseSeconds">逻辑流逝时间</param>
    /// <param name="realElapseSeconds">真实流逝时间</param>
    protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

        // 检查预加载是否完成
        if (m_PreloadCount >= m_PreloadTotal)
        {
            // 设置下一个场景ID并切换到场景切换流程
            //procedureOwner.SetData<VarInt32>("NextSceneId", GameEntry.Config.GetInt("Scene.Menu"));
            //ChangeState<ProcedureChangeScene>(procedureOwner);
        }
    }

    /// <summary>
    /// 预加载游戏所需的基础资源
    /// </summary>
    private void PreloadResources()
    {
        // 计算预加载总数（配置文件 + 数据表 + 其他资源）
        m_PreloadTotal = 3; // 示例值，实际应根据加载内容计算

        // 加载配置文件
       /* LoadConfig("DefaultConfig");
        LoadConfig("NetworkConfig");

        // 加载数据表
        LoadDataTable("Entity");
        LoadDataTable("Skill");
        LoadDataTable("Item");*/

        // 可以添加更多预加载内容...

        // 示例：加载其他资源
        m_PreloadTotal++;
        /*GameEntry.Resource.LoadAsset("Assets/GameMain/Textures/LoadingBG.asset", new LoadAssetCallbacks(
            (assetName, asset, duration, userData) =>
            {
                Log.Info("Preload loading background success.");
                m_PreloadCount++;
            },
            (assetName, errorMessage, userData) =>
            {
                Log.Error($"Preload loading background failure: {errorMessage}");
                m_PreloadCount++;
            }));*/
    }

    /// <summary>
    /// 加载配置文件
    /// </summary>
    /// <param name="configName">配置名称</param>
    private void LoadConfig(string configName)
    {
        if (m_LoadedConfigs.Contains(configName))
        {
            return;
        }

        m_LoadedConfigs.Add(configName);
        string configAssetName = AssetUtility.GetConfigAsset(configName);

        // 异步加载配置文件
        GameEntry.Resource.LoadAsset(configAssetName, new LoadAssetCallbacks(
            (assetName, asset, duration, userData) =>
            {
                Log.Info($"Load config '{configName}' success.");
                m_PreloadCount++;
            },
            (assetName, errorMessage, userData) =>
            {
                Log.Error($"Load config '{configName}' failure: {errorMessage}");
                m_PreloadCount++;
            }));
    }

    /// <summary>
    /// 加载数据表
    /// </summary>
    /// <param name="dataTableName">数据表名称</param>
    private void LoadDataTable(string dataTableName)
    {
        if (m_LoadedDataTables.Contains(dataTableName))
        {
            return;
        }

        m_LoadedDataTables.Add(dataTableName);
        string dataTableAssetName = AssetUtility.GetDataTableAsset(dataTableName);

        // 异步加载数据表
        GameEntry.Resource.LoadAsset(dataTableAssetName, new LoadAssetCallbacks(
            (assetName, asset, duration, userData) =>
            {
                Log.Info($"Load data table '{dataTableName}' success.");
                m_PreloadCount++;
            },
            (assetName, errorMessage, userData) =>
            {
                Log.Error($"Load data table '{dataTableName}' failure: {errorMessage}");
                m_PreloadCount++;
            }));
    }
}