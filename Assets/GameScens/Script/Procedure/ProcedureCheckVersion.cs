using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using UnityGameFramework.Runtime;
using WeChatWASM;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
using GameFramework;
using System.Data.SqlTypes;
/// <summary>
/// 微信小游戏版本检查流程
/// </summary>
public class ProcedureCheckVersion : ProcedureBase
{
    #region 配置常量
    // 服务器构建报告地址
    public const string SERVER_REPORT_URL = "https://636c-cloud1-7gd5lv7o9a24a84f-1359799964.tcb.qcloud.la/webgl/StreamingAssets/BuildReport.xml";
    #endregion

    #region 状态控制
    private bool isCheckDone = false;
    private bool requireUpdate = false;

    // 解析的报告信息
    private BuildReport serverReport;
    private BuildReport localReport;

    // 需要更新的资源列表
    public List<ResourceInfo> ResourcesToUpdate { get; private set; } = new List<ResourceInfo>();

    // 存储服务器报告的XML内容
    private string serverReportXmlContent;
    #endregion

    public override bool UseNativeDialog => true;

    protected override void OnEnter(ProcedureOwner procedureOwner)
    {
        base.OnEnter(procedureOwner);
        ResetStates();
        Log.Info("===== 开始版本检查流程 =====");
        StartDownloadReport();
    }

    protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
        if (!isCheckDone) return;
        isCheckDone = false;

        
        // 版本比较
        requireUpdate = serverReport.InternalResourceVersion > localReport.InternalResourceVersion;
        Log.Info($"版本对比：服务器[{serverReport.InternalResourceVersion}] vs 本地[{localReport.InternalResourceVersion}]，更新需求：{requireUpdate}");
        // 将需要更新的资源传递给更新流程
        procedureOwner.SetData("SeverResourceWrapper", new SeverResourceWrapper(ResourcesToUpdate, serverReportXmlContent, serverReport));
        if (requireUpdate)
        {
            // 对比资源差异
            CompareResources();

            if (ResourcesToUpdate.Count > 0)
            {
                Log.Info($"检测到 {ResourcesToUpdate.Count} 个资源需要更新，切换至更新流程");
                //ChangeState<ProcedureUpdateVersion>(procedureOwner);
                ChangeState<ProcedureUpdateResources>(procedureOwner);
            }
            else
            {
                Log.Info("资源已是最新版本，切换至验证流程");
                 ChangeState<ProcedureVerifyResources>(procedureOwner);
            }
        }
        else
        {
            Log.Info("资源为最新版本，切换至验证流程");
             ChangeState<ProcedureVerifyResources>(procedureOwner);
        }
    }

    
    /// <summary>
    /// 对比服务器和本地资源差异
    /// </summary>
    private void CompareResources()
    {
        ResourcesToUpdate.Clear();

        // 防御性检查：本地资源列表可能为null（例如解析失败或首次运行）
        if (localReport.Resources == null)
        {
            Log.Warning("本地资源列表为null，已初始化为空列表");
            localReport.Resources = new List<ResourceInfo>(); // 初始化为空列表
        }

        // 防御性检查：服务器资源列表不能为空（否则无法对比）
        if (serverReport.Resources == null)
        {
            Log.Error("服务器资源列表为null，无法进行资源对比");
            return; // 提前返回避免后续空引用
        }

        try
        {
            // 创建本地资源字典（按名称索引）
            var localDict = localReport.Resources.ToDictionary(r => r.Name);

            foreach (var serverResource in serverReport.Resources)
            {
                if (localDict.TryGetValue(serverResource.Name, out var localResource))
                {
                    // 检查哈希值是否变化
                    if (serverResource.HashCode != localResource.HashCode)
                    {
                        ResourcesToUpdate.Add(serverResource);
                        Log.Info($"资源 '{serverResource.Name}' 有更新: {localResource.HashCode} -> {serverResource.HashCode}");
                    }
                }
                else
                {
                    // 本地不存在的新资源
                    ResourcesToUpdate.Add(serverResource);
                    Log.Info($"发现新资源 '{serverResource.Name}'");
                }
            }
        }
        catch (Exception e)
        {
            Log.Error($"资源对比过程中发生异常: {e.Message}");
        }
    }

    private void ResetStates()
    {
        isCheckDone = false;
        requireUpdate = false;
        serverReport = new BuildReport();
        localReport = new BuildReport();
        ResourcesToUpdate.Clear();
        serverReportXmlContent = null;
    }

    private void StartDownloadReport()
    {
        Log.Info($"开始下载构建报告：{SERVER_REPORT_URL}");

        WX.DownloadFile(new DownloadFileOption
        {
            url = SERVER_REPORT_URL,
            success = OnDownloadSuccess,
            fail = OnDownloadFailed
        });
    }

    private void OnDownloadSuccess(DownloadFileSuccessCallbackResult res)
    {
        if (res.statusCode != 200)
        {
            Log.Error($"下载失败（状态码：{res.statusCode}）");
            ShowErrorDialog("构建报告下载失败");
            return;
        }

        Log.Info($"下载成功，临时路径：{res.tempFilePath}");
        ParseReportFromTempFile(res.tempFilePath);
        LoadLocalVersion();
        isCheckDone = true;
    }

    private void ParseReportFromTempFile(string tempFilePath)
    {
        try
        {
            var fs = WX.GetFileSystemManager();
            byte[] fileData = fs.ReadFileSync(tempFilePath);
            serverReportXmlContent = Encoding.UTF8.GetString(fileData);
            serverReport = BuildReportParser.Parse(serverReportXmlContent);

            Log.Info($"服务器版本解析成功: v{serverReport.InternalResourceVersion}, 资源数量: {serverReport.Resources.Count}");
        }
        catch (Exception e)
        {
            Log.Error($"XML解析失败：{e.Message}");
            ShowErrorDialog("构建报告解析失败");
        }
    }

    private void LoadLocalVersion()
    {
        var fs = WX.GetFileSystemManager();
        try
        {
            // 定义本地报告路径
            string localReportPath = $"{WX.env.USER_DATA_PATH}/__GAME_FILE_CACHE/StreamingAssets/BuildReport.xml";
            string reportDirectory = localReportPath.Substring(0, localReportPath.LastIndexOf('/'));

            // 步骤1：检查并创建目录（首次运行时可能不存在）
            try
            {
                fs.AccessSync(reportDirectory); // 检查目录是否存在
            }
            catch
            {
                // 目录不存在，递归创建（包括父目录）
                fs.MkdirSync(reportDirectory, true);
                Log.Info($"目录创建成功：{reportDirectory}");
            }

            // 步骤2：检查文件是否存在
            try
            {
                fs.AccessSync(localReportPath); // 检查文件是否存在
            }
            catch
            {
                // 文件不存在，视为初始版本
                Log.Warning("本地报告文件不存在，视为初始版本");
                localReport = new BuildReport { InternalResourceVersion = 0, Resources = new List<ResourceInfo>() };
                return;
            }

            // 步骤3：读取并解析文件
            string localXml = fs.ReadFileSync(localReportPath, "utf8");
            localReport = BuildReportParser.Parse(localXml);

            // 防御性检查：确保Resources非空（避免解析后为null）
            if (localReport.Resources == null)
            {
                localReport.Resources = new List<ResourceInfo>();
            }

            Log.Info($"本地版本解析成功: v{localReport.InternalResourceVersion}, 资源数量: {localReport.Resources.Count}");
        }
        catch (Exception e)
        {
            // 其他异常（如解析失败）
            Log.Error($"加载本地报告失败: {e.Message}，视为初始版本");
            localReport = new BuildReport { InternalResourceVersion = 0, Resources = new List<ResourceInfo>() };
        }
    }

    private void OnDownloadFailed(GeneralCallbackResult err)
    {
        Log.Error($"下载失败：{err.errMsg}");
        ShowErrorDialog($"网络请求失败：{err.errMsg}");
    }

    private void ShowErrorDialog(string msg)
    {
        WX.ShowModal(new ShowModalOption
        {
            title = "版本检查异常",
            content = msg,
            showCancel = false,
            success = _ => isCheckDone = true
        });
    }
}

/// <summary>
/// 资源列表包装类 - 用于FSM数据传递
/// </summary>
public class SeverResourceWrapper : Variable
{
    public List<ResourceInfo> ResourceInfoListValue;
    public string XMLStringValue;
    public BuildReport XMLReportValue;


    public override Type Type => typeof(SeverResourceWrapper);
    public SeverResourceWrapper(List<ResourceInfo> resourceInfoListValue=null, string xMLString=null, BuildReport xMLReport = null)
    {
        ResourceInfoListValue = resourceInfoListValue;
        XMLStringValue = xMLString;
        XMLReportValue = xMLReport;
    }

    public SeverResourceWrapper(BuildReport serverReport)
    {
        XMLReportValue = serverReport;
    }
    public SeverResourceWrapper(string xMLString = null)
    {
        XMLStringValue = xMLString;
    }

    public SeverResourceWrapper(SeverResourceWrapper severResourceWrapper)
    {
        ResourceInfoListValue = severResourceWrapper.ResourceInfoListValue;
        XMLStringValue = severResourceWrapper.XMLStringValue;
        XMLReportValue = severResourceWrapper.XMLReportValue;
        severResourceWrapper.Clear();
        severResourceWrapper = null;
    }

    public override void Clear()
    {
        ResourceInfoListValue = null;
        XMLReportValue=null;
        XMLStringValue = null;
        // throw new NotImplementedException();
    }

    public override object GetValue()
    {
        //t/hrow new NotImplementedException();
        return this;
    }

    public override void SetValue(object value )
    {
        // throw new NotImplementedException();
       // this.ResourceInfoListValue = value.ToString();
    }
}

