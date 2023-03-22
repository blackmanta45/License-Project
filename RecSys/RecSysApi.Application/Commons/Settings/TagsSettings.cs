using RecSysApi.Application.Interfaces.Settings;

namespace RecSysApi.Application.Commons.Settings;

public class TagsSettings : ITagsSettings
{
    public string Username { get; set; }
    public string Password { get; set; }
}