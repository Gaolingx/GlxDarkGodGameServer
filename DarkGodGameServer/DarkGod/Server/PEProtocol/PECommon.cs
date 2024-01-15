﻿//功能：客户端服务端共用工具类
using PENet;
using PEProtocol;

public enum PELogType
{
    Log = 0,
    Warn = 1,
    Error = 2,
    Info = 3
}

public class PECommon
{


    public static void Log(string msg = "", PELogType tp = PELogType.Log)
    {
        LogLevel lv = (LogLevel)tp;
        PETool.LogMsg(msg, lv);
    }

    //战斗力计算
    public static int GetFightByProps(PlayerData pd)
    {
        return pd.lv * 100 + pd.ad + pd.ap + pd.addef + pd.apdef;
    }

    public static int GetPowerLimit(int lv)
    {
        return ((lv - 1) / 10) * 150 + 150;
    }

    public static int GetExpUpValByLv(int lv)
    {
        return 100 * lv * lv;
    }
}