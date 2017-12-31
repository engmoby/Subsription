using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Subscription.Common;

namespace Subscription.BLL.DTOs
{
    public class ProductNamesDto
    {
        public long ProductId { get; set; }
        public string ProductTitle { get; set; }
        public string ProductDesc { get; set; } 
    }
}
