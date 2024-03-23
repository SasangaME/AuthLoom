using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthLoom.Models
{
    public class AuthSettings
    {
        public string BaseUrl { get; set; } = string.Empty;
        public bool SuperAdminEnabled { get; set; }
        public List<Endpoint> Endpoints { get; set; } 
    }
}
