using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthLoom.Models
{
    public class Endpoint
    {
        public string Path { get; set; } = string.Empty;
        public string Roles { get; set; } = string.Empty;
        public string Method { get; set; } = string.Empty;
    }
}
