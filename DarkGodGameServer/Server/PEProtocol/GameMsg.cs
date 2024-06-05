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

        public PshPower pshPower;

        public ReqTakeTaskReward reqTakeTaskReward;
        public RspTakeTaskReward rspTakeTaskReward;

        public PshTaskPrgs pshTaskPrgs;

        public ReqFBFight reqFBFight;
        public RspFBFight rspFBFight;

        public ReqFBFightEnd reqFBFightEnd;
        public RspFBFightEnd rspFBFightEnd;
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

        public long time;

        public string[] taskArr;
        public int fuben;//记录关卡信息
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
        public int diamond;
        public int coin;
        public int power;
    }
    #endregion

    #region 体力回复相关
    [Serializable]
    public class PshPower
    {
        public int power;
    }
    #endregion

    #region 副本战斗相关
    [Serializable]
    public class ReqFBFight
    {
        public int fbid;
    }
    [Serializable]
    public class RspFBFight
    {
        public int fbid;
        public int power;
    }

    [Serializable]
    public class ReqFBFightEnd
    {
        public bool win;
        public int fbid;
        public int resthp;
        public int costtime;
    }
    [Serializable]
    public class RspFBFightEnd
    {
        public bool win;
        public int fbid;
        public int resthp;
        public int costtime;

        //副本奖励
        public int coin;
        public int lv;
        public int exp;
        public int crystal;
        public int fuben;
    }
    #endregion

    #region 任务奖励相关
    [Serializable]
    public class ReqTakeTaskReward
    {
        public int rid;
    }

    [Serializable]
    public class RspTakeTaskReward
    {
        public int coin;
        public int lv;
        public int exp;
        public string[] taskArr;
    }

    [Serializable]
    public class PshTaskPrgs
    {
        public string[] taskArr;
    }
    #endregion
    public enum ErrorCode
    {
        None = 0,//没有错误
        ServerDataError,//服务器数据异常
        UpdateDBError,//更新数据库错误
        ClientDataError,//客户端数据异常

        AcctIsOnline,//账号已经上线
        WrongPass,//密码错误
        NameIsExist,//名字已经存在

        LackLevel,//等级不够
        LackCoin,//金币不够
        LackCrystal,//水晶不够
        LackDiamond,//钻石不够
        LackPower,//体力不足
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

        //体力计算相关
        PshPower = 209,

        //任务奖励相关
        ReqTakeTaskReward = 210,
        RspTakeTaskReward = 211,

        //任务进度推送相关
        PshTaskPrgs = 212,

        //副本战斗相关 300
        ReqFBFight = 301,
        RspFBFight = 302,

        ReqFBFightEnd = 303,
        RspFBFightEnd = 304,
    }

    public class SrvCfg
    {
        public const string srvIP = "127.0.0.1";
        public const int srvPort = 17666;
    }
}