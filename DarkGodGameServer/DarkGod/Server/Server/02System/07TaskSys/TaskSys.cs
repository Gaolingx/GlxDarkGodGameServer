//功能：任务奖励系统

using PEProtocol;
using static CfgSvc;

public class TaskSys
{
    private static TaskSys instance = null;
    public static TaskSys Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new TaskSys();
            }
            return instance;
        }
    }
    private CacheSvc cacheSvc = null;
    private CfgSvc cfgSvc = null;

    public void Init()
    {
        cacheSvc = CacheSvc.Instance;
        cfgSvc = CfgSvc.Instance;
        PECommon.Log("TaskSys Init Done.");
    }

    public void ReqTakeTaskReward(MsgPack pack)
    {
        ReqTakeTaskReward data = pack.msg.reqTakeTaskReward;

        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.RspTakeTaskReward
        };

        PlayerData pd = cacheSvc.GetPlayerDataBySession(pack.session);

        //领取相应奖励，变化相关数据，更新到数据库...
        //根据任务id获取相应任务数据（任务奖励配置数据）
        TaskRewardCfg trc = cfgSvc.GetTaskRewardCfg(data.rid);
        //从PlayerData中获取解析后的进度数据（对应rid的进度）
        TaskRewardData trd = CalcTaskRewardData(pd, data.rid);

        //校验玩家数据，发放奖励
        if(trd.prgs==trc.count&& !trd.taked)
        {
            pd.coin += trc.coin; //配置文件从服务器读取，客户端只传任务id，防止作弊
            PECommon.CalcExp(pd, trc.exp);
            trd.taked = true;
            //更新任务进度数据
            CalcTaskArr(pd, trd);

            if(!cacheSvc.UpdatePlayerData(pd.id,pd))
            {
                msg.err = (int)ErrorCode.UpdateDBError;
            }
            else
            {
                RspTakeTaskReward rspTakeTaskReward = new RspTakeTaskReward
                {
                    coin = pd.coin,
                    lv = pd.lv,
                    exp = pd.exp,
                    taskArr = pd.taskArr
                };
                msg.rspTakeTaskReward = rspTakeTaskReward;

            }
        }
        else
        {
            msg.err = (int)ErrorCode.ClientDataError;
        }
        pack.session.SendMsg(msg);
    }

    //根据data.id，获取taskArr中对应数据
    public TaskRewardData CalcTaskRewardData(PlayerData pd, int rid)
    {
        TaskRewardData trd = null;
        for (int i = 0; i < pd.taskArr.Length; i++)
        {
            string[] taskinfo = pd.taskArr[i].Split('|');
            //1|0|0
            if (int.Parse(taskinfo[0]) == rid)
            {
                trd = new TaskRewardData
                {
                    ID = int.Parse(taskinfo[0]),
                    prgs = int.Parse(taskinfo[1]),
                    taked = taskinfo[2].Equals("1")
                };
                break;
            }
        }
        return trd;
    }

    //根据传入的进度数据（TaskRewardData），找到PlayerData中对应数组中的某项，并修改其数据（将TaskRewardData还原成taskArr）
    public void CalcTaskArr(PlayerData pd, TaskRewardData trd)
    {
        string result = trd.ID + "|" + trd.prgs + '|' + (trd.taked ? 1 : 0);
        int index = -1;
        for (int i = 0; i < pd.taskArr.Length; i++)
        {
            string[] taskinfo = pd.taskArr[i].Split('|');
            if (int.Parse(taskinfo[0]) == trd.ID)
            {
                index = i;
                break;
            }
        }
        pd.taskArr[index] = result;
    }
}