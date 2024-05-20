//功能：客户端服务端共用工具类
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

    //计算当前级别升至下一级需要的经验值
    public static int GetExpUpValByLv(int lv)
    {
        return 100 * lv * lv;
    }

    //角色升级逻辑
    public static void CalcExp(PlayerData pd, int addExp)
    {
        int curtLv = pd.lv;
        int curtExp = pd.exp;
        int addRestExp = addExp; //定义升级完后剩余的经验值
        while (true)
        {
            int upNeedExp = GetExpUpValByLv(curtLv) - curtExp; //计算当前升级所需要的经验值：升级所需要的总经验值-当前已拥有的经验值
            //如果要增加的经验值大于等于升级需要的经验值，则等级+1
            if (addRestExp >= upNeedExp)
            {
                curtLv += 1;
                //升级完成后，将已有的经验值重置为0
                curtExp = 0;
                //升级完成后所剩余的经验值-=所消耗的经验值
                addRestExp -= upNeedExp;
            }
            else
            {
                pd.lv = curtLv;
                pd.exp = curtExp + addRestExp; //玩家当前经验值=升级完后剩余的经验值（要增加的经验值）+当前的经验值
                break;
            }
        }
    }

    //体力恢复间隙
    public const int PowerAddSpace = 5; //单位：分钟
    //每次回复的体力计数
    public const int PowerAddCount = 2;
}