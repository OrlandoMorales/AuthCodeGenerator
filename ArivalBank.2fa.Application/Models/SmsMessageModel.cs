using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArivalBank._2fa.Application.Models
{
    public class SmsMessageModel
    {
        public string MessageBody { get; set; }
        public string FromPhoneNumber { get; set; }
        public string ToPhoneNumber { get; set; }
    }
}
