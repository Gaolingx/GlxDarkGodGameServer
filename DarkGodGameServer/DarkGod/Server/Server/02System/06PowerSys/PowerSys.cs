//功能：体力恢复系统

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

    public void Init()
    {
        cacheSvc = CacheSvc.Instance;

        //每过5分钟执行一次CalcPowerAdd函数
        TimerSvc.Instance.AddTimeTask(CalcPowerAdd, PECommon.PowerAddSpace, PETimeUnit.Minute, 0);
        PECommon.Log("PowerSys Init Done.");
    }

    private void CalcPowerAdd(int tid)
    {
        //计算体力增长

    }
}