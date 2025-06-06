using GameFramework.Resource;

using WeChatWASM;

/// <summary>
/// ��Դ���ظ����� - ����UGF��ܵ�΢�Ż���
/// </summary>
public static class ResourceLoader
{
    /// <summary>������Դ·����΢���û�����Ŀ¼��</summary>
    public static string BasePath => $"{WX.env.USER_DATA_PATH}/__GAME_FILE_CACHE/StreamingAssets";

    /// <summary>��ȡ��Դ����·��</summary>
    public static string GetResourcePath(string resourceName)
    {
        return $"{BasePath}/{resourceName}";
    }

    /// <summary>������Դ������UGF�ӿڣ�</summary>
    public static void LoadAsset(string resourceName, LoadAssetCallbacks callbacks, object userData = null)
    {
        string fullPath = GetResourcePath(resourceName);
        GameEntry.Resource.LoadAsset(fullPath, callbacks, userData);
    }


}

/*// ʾ��������Ϸ�����м�����Դ
ResourceLoader.LoadAsset("UI/UIForms/UserInfo", new LoadAssetCallbacks(
    (assetName, asset, duration, userData) => {
        // ���سɹ�����
    },
    (assetName, status, errorMessage, userData) => {
        // ����ʧ�ܴ���
    }
));*/