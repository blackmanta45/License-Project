namespace RecSysApi.Application.Interfaces.Settings
{
    public interface IUpvSettings
    {
        string Protocol { get; set; }
        string Domain { get; set; }
        string Username { get; set; }
        string Password { get; set; }
    }
}