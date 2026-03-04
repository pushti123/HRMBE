namespace Api.Model.RequestModel.User
{
    public class EditPermissionRequestModel
    {
        public int PermissionId { get; set; }
        public List<int> RoleId { get; set; }
    }
}
