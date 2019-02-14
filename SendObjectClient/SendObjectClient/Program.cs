using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Xml.Serialization;

namespace SendObjectClient
{
    class Program
    {
        static int port = 8005;
        static string address = "127.0.0.1";


        static void Main(string[] args)
        {

            try
            {
                IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(ipPoint);

                Console.WriteLine("Input message: ");
                string message = Console.ReadLine();
                string name = "Evgen";
                byte[] foto = File.ReadAllBytes("pic.jpg");

                PersonMessage p1 = new PersonMessage(1, name, message, new List<string> { "newMessage", "cSharp", "XML", "rulez" }, foto);
                Console.WriteLine("\nPackage:\n" + p1);

                byte[] persMess = PersonMessageToXMLByteArray(p1);
                PersonMessageToXML(p1);

                Console.WriteLine("\nDeserialized package:\n\n" + (XMLByteArrayToPersonMessage(persMess).ToString()) + "\n");

                socket.Send(persMess);

                //  GET ANSWER  
                byte[] data = new byte[256];
                StringBuilder builder = new StringBuilder();
                int bytes = 0;

                do
                {
                    bytes = socket.Receive(data, data.Length, 0);
                    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                } while (socket.Available > 0);

                Console.WriteLine("Server says: " + builder.ToString());

                // CLOSE SOCKET
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.Read();
        }

        public static byte[] ObjectToByteArray(Object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        public static Object ByteArrayToObject(byte[] arrBytes)
        {
            using (var memStream = new MemoryStream())
            {
                var binForm = new BinaryFormatter();
                memStream.Write(arrBytes, 0, arrBytes.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                var obj = binForm.Deserialize(memStream);
                return obj;
            }
        }

        public static byte[] PersonMessageToXMLByteArray(PersonMessage obj)
        {
            XmlSerializer xmlSer = new XmlSerializer(typeof(PersonMessage));
            using (var ms = new MemoryStream())
            {
                xmlSer.Serialize(ms, obj);
                return ms.ToArray();
            }

        }

        public static PersonMessage XMLByteArrayToPersonMessage(byte[] arrBytes)
        {
            using (var memStream = new MemoryStream())
            {
                XmlSerializer xmlSer = new XmlSerializer(typeof(PersonMessage));

                memStream.Write(arrBytes, 0, arrBytes.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                PersonMessage person = (PersonMessage)xmlSer.Deserialize(memStream);

                return person;
            }
        }

        public static void PersonMessageToXML(PersonMessage obj)
        {
            XmlSerializer xmlSer = new XmlSerializer(typeof(PersonMessage));
            using (var fs = new FileStream("person.txt", FileMode.OpenOrCreate, FileAccess.Write))
            {
                xmlSer.Serialize(fs, obj);
            }

        }


    }
}
