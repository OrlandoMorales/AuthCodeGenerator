using ArivalBank._2fa.Application.Interfaces;
using ArivalBank._2fa.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArivalBank._2fa.Infrastructure.Messaging
{
    public class TwilioGatewayMessaging : ISmsGatewayMessaging
    {
        public bool DeliverMessage(SmsMessageModel smsMessageModel)
        {
            throw new NotImplementedException();
        }
    }
}
