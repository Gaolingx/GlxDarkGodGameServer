using System;
using PENet;

//功能：网络通信协议（客户端服务端共用）
namespace PEProtocol
{
    [Serializable]
    public class GameMsg : PEMsg
    {
        public ReqLogin? reqLogin;
        public RspLogin? rspLogin;

        public ReqRename? reqRename;
        public RspRename? rspRename;
    }

    #region 登陆相关
    [Serializable]
    public class ReqLogin
    {
        public string? acct;
        public string? pass;
    }

    //回应登陆消息
    [Serializable]
    public class RspLogin
    {
        public PlayerData? playerData;
    }

    //定义账号相关数据
    [Serializable]
    public class PlayerData
    {
        public int id;
        public string? name;
        public int lv;
        public int exp;
        public int power;
        public int coin;
        public int diamond;
        //TOADD
    }

    [Serializable]
    public class ReqRename
    {
        public string? name;
    }
    [Serializable]
    public class RspRename
    {
        public string? name;
    }
    #endregion

    public enum ErrorCode
    {
        None = 0,  //没有错误

        UpdateDBError, //更新数据库错误

        AcctIsOnline, //账号已经上线
        WrongPass,  //密码错误
        NameIsExist,  //名字已经存在
    }

    public enum CMD
    {
        None = 0,
        //登录相关 100
        ReqLogin = 101,
        RspLogin = 102,

        ReqRename = 103,
        RspRename = 104,
    }
    public class SrvCfg
    {
        public const string srvIP = "127.0.0.1";
        public const int srvPort = 17666;
    }
}