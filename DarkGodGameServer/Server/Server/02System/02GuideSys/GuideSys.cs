//功能：引导业务系统

using PEProtocol;
using static CfgSvc;

public class GuideSys
{
    private static GuideSys instance = null;
    public static GuideSys Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GuideSys();
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
        PECommon.Log("GuideSys Init Done.");
    }

    public void ReqGuide(MsgPack pack)
    {
        ReqGuide data = pack.msg.reqGuide;

        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.RspGuide
        };

        //从缓存中获取玩家数据
        PlayerData pd = cacheSvc.GetPlayerDataBySession(pack.session);
        GuideCfg gc = cfgSvc.GetGuideCfg(data.guideid);

        //更新引导ID
        if (pd.guideid == data.guideid)
        {

            //检测是否为智者点拨任务
            if (pd.guideid == 1001)
            {
                TaskSys.Instance.CalcTaskPrgs(pd, ConstantsCfg.taskID_01);
            }
            pd.guideid += 1;

            //更新玩家数据（获取任务相应奖励，并将奖励数据更新到数据库中，最后将更新的结果返回给客户端）
            pd.coin += gc.coin;
            PECommon.CalcExp(pd, gc.exp);

            //将数据更新到数据库中
            if(!cacheSvc.UpdatePlayerData(pd.id,pd))
            {
                msg.err = (int)ErrorCode.UpdateDBError;
            }
            else
            {
                //数据库更新没有出错则将消息回应给客户端
                RspGuide rspGuide = new RspGuide
                {
                    guideid = pd.guideid,
                    coin = pd.coin,
                    lv = pd.lv,
                    exp = pd.exp,
                };
                msg.rspGuide = rspGuide;
            }
        }
        else
        {
            msg.err = (int)ErrorCode.ServerDataError;
        }
        pack.session.SendMsg(msg);
    }


}
