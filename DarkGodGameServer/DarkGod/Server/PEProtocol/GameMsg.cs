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

    }

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

    public enum ErrorCode
    {
        None = 0,  //没有错误

        AcctIsOnline, //账号已经上线
    }

    public enum CMD
    {
        None = 0,
        //登录相关 100
        ReqLogin = 101,
        RspLogin = 102,
    }
    public class SrvCfg
    {
        public const string srvIP = "127.0.0.1";
        public const int srvPort = 17666;
    }
}