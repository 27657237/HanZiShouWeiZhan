using GameFramework.Resource;

using WeChatWASM;

/// <summary>
/// 资源加载辅助类 - 适配UGF框架的微信环境
/// </summary>
public static class ResourceLoader
{
    /// <summary>基础资源路径（微信用户数据目录）</summary>
    public static string BasePath => $"{WX.env.USER_DATA_PATH}/__GAME_FILE_CACHE/StreamingAssets";

    /// <summary>获取资源完整路径</summary>
    public static string GetResourcePath(string resourceName)
    {
        return $"{BasePath}/{resourceName}";
    }

    /// <summary>加载资源（适配UGF接口）</summary>
    public static void LoadAsset(string resourceName, LoadAssetCallbacks callbacks, object userData = null)
    {
        string fullPath = GetResourcePath(resourceName);
        GameEntry.Resource.LoadAsset(fullPath, callbacks, userData);
    }


}

/*// 示例：在游戏代码中加载资源
ResourceLoader.LoadAsset("UI/UIForms/UserInfo", new LoadAssetCallbacks(
    (assetName, asset, duration, userData) => {
        // 加载成功处理
    },
    (assetName, status, errorMessage, userData) => {
        // 加载失败处理
    }
));*/