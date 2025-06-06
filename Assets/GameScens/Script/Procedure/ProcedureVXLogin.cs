using GameFramework;
using GameFramework.Fsm;
using GameFramework.Procedure;
using Newtonsoft.Json;
using System;
using System.Security.Cryptography;
using UnityEngine;
using UnityGameFramework.Runtime;
using WeChatWASM;

public class ProcedureVXLogin : ProcedureBase
{
    
    protected PlayerData _datatemp; //临时玩家核心数据集

    public override bool UseNativeDialog { get =>true; }

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
        WXBase.cloud.Init(new CallFunctionInitParam()
        {
            env = "cloud1-7gd5lv7o9a24a84f",
            traceUser = false
        });
        
    } 
    // 摘要:
    //     进入状态时调用。
    //
    // 参数:
    //   procedureOwner:
    //     流程持有者。
    protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
    {
        
        base.OnEnter(procedureOwner);
        Login(() =>//登陆 获取返回的code 然后继续执行
        {
            GetPlayerDataWithCode(_datatemp.code, () => // 调用云函数获取openid
            {
                GetSetting(() =>//查看授权
                {
                    CheckPlayerExists(_datatemp.openid, () => //检查用户存在则返回数据，不存在则注册
                    {
                        string wxUserDataPath = WX.env.USER_DATA_PATH;
                        Log.Error("路径:"+wxUserDataPath); Debug.LogError("路径:"+wxUserDataPath);
                        Debug.Log(GameEntry.UI.OpenUIForm(UIFormId.MenuForm));
                        OnDataDownloadComplete();
                    });
                   
                }); 
                
            });
            
        });
       
    }
    // 从服务器下载数据后触发事件
    public void OnDataDownloadComplete()
    {
        // 创建自定义事件参数
        PlayerDataEventArgs e = ReferencePool.Acquire<PlayerDataEventArgs>();
        e.data = _datatemp;

        // 触发事件（使用FireNow立即处理，或Fire下一帧处理）
        GameEntry.Event.FireNow(EventEmum.WritePlayerDataEventID, e); // 立即触发写入
        Debug.Log($"Before releasing, ob___________ject state: {e.data != null}");

    }

    private  void Login(Action AC)
    {
        LoginOption info = new LoginOption();
        info.complete = (aa) => { /*登录完成处理,成功失败都会调*/ };
        info.fail = (aa) => { Debug.Log("登录失败: " + aa.errMsg); };
        info.success = (aa) =>
        {
            //登录成功处理
            Debug.Log("__OnLogin success登陆成功!查看Code：" + aa.code);
            _datatemp.code = aa.code;
            AC.Invoke();
            
        };
        WX.Login(info);
    }
    private void GetPlayerDataWithCode(string code,Action AC)
    {
        // 构造要传递给云函数的参数（修改：action改为"getOpenidByCode"）
        var dataObj = new
        {
            action = "getOpenidByCode", // 匹配云函数的case分支
            data = new { code = code }
        };

        // 调用云函数
        WXBase.cloud.CallFunction(new CallFunctionParam()
        {
            name = "GetPlayerDatas",
            data = dataObj,
            success = (res) =>
            {
                Debug.Log("云函数调用成功");
                Debug.Log(res.result);
                // 处理从云函数返回的数据
                ProcessPlayerData(res.result,AC);

            },
            fail = (res) =>
            {
                Debug.Log("云函数调用失败");
                Debug.Log(res.errMsg);
            },
            complete = (res) =>
            {
                Debug.Log("云函数调用结束");

            }
        });
    }
    private void GetSetting(Action AC)
    {
        GetSettingOption info = new GetSettingOption();
        info.complete = (aa) => { /*获取完成*/ };
        info.fail = (aa) => { /*获取失败*/};
        info.success = (aa) =>
        {
            //如果已经授权则直接获取用户信息
            if (!aa.authSetting.ContainsKey("scope.userInfo") || !aa.authSetting["scope.userInfo"])
            {
                GetShouquan(AC);//调起授权
               
            }
            else
            {
                Getuserinfo(AC);

            }
           
        };
        WX.GetSetting(info);
    }

    private  void GetShouquan(Action AC)
    {
        //调用请求获取用户信息
        WXUserInfoButton btn = WX.CreateUserInfoButton(0, 0, Screen.width, Screen.height, "zh_CN", true);
        btn.OnTap((WXUserInfoResponse res) =>
        {
            if (res.errCode == 0)
            {
                Getuserinfo(AC);
            }
            else
            {
                Debug.Log("用户未允许获取个人信息，错误码: " + res.errCode + "，错误信息: " + res.errMsg);
            }
            btn.Hide();
        });
    }
    
    private void Getuserinfo(Action AC)
    {
        // 《获取用户信息》（授权时主动获取用户信息）
        GetUserInfoOption userInfoOption = new GetUserInfoOption()
        {
            withCredentials = true,
            lang = "zh_CN",
            success = (data) =>
            {
                // 打印用户信息（新增）
                Debug.Log("用户昵称: " + data.userInfo.nickName);
                Debug.Log("用户头像: " + data.userInfo.avatarUrl);
                Debug.Log("用户性别: " + (data.userInfo.gender == 1 ? "男" : "女"));
                Debug.Log("用户国家: " + data.userInfo.country);
                Debug.Log("用户省份: " + data.userInfo.province);
                Debug.Log("用户城市: " + data.userInfo.city);
                Debug.Log("用户语言: " + data.userInfo.language);
                
                try
                {
                    PlayerUserInfoEventArgs I = ReferencePool.Acquire<PlayerUserInfoEventArgs>();
                    I.info = data.userInfo;
                    GameEntry.Event.FireNow(EventEmum.WritePlayerUserInfoEventID, I);
                    //ReferencePool.Release(I);
                }
                catch (Exception)
                {

                    throw;
                }
                AC.Invoke();
            },
            fail = (res) =>
            {
                Debug.Log("获取用户信息失败: " + res.errMsg);
            }
        };
        WX.GetUserInfo(userInfoOption);
    }
    
    //获取OPenID
    private void ProcessPlayerData(object result, Action AC)
    {
        try
        {
           
            string resultJson = result.ToString();
            OpenidResult openidResult = JsonUtility.FromJson<OpenidResult>(resultJson);

            if (!openidResult.success)
            {
                Debug.LogError("获取openid失败: " + resultJson);
                return;
            }

            _datatemp.openid = openidResult.openid;
            Debug.Log("解析到openid: " + _datatemp.openid);
            AC.Invoke();
        }
        catch (Exception e)
        {
            Debug.LogError("处理openid数据时发生错误: " + e.Message);
        }
    }
    // 检查用户是否存在的方法
    private  void CheckPlayerExists(string openid, Action AC)
    {
        // 构造checkPlayer的参数
        var checkData = new
        {
            action = "checkPlayer",
            data = new { playerId = openid } // 传递openid作为playerId
        };

        // 调用云函数checkPlayer
        WXBase.cloud.CallFunction(new CallFunctionParam()
        {
            name = "GetPlayerDatas",
            data = checkData,
            success = (res) =>
            {
                 string json = res.result.ToString();
                CheckPlayerResult checkRes = JsonConvert.DeserializeObject<CheckPlayerResult>(json);
                if (checkRes.success)
                {
                    if (checkRes.data != null)
                    {
                        Debug.Log("用户已存在，数据存储玩家核心数据:");
                        _datatemp.gold= checkRes.data.gold;
                        _datatemp.diamonds= checkRes.data.diamonds;
                        _datatemp.turret_talent_code = checkRes.data.turret_talent_code;
                        _datatemp.wall_talent_code = checkRes.data.wall_talent_code;
                        AC.Invoke();
                    }
                    else
                    {
                        // 情况2：用户不存在，调用addNewPlayer注册
                        Debug.Log("用户不存在，开始注册新用户...");
                        RegisterNewPlayer(openid,AC);
                    }
                }
                else
                {
                    Debug.LogError("checkPlayer失败: " + checkRes.success);
                }
            },
            fail = (res) =>
            {
                Debug.LogError("checkPlayer调用失败: " + res.errMsg);
            }
        });
    }
    private  void RegisterNewPlayer(string openid,Action AC)
    {
        // 仅传递player_id（openid），其他字段由云函数设置默认值（关键修改）
        WXBase.cloud.CallFunction(new CallFunctionParam()
        {
            name = "GetPlayerDatas",
            data = new
            {
                action = "addNewPlayer",
                data = new { player_id = openid } // 仅传openid
            },
            success = (res) => {
                Debug.Log("注册成功，返回数据：" + res.result);
                string json = res.result.ToString();
                CheckPlayerResult checkRes = JsonConvert.DeserializeObject<CheckPlayerResult>(json);
                // 注册成功后，可再次调用checkPlayer同步数据，或直接使用返回的新用户数据
                _datatemp.gold = checkRes.data.gold;
                _datatemp.diamonds = checkRes.data.diamonds;
                _datatemp.turret_talent_code = checkRes.data.turret_talent_code;
                _datatemp.wall_talent_code = checkRes.data.wall_talent_code;
                AC.Invoke();
            },
            fail = (res) => Debug.LogError("注册失败: " + res.errMsg)
        });
    }
}
// 解析获取openid的返回数据的类（需标记为可序列化）
[System.Serializable]
public class OpenidResult
{
    public bool success;
    public string openid;
}
// 解析checkPlayer返回数据的类
[System.Serializable]
public class CheckPlayerResult
{
    public bool success;
    public PlayerData data;
}
internal class CallFunctionInitParam : ICloudConfig
{
    public string env { get; set; }
    public bool traceUser { get; set; }
}