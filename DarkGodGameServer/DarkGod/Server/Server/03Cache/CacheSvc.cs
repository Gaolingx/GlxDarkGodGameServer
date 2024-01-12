//功能：缓存层
using System.Collections.Generic;
using PEProtocol;


public class CacheSvc
{
    private static CacheSvc instance = null;
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
    private DBMgr dbMgr;

    //定义一个字典，用于存储当前上线的账号与与它对应的ServerSession连接。
    private Dictionary<string, ServerSession> onLineAcctDic = new Dictionary<string, ServerSession>();
    private Dictionary<ServerSession, PlayerData> onLineSessionDic = new Dictionary<ServerSession, PlayerData>();

    public void Init()
    {
        dbMgr = DBMgr.Instance;
        PECommon.Log("CacheSvc Init Done.");
    }

    //判断当前账号是否已经上线
    public bool IsAcctOnLine(string acct)
    {
        return onLineAcctDic.ContainsKey(acct);  //如果key存在说明账号在线，则返回true，否则返回false
    }

    /// <summary>
    /// 根据账号密码返回对应账号数据，密码错误返回null，账号不存在则默认创建新账号
    /// </summary>
    //用于获取当前PlayerData
    public PlayerData GetPlayerData(string acct, string pass)
    {
        //从数据库中查找账号数据
        return dbMgr.QueryPlayerData(acct, pass);
    }

    /// <summary>
    /// 账号上线，缓存数据
    /// </summary>
    public void AcctOnline(string acct, ServerSession session, PlayerData playerData)
    {
        onLineAcctDic.Add(acct, session);
        onLineSessionDic.Add(session, playerData);
    }

    //判断当前名字是否存在：遍历当前数据表内的所有数据（查询数据表），如果不存在，则该名字合法
    public bool IsNameExist(string name)
    {
        return dbMgr.QueryNameData(name);
    }

    //从字典缓存中获取playerdata
    public PlayerData GetPlayerDataBySession(ServerSession session)
    {
        if (onLineSessionDic.TryGetValue(session, out PlayerData playerData))
        {
            return playerData;
        }
        else
        {
            return null;
        }
    }

    public bool UpdatePlayerData(int id, PlayerData playerData)
    {
        return dbMgr.UpdatePlayerData(id, playerData);
    }
}