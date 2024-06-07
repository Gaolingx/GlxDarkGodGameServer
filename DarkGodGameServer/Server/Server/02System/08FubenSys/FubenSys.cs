//功能：副本战斗业务


using PEProtocol;
using static CfgSvc;

public class FubenSys
{
    private static FubenSys instance = null;
    public static FubenSys Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new FubenSys();
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
        PECommon.Log("FubenSys Init Done.");
    }

    public void ReqFBFight(MsgPack pack)
    {
        ReqFBFight data = pack.msg.reqFBFight;

        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.RspFBFight
        };

        PlayerData pd = cacheSvc.GetPlayerDataBySession(pack.session);
        int powerInMission = cfgSvc.GetMapCfg(data.fbid).power;

        //1.判断关卡是否符合需求
        if (pd.fuben < data.fbid) //pd.fuben代表玩家副本战斗进度
        {
            msg.err = (int)ErrorCode.ClientDataError;
        }
        //2.判断体力是否满足要求
        else if (pd.power < powerInMission)
        {
            msg.err = (int)ErrorCode.LackPower;
        }
        //3.数据合法，扣除消耗体力
        else
        {
            pd.power -= powerInMission;
            //4.更新玩家数据至DB
            if (cacheSvc.UpdatePlayerData(pd.id, pd))
            {
                //5.更新数据库成功，将消息发回客户端，开始战斗
                RspFBFight rspFBFight = new RspFBFight
                {
                    fbid = data.fbid,
                    power = pd.power
                };
                msg.rspFBFight = rspFBFight;
            }
            else
            {
                //6.更新数据库失败，将消息发回客户端，提示错误码
                msg.err = (int)ErrorCode.UpdateDBError;
            }
        }
        pack.session.SendMsg(msg);
    }

    public void ReqFBFightEnd(MsgPack pack)
    {
        ReqFBFightEnd data = pack.msg.reqFBFightEnd;

        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.RspFBFightEnd

        };

        //校验战斗是否合法
        if (data.win)
        {
            if (data.costtime > 0 && data.resthp > 0)
            {
                //根据副本ID获取相应奖励
                MapCfg rd = cfgSvc.GetMapCfg(data.fbid);
                PlayerData pd = cacheSvc.GetPlayerDataBySession(pack.session);

                //任务进度数据更新
                TaskSys.Instance.CalcTaskPrgs(pd, ConstantsCfg.taskID_02);

                pd.coin += rd.coin;
                pd.crystal += rd.crystal;
                PECommon.CalcExp(pd, rd.exp);

                //更新副本进度id
                //判断关卡是否重复
                if (pd.fuben == data.fbid)
                {
                    pd.fuben += 1;
                }

                //更新奖励数据到玩家数据库
                if (!cacheSvc.UpdatePlayerData(pd.id, pd))
                {
                    msg.err = (int)ErrorCode.UpdateDBError;
                }
                else
                {
                    //发送玩家数据、副本奖励数据
                    RspFBFightEnd rspFBFightEnd = new RspFBFightEnd
                    {
                        win = data.win,
                        fbid = data.fbid,
                        resthp = data.resthp,
                        costtime = data.costtime,

                        coin = pd.coin,
                        lv = pd.lv,
                        exp = pd.exp,
                        crystal = pd.crystal,
                        fuben = pd.fuben
                    };

                    msg.rspFBFightEnd = rspFBFightEnd;
                }
            }

        }
        else
        {
            msg.err = (int)ErrorCode.ClientDataError;
        }

        pack.session.SendMsg(msg);
    }
}