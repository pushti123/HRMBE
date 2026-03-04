namespace Api.Model
{
    public class UpdateTicketStatusReqModel
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public string? Comment { get; set; }
    }
}
