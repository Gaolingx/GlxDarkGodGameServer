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
            pd.guideid += 1;

            //更新玩家数据（获取任务相应奖励，并将奖励数据更新到数据库中，最后将更新的结果返回给客户端）
            pd.coin += gc.coin;
            CalcExp(pd, gc.exp);

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

    //角色升级逻辑
    private void CalcExp(PlayerData pd, int addExp)
    {
        int curtLv = pd.lv;
        int curtExp = pd.exp;
        int addRestExp = addExp; //定义升级完后剩余的经验值
        while(true)
        {
            int upNeedExp = PECommon.GetExpUpValByLv(curtLv) - curtExp; //计算当前升级所需要的经验值：升级所需要的总经验值-当前已拥有的经验值
            //如果要增加的经验值大于等于升级需要的经验值，则等级+1
            if(addRestExp >= upNeedExp)
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
}
