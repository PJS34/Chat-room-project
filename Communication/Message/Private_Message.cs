using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication
{
    public class Private_Message : Message
    {
        private string NameDest;
        public Private_Message(string msg, string name, string UserDest) : base(msg, name)
        {
            this.NameDest = UserDest;
        }
    }
}
