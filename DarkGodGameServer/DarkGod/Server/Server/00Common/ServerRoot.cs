

//功能：服务器初始化
public class ServerRoot
{
    private static ServerRoot instance = null;
    public static ServerRoot Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ServerRoot();
            }
            return instance;
        }
    }

    public void Init()
    {
        //数据层
        DBMgr.Instance.Init();

        //服务层
        CacheSvc.Instance.Init();
        NetSvc.Instance.Init();

        //业务系统层
        LoginSys.Instance.Init();


    }

    public void Update()
    {
        NetSvc.Instance.Update();
    }
}


