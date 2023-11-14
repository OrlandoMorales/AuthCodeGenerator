using ArivalBank._2fa.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArivalBank._2fa.Application.Interfaces
{
    public interface ISmsGatewayMessaging
    {
        bool DeliverMessage(SmsMessageModel smsMessageModel);
    }
}
