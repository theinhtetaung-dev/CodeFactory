namespace CodeFactory.Singleton;

public class ConfigSettingV2
{
    public static ConfigSettingV2 _instance;
    public String _dbConnectionString { get; set; }

    public ConfigSettingV2(string dbConnectionString)
    {
        _dbConnectionString = dbConnectionString;
    }
}
