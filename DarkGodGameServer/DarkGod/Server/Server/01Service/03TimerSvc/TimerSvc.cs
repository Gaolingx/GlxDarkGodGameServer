//功能：定时服务


public class TimerSvc
{
    private static TimerSvc instance = null;
    public static TimerSvc Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new TimerSvc();
            }
            return instance;
        }
    }

    PETimer pt = null;

    public void Init()
    {
        pt = new PETimer();

        //设置定时器日志输出
        pt.SetLog((string info) =>
        {
            PECommon.Log(info);
        });
        PECommon.Log("TimerSvc Init Done.");
    }

    public void Update()
    {
        pt.Update();
    }

    public int AddTimeTask(Action<int> callback, double delay, PETimeUnit timeUnit = PETimeUnit.Millisecond, int count = 1)
    {
        return pt.AddTimeTask(callback, delay, timeUnit, count);
    }

    public long GetNowTime()
    {
        return (long)pt.GetMillisecondsTime();
    }
}