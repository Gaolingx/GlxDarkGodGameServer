

//功能：登陆业务系统，负责处理登录和重命名的相关逻辑
using PEProtocol;

public class LoginSys
{
    private static LoginSys instance = null;
    public static LoginSys Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new LoginSys();
            }
            return instance;
        }
    }
    private CacheSvc cacheSvc = null;
    private TimerSvc timerSvc = null;

    public void Init()
    {
        cacheSvc = CacheSvc.Instance;
        timerSvc = TimerSvc.Instance;
        PECommon.Log("LoginSys Init Done.");
    }

    public void ReqLogin(MsgPack pack)
    {
        ReqLogin data = pack.msg.reqLogin;
        //当前账号是否已经上线
        //要判断一个账号是否上线，我们可以从一个缓存里面获取当前在系统里面获取当前服务器上线了哪些玩家，判断有没有这个账号，
        //因此我们需要创建一个缓存层。
        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.RspLogin
        };

        //这里可以使用一种叫做错误码的概念实现它，与CMD同样，属于枚举类型
        if (cacheSvc.IsAcctOnLine(data.acct))
        {
            //已上线：返回错误信息，表示此次登录无效，同时通知客户端
            msg.err = (int)ErrorCode.AcctIsOnline;
        }
        else
        {
            //未上线：从缓存拉取数据
            //账号是否存在
            PlayerData pd = cacheSvc.GetPlayerData(data.acct, data.pass);
            //判断当前数据是否获取到
            if (pd ==null)
            {
                //存在，密码错误
                msg.err = (int)ErrorCode.WrongPass; //如果为空说明密码错误，返回错误码
            }
            else
            {
                //对于离线玩家的体力计算：记录玩家最后一次在线时间，当玩家再次上线时，根据当前上线时间 - 原来上次在线的时间，算出时间差，根据这个时间差，除以体力回复的间隔，算出应该增长的体力，并不超过上限。

                //计算离线体力增长
                int power = pd.power; //当前体力值
                //获取当前时间
                long nowTime = timerSvc.GetNowTime();
                //计算登录时间间隔
                long milliseconds = nowTime - pd.time;
                //计算需要增加的体力值
                int addPower = (int)(milliseconds / (1000 * 60 * PECommon.PowerAddSpace)) * PECommon.PowerAddCount;
                if (addPower > 0)
                {
                    int powerMax = PECommon.GetPowerLimit(pd.lv);
                    if (pd.power < powerMax)
                    {
                        pd.power += addPower;
                        if (pd.power > powerMax)
                        {
                            pd.power = powerMax; //不超过体力上限
                        }
                    }
                }

                //判断是否需要更新数据库
                if (power != pd.power)
                {
                    cacheSvc.UpdatePlayerData(pd.id, pd);
                }

                //不存在，创建默认的账号密码（账号不存在），或者将当前获取到的有效的账号数据返回给客户端（账号存在）
                msg.rspLogin = new RspLogin
                {
                    playerData = pd
                };

                //缓存账号数据
                //获取到有效账号数据之后，将它缓存到缓存层内，避免重复登录造成数据冲突
                cacheSvc.AcctOnline(data.acct, pack.session, pd);
            }
           
        }

        //处理完上述逻辑后，回应客户端
        //你需要先拿到client与client的session才能发消息
        pack.session.SendMsg(msg);
    }

    public void ReqRename(MsgPack pack)
    {
        ReqRename data = pack.msg.reqRename;
        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.RspRename
        };

        //判断当前名字是否已经存在
        if (cacheSvc.IsNameExist(data.name))
        {
            //存在：返回错误码，让玩家重新选择名字
            msg.err = (int)ErrorCode.NameIsExist;
        }
        else
        {
            //不存在：更新缓存，以及数据库，再返回给客户端
            PlayerData playerData = cacheSvc.GetPlayerDataBySession(pack.session);
            //更新playerdata中的名字
            playerData.name = data.name;

            //将name数据更新到MySQL中
            if (!cacheSvc.UpdatePlayerData(playerData.id, playerData))
            {
                msg.err = (int)ErrorCode.UpdateDBError;
            }
            else
            {
                msg.rspRename = new RspRename
                {
                    name = data.name
                };
            }
        }
        pack.session.SendMsg(msg);
    }

    public void ClearOfflineData(ServerSession session)
    {
        //写入下线时间，用于离线体力的计算
        PlayerData pd = cacheSvc.GetPlayerDataBySession(session);
        if (pd != null)
        {
            pd.time = timerSvc.GetNowTime();
            if (!cacheSvc.UpdatePlayerData(pd.id, pd))
            {
                PECommon.Log("Update offline time error", PELogType.Error);
            }
            cacheSvc.AcctOffLine(session);
        }
    }
}