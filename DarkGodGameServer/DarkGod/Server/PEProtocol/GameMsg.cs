//功能：网络通信协议（客户端服务端共用）
using System;
using PENet;


//Reqxxx：客户端请求服务器
//Rspxxx：服务器回应客户端
namespace PEProtocol
{
    [Serializable]
    public class GameMsg : PEMsg
    {
        public ReqLogin reqLogin;
        public RspLogin rspLogin;

        public ReqRename reqRename;
        public RspRename rspRename;

        public ReqGuide reqGuide;
        public RspGuide rspGuide;

        public ReqStrong reqStrong;
        public RspStrong rspStrong;

        public SndChat sndChat;
        public PshChat pshChat;

        public ReqBuy reqBuy;
        public RspBuy rspBuy;
    }

    #region 登录相关
    [Serializable]
    public class ReqLogin
    {
        public string acct;
        public string pass;
    }

    [Serializable]
    public class RspLogin
    {
        public PlayerData playerData;
    }

    [Serializable]
    public class PlayerData
    {
        public int id;
        public string name;
        public int lv;
        public int exp;
        public int power;
        public int coin;
        public int diamond;
        public int crystal;

        public int hp;
        public int ad;
        public int ap;
        public int addef;
        public int apdef;
        public int dodge;//闪避概率
        public int pierce;//穿透比率
        public int critical;//暴击概率

        public int guideid;//引导id
        public int[] strongArr;//数组的索引号代表强化的部位，索引号对应的数据代表星级
        //TOADD
    }

    [Serializable]
    public class ReqRename
    {
        public string name;
    }
    [Serializable]
    public class RspRename
    {
        public string name;
    }
    #endregion

    #region 引导相关
    [Serializable]
    public class ReqGuide
    {
        public int guideid; //客户端发送已经完成引导任务的id
    }

    [Serializable]
    public class RspGuide
    {
        public int guideid; //服务器更新引导id返回给客户端
        public int coin;
        public int lv;
        public int exp;
    }
    #endregion

    #region 强化相关
    //只传输核心数据
    [Serializable]
    public class ReqStrong
    {
        public int pos; //强化部位
    }
    [Serializable]
    public class RspStrong
    {
        public int coin;
        public int crystal;
        public int hp;
        public int ad;
        public int ap;
        public int addef;
        public int apdef;
        public int[] strongArr; //强化数据更新
    }
    #endregion

    #region 聊天相关
    [Serializable]
    public class SndChat
    {
        public string chat;
    }

    [Serializable]
    public class PshChat
    {
        public string name;
        public string chat;
    }
    #endregion

    #region 资源交易相关
    [Serializable]
    public class ReqBuy
    {
        public int type;
        public int cost;
    }

    [Serializable]
    public class RspBuy
    {
        public int type;
        public int dimond;
        public int coin;
        public int power;
    }
    #endregion

    public enum ErrorCode
    {
        None = 0,//没有错误
        ServerDataError,//服务器数据异常
        UpdateDBError,//更新数据库错误

        AcctIsOnline,//账号已经上线
        WrongPass,//密码错误
        NameIsExist,//名字已经存在

        LackLevel,//等级不够
        LackCoin,//金币不够
        LackCrystal,//水晶不够
        LackDiamond,//钻石不够
    }

    public enum CMD
    {
        None = 0,
        //登录相关 100
        ReqLogin = 101,
        RspLogin = 102,

        ReqRename = 103,
        RspRename = 104,

        //引导任务相关 200
        ReqGuide = 201,
        RspGuide = 202,

        //强化相关
        ReqStrong = 203,
        RspStrong = 204,

        //聊天相关
        SndChat = 205,
        PshChat = 206,

        //资源交易相关
        ReqBuy = 207,
        RspBuy = 208,

    }

    public class SrvCfg
    {
        public const string srvIP = "127.0.0.1";
        public const int srvPort = 17666;
    }
}