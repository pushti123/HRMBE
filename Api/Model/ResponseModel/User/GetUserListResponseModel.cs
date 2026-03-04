namespace Api.Model.ResponseModel.User
{
    public class GetUserListResponseModel
    {
        public int TotalCount { get; set; }

        public List<UserDetail> UserDetails { get; set; }
        public class UserDetail
        {
            public int Id { get; set; }
            public string UserId { get; set; }
            public string FullName { get; set; }
            public string RoleName { get; set; }
            public string Email { get; set; }
            public string DepartmentName { get; set; }
            public string DesignationName { get; set; }

        }
    }
}
