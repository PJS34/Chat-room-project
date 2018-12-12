
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
                Console.WriteLine(msg.P.Username + " says : " + msg.Msg);
            }
        }
        public void start()
        {

            comm = new TcpClient(hostname, port);
            Console.WriteLine("Connection established");
            Profile tryProfile;
            string choix;
            do
            {
                Console.WriteLine("Avez vous déja un compte ? yes/no");
                choix = Console.ReadLine();
            } while (!choix.Equals("yes") && !choix.Equals("no"));
            AuthMessage verifauth;
            if (choix.Equals("yes"))
            {
               
                do
                {

                   tryProfile= AskInformations();
                    Message auth = new AuthMessage("Login", tryProfile);
                    Net.SendMsg(comm.GetStream(), auth);
                    verifauth = (AuthMessage)Net.rcvMsg(comm.GetStream());
                    if (!verifauth.Success)
                    {
                        Console.WriteLine("Utilisateur inconnu");
                    }
                } while (!verifauth.Success);
                Console.WriteLine("Login validé par le serveur");
               

            }
            else
            {

                do
                {

                    tryProfile = AskInformations();
                    Message auth = new AuthMessage("Register", tryProfile);
                    Net.SendMsg(comm.GetStream(), auth);
                    verifauth = (AuthMessage)Net.rcvMsg(comm.GetStream());
                    if (!verifauth.Success)
                    {
                        Console.WriteLine("N'as pas pu etre ajouté, le nom est déja pris");
                    }
                } while (!verifauth.Success);
                Console.WriteLine("Enrengistrement validé par le serveur"); ;
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
                     Destination = Console.ReadLine();
                    //Please use the Dest String
  


                    //Lance la reception des messages
                    SendingMessageTopic(tryProfile);

                    break;
                case "B":
                    //comm = new TcpClient(hostname, port);
                    //Send identity, la recevoir dans server avant de lancer le thread. Devra surement etre synchronize.
                    Net.SendIdentity(comm.GetStream(), tryProfile);
                    Console.WriteLine("Connection established");


                    //Lance la reception des messages
                    SendingPersonalMesage(tryProfile);
                    //Choose a user
                    break;
                default:
                    Console.WriteLine("Invalide");
                    break;
            }
          
        }

        private static Profile AskInformations()
        {
            string name;
            string psw;
            Console.Write("Your name : ");
            name = Console.ReadLine();
            Console.WriteLine();
            Console.Write("Your password  : ");
            psw = Console.ReadLine();
           Profile tryProfile = new Profile(name, psw);
            return tryProfile;
        }

        private void SendingPersonalMesage(Profile tryProfile)
        {
            throw new NotImplementedException();
        }

        private void displayAllTopics()
        {
            List<String> Topics = new List<String>();
            IFormatter formater = new BinaryFormatter();
            Stream stream = new FileStream("D:\\EFREI\\S7\\C#\\Projet\\AllTopics.txt", FileMode.Open, FileAccess.Read);
            Topics = (List<String>)formater.Deserialize(stream);
            foreach (String topic in Topics)
            {
                Console.WriteLine(topic);
            }
            stream.Close();


        }

        private void SendingMessageTopic(Profile tryProfile)
        {
            new Thread(RecieveMessage).Start();
            //send
            while (true)
            {
                string message = Console.ReadLine();
                Message msg = new TopicMessage(message, tryProfile,Destination);
                Net.SendMsg(comm.GetStream(), msg);
            }
        }

        
    }
}
