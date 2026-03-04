namespace Api.Model.RequestModel.User
{
    public class GetUserListRequestModel
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public bool OrderBy { get; set; }
    }
}
