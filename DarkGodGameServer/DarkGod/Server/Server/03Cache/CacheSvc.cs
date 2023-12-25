using System.Collections.Generic;
using PEProtocol;

//功能：缓存层
public class CacheSvc
{
    private static CacheSvc? instance = null;
    public static CacheSvc Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new CacheSvc();
            }
            return instance;
        }
    }

    //定义一个字典，用于存储当前上线的账号与与它对应的ServerSession连接。
    private Dictionary<string, ServerSession> onLineAcctDic = new Dictionary<string,ServerSession>();
    private Dictionary<ServerSession, PlayerData> onLineSessionDic = new Dictionary<ServerSession,PlayerData>();
    public void Init()
    {
        PECommon.Log("CacheSvc Init Done.");
    }

    //判断当前账号是否已经上线
    public bool IsAcctOnline(string acct)
    {
        return onLineAcctDic.ContainsKey(acct);  //如果key存在说明账号在线，则返回true，否则返回false
    }

    /// <summary>
    /// 根据账号密码返回对应账号数据，密码错误返回null，账号不存在则默认创建新账号
    /// </summary>
    //用于获取当前PlayerData
    public PlayerData GetPlayerData(string acct, string pass)
    {
        //TODO
        //从数据库中查找账号数据
        return null;
    }

    /// <summary>
    /// 账号上线，缓存账号数据
    /// </summary>
    public void AcctOnline(string acct, ServerSession session, PlayerData playerData)
    {
        onLineAcctDic.Add(acct, session);
        onLineSessionDic.Add(session, playerData);

    }


}