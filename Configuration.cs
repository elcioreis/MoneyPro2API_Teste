namespace MoneyPro2;
public static class Configuration
{
    public static string JwtKey = "<carregado de appsettings.json>";
    public static string ApiKeyName = "<carregado de appsettings.json>";
    public static string ApiKey = "<carregado de appsettings.json>";
    public static SmtpConfiguration Smtp = new();

    public class SmtpConfiguration
    {
        public string Host { get; set; }
        public int Port { get; set; } = 25;
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}