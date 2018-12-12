using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication
{
    [Serializable]
    public class AuthMessage : Message
    {
        private Boolean success;
        public AuthMessage(string msg, Profile p) : base(msg, p)
        {

        }

        public bool Success { get => success; set => success = value; }
    }
}
