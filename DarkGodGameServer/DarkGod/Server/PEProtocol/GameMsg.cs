//功能：网络通信协议（客户端服务端共用）
using System;
using PENet;


namespace PEProtocol
{
    [Serializable]
    public class GameMsg : PEMsg
    {
        public ReqLogin reqLogin;
        public RspLogin rspLogin;

        public ReqRename reqRename;
        public RspRename rspRename;

        public ReqGuide reqGuide;
        public RspGuide rspGuide;
    }

    #region 登录相关
    [Serializable]
    public class ReqLogin
    {
        public string acct;
        public string pass;
    }

    [Serializable]
    public class RspLogin
    {
        public PlayerData playerData;
    }

    [Serializable]
    public class PlayerData
    {
        public int id;
        public string name;
        public int lv;
        public int exp;
        public int power;
        public int coin;
        public int diamond;

        public int hp;
        public int ad;
        public int ap;
        public int addef;
        public int apdef;
        public int dodge;//闪避概率
        public int pierce;//穿透比率
        public int critical;//暴击概率

        public int guideid;//引导id
        //TOADD
    }

    [Serializable]
    public class ReqRename
    {
        public string name;
    }
    [Serializable]
    public class RspRename
    {
        public string name;
    }
    #endregion

    #region 引导相关
    [Serializable]
    public class ReqGuide
    {
        public int guideid; //客户端发送已经完成引导任务的id
    }

    [Serializable]
    public class RspGuide
    {
        public int guideid; //服务器更新引导id返回给客户端
        public int coin;
        public int lv;
        public int exp;
    }

    #endregion


    public enum ErrorCode
    {
        None = 0,//没有错误

        UpdateDBError,//更新数据库错误

        AcctIsOnline,//账号已经上线
        WrongPass,//密码错误
        NameIsExist,//名字已经存在
    }

    public enum CMD
    {
        None = 0,
        //登录相关 100
        ReqLogin = 101,
        RspLogin = 102,

        ReqRename = 103,
        RspRename = 104,

        //主城相关 200
        ReqGuide = 200,
        RspGuide = 201,

    }

    public class SrvCfg
    {
        public const string srvIP = "127.0.0.1";
        public const int srvPort = 17666;
    }
}