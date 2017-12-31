namespace Subscription.BLL.DTOs
{
    public class RefreshTokenDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public System.DateTime IssuedUtc { get; set; }
        public System.DateTime ExpiresUtc { get; set; }
        public string ProtectedTicket { get; set; }
    }
}
