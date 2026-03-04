namespace Api.Model.RequestModel
{
    public class AddTicketReqModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Subject { get; set; } = null!;

        public string Description { get; set; } = null!;
    }
}
