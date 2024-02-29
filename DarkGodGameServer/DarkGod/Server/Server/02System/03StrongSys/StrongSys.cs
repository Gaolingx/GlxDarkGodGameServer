//功能：强化升级系统

using PEProtocol;

public class StrongSys
{
    private static StrongSys instance = null;
    public static StrongSys Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new StrongSys();
            }
            return instance;
        }
    }
    private CacheSvc cacheSvc = null;

    public void Init()
    {
        cacheSvc = CacheSvc.Instance;
        PECommon.Log("StrongSys Init Done.");
    }

    public void ReqStrong(MsgPack pack)
    {
        ReqStrong data = pack.msg.reqStrong;

        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.RspStrong
        };

        PlayerData pd = cacheSvc.GetPlayerDataBySession(pack.session);
        int curtStarLv = pd.strongArr[data.pos];
        //条件判断

        //满足条件扣除相应资源

        //增加星级所产生的属性增益

        //将消息发回客户端
    }
}