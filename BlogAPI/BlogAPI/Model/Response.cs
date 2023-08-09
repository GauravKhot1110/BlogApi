using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogAPI.Authentication
{
    public class Response
    {
        public string Status { get; set; }
        public string Massage { get; set; }
        public object data { get; set; }


    }
}
