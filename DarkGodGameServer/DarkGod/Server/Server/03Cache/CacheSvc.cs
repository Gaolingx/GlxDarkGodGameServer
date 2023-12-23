
//功能：缓存层
public class CacheSvc
{
    private static CacheSvc? instance = null;
    public static CacheSvc Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new CacheSvc();
            }
            return instance;
        }
    }
    public void Init()
    {
        PECommon.Log("CacheSvc Init Done.");
    }
}