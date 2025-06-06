using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityGameFramework.Runtime;
using WeChatWASM;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

/// <summary>
/// 资源验证流程 - 验证本地缓存资源与服务器版本报告的匹配情况
/// </summary>
public class ProcedureVerifyResources : ProcedureBase
{
    #region 状态控制
    private bool isVerificationComplete = false;
    private BuildReport serverReport;  // 服务器版本报告
    private List<ResourceInfo> invalidResources = new List<ResourceInfo>();  // 验证失败的资源列表
    private const int Retry=5;//最大重试次数
    private static int number=0;
    private SeverResourceWrapper _SeverResourceWrapper;
    #endregion

    public override bool UseNativeDialog => true;

    protected override void OnEnter(ProcedureOwner procedureOwner)
    {
        base.OnEnter(procedureOwner);
        Log.Info("===== 开始资源验证流程 =====");
     
        // 重置状态
        isVerificationComplete = false;
        invalidResources.Clear();

        // 从流程数据获取服务器报告（从检查流程传递的最新报告）
        _SeverResourceWrapper = procedureOwner.GetData<SeverResourceWrapper>("SeverResourceWrapper");
        serverReport = _SeverResourceWrapper.XMLReportValue;
        if (serverReport == null)
        {
            Log.Error("未获取到服务器报告数据，无法进行验证");
            isVerificationComplete = true;
            return;
        }

        Log.Info($"开始验证资源，共{serverReport.Resources.Count}个资源需要验证。");

        // 开始逐个验证资源
        VerifyResources();
    }

    protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

        if (!isVerificationComplete) return;
        // 传递需要更新的资源列表给更新流程
        procedureOwner.SetData("ResourcesToUpdate",new SeverResourceWrapper(_SeverResourceWrapper));
        // 处理验证结果
        if (invalidResources.Count == 0)
        {
            Log.Info("所有资源验证通过，准备进入游戏流程");
            number = 0;
            // 切换到预加载流程或进入游戏流程
            ChangeState<ProcedurePreload>(procedureOwner);
        }
        else
        {
            Log.Warning($"发现{invalidResources.Count}个资源验证失败，将进行资源更新,当前重试次数：{number}");
            if (number < Retry)
            {
                number++;
                // 切换到资源更新流程
                ChangeState<ProcedureUpdateResources>(procedureOwner);
            }
            else
            {
                WX.ShowModal(new ShowModalOption
                {
                    title = "版本资源检查更新异常",
                    content = $"共有{invalidResources.Count}个资源发生异常",
                    showCancel = false,
                });
                isVerificationComplete = false;
            }
           
        }
    }

    /// <summary>
    /// 逐个验证资源
    /// </summary>
    private void VerifyResources()
    {
        var fs = WX.GetFileSystemManager();
        int totalResources = serverReport.Resources.Count;
        int processedCount = 0; // 记录已处理的资源数量

        foreach (var resource in serverReport.Resources)
        {
            string resourceName = $"{resource.Name}.{resource.Extension}";
            string localPath = resource.LocalPath;

            try
            {
                // 1. 同步检查文件是否存在
                fs.AccessSync(localPath);

                // 2. 创建 GetFileInfoOption 实例
                var option = new GetFileInfoOption();
                option.filePath = localPath;

                // 3. 定义成功回调：验证文件大小
                option.success = (GetFileInfoSuccessCallbackResult result) =>
                {
                    // 验证文件大小
                    if (result.size != resource.Length)
                    {
                        Log.Warning($"资源 '{resourceName}' 大小不匹配（本地:{result.size} 服务器:{resource.Length}）");
                        invalidResources.Add(resource);
                    }
                    else
                    {
                        Log.Info($"资源 '{resourceName}' 验证合格\n" +
                                 $"  路径: {localPath}\n" +
                                 $"  大小: {resource.Length} 字节\n" +
                                 $"  哈希: {resource.HashCode}");
                    }

                    // 资源处理完成后计数，所有资源处理完后更新状态
                    processedCount++;
                    if (processedCount >= totalResources)
                    {
                        Log.Info($"资源验证完成: {totalResources - invalidResources.Count}通过, {invalidResources.Count}失败");
                        isVerificationComplete = true;
                    }
                };

                // 4. 定义失败回调：标记资源无效
                option.fail = (FileError error) =>
                {
                    Log.Warning($"资源 '{resourceName}' 获取文件信息失败！错误信息: {error.errMsg}");
                    invalidResources.Add(resource);

                    // 资源处理完成后计数
                    processedCount++;
                    if (processedCount >= totalResources)
                    {
                        Log.Info($"资源验证完成: {totalResources - invalidResources.Count}通过, {invalidResources.Count}失败");
                        isVerificationComplete = true;
                    }
                };

                // 5. 触发异步获取文件信息
                fs.GetFileInfo(option);
            }
            catch (Exception ex)
            {
                Log.Warning($"资源 '{resourceName}' 不存在或访问失败: {localPath}，错误: {ex.Message}");
                invalidResources.Add(resource);

                // 异常时仍需计数（避免死等）
                processedCount++;
                if (processedCount >= totalResources)
                {
                    Log.Info($"资源验证完成: {totalResources - invalidResources.Count}通过, {invalidResources.Count}失败");
                    isVerificationComplete = true;
                }
            }
        }
    }
}