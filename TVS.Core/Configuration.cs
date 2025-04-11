namespace TVS.Core;

public static class Configuration
{
    public static string BackendUrl { get; set; } = "http://localhost:5296/api/";
    public static string FrontendUrl { get; set; } = string.Empty;
}