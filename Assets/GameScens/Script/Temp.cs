using UnityEngine;
using UnityGameFramework.Runtime;
using System.IO;
using System.Text;
using System;

public class VersionFileDebugger : MonoBehaviour
{
    [Tooltip("直接拖放版本文件到这里")]
    public TextAsset versionFileAsset;
    [SerializeField]
    private TextAsset m_BuildInfoTextAsset = null;

    [ContextMenu("解析版本文件")]
    public void ParseVersionFile()
    {
        if (versionFileAsset == null)
        {
            Log.Error("请先将版本文件拖放到Inspector面板的versionFileAsset字段中");
            return;
        }

        Log.Info($"开始解析版本文件: {versionFileAsset.name}");
        Log.Info($"文件大小: {versionFileAsset.bytes.Length} 字节");

        try
        {
            using (MemoryStream stream = new MemoryStream(versionFileAsset.bytes))
            using (BinaryReader reader = new BinaryReader(stream))
            {
                // 读取文件头部标识
                uint fileHeader = reader.ReadUInt32();
                string headerString = Encoding.ASCII.GetString(BitConverter.GetBytes(fileHeader));
                Log.Info($"文件头部标识: 0x{fileHeader:X8} ({headerString})");

                // 验证是否为UGF版本文件格式
                if (fileHeader != 0x564C5352) // 'RSLV' in ASCII (Resource Version List)
                {
                    Log.Warning("文件格式不匹配标准UGF版本文件，尝试继续解析...");
                }

                // 读取版本信息
                string gameVersion = reader.ReadString();
                int internalResourceVersion = reader.ReadInt32();
                int resourceCount = reader.ReadInt32();

                Log.Info($"游戏版本: {gameVersion}");
                Log.Info($"内部资源版本: {internalResourceVersion}");
                Log.Info($"资源总数: {resourceCount}");

                // 读取资源列表
                Log.Info("开始解析资源列表...");
                int displayCount = Mathf.Min(10, resourceCount);

                for (int i = 0; i < displayCount; i++)
                {
                    string resourceName = reader.ReadString();
                    string resourceVariant = reader.ReadString();
                    string resourceExtension = reader.ReadString();
                    int resourceLength = reader.ReadInt32();
                    int resourceHashCode = reader.ReadInt32();
                    byte loadType = reader.ReadByte();
                    int packedVersion = reader.ReadInt32();

                    Log.Info($"资源 #{i + 1}: {resourceName}{resourceExtension} (大小: {resourceLength}B, 哈希: 0x{resourceHashCode:X8})");
                    Log.Info($"  变体: {resourceVariant}, 加载类型: {loadType}, 打包版本: {packedVersion}");
                }

                if (resourceCount > 10)
                {
                    Log.Info($"... 还有 {resourceCount - 10} 个资源未显示");
                }

                Log.Info("版本文件解析完成");
            }
        }
        catch (System.Exception e)
        {
            Log.Error($"解析版本文件时发生异常: {e.Message}");
            Debug.LogException(e);
        }
    }
}