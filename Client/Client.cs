
using Communication;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    enum TypeMessage { PM, Topic }
    [Serializable]
    public class Client
    {
        private string hostname;
        private int port;
        private String Destination;

        private TcpClient comm;
        public Client(string h, int p)
        {
            hostname = h;
            port = p;
            comm = null;
        }

        public void RecieveMessage()
        {
            while (true)
            {
                Message msg = Net.rcvMsg(comm.GetStream());

                //Console.WriteLine(msg);
                Console.WriteLine(msg.Name + " says : " + msg.Msg);
            }
        }
        public void start()
        {

            ProfilsContainer profils = new ProfilsContainer();
            /*profils.add(new Profile("Julien", "Julien"));
            profils.add(new Profile("Greg", "Greg"));
             profils.SerializeProfileList();*/

            profils.Deserialize();
            Profile tryProfile;
            string choix;
            do
            {
                Console.WriteLine("Avez vous déja un compte ? yes/no");
                choix = Console.ReadLine();
            } while (!choix.Equals("yes") && !choix.Equals("no"));
            if (choix.Equals("yes"))
            {
                tryProfile = Identification(profils);
            }
            else
            {
                tryProfile = ProfileRegister(profils);
            }

            Console.WriteLine("Welcome to the client interface");
            Console.WriteLine("Please choose what do you want to do : ");
            Console.WriteLine("A) Join a Topic");
            Console.WriteLine("B) Send a private Message");
            choix = Console.ReadLine();
            switch (choix)
            {
                case "A":

                    Console.WriteLine("Please choose a topic : ");
                    //choose a topic
                    //displayAllTopics, passer par la serialization deserialization dans un fichier txt
                    displayAllTopics();
                    //Please use the Dest String


                    break;
                case "B":
                    //Choose a user
                    break;
                default:
                    Console.WriteLine("Invalide");
                    break;
            }
            comm = new TcpClient(hostname, port);
            //Send identity, la recevoir dans server avant de lancer le thread. Devra surement etre synchronize.
            Net.SendIdentity(comm.GetStream(), tryProfile);
            Console.WriteLine("Connection established");


            //Lance la reception des messages
            SendingMessageTopic(tryProfile);
        }

        private void displayAllTopics()
        {
            List<String> Topics = new List<string>();
            IFormatter formater = new BinaryFormatter();
            Stream stream = new FileStream("D:\\EFREI\\S7\\C#\\Projet\\AllTopics.txt", FileMode.Open, FileAccess.Read);
            ListTopics = (Dictionary<String, List<Receiver>>)formater.Deserialize(stream);
            foreach (String topic in ListTopics.Keys)
            {
                ListTopics[topic].Clear();
            }
            stream.Close();


        }

        private void SendingMessageTopic(Profile tryProfile)
        {
            new Thread(RecieveMessage).Start();
            while (true)
            {
                string message = Console.ReadLine();
                Message msg = new Message(message, tryProfile.Username);
                Net.SendMsg(comm.GetStream(), msg);
            }
        }

        private Profile ProfileRegister(ProfilsContainer profils)
        {
            string name;
            string psw;
            Console.Write("Your name : ");
            name = Console.ReadLine();
            Console.WriteLine();
            Console.Write("Your password  : ");
            psw = Console.ReadLine();
            Profile p = new Profile(name, psw);
            profils.add(p);
            profils.SerializeProfileList();
            return p;
        }

        private Profile Identification(ProfilsContainer profils)
        {
            Profile tryProfile;
            Console.WriteLine("Identification");
            do
            {

                Console.WriteLine("Votre nom ?");
                string name = Console.ReadLine();
                Console.WriteLine("Votre mdp ?");
                string mdp = Console.ReadLine();
                tryProfile = new Profile(name, mdp);
            } while (!profils.contains(tryProfile));
            return tryProfile;
        }
    }
}
