using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArivalBank._2fa.Application.Models
{
    public class AuthorizationCodeRequestModel
    {
        [Required]
        [RegularExpression(@"^[0-9]+$")]
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
