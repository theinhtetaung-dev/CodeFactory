namespace CodeFactory.Singleton;

public class ConfigSetting
{
    public static ConfigSetting _instance;

    public string DbConnectionString { get; set; }

    private ConfigSetting()
    {
        DbConnectionString = "Server=myServer;Database=myDataBase;User Id=myUsername;Password=myPassword;";
    }

    public static ConfigSetting GetInstance()
    {
        if (_instance == null)
        {
            _instance = new ConfigSetting();
        }
        return _instance;
    }
}
