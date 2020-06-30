using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class cod4master
{
    class Program
    {

        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
        public static void Main()
        {

            byte[] data = new byte[1024];
            int port = 20810;
            Console.Title = "cod4master.activision.com";
            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, port);
            UdpClient newsock = new UdpClient(ipep);
            Console.WriteLine("cod4master.activision.com by SniperViper");
            Console.WriteLine("Started Listening on port {0}...", port);
            var lineCount = File.ReadLines("servers.txt").Count();
            Console.Title=(lineCount + " Servers registered");
            Console.WriteLine("");
            IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
            //data = newsock.Receive(ref sender);
            
            //Console.WriteLine("Message received from {0}:", sender.ToString());
            //Console.WriteLine(Encoding.ASCII.GetString(data, 0, data.Length));

            int gif = 1;

            while (gif < 10)
            {
                data = newsock.Receive(ref sender);
                string message = Encoding.ASCII.GetString(data, 0, data.Length);
                //Console.WriteLine("Client -> Server: {0}", Encoding.ASCII.GetString(data, 0, data.Length));
                // newsock.Send(data, data.Length, sender);
                // var TEST = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0x67, 0x65, 0x74, 0x73, 0x65, 0x72, 0x76, 0x65, 0x72, 0x73, 0x52, 0x65, 0x73, 0x70, 0x6F, 0x6E, 0x73, 0x65, 0x00, 0x40, 0x0A, 0x00, 0x5C, 0x7F, 0x00, 0x00, 0x01, 0x71, 0x21, 0x5C, 0x45, 0x4F, 0x46 };

                //Convert.ToInt64(hexValue, 16); (HEX2DECIMAL)
                //string.format("{0:x}", decValue); (DECIMAL2HEX)

                if (message.Contains("getservers"))

                {
                    int LOOP_Count = 0;
                    int srv_counter = 0;

                    Console.WriteLine(DateTime.Now + ": " + "Getservers request from: {0}", sender.ToString(), Encoding.ASCII.GetString(data, 0, data.Length));
                    

                    //var Body = new byte[] { 0x7F, 0x00, 0x00, 0x01, 0x71, 0x21 };
                    
                    while (LOOP_Count < 1)
                    {
                        try
                        {
                            var Header = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0x67, 0x65, 0x74, 0x73, 0x65, 0x72, 0x76, 0x65, 0x72, 0x73, 0x52, 0x65, 0x73, 0x70, 0x6F, 0x6E, 0x73, 0x65, 0x00, 0x40, 0x0A, 0x00, 0x5C };
                            string[] server_list = File.ReadAllLines("servers.txt");
                            
                            string REP = server_list[srv_counter];
                            srv_counter++;
                            //string REP = "C0A8016F7121";


                            //int intValue = 127;
                            //string hexValue = intValue.ToString("X");
                            //Console.WriteLine("TEST MESSAGE IGNORE: {0}",hexValue);

                            byte[] stest = StringToByteArray(REP);

                            //var Body = File.ReadAllBytes("servers.txt");

                            var Footer = new byte[] { 0x5C, 0x45, 0x4F, 0x46 };

                            var Packet = new MemoryStream();
                            Packet.Write(Header, 0, Header.Length);
                            Packet.Write(stest, 0, stest.Length);
                            Packet.Write(Footer, 0, Footer.Length);
                            var packetc = Packet.ToArray();


                            newsock.Send(packetc, packetc.Length, sender);

                            //Console.WriteLine("Server -> Client: {0}", Encoding.ASCII.GetString(packetc, 0, packetc.Length));
                        }
                        catch (Exception e)
                        {
                            //Console.WriteLine(e);
                            LOOP_Count = 200;
                            Console.WriteLine(DateTime.Now + ": " + "Sent ({0}) servers to {1}", srv_counter, sender.ToString());
                        }

                    }


                    ;
                }
                else if (message.Contains("heartbeat COD-4"))
                {
                    Console.WriteLine(DateTime.Now + ": " + "Heartbeat from: {0}", sender.ToString());
                    //Console.WriteLine("Message received from {0}", sender.ToString());
                    //Console.WriteLine("Heartbeat Recieved!");
                    string New_Server = sender.ToString();

                    //Gets first IP before the first .
                    var First = New_Server.Split(new string[] { "." }, StringSplitOptions.None)[0]
                    .Split('.')[0]
                    .Trim();

                    //Gets second ip
                    var Second = New_Server.Split(new string[] { "." }, StringSplitOptions.None)[1]
                        .Split('.')[0]
                        .Trim();

                    //gets third IP
                    var Third = New_Server.Split(new string[] { "." }, StringSplitOptions.None)[2]
                     .Split('.')[0]
                     .Trim();

                    //Gets Fourth num from IP after .
                    var Fourth_1 = New_Server.Split(new string[] { ":" }, StringSplitOptions.None)[0]
                    .Split(':')[0]
                    .Trim();
                    var Fourth_2 = Fourth_1.Split(new string[] { "." }, StringSplitOptions.None)[3]
                    .Split('.')[0]
                    .Trim();

                    var PORT_Srv = New_Server.Split(new string[] { ":" }, StringSplitOptions.None)[1]
                    .Split(':')[0]
                    .Trim();

                    int One = Int32.Parse(First);
                    string One_Hex = One.ToString("X");
                    if (One_Hex.Length == 1)
                    {
                        One_Hex = "0" + One_Hex;
                    }

                    int Two = Int32.Parse(Second);
                    string Two_Hex = Two.ToString("X");
                    if (Two_Hex.Length == 1)
                    {
                        Two_Hex = "0" + Two_Hex;
                    }

                    int Three = Int32.Parse(Third);
                    string Three_Hex = Three.ToString("X");
                    if (Three_Hex.Length == 1)
                    {
                        Three_Hex = "0" + Three_Hex;
                    }

                    int Four = Int32.Parse(Fourth_2);
                    string Four_Hex = Four.ToString("X");
                    if (Four_Hex.Length == 1)
                    {
                        Four_Hex = "0" + Four_Hex;
                    }

                    int Port_Decimal = Int32.Parse(PORT_Srv);
                    string Port_Hex = Port_Decimal.ToString("X");

                    string read_test = System.IO.File.ReadAllText("servers.txt");

                    if (read_test.Contains(One_Hex + Two_Hex + Three_Hex + Four_Hex + Port_Hex))
                    {
                        Console.WriteLine(DateTime.Now + ": " + "Server already in serverlist");
                    }
                    else
                    {
                        System.IO.File.AppendAllText("servers.txt", Environment.NewLine + One_Hex + Two_Hex + Three_Hex + Four_Hex + Port_Hex);
                        var serverCount = File.ReadLines("servers.txt").Count();
                        Console.Title = (serverCount + " Servers registered");
                        Console.WriteLine(DateTime.Now + ": " + One_Hex + Two_Hex + Three_Hex + Four_Hex + Port_Hex + " - ADDED TO SERVERLIST!");
                    }
   
                   
                    ;
                }
                else
                {
                    //Console.WriteLine("Nothing message was not understood!");
                    Console.WriteLine(DateTime.Now + ": " + sender.ToString() + " - " +Encoding.ASCII.GetString(data, 0, data.Length));
                    ;
                }


                //newsock.Send(TEST, TEST.Length, sender);
                //Console.WriteLine("Server -> Client: {0}", Encoding.ASCII.GetString(TEST, 0, TEST.Length));

            }

        }
    }

}