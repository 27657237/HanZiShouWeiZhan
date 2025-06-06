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
    
    protected PlayerData _datatemp; //��ʱ��Һ������ݼ�

    public override bool UseNativeDialog { get =>true; }

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
        WXBase.cloud.Init(new CallFunctionInitParam()
        {
            env = "cloud1-7gd5lv7o9a24a84f",
            traceUser = false
        });
        
    } 
    // ժҪ:
    //     ����״̬ʱ���á�
    //
    // ����:
    //   procedureOwner:
    //     ���̳����ߡ�
    protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
    {
        
        base.OnEnter(procedureOwner);
        Login(() =>//��½ ��ȡ���ص�code Ȼ�����ִ��
        {
            GetPlayerDataWithCode(_datatemp.code, () => // �����ƺ�����ȡopenid
            {
                GetSetting(() =>//�鿴��Ȩ
                {
                    CheckPlayerExists(_datatemp.openid, () => //����û������򷵻����ݣ���������ע��
                    {
                        string wxUserDataPath = WX.env.USER_DATA_PATH;
                        Log.Error("·��:"+wxUserDataPath); Debug.LogError("·��:"+wxUserDataPath);
                        Debug.Log(GameEntry.UI.OpenUIForm(UIFormId.MenuForm));
                        OnDataDownloadComplete();
                    });
                   
                }); 
                
            });
            
        });
       
    }
    // �ӷ������������ݺ󴥷��¼�
    public void OnDataDownloadComplete()
    {
        // �����Զ����¼�����
        PlayerDataEventArgs e = ReferencePool.Acquire<PlayerDataEventArgs>();
        e.data = _datatemp;

        // �����¼���ʹ��FireNow����������Fire��һ֡����
        GameEntry.Event.FireNow(EventEmum.WritePlayerDataEventID, e); // ��������д��
        Debug.Log($"Before releasing, ob___________ject state: {e.data != null}");

    }

    private  void Login(Action AC)
    {
        LoginOption info = new LoginOption();
        info.complete = (aa) => { /*��¼��ɴ���,�ɹ�ʧ�ܶ����*/ };
        info.fail = (aa) => { Debug.Log("��¼ʧ��: " + aa.errMsg); };
        info.success = (aa) =>
        {
            //��¼�ɹ�����
            Debug.Log("__OnLogin success��½�ɹ�!�鿴Code��" + aa.code);
            _datatemp.code = aa.code;
            AC.Invoke();
            
        };
        WX.Login(info);
    }
    private void GetPlayerDataWithCode(string code,Action AC)
    {
        // ����Ҫ���ݸ��ƺ����Ĳ������޸ģ�action��Ϊ"getOpenidByCode"��
        var dataObj = new
        {
            action = "getOpenidByCode", // ƥ���ƺ�����case��֧
            data = new { code = code }
        };

        // �����ƺ���
        WXBase.cloud.CallFunction(new CallFunctionParam()
        {
            name = "GetPlayerDatas",
            data = dataObj,
            success = (res) =>
            {
                Debug.Log("�ƺ������óɹ�");
                Debug.Log(res.result);
                // ������ƺ������ص�����
                ProcessPlayerData(res.result,AC);

            },
            fail = (res) =>
            {
                Debug.Log("�ƺ�������ʧ��");
                Debug.Log(res.errMsg);
            },
            complete = (res) =>
            {
                Debug.Log("�ƺ������ý���");

            }
        });
    }
    private void GetSetting(Action AC)
    {
        GetSettingOption info = new GetSettingOption();
        info.complete = (aa) => { /*��ȡ���*/ };
        info.fail = (aa) => { /*��ȡʧ��*/};
        info.success = (aa) =>
        {
            //����Ѿ���Ȩ��ֱ�ӻ�ȡ�û���Ϣ
            if (!aa.authSetting.ContainsKey("scope.userInfo") || !aa.authSetting["scope.userInfo"])
            {
                GetShouquan(AC);//������Ȩ
               
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
        //���������ȡ�û���Ϣ
        WXUserInfoButton btn = WX.CreateUserInfoButton(0, 0, Screen.width, Screen.height, "zh_CN", true);
        btn.OnTap((WXUserInfoResponse res) =>
        {
            if (res.errCode == 0)
            {
                Getuserinfo(AC);
            }
            else
            {
                Debug.Log("�û�δ�����ȡ������Ϣ��������: " + res.errCode + "��������Ϣ: " + res.errMsg);
            }
            btn.Hide();
        });
    }
    
    private void Getuserinfo(Action AC)
    {
        // ����ȡ�û���Ϣ������Ȩʱ������ȡ�û���Ϣ��
        GetUserInfoOption userInfoOption = new GetUserInfoOption()
        {
            withCredentials = true,
            lang = "zh_CN",
            success = (data) =>
            {
                // ��ӡ�û���Ϣ��������
                Debug.Log("�û��ǳ�: " + data.userInfo.nickName);
                Debug.Log("�û�ͷ��: " + data.userInfo.avatarUrl);
                Debug.Log("�û��Ա�: " + (data.userInfo.gender == 1 ? "��" : "Ů"));
                Debug.Log("�û�����: " + data.userInfo.country);
                Debug.Log("�û�ʡ��: " + data.userInfo.province);
                Debug.Log("�û�����: " + data.userInfo.city);
                Debug.Log("�û�����: " + data.userInfo.language);
                
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
                Debug.Log("��ȡ�û���Ϣʧ��: " + res.errMsg);
            }
        };
        WX.GetUserInfo(userInfoOption);
    }
    
    //��ȡOPenID
    private void ProcessPlayerData(object result, Action AC)
    {
        try
        {
           
            string resultJson = result.ToString();
            OpenidResult openidResult = JsonUtility.FromJson<OpenidResult>(resultJson);

            if (!openidResult.success)
            {
                Debug.LogError("��ȡopenidʧ��: " + resultJson);
                return;
            }

            _datatemp.openid = openidResult.openid;
            Debug.Log("������openid: " + _datatemp.openid);
            AC.Invoke();
        }
        catch (Exception e)
        {
            Debug.LogError("����openid����ʱ��������: " + e.Message);
        }
    }
    // ����û��Ƿ���ڵķ���
    private  void CheckPlayerExists(string openid, Action AC)
    {
        // ����checkPlayer�Ĳ���
        var checkData = new
        {
            action = "checkPlayer",
            data = new { playerId = openid } // ����openid��ΪplayerId
        };

        // �����ƺ���checkPlayer
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
                        Debug.Log("�û��Ѵ��ڣ����ݴ洢��Һ�������:");
                        _datatemp.gold= checkRes.data.gold;
                        _datatemp.diamonds= checkRes.data.diamonds;
                        _datatemp.turret_talent_code = checkRes.data.turret_talent_code;
                        _datatemp.wall_talent_code = checkRes.data.wall_talent_code;
                        AC.Invoke();
                    }
                    else
                    {
                        // ���2���û������ڣ�����addNewPlayerע��
                        Debug.Log("�û������ڣ���ʼע�����û�...");
                        RegisterNewPlayer(openid,AC);
                    }
                }
                else
                {
                    Debug.LogError("checkPlayerʧ��: " + checkRes.success);
                }
            },
            fail = (res) =>
            {
                Debug.LogError("checkPlayer����ʧ��: " + res.errMsg);
            }
        });
    }
    private  void RegisterNewPlayer(string openid,Action AC)
    {
        // ������player_id��openid���������ֶ����ƺ�������Ĭ��ֵ���ؼ��޸ģ�
        WXBase.cloud.CallFunction(new CallFunctionParam()
        {
            name = "GetPlayerDatas",
            data = new
            {
                action = "addNewPlayer",
                data = new { player_id = openid } // ����openid
            },
            success = (res) => {
                Debug.Log("ע��ɹ����������ݣ�" + res.result);
                string json = res.result.ToString();
                CheckPlayerResult checkRes = JsonConvert.DeserializeObject<CheckPlayerResult>(json);
                // ע��ɹ��󣬿��ٴε���checkPlayerͬ�����ݣ���ֱ��ʹ�÷��ص����û�����
                _datatemp.gold = checkRes.data.gold;
                _datatemp.diamonds = checkRes.data.diamonds;
                _datatemp.turret_talent_code = checkRes.data.turret_talent_code;
                _datatemp.wall_talent_code = checkRes.data.wall_talent_code;
                AC.Invoke();
            },
            fail = (res) => Debug.LogError("ע��ʧ��: " + res.errMsg)
        });
    }
}
// ������ȡopenid�ķ������ݵ��ࣨ����Ϊ�����л���
[System.Serializable]
public class OpenidResult
{
    public bool success;
    public string openid;
}
// ����checkPlayer�������ݵ���
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