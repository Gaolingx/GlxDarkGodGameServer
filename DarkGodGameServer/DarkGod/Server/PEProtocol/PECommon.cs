//功能：客户端服务端共用工具类
using PENet;

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
}