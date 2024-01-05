//功能：数据库管理类


using System;
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
    //根据账号和密码来查找数据库有没有对应的玩家数据
    public PlayerData? QueryPlayerData(string acct,string pass)
    {
        bool isNew = true;
        PlayerData? playerData = null;
        MySqlDataReader? reader = null;

        try
        {
            MySqlCommand cmd = new MySqlCommand("select * from account where acct = @acct", DBconn);
            cmd.Parameters.AddWithValue("acct", acct);
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                isNew = false;
                string _pass = reader.GetString("pass");
                //如果数据库中找到数据，则判断当前账号对应的密码和客户端传入的密码是否一致
                if (_pass.Equals(pass))
                {
                    //如果相等则密码正确，返回玩家数据
                    playerData = new PlayerData
                    {
                        id = reader.GetInt32("id"),
                        name = reader.GetString("name"),
                        lv = reader.GetInt32("level"),
                        exp = reader.GetInt32("exp"),
                        power = reader.GetInt32("power"),
                        coin = reader.GetInt32("coin"),
                        diamond = reader.GetInt32("diamond"),
                        //TOADD
                    };
                }

            }
        }
        catch (Exception e)
        {
            PECommon.Log("Query PlayerData By Acct&Pass Error:" + e, PELogType.Error);
        }

        //执行reader = cmd.ExecuteReader();时，如果数据库不存在玩家account，我们就需要默认为玩家创建一个新的账号
        finally
        {
            if (isNew == true)
            {
                //不存在账号数据，创建新的默认账号数据，并返回
                playerData = new PlayerData
                {
                    id = -1,
                    name = "",
                    lv = 1,
                    exp = 0,
                    power = 150,
                    coin = 5000,
                    diamond = 500,
                    //TOADD
                };
                //获取主键id
                playerData.id = InsertNewAcctData(acct, pass, playerData);
            }
        }
        return playerData;
    }

    //将新创建的账号数据插入到数据库中
    private int InsertNewAcctData(string acct, string pass, PlayerData pd)
    {

    }
}