namespace FreeLancing.Models.ApiModels
{
    public class PrivateChatMessage
    {
        public string Text { get; set; }
        public string Time { get; set; }
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public bool isSender { get; set; }
    }
}
