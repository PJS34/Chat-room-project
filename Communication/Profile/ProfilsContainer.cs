using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Communication
{
    
    public class ProfilsContainer
    {
        private List<Profile> profilesList;
        public ProfilsContainer()
        {
            profilesList = new List<Profile>();
        }
        public ProfilsContainer(List<Profile> CopyList)
        {
            profilesList = CopyList;
        }
        public void add(Profile p)
        {
            profilesList.Add(p);
        }
        public void SerializeProfileList()
        {
            IFormatter formater = new BinaryFormatter();
            Stream stream = new FileStream("D:\\EFREI\\S7\\C#\\Projet\\Allprofiles.txt", FileMode.Create, FileAccess.Write);
            formater.Serialize(stream, profilesList);
            stream.Close();
            
        }
        public void Deserialize()
        {
            IFormatter formater = new BinaryFormatter();
            Stream stream = new FileStream("D:\\EFREI\\S7\\C#\\Projet\\Allprofiles.txt", FileMode.Open, FileAccess.Read);
            profilesList = (List<Profile>)formater.Deserialize(stream);
            stream.Close();

        }
        public Boolean contains(Profile p)
        {
            return profilesList.Contains(p);
        }
        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            foreach(Profile p in profilesList)
            {
                str.Append(p +"\n");
            }

            return str.ToString();
        }
    }

}
