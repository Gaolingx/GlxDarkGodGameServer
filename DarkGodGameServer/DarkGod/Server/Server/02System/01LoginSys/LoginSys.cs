

//功能：登陆业务系统
using PEProtocol;

public class LoginSys
{
    private static LoginSys? instance = null;
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

    public void Init()
    {
        PECommon.Log("LoginSys Init Done.");
    }

    public void ReqLogin(MsgPack pack)
    {
        //当前账号是否已经上线

        //已上线：返回错误信息
        //未上线：
        //账号是否存在
        //存在，检测密码
        //不存在，创建默认的账号密码

        //处理完上述逻辑后，回应客户端
        //你需要先拿到client与client的session才能发消息
        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.RspLogin,
            rspLogin = new RspLogin
            {

            }
        };

        pack.session.SendMsg(msg);

    }
}