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
        for(int i = 0; i < lst.Count; i++)
        {
            lst[i].SendMsg(msg);
        }

    }
}
