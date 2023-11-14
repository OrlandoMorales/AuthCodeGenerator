using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArivalBank._2fa.Domain.Configuration
{
    public class AppConfiguration
    {
        public int CodeDurationMinutes { get; set; }
        public int MaxCodesByPhone { get; set; }
        public bool IsSmsActive { get; set; }
        public int CodeLength { get; set; }
    }
}
