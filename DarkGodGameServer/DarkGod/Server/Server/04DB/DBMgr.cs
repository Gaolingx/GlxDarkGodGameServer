//功能：数据库管理类


using MySql.Data.MySqlClient;
using PEProtocol;

public class DBMgr
{
    private static DBMgr? instance = null;
    public static DBMgr Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new DBMgr();
            }
            return instance;
        }
    }
    //定义数据库连接
    private MySqlConnection? DBconn;

    public void Init()
    {
        DBconn = new MySqlConnection("server=localhost;User Id = root;password=Gao123456;Database=darkgod;Charset = utf8mb3");
        PECommon.Log("DBMgr Init Done.");
    }


    //数据库账号查询
    public PlayerData? QueryPlayerData(string acct,string pass)
    {
        PlayerData? playerData = null;

        //TODO
        return playerData;
    }
}