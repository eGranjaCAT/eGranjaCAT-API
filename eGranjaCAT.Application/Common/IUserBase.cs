namespace eGranjaCAT.Application.Common
{
    public interface IUserBase
    {
        string Id { get; set; }
        string Name { get; set; }
        string Lastname { get; set; }
        string Email { get; set; }
        string Role { get; set; }
    }
}
