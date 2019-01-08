
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
        private TcpClient comm2;
        private static Semaphore sem;
        public Client(string h, int p)
        {
            hostname = h;
            port = p;
            comm = null;
            comm2 = null;
            sem = new Semaphore(0, 1);
        }


        public void start()
        {

            comm = new TcpClient(hostname, port);
            comm2 = new TcpClient(hostname, port);
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
                    tryProfile = AskInformations();
                    Message auth = new AuthMessage("Login", tryProfile);
                    Net.SendMsg(comm.GetStream(), auth);
                    verifauth = (AuthMessage)Net.rcvMsg(comm.GetStream());
                    if (!verifauth.Success)
                    {

                        Console.WriteLine("Utilisateur inconnu ou déja connecté");
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
            new Thread(RecieveMessage).Start();
            // new Thread(RecievePersonalMessage).Start();
            choix = LaunchMenu(tryProfile);

        }

        private string LaunchMenu(Profile tryProfile)
        {
            string choix;
            do
            {
                Console.WriteLine("Welcome to the client interface");
                Console.WriteLine("Please choose what do you want to do : ");
                Console.WriteLine("A) Join a Topic");
                Console.WriteLine("B) Add a Topic");
                Console.WriteLine("C) ListTheTopics");
                Console.WriteLine("D) ListConnectedUsers");
                Console.WriteLine("E) Send a private Message");
                choix = Console.ReadLine();
                switch (choix)
                {
                    case "A":

                        Console.WriteLine("Please choose a topic : ");
                        Destination = Console.ReadLine();
                        SendingMessageTopic(tryProfile);

                        break;
                    case "B":
                        Console.WriteLine("What is the new subject ?");
                        Destination = Console.ReadLine();
                        CreateTopicRequest(tryProfile);
                        break;
                    case "C":
                        sem.WaitOne();
                        Message msg = new RequestMessage("RequireListTopic", tryProfile);
                        Net.SendMsg(comm2.GetStream(), msg);
                        msg = Net.rcvMsg(comm2.GetStream());
                        Console.WriteLine(msg.Msg);
                        sem.Release();
                        break;
                    case "D":
                        Message msg2 = new RequestMessage("RequireListUsers", tryProfile);
                        Net.SendMsg(comm2.GetStream(), msg2);
                        msg = Net.rcvMsg(comm2.GetStream());
                        Console.WriteLine(msg.Msg);
                        break;
                    case "E":
                        Console.WriteLine("to whom do you want to talk ?");
                        Destination = Console.ReadLine();
                        SendingPersonalMesage(tryProfile);
                        break;

                    default:
                        Console.WriteLine("Invalide");
                        break;
                }
                //  new Thread(RecieveMessage).Start();
            } while (!choix.Equals("exit"));
            return choix;
        }

        private Boolean BackToMenu(String s)
        {
            if (s.Equals("quit"))
            {
                return true;
            }
            return false;
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

       

        private void displayAllTopics()
        {
            
            Message msg = Net.rcvMsg(comm.GetStream());
            Console.WriteLine(msg.Msg);
          


        }
      
        public void RecieveMessage()
        {

            while (true)
            {
                
               
                Message msg = Net.rcvMsg(comm.GetStream());
                if (msg.GetType().Equals(typeof(Private_Message)))
                {
                    Console.WriteLine(msg.P.Username + " says personally : " + msg.Msg);
                }
                else
                {
                    //Console.WriteLine(msg);
                    Console.WriteLine(msg.P.Username + " says : " + msg.Msg);
                }
            }
        }
       
        private void CreateTopicRequest(Profile tryProfile)
        {
            Message msg = new TopicMessage("Creation", tryProfile, Destination);
            Net.SendMsg(comm.GetStream(), msg);
            SendingMessageTopic(tryProfile);
        }
        private void SendingPersonalMesage(Profile tryProfile)
        {
            Console.WriteLine("Send private message");
            while (true)
            {
                string message = Console.ReadLine();
                if (message.Equals("quit"))
                {
                    break;
                }
                Message msg = new Private_Message(message, tryProfile, Destination);
                Net.SendMsg(comm.GetStream(), msg);
            }
        }
        private void SendingMessageTopic(Profile tryProfile)
        {

            TopicMessage msgTopic = new TopicMessage("InscriptionTopic", tryProfile, Destination);
            Net.SendMsg(comm2.GetStream(), msgTopic);
            Console.WriteLine("Welcome on this topic !!!! Here we talk about : " + Destination);
            while (true)
            {
                
                string message = Console.ReadLine();
                
                Message msg = new TopicMessage(message, tryProfile, Destination);
                Net.SendMsg(comm.GetStream(), msg);
                if (message.Equals("quit"))
                {
                    break;
                }
            }
        }


    }
}
