//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework.Resource;

using System.Collections;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.Networking;
using UnityGameFramework.Runtime;


/// <summary>
/// 游戏入口。
/// </summary>
public partial class GameEntry : MonoBehaviour
{
   
    public static PlayerDataManager PlayerData { get; private set; }
    public static UIConfigTable UIConfigTable { get; private set; }

    void InitCustomComponents()
    {
        PlayerData = UnityGameFramework.Runtime.GameEntry.GetComponent<PlayerDataManager>();
        StartCoroutine(LoadConfigTable());
    }
    private IEnumerator LoadConfigTable()
    {
        yield return new WaitForSecondsRealtime(2f); //延迟一帧让资源配置表等变量赋值



        /* string configAssetName = AssetUtility.GetDataTableAsset("UIForm.asset");

         GameEntry.Resource.LoadAsset(configAssetName, typeof(UIConfigTable), new LoadAssetCallbacks(
             (assetName, asset, duration, userData) =>
             {
                 // 加载成功回调
                 UIConfigTable = asset as UIConfigTable;
                 if (UIConfigTable == null)
                 {
                     Debug.LogError("Failed to load UIForm.");
                 }
                 else
                 {
                     Debug.Log("加载配置表成功UIForm");
                 }
             },
             (assetName, status, errorMessage, userData) =>
             {
                 // 加载失败回调
                 Debug.LogError($"Failed to load UIConfigTable: {errorMessage}");
             },
             (assetName, progress, userData) =>
             {
                 // 加载进度回调
                 Debug.Log($"Loading UIConfigTable: {progress * 100f}%");
             }
         ), null);*/
    }
}


