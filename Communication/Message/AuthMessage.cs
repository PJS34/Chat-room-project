using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication
{
    public class AuthMessage : Message
    {
        public AuthMessage(string msg, Profile p) : base(msg, p)
        {
            
        }

    }
}
