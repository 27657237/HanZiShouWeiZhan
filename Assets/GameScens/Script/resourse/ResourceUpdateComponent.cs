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
    // 摘要:
    //     状态初始化时调用。
    //
    // 参数:
    //   procedureOwner:
    //     流程持有者。
    protected override void OnInit(IFsm<IProcedureManager> procedureOwner)
    {
        base.OnInit(procedureOwner);
       fs = WX.GetFileSystemManager();
        // 【推荐】原本使用WebRequest的话可按如下修改
       
        
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
            bundle.WXUnload(false); //bundle还是AssetBundle类型，但需要调用扩展方法WXUnload()才可真正卸载
        }

        
    }*/
    // 摘要:
    //     进入状态时调用。
    //
    // 参数:
    //   procedureOwner:
    //     流程持有者。
    protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
    {

        base.OnEnter(procedureOwner);
       
    }
    //
    // 摘要:
    //     离开状态时调用。
    //
    // 参数:
    //   procedureOwner:
    //     流程持有者。
    //
    //   isShutdown:
    //     是否是关闭状态机时触发。
    protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
    {
        base.OnLeave(procedureOwner, isShutdown);
    }
}
