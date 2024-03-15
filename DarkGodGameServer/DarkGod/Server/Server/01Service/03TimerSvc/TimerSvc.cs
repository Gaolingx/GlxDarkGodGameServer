//功能：定时服务


public class TimerSvc
{
    class TaskPack
    {
        public int tid;
        public Action<int> cb;
        public TaskPack(int tid, Action<int> cb)
        {
            this.tid = tid;
            this.cb = cb;
        }
    }

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
    Queue<TaskPack> tpQue = new Queue<TaskPack>();
    private static readonly string tpQueLock = "tpQueLock";

    public void Init()
    {
        pt = new PETimer(100); //每隔100ms进行一次定时任务检测
        tpQue.Clear(); //清空Quene

        //设置日志输出
        pt.SetLog((string info) => {
            PECommon.Log(info);
        });

        pt.SetHandle((Action<int> cb, int tid) => {
            if (cb != null)
            {
                lock (tpQueLock)
                {
                    tpQue.Enqueue(new TaskPack(tid, cb)); //把满足目标的定时任务放入队列中
                }
            }
        });

        PECommon.Log("TimerSvc Init Done.");
    }

    public void Update() //使用独立的线程驱动定时器，优化性能
    {
        //检测到tpQue有满足条件的定时任务，立即执行
        while (tpQue.Count > 0)
        {
            TaskPack tp = null;
            lock (tpQueLock)
            {
                tp = tpQue.Dequeue();
            }

            if (tp != null)
            {
                tp.cb(tp.tid);
            }
        }
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