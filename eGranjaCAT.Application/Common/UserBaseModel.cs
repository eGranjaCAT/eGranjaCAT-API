namespace eGranjaCAT.Application.Common
{
    public class UserBaseModel : IUserBase
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}