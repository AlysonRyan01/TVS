namespace TVS.Api;

public static class ApiConfig
{
    public static SmtpConfiguration Smtp = new SmtpConfiguration();
    
    public class SmtpConfiguration
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}