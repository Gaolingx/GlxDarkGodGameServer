//功能：聊天业务系统


using PEProtocol;

public class ChatSys
{
    private static ChatSys instance = null;
    public static ChatSys Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ChatSys();
            }
            return instance;
        }
    }
    private CacheSvc cacheSvc = null;

    public void Init()
    {
        cacheSvc = CacheSvc.Instance;
        PECommon.Log("ChatSys Init Done.");
    }

    public void SndChat(MsgPack pack)
    {
        //世界消息推送
        SndChat data = pack.msg.sndChat;
        PlayerData pd = cacheSvc.GetPlayerDataBySession(pack.session);

        //任务进度数据更新
        TaskSys.Instance.CalcTaskPrgs(pd, TaskConstantsCfg.taskID_06);

        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.PshChat,
            pshChat = new PshChat
            {
                name = pd.name,
                chat = data.chat
            }
        };

        //将消息广播给所有在线客户端
        //获取所有玩家的session，再逐一发消息
        List<ServerSession> lst = cacheSvc.GetOnlineServerSessions();
        byte[] bytes = PENet.PETool.PackNetMsg(msg);
        for(int i = 0; i < lst.Count; i++)
        {
            lst[i].SendMsg(bytes);
            //注意：你不应该直接使用 lst[i].SendMsg(msg); 重复的序列化会影响性能，但由于玩家收到的消息是一样的，
            //我们应该先把需要重复序列化的网络消息（类）先序列化成二进制数据，再发送给客户端，节约序列化的CPU时间
        }

    }
}
