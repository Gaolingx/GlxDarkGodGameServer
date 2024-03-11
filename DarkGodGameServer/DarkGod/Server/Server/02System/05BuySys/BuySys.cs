//功能：交易购买系统


using PEProtocol;

public class BuySys
{
    private static BuySys instance = null;
    public static BuySys Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new BuySys();
            }
            return instance;
        }
    }
    private CacheSvc cacheSvc = null;

    public void Init()
    {
        cacheSvc = CacheSvc.Instance;
        PECommon.Log("BuySys Init Done.");
    }

    public void ReqBuy(MsgPack pack)
    {
        ReqBuy data = pack.msg.reqBuy;
        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.RspBuy
        };

        //获取玩家数据
        PlayerData pd = cacheSvc.GetPlayerDataBySession(pack.session);

        //校验客户端数据
        if(pd.diamond <data.cost)
        {
            msg.err = (int)ErrorCode.LackDiamond;

        }
        else
        {
            //扣除消耗的资源
            pd.diamond -= data.cost;
            //根据购买类型增加相应资源
            switch(data.type)
            {
                //体力
                case 0:
                    pd.power += 100;
                    break;
                case 1:
                    pd.coin += 1000;
                    break;

            }
        }

        //更新玩家数据
        if(!cacheSvc.UpdatePlayerData(pd.id, pd))
        {
            msg.err = (int)ErrorCode.UpdateDBError;
        }
        else
        {
            RspBuy rspBuy = new RspBuy
            {
                type = data.type,
                dimond = pd.diamond,
                coin = pd.coin,
                power = pd.power
            };
            msg.rspBuy = rspBuy;
        }
        //将数据发回客户端
        pack.session.SendMsg(msg);
    }
}