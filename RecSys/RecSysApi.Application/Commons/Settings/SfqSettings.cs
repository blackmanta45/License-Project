using RecSysApi.Application.Interfaces.Settings;

namespace RecSysApi.Application.Commons.Settings;

public class SfqSettings : ISfqSettings
{
    public string SfqUsername { get; set; }
    public string SfqPassword { get; set; }
}