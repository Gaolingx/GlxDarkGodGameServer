//功能：配置数据服务


using System.Xml;

public class CfgSvc
{
    private static CfgSvc instance = null;
    public static CfgSvc Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new CfgSvc();
            }
            return instance;
        }
    }

    public void Init()
    {
        InitGuideCfg();
        PECommon.Log("CfgSvc Init Done.");
    }

    #region 自动引导配置
    private Dictionary<int, GuideCfg> guideDic = new Dictionary<int, GuideCfg>();
    private void InitGuideCfg()
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(@"F:\GitHub\GlxDarkGodGameServer\DarkGodGameServer\DarkGod\Server\ServerCfg\guide.xml");     //根据配置文件本地路径修改

        XmlNodeList nodLst = doc.SelectSingleNode("root").ChildNodes;

        for (int i = 0; i < nodLst.Count; i++)
        {
            XmlElement ele = nodLst[i] as XmlElement;

            if (ele.GetAttributeNode("ID") == null)
            {
                continue;
            }
            int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
            GuideCfg guideCfg = new GuideCfg
            {
                ID = ID
            };

            foreach (XmlElement e in nodLst[i].ChildNodes)
            {
                switch (e.Name)
                {
                    case "coin":
                        guideCfg.coin = int.Parse(e.InnerText);
                        break;
                    case "exp":
                        guideCfg.exp = int.Parse(e.InnerText);
                        break;
                }
            }
            guideDic.Add(ID, guideCfg);
        }
        PECommon.Log("GuideCfg Init Done.");

    }
    public GuideCfg GetGuideCfg(int id)
    {
        GuideCfg agc = null;
        if (guideDic.TryGetValue(id, out agc))
        {
            return agc;
        }
        return null;
    }
    #endregion

    public class GuideCfg : BaseData<GuideCfg>
    {
        public int coin;
        public int exp;
    }

    public class BaseData<T>
    {
        public int ID;
    }
}
