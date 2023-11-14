using ArivalBank._2fa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArivalBank._2fa.Application.Models
{
    public class CachedAuthorizationCodeModel
    {
        public bool IsCached { get; set; }
        public AuthorizationCode? AuthorizationCode { get; set; }
    }
}