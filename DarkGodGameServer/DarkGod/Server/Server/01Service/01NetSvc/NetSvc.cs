
//功能：网络服务
using PENet;
using PEProtocol;
using System.Collections.Generic;

public class MsgPack
{
    public ServerSession? session;
    public GameMsg? msg;
    public MsgPack(ServerSession session, GameMsg msg)
    {
        this.session = session;
        this.msg = msg;
    }
}

public class NetSvc
{
    private static NetSvc? instance = null;
    public static NetSvc Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new NetSvc();
            }
            return instance;
        }
    }
    public static readonly string obj = "lock";
    private Queue<MsgPack> msgPackQue = new Queue<MsgPack>();

    public void Init()
    {
        PESocket<ServerSession, GameMsg> server = new PESocket<ServerSession, GameMsg>();
        server.StartAsServer(SrvCfg.srvIP, SrvCfg.srvPort);

        PECommon.Log("NetSvc Init Done.");
    }

    public void AddMsgQue(ServerSession session, GameMsg msg)
    {
        //Q:为什么网络消息包发送到服务器之后要锁到一个单线程处理
        //A:而不是一收到消息就就立即在 ServerSession 中调用 AddMsgQue 处理？
        //主要是考虑到数据安全，因为在 ServerSession 收消息的 AddMsgQue 是一个多线程的方法
        //而在逻辑处理部分（NetSvc），如果让一个数据被两个甚至更多线程访问，就会引发数据竞争，导致意料之外的问题
        //为了解决这种问题，我们应尽量将这些数据放在单线程中处理。

        //Q:为什么要在接收网络消息时使用多线程？
        //A:主要是为了提高效率，因为网络通信其实是一个I/O密集型操作，这个部分使用单线程相对而言比较慢
        //而多线程可大大提高效率。

        //Q:可不可以逻辑处理部分也使用多线程，然后在最终修改数据的地方加锁？
        //A:可以，而且相对而言性能会更高，但是实现难度相对较大，要考虑的因素也很多，开发起来麻烦很多。
        //我们为了提高开发效率，就直接使用锁死 obj 然后加到一个线程队列里面去处理。
        lock (obj)
        {
            msgPackQue.Enqueue(new MsgPack(session, msg));
        }
    }

    //通过while死循环，实现持续检测消息包队列内有没有网络消息，如果有则立即处理，反之则跳过，直至下次循环
    public void Update()
    {
        if(msgPackQue.Count > 0)
        {
            PECommon.Log("PackCount:"+msgPackQue.Count);
            lock (obj)
            {
                    MsgPack pack = msgPackQue.Dequeue();
                    HandOutMsg(pack);
            }
        }
    }

    private void HandOutMsg(MsgPack pack)
    {
        switch ((CMD)pack.msg.cmd) 
        {
            case CMD.RspLogin:
                LoginSys.Instance.ReqLogin(pack);  //客户端发送一条请求登录的消息，会传入NetSvc，然后通过ReqLogin()分发到LoginSys中，
                break;
            case CMD.ReqRename:
                LoginSys.Instance.ReqRename(pack);
                break;
        }
    }
}