using Repository.Pattern.Ef6;
using System.ComponentModel.DataAnnotations;

namespace Subscription.DAL.Entities.Model
{
    public class RefreshToken:Entity
    {
        [Key]
        public string Id { get; set; }
        public string UserName { get; set; } 
        public System.DateTime IssuedUtc { get; set; }
        public System.DateTime ExpiresUtc { get; set; }
        public string ProtectedTicket { get; set; }
    }
}
