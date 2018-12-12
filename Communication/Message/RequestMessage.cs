using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication
{
    [Serializable]
    public class RequestMessage : Message
    {
        public RequestMessage(string msg, Profile p) : base(msg, p)
        {

        }
    }
}
