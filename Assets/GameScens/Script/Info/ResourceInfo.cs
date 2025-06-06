using WeChatWASM;

/// <summary>
/// 资源信息类 - 存储单个资源的元数据
/// </summary>
public class ResourceInfo
{
    /// <summary>资源名称（如"UI/UIForms/UserInfo"）</summary>
    public string Name { get; set; }

    /// <summary>资源扩展名（如".dat"）</summary>
    public string Extension { get; set; }

    /// <summary>资源哈希值（用于版本比对）</summary>
    public string HashCode { get; set; }

    /// <summary>资源类型（1=可更新资源）</summary>
    public int LoadType { get; set; }

    /// <summary>是否已打包</summary>
    public bool Packed { get; set; }

    /// <summary>资源文件大小（字节）</summary>
    public long Length { get; set; }
    /// <summary>资源文件在服务器上的完整URL</summary>
    public string RemoteUrl => $"{GetServerBaseUrl()}{Name}.{Extension}";

    /// <summary>资源文件在本地缓存中的完整路径</summary>
    public string LocalPath => $"{WX.env.USER_DATA_PATH}/__GAME_FILE_CACHE/StreamingAssets/{Name}.{Extension}";

    /// <summary>获取服务器基础URL（移除文件名部分）</summary>
    private string GetServerBaseUrl()
    {
        string baseUrl = ProcedureCheckVersion.SERVER_REPORT_URL;
        int lastSlash = baseUrl.LastIndexOf('/');
        return lastSlash >= 0 ? baseUrl.Substring(0, lastSlash + 1) : baseUrl;
    }
}