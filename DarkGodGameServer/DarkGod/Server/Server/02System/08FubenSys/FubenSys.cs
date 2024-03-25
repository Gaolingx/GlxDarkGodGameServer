//功能：副本战斗业务


using PEProtocol;

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

        //1.检测体力是否满足要求
        //2.检测副本id是否有效
        PlayerData pd = cacheSvc.GetPlayerDataBySession(pack.session);
        int powerInMission = cfgSvc.GetMapCfg(data.fbid).power;
        //3.扣除相应体力
        //4.将消息发回客户端，开始战斗
    }
}