namespace Api.Model.ResponseModel.Auth
{
    public class LoginResponseModel
    {
        public string Email {  get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }

        public string Token {  get; set; }
        public string RefreshToken { get; set; }
        public string? ProfilePic { get; set; }

        public List<PermissionDetail> PermissionDetailList {  get; set; }

        public class PermissionDetail
        {
            public string PerrmissionName { get; set; }
            public bool HasPermission {  get; set; }
        }
    }
}
