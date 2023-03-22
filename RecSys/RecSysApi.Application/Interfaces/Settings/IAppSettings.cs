namespace RecSysApi.Application.Interfaces.Settings
{
    public interface IAppSettings
    {
        string Secret { get; set; }
        string DbConnectionString { get; set; }
    }
}