
using Communication;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Projet
{
    public class Server
    {
        private int port;
       private static ProfilsContainer profils = new ProfilsContainer();
        /* profils.add(new Profile("Julien", "Julien"));
         profils.add(new Profile("Greg", "Greg"));
         profils.SerializeProfileList();*/

        
        // private static List<Receiver> ConnectedUsers;
        private static Dictionary<Profile, Receiver> ConnectedUsers;
        private static Dictionary<String, List<Receiver>> ListTopics;

        public static Dictionary<String, List<Receiver>> _ListTopics { get => ListTopics; set => ListTopics = value; }

       

        public static Dictionary<Profile, Receiver> _ConnectedUsers { get => ConnectedUsers; set => ConnectedUsers = value; }

        public Server(int port)
        {
            this.port = port;
            ConnectedUsers = new Dictionary<Profile, Receiver>();
            ListTopics = new Dictionary<String, List<Receiver>>();
           // newTopic("C#");
           // newTopic("Programming");
            //SerializeTopicList();
            DeserializeTopicList();
            profils.Deserialize();


        }
        public static void checkUserInTopic(TopicMessage msgTopic,Receiver r)
        {
            
        }
        public void SerializeTopicList()
        {
            List<String> ListTopicsName = new List<String>();
            //ListTopicsName = ListTopics.Keys;
            foreach (String s in ListTopics.Keys)
            {
                ListTopicsName.Add(s);
            }
           
            IFormatter formater = new BinaryFormatter();
            Stream stream = new FileStream("D:\\EFREI\\S7\\C#\\Projet\\AllTopics.txt", FileMode.Create, FileAccess.Write);
            formater.Serialize(stream, ListTopicsName);
            stream.Close();

        }
        public void DeserializeTopicList()
        {
            ListTopics.Clear();
            List<String> ListTopicsName = new List<String>();
            IFormatter formater = new BinaryFormatter();
            Stream stream = new FileStream("D:\\EFREI\\S7\\C#\\Projet\\AllTopics.txt", FileMode.Open, FileAccess.Read);
            ListTopicsName = (List<String>)formater.Deserialize(stream);
            foreach (String topic in ListTopicsName)
            {
                List<Receiver> UserByTopic = new List<Receiver>();
                ListTopics.Add(topic, UserByTopic);
            }
            stream.Close();

        }
        public void newTopic(string name)
        {
            List<Receiver> usersByTopic = new List<Receiver>();
            ListTopics.Add(name, usersByTopic);
            SerializeTopicList();
        }


   

        public static void BroadcastByTopic(TopicMessage msg)
        {

            if (ListTopics.ContainsKey(msg.TopicName))
            {

                foreach (Receiver User in ListTopics[msg.TopicName])
                {
                    Net.SendMsg(User.Comm.GetStream(), msg);
                }
            }
            else
            {
                Console.WriteLine("Topic inexistant");
            }

        }
        /*
        public static void BroadcastTheMessage(Message msg)
        {
            foreach (Receiver User in ConnectedUsers.Values)
            {
                Net.SendMsg(User.Comm.GetStream(), msg);
            }
        }
        */
        public static Boolean ProfileRegister(AuthMessage msg)
        {
           if(!profils.CheckPossiblRegister(msg.P.Username))
            {
                return false;
            }
            profils.add(msg.P);
            profils.SerializeProfileList();
            return true;
        }

        public static Boolean Identification(AuthMessage msg)
        {
           
            Console.WriteLine("Identification");
            if (profils.contains(msg.P))
            {
                return true;
            }
            else
            {
                //send bool false
                return false;
            }
           
        }
        public void start()
        {
            TcpListener l = new TcpListener(new IPAddress(new byte[] { 127, 0, 0, 1 }), port);
            l.Start();
            Console.WriteLine("Stating the server");
            while (true)
            {
                
                TcpClient comm = l.AcceptTcpClient();
                //Profile newProfileConnected = Net.RcvIdentity(comm.GetStream());
                //Console.WriteLine("Connection established with @" + newProfileConnected.Username);
                Console.WriteLine("Connection etablished");
                Receiver newUser = new Receiver(comm);
                //ConnectedUsers.Add(newProfileConnected, newUser);

                new Thread(newUser.doOperation).Start();


            }



        }
    }
}
