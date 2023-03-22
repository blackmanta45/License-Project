using RecSysApi.Application.Interfaces.Settings;

namespace RecSysApi.Application.Commons.Settings;

public class UpvSettings : IUpvSettings
{
    public string Protocol { get; set; }
    public string Domain { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}