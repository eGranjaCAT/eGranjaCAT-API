using eGranjaCAT.Application.DTOs.User;


namespace eGranjaCAT.Application.DTOs.Farm
{
    public class GetFarmDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public GetUserDTO User { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool Updated { get; set; }
    }
}
