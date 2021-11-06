using System;
using System.Net;
using System.Net.Sockets;

namespace testSocketServer
{
    static class SocketExtensions
    {
        public static bool IsConnected(this Socket socket)
        {
            try
            {
                return !(socket.Poll(1, SelectMode.SelectRead) && socket.Available == 0);
            }
            catch (SocketException) { return false; }
        }
    }
    class MainClass
    {
        public static TcpListener serv;
        public static PropertyClass[] properties = new PropertyClass[8];
        public static NetworkStream Stream;
        public static void Main(string[] args)
        {
            String IP = "127.0.0.1";
            UInt16 port = 8080;
            if (args.Length > 0)
            {
                IP = args[0];
                Console.WriteLine("Default host changed to:" + IP);
            }
            if (args.Length > 1)
            {
                port = UInt16.Parse(args[1]);
                Console.WriteLine("Default port changed to:" + port.ToString());
            }


            try
            {
                serv = new TcpListener(IPAddress.Parse(IP), port);
                serv.Start();
                Console.WriteLine("Server started to "+IP+":"+port.ToString());
                TcpClient client;
            Wait4rConnection:
                client = serv.AcceptTcpClient();
                Console.WriteLine("A client connected.");
                Stream = client.GetStream();
                for (int i = 0; i < properties.Length; i++)
                {
                    properties[i] = new PropertyClass("VAR_" + Convert.ToChar('A' + i));
                    switch (i % 3) {
                        case 0:
                            properties[i].InitAsInt(-127, 127, 0); break;
                        case 1:
                            properties[i].InitAsFloat((float)0.0, (float)10.0, (float)3.0); break;
                        case 2:
                        default: properties[i].InitAsString(32, "TEST"); break;
                    }

                }
                while (SocketExtensions.IsConnected(client.Client)) 
                {
                    var buffer = new byte[1024];
                    // wait for data to be received
                    var bytesRead = Stream.Read(buffer, 0, buffer.Length);
                    var r = System.Text.Encoding.UTF8.GetString(buffer);
                    // write received data to the console
            
                  
                    Console.WriteLine("RX:" + r.Substring(0, bytesRead));
                    //PING COMMAND
                    if (r.StartsWith("OK", StringComparison.OrdinalIgnoreCase))
                    {
                        SendLine("OK");
                    }
                    //GET LIST OF PARAMETERS
                    if (r.StartsWith("LIST", StringComparison.OrdinalIgnoreCase))
                    {
                        for (int i = 0; i < properties.Length; i++)
                        {
                            SendLine(properties[i].toString());
                        }
                        SendLine("OK");
                    }
                    if (r.StartsWith("GET ", StringComparison.OrdinalIgnoreCase))
                    {
                        bool OK = false;
                        string val = (r + ' ').Split(' ')[1];
                        string test = "";
                        for (int i = 0; i < properties.Length; i++)
                        {
                            test = properties[i].name;
                            if (test.Equals(val.Substring(0, val.IndexOfAny("\r\n\0".ToCharArray()))))
                            {
                                OK = true;
                                SendLine(properties[i].Value.ToString());
                                SendLine("OK");
                            }
                        }
                        if (!OK)
                        {
                            SendLine("ERROR: WRONG VARIABLE");
                            SendLine("OK");
                        }
                    }
                    if (r.StartsWith("SET ", StringComparison.OrdinalIgnoreCase))
                    {
                        string line = r;
                        if (line.Split('=').Length == 2)
                        {
                            string key = line.Substring(4, line.Length - 5).Split('=')[0];
                            string value = line.Split('=')[1];
                            for (int i = 0; i < properties.Length; i++)
                            {
                                if (properties[i].name.Equals(key))
                                {
                                    properties[i].set(value);
                                    SendLine(properties[i].Value.ToString());
                                    SendLine("OK");
                                }
                            }
                        }
                        else {
                            SendLine("ERROR: FAILED FORMAT. RIGHT IS \"SET VAR=VAL");
                            SendLine("OK");
                        }
                    }


                }
                Console.WriteLine("A client disconnected.");
                goto Wait4rConnection;
            } catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }

        public static void SendLine(String message) {
            Console.WriteLine("TX:" + message);
            Stream.Write(System.Text.Encoding.UTF8.GetBytes(message+"\n"), 0, message.Length+1);
        }
    }
}
