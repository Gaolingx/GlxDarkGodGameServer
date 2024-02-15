//功能：引导业务系统

using PEProtocol;

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

    public void Init()
    {
        cacheSvc = CacheSvc.Instance;
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

        //更新引导ID
        if (pd.guideid == data.guideid)
        {
            pd.guideid += 1;

            //更新玩家数据（获取任务相应奖励，并将奖励数据更新到数据库中，最后将更新的结果返回给客户端）

        }
        else
        {
            msg.err = (int)ErrorCode.ServerDataError;
        }
        pack.session.SendMsg(msg);
    }
}
