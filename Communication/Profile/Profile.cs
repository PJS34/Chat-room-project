using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication
{
    [Serializable]
    public class Profile
    {
        static int idCCount =0;
        int id;
        private string username;
        private string password;

        public Profile(string username, string password)
        {
            this.id = idCCount;
            idCCount++;
            this.username = username;
            this.password = password;
            
        }

        public string Username { get => username; set => username = value; }

        public override bool Equals(object obj)
        {
            var profile = obj as Profile;
            return profile != null &&
                   username == profile.username &&
                   password == profile.password;
        }

        public override int GetHashCode()
        {
            var hashCode = 1710835385;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(username);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(password);
            return hashCode;
        }
        public override string ToString()
        {
            return "username = " + username + "password" + password;
        }
    }
}
