namespace eGranjaCAT.Domain.Common
{
    public interface IUserBase
    {
        string Name { get; set; }
        string Lastname { get; set; }
        string Role { get; set; }
        string? Email { get; set; }
    }
}