using UnityEngine;
using UnityGameFramework.Runtime;
using System.IO;
using System.Text;
using System;

public class VersionFileDebugger : MonoBehaviour
{
    [Tooltip("ֱ���ϷŰ汾�ļ�������")]
    public TextAsset versionFileAsset;
    [SerializeField]
    private TextAsset m_BuildInfoTextAsset = null;

    [ContextMenu("�����汾�ļ�")]
    public void ParseVersionFile()
    {
        if (versionFileAsset == null)
        {
            Log.Error("���Ƚ��汾�ļ��Ϸŵ�Inspector����versionFileAsset�ֶ���");
            return;
        }

        Log.Info($"��ʼ�����汾�ļ�: {versionFileAsset.name}");
        Log.Info($"�ļ���С: {versionFileAsset.bytes.Length} �ֽ�");

        try
        {
            using (MemoryStream stream = new MemoryStream(versionFileAsset.bytes))
            using (BinaryReader reader = new BinaryReader(stream))
            {
                // ��ȡ�ļ�ͷ����ʶ
                uint fileHeader = reader.ReadUInt32();
                string headerString = Encoding.ASCII.GetString(BitConverter.GetBytes(fileHeader));
                Log.Info($"�ļ�ͷ����ʶ: 0x{fileHeader:X8} ({headerString})");

                // ��֤�Ƿ�ΪUGF�汾�ļ���ʽ
                if (fileHeader != 0x564C5352) // 'RSLV' in ASCII (Resource Version List)
                {
                    Log.Warning("�ļ���ʽ��ƥ���׼UGF�汾�ļ������Լ�������...");
                }

                // ��ȡ�汾��Ϣ
                string gameVersion = reader.ReadString();
                int internalResourceVersion = reader.ReadInt32();
                int resourceCount = reader.ReadInt32();

                Log.Info($"��Ϸ�汾: {gameVersion}");
                Log.Info($"�ڲ���Դ�汾: {internalResourceVersion}");
                Log.Info($"��Դ����: {resourceCount}");

                // ��ȡ��Դ�б�
                Log.Info("��ʼ������Դ�б�...");
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

                    Log.Info($"��Դ #{i + 1}: {resourceName}{resourceExtension} (��С: {resourceLength}B, ��ϣ: 0x{resourceHashCode:X8})");
                    Log.Info($"  ����: {resourceVariant}, ��������: {loadType}, ����汾: {packedVersion}");
                }

                if (resourceCount > 10)
                {
                    Log.Info($"... ���� {resourceCount - 10} ����Դδ��ʾ");
                }

                Log.Info("�汾�ļ��������");
            }
        }
        catch (System.Exception e)
        {
            Log.Error($"�����汾�ļ�ʱ�����쳣: {e.Message}");
            Debug.LogException(e);
        }
    }
}