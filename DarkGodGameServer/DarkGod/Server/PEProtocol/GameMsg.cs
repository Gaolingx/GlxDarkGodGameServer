﻿using System;
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
        //TODO
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