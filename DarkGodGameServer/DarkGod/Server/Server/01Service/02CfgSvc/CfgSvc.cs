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
        InitStrongCfg();
        InitBuyCfg();
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

    #region 强化升级配置
    private Dictionary<int, Dictionary<int, StrongCfg>> strongDic = new Dictionary<int, Dictionary<int, StrongCfg>>();
    private void InitStrongCfg()
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(@"F:\GitHub\GlxDarkGodGameServer\DarkGodGameServer\DarkGod\Server\ServerCfg\strong.xml");

        XmlNodeList nodLst = doc.SelectSingleNode("root").ChildNodes;

        for (int i = 0; i < nodLst.Count; i++)
        {
            XmlElement ele = nodLst[i] as XmlElement;

            if (ele.GetAttributeNode("ID") == null)
            {
                continue;
            }
            int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
            StrongCfg sd = new StrongCfg
            {
                ID = ID
            };

            foreach (XmlElement e in nodLst[i].ChildNodes)
            {
                int val = int.Parse(e.InnerText);
                switch (e.Name)
                {
                    case "pos":
                        sd.pos = val;
                        break;
                    case "starlv":
                        sd.startlv = val;
                        break;
                    case "addhp":
                        sd.addhp = val;
                        break;
                    case "addhurt":
                        sd.addhurt = val;
                        break;
                    case "adddef":
                        sd.adddef = val;
                        break;
                    case "minlv":
                        sd.minlv = val;
                        break;
                    case "coin":
                        sd.coin = val;
                        break;
                    case "crystal":
                        sd.crystal = val;
                        break;
                }
            }

            Dictionary<int, StrongCfg> dic = null;
            if (strongDic.TryGetValue(sd.pos, out dic))
            {
                dic.Add(sd.startlv, sd);
            }
            else
            {
                dic = new Dictionary<int, StrongCfg>();
                dic.Add(sd.startlv, sd);

                strongDic.Add(sd.pos, dic);
            }
        }
        PECommon.Log("StrongCfg Init Done.");
    }
    public StrongCfg GetStrongCfg(int pos, int starlv)
    {
        StrongCfg sd = null;
        Dictionary<int, StrongCfg> dic = null;
        if (strongDic.TryGetValue(pos, out dic))
        {
            if (dic.ContainsKey(starlv))
            {
                sd = dic[starlv];
            }
        }
        return sd;
    }
    #endregion

    #region 资源交易配置
    private Dictionary<int, BuyCfg> buyCfgDic = new Dictionary<int, BuyCfg>();
    private void InitBuyCfg()
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(@"F:\GitHub\GlxDarkGodGameServer\DarkGodGameServer\DarkGod\Server\ServerCfg\buyCfg.xml");     //根据配置文件本地路径修改

        XmlNodeList nodLst = doc.SelectSingleNode("root").ChildNodes;

        for (int i = 0; i < nodLst.Count; i++)
        {
            XmlElement ele = nodLst[i] as XmlElement;

            if (ele.GetAttributeNode("ID") == null)
            {
                continue;
            }
            int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
            BuyCfg buyCfg = new BuyCfg
            {
                ID = ID
            };

            foreach (XmlElement e in nodLst[i].ChildNodes)
            {
                switch (e.Name)
                {
                    case "buyCostDiamondOnce":
                        buyCfg.buyCostDiamondOnce = int.Parse(e.InnerText);
                        break;
                    case "amountEachPurchase":
                        buyCfg.amountEachPurchase = int.Parse(e.InnerText);
                        break;
                }
            }
            buyCfgDic.Add(ID, buyCfg);
        }
        PECommon.Log("BuyCfg Init Done.");

    }
    public BuyCfg GetBuyCfg(int id)
    {
        BuyCfg bc = null;
        if (buyCfgDic.TryGetValue(id, out bc))
        {
            return bc;
        }
        return null;
    }
    #endregion

    public class GuideCfg : BaseData<GuideCfg>
    {
        public int coin;
        public int exp;
    }

    public class StrongCfg : BaseData<StrongCfg>
    {
        public int pos;
        public int startlv;
        public int addhp;
        public int addhurt;
        public int adddef;
        public int minlv;
        public int coin;
        public int crystal;
    }

    public class BuyCfg : BaseData<BuyCfg>
    {
        public int buyCostDiamondOnce;
        public int amountEachPurchase;
    }

    public class BaseData<T>
    {
        public int ID;
    }
}
