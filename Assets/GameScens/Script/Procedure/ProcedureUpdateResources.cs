using System.Collections.Generic;
using System;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
using WeChatWASM;
using UnityEngine;

/// <summary>
/// 资源更新流程
/// </summary>
public class ProcedureUpdateResources : ProcedureBase
{
    private SeverResourceWrapper _SeverResourceWrapper;

    // 需要更新的资源列表
    private List<ResourceInfo> resourcesToUpdate;

    // 当前正在更新的资源索引
    private int currentIndex = -1;

    // 服务器报告的XML内容（用于更新完成后覆盖本地）
    private string serverReportXml;

    // 更新状态
    private bool isUpdating = false;
    private bool allSuccess = true;

    public override bool UseNativeDialog { get => throw new NotImplementedException(); }

    protected override void OnEnter(ProcedureOwner procedureOwner)
    {
        base.OnEnter(procedureOwner);

        // 获取从检查流程传递过来的数据
        _SeverResourceWrapper = procedureOwner.GetData<SeverResourceWrapper>("SeverResourceWrapper");
        resourcesToUpdate = _SeverResourceWrapper.ResourceInfoListValue;
        serverReportXml = _SeverResourceWrapper.XMLStringValue;
        
        if (resourcesToUpdate == null || resourcesToUpdate.Count == 0)
        {
            Log.Warning("没有需要更新的资源");
            procedureOwner.SetData("ResourcesToUpdate", _SeverResourceWrapper);
            // 直接切换到验证流程
            ChangeState<ProcedureVerifyResources>(procedureOwner);
            return;
        }

        Log.Info($"开始资源更新流程，共 {resourcesToUpdate.Count} 个资源需要更新");
        currentIndex = 0;
        isUpdating = true;
        allSuccess = true;

        // 开始第一个资源更新
        DownloadNextResource();
    }

    protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
        if (!isUpdating)
        {
            //这需要完善，将服务器报告传递给下一个人
            procedureOwner.SetData("SeverResourceWrapper", new SeverResourceWrapper(_SeverResourceWrapper));
            Debug.Log(_SeverResourceWrapper.XMLReportValue);
            ChangeState<ProcedureVerifyResources>(procedureOwner);
        }
        
    }

    private void DownloadNextResource()
    {
        if (currentIndex >= resourcesToUpdate.Count)
        {
            // 所有资源更新完成
            FinishUpdate();
            return;
        }

        var resource = resourcesToUpdate[currentIndex];
        Log.Info($"开始更新资源 ({currentIndex + 1}/{resourcesToUpdate.Count}): {resource.Name},资源下载位置：{resource.RemoteUrl}，最终保存位置：{resource.LocalPath}");
        //string localPath = resource.LocalPath.Replace("http://usr/", "wxfile://usr/");
        // 确保目标目录存在
        var fs = WX.GetFileSystemManager();
        string dirPath = GetDirectoryPath(resource.LocalPath);

        // 递归创建目录
        if (fs.AccessSync(dirPath) != "access:ok")
        {
            fs.MkdirSync(dirPath, true);
            Log.Info($"目录创建成功：{dirPath}");
        }

        WX.DownloadFile(new DownloadFileOption
        {
            url = resource.RemoteUrl,
            filePath = resource.LocalPath, // 直接保存到最终位置
            success = res =>
            {
                if (res.statusCode == 200)
                {
                    Log.Info($"资源下载成功: {resource.Name}");
                    currentIndex++;
                    DownloadNextResource();
                }
                else
                {
                    Log.Error($"资源下载失败: {resource.Name} (状态码: {res.statusCode})");
                    HandleDownloadError(resource);
                }
            },
            fail=( err) =>
            {
                Log.Error($"资源下载失败: {resource.Name} ({err.errMsg})");
                HandleDownloadError(resource);
            }
        });
    }
    // 从文件路径提取目录路径
    private string GetDirectoryPath(string filePath)
    {
        int lastSlash = filePath.LastIndexOf('/');
        return lastSlash >= 0 ? filePath.Substring(0, lastSlash) : filePath;
    }
    private void HandleDownloadError(ResourceInfo resource)
    {
        // 标记失败，但继续尝试更新其他资源
        allSuccess = false;

        // 显示错误提示（但不中断流程）
        WX.ShowModal(new ShowModalOption
        {
            title = "资源更新失败",
            content = $"资源 {resource.Name} 更新失败，游戏可能无法正常运行",
            showCancel = false
        });

        currentIndex++;
        DownloadNextResource();
    }

    private void FinishUpdate()
    {
        isUpdating = false;

        if (allSuccess)
        {
            // 更新服务器报告到本地
            SaveServerReport();

            Log.Info("所有资源更新完成");
           
        }
        else
        {
            Log.Warning("资源更新完成，但有部分失败");
            // 仍然继续游戏，但显示警告
            WX.ShowModal(new ShowModalOption
            {
                title = "资源更新",
                content = "资源更新完成，但有部分资源更新失败，游戏可能无法正常运行",
                showCancel = false,
                success = _ =>
                {
                    // 更新服务器报告到本地
                    SaveServerReport();
                    // ChangeState<ProcedureVerifyResources>(procedureOwner);
                }
            });
        }
      
    }

    /// <summary>
    /// 保存服务器报告到本地（安全覆盖）
    /// </summary>
    private void SaveServerReport()
    {
        if (string.IsNullOrEmpty(serverReportXml))
        {
            Log.Warning("没有可用的服务器报告内容");
            return;
        }

        try
        {
            var fs = WX.GetFileSystemManager();
            var reportPath = $"{WX.env.USER_DATA_PATH}/__GAME_FILE_CACHE/StreamingAssets/BuildReport.xml";

            // 使用微信API写入文件
            fs.WriteFileSync(reportPath, serverReportXml, "utf8");
            Log.Info("服务器报告已更新到本地");
        }
        catch (Exception e)
        {
            Log.Error($"更新服务器报告失败: {e.Message}");
        }
    }
}