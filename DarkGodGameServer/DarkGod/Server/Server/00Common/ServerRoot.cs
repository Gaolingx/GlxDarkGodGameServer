﻿

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

    //初始化
    public void Init()
    {
        //数据层TODO

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


