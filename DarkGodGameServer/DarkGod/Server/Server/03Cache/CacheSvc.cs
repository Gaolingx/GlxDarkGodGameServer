using System.Collections.Generic;


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
    private Dictionary<string,ServerSession> onLineAcctDic = new Dictionary<string,ServerSession>();
    public void Init()
    {
        PECommon.Log("CacheSvc Init Done.");
    }

    //判断当前账号是否已经上线
    public bool IsAcctOnline(string acct)
    {
        return onLineAcctDic.ContainsKey(acct);  //如果key存在说明账号在线，则返回true，否则返回false
    }


}