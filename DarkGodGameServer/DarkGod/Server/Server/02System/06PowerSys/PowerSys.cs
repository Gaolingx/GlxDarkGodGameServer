//功能：体力恢复系统

using PEProtocol;

public class PowerSys
{
    private static PowerSys instance = null;
    public static PowerSys Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PowerSys();
            }
            return instance;
        }
    }
    private CacheSvc cacheSvc = null;
    private TimerSvc timerSvc = null;

    public void Init()
    {
        cacheSvc = CacheSvc.Instance;
        timerSvc = TimerSvc.Instance;

        //每过5分钟执行一次CalcPowerAdd函数
        TimerSvc.Instance.AddTimeTask(CalcPowerAdd, PECommon.PowerAddSpace, PETimeUnit.Minute, 0);
        PECommon.Log("PowerSys Init Done.");
    }

    private void CalcPowerAdd(int tid)
    {
        //计算体力增长
        PECommon.Log("All Online Player Calc Power Incress....");
        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.PshPower
        };
        msg.pshPower = new PshPower();

        //所有在线玩家获得实时的体力增长推送数据
        Dictionary<ServerSession, PlayerData> onlineDic = cacheSvc.GetOnlineCache();
        foreach (var item in onlineDic)
        {
            PlayerData pd = item.Value;
            ServerSession session = item.Key;

            //应用等级限制
            int powerMax = PECommon.GetPowerLimit(pd.lv);
            if (pd.power >= powerMax)
            {
                continue; //跳过，继续判断下一个玩家
            }
            else
            {
                pd.power += PECommon.PowerAddCount;
                //加个判断，防止超出限制
                if (pd.power > powerMax)
                {
                    pd.power = powerMax;
                }
            }

            if (!cacheSvc.UpdatePlayerData(pd.id, pd))
            {
                msg.err = (int)ErrorCode.UpdateDBError;
            }
            else
            {
                msg.pshPower.power = pd.power;
                session.SendMsg(msg);
            }

        }
    }
}