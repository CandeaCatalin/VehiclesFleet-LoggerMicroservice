namespace LoggerMicroservice.Settings;

public static class AppSettingsConstants
{
    public static class Section
    {
        public const string Database = "Database";
        public const string Authorization = "Authorization";
    }

    public static class Keys
    {
        public const string ConnectionString = "ConnectionString";
        public const string JwtSecretKey = "JwtSecretKey";
    }
}