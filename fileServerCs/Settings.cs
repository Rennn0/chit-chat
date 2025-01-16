using System.Configuration;

namespace fileServerCs;

public class Settings
{
    public static MongoDbSettings MongoSettings { get; } =
        new MongoDbSettings()
        {
            ConnectionString =
                ConfigurationManager.ConnectionStrings["MongoDB"]?.ConnectionString ?? string.Empty,
            Database = "chitChat",
        };

    public static RabbitSettings RabbitSettings { get; } =
        new RabbitSettings()
        {
            Host = ConfigurationManager.AppSettings["RabbitHost"] ?? string.Empty,
            Password = ConfigurationManager.AppSettings["RabbitPassword"] ?? string.Empty,
            Username = ConfigurationManager.AppSettings["RabbitUsername"] ?? string.Empty,
        };
}

public sealed class MongoDbSettings
{
    public string ConnectionString { get; set; } = string.Empty;
    public string Database { get; set; } = string.Empty;
}

public sealed class RabbitSettings
{
    public string Host { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
