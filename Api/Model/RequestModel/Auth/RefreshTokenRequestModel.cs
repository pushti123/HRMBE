namespace Api.Model.RequestModel.Auth
{
    public class RefreshTokenRequestModel
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
