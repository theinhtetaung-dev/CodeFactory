// See https://aka.ms/new-console-template for more information
using CodeFactory.Singleton;

Console.WriteLine("Hello, World!");

ConfigSetting config1 = ConfigSetting.GetInstance();
ConfigSetting config2 = ConfigSetting.GetInstance();

Console.WriteLine($"Config1 Connection String: {config1.DbConnectionString}");
Console.WriteLine($"Config2 Connection String: {config2.DbConnectionString} \n");

Console.WriteLine("Changing the connection string in config1...");
config1.DbConnectionString = "Server=newServer;Database=newDataBase;User Id=newUsername;Password=newPassword;";

Console.WriteLine("After Chaning the Config 1 : The Result of Config 2 is : ");
Console.WriteLine($"Config2 Connection String: {config2.DbConnectionString}");

if( config1.DbConnectionString == config2.DbConnectionString)
{
    Console.WriteLine("Both Config1 and Config2 have the same connection string as used same instance!.");
}
