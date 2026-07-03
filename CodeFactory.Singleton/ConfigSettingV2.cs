namespace CodeFactory.Singleton;

public class ConfigSettingV2
{
    private static ConfigSettingV2 _instance;
    public String dbConnectionString { get; set; }

    public ConfigSettingV2(string _dbConnectionString)
    {
        dbConnectionString = _dbConnectionString;
    }

    public static ConfigSettingV2 GetInstance(string? dbConnectionString = null)
    {
        if (_instance == null)
        {
            _instance = new ConfigSettingV2(dbConnectionString);
        }
        return _instance;
    }
}
