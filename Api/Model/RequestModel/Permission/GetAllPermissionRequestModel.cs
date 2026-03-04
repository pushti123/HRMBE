namespace Api.Model.RequestModel.Permission
{
    public class GetAllPermissionRequestModel
    {
        public int PageSize {  get; set; }
        public int PageIndex { get; set; }
        public bool OrderBy {  get; set; }
    }
}
