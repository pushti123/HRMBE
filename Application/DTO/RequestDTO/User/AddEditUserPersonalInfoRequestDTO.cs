namespace Application.DTO.RequestDTO.User
{
    public class AddEditUserPersonalInfoRequestDTO
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Address{ get; set; }
        public string PinCode { get; set; }
        public string ContactNo { get; set; }
        public string AadharNumber { get; set; }
        public string Gender { get; set; }
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string ConfirmPassword { get; set; }
        public dynamic? ProfilePic { get; set; }
        public bool? IsChanged { get; set; }
    }
}
