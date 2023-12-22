

//功能：服务器入口
class ServerStart
{
    static void Main(String[] args)
    {
        ServerRoot.Instance.Init();

        while (true)
        {
            ServerRoot.Instance.Update();
        }
    }
}
