using System;

using GameFramework;
using GameFramework.Resource;
using UnityGameFramework.Runtime;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using GameFramework.Procedure;
using GameFramework.Fsm;
using WeChatWASM;
using UnityEngine.Networking;


public class ResourceUpdateComponent :ProcedureBase
{
    WXFileSystemManager fs;

    public override bool UseNativeDialog { get => true; }

    //
    // ժҪ:
    //     ״̬��ʼ��ʱ���á�
    //
    // ����:
    //   procedureOwner:
    //     ���̳����ߡ�
    protected override void OnInit(IFsm<IProcedureManager> procedureOwner)
    {
        base.OnInit(procedureOwner);
       fs = WX.GetFileSystemManager();
        // ���Ƽ���ԭ��ʹ��WebRequest�Ļ��ɰ������޸�
       
        
    }
    /*IEnumerator Down()
    {
        UnityWebRequest bundleReq = WXAssetBundle.GetAssetBundle(url); // UnityWebRequestAssetBundle => WXAssetBundle
        yield return bundleReq.SendWebRequest();
        if (bundleReq.isHttpError)
        {
            Debug.LogError(GetType() + "/ERROR/" + bundleReq.error);
        }
        else
        {
            AssetBundle bundle = (bundleReq.downloadHandler as DownloadHandlerWXAssetBundle).assetBundle; // DownloadHandlerAssetBundle => DownloadHandlerWXAssetBundle
            bundle.WXUnload(false); //bundle����AssetBundle���ͣ�����Ҫ������չ����WXUnload()�ſ�����ж��
        }

        
    }*/
    // ժҪ:
    //     ����״̬ʱ���á�
    //
    // ����:
    //   procedureOwner:
    //     ���̳����ߡ�
    protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
    {

        base.OnEnter(procedureOwner);
       
    }
    //
    // ժҪ:
    //     �뿪״̬ʱ���á�
    //
    // ����:
    //   procedureOwner:
    //     ���̳����ߡ�
    //
    //   isShutdown:
    //     �Ƿ��ǹر�״̬��ʱ������
    protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
    {
        base.OnLeave(procedureOwner, isShutdown);
    }
}
