using RecSysApi.Application.Interfaces.Settings;

namespace RecSysApi.Application.Commons.Settings;

public class AppSettings : IAppSettings
{
    public string Secret { get; set; }
    public string DbConnectionString { get; set; }
}