using WeChatWASM;

/// <summary>
/// ��Դ��Ϣ�� - �洢������Դ��Ԫ����
/// </summary>
public class ResourceInfo
{
    /// <summary>��Դ���ƣ���"UI/UIForms/UserInfo"��</summary>
    public string Name { get; set; }

    /// <summary>��Դ��չ������".dat"��</summary>
    public string Extension { get; set; }

    /// <summary>��Դ��ϣֵ�����ڰ汾�ȶԣ�</summary>
    public string HashCode { get; set; }

    /// <summary>��Դ���ͣ�1=�ɸ�����Դ��</summary>
    public int LoadType { get; set; }

    /// <summary>�Ƿ��Ѵ��</summary>
    public bool Packed { get; set; }

    /// <summary>��Դ�ļ���С���ֽڣ�</summary>
    public long Length { get; set; }
    /// <summary>��Դ�ļ��ڷ������ϵ�����URL</summary>
    public string RemoteUrl => $"{GetServerBaseUrl()}{Name}.{Extension}";

    /// <summary>��Դ�ļ��ڱ��ػ����е�����·��</summary>
    public string LocalPath => $"{WX.env.USER_DATA_PATH}/__GAME_FILE_CACHE/StreamingAssets/{Name}.{Extension}";

    /// <summary>��ȡ����������URL���Ƴ��ļ������֣�</summary>
    private string GetServerBaseUrl()
    {
        string baseUrl = ProcedureCheckVersion.SERVER_REPORT_URL;
        int lastSlash = baseUrl.LastIndexOf('/');
        return lastSlash >= 0 ? baseUrl.Substring(0, lastSlash + 1) : baseUrl;
    }
}