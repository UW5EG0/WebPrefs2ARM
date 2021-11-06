using System;
using System.Collections.Generic;
using System.Net.Sockets;
using Gtk;

namespace chipConf
{
    public class SocketClass
    {
        private string host;
        private UInt16 port;
        private TcpClient socket;
        private NetworkStream socketStream;
        public List<PropertyClass> properties = new List<PropertyClass>();
        public string Host { get => host; set => host = value; }
        public ushort Port { get => port; set => port = value; }



        public SocketClass(string host, UInt16 port)
        {
            this.socket = new TcpClient();
            this.Host = host;
            this.port = port;
            try
            {
                this.socketStream = this.socket.GetStream();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }
        public void SendLine(String message)
        {
            this.socketStream.Write(System.Text.Encoding.UTF8.GetBytes(message + "\n"), 0, message.Length + 1);
        }

        public void LoadValues()
        {
            properties.ForEach(currentProperty => {
            SendLine("GET "+currentProperty.propertyName);
            var buffer = new byte[1024];
                for (int i = 0; i < 1023; i++)
                {
                    buffer[i] = 0;
                }
            bool running = true;
            // wait for data to be received
            int bytesRead;
            do
            {
            
                bytesRead = this.socketStream.Read(buffer, 0, buffer.Length);
                var r = System.Text.Encoding.UTF8.GetString(buffer);
                r.Split('\n');
                string[] array = r.Split('\n');
                for (int i = 0; i < array.Length; i++)
                {
                    string line = array[i];
                    if (line.StartsWith("OK", StringComparison.OrdinalIgnoreCase)) running = false;
                    else
                    {
                        try
                        {
                            PropertyClass current = new PropertyClass(line.Split(' ')[0])
                            {
                                propertyType = line.Split(' ')[1]
                            };
                            string range = line.Substring(line.IndexOf('[') + 1, line.IndexOf(']') - line.IndexOf('[') - 1);
                            current.ParseRange(range);
                            string value = line.Substring(line.IndexOf('=') + 1, line.Length - line.IndexOf('=') - 1);
                            current.ParseValue(value);
                            properties.Add(current);
                        }
                        catch (Exception e)
                        {
                            running = false;
                            Console.WriteLine(e.StackTrace);
                        }
                    }
                }
            } while (running);
            });
        }

        internal void SaveValues() { 
        
        }

        internal void resetDevice() {
        
        }
        internal void GetSpecs()
        {
            SendLine("LIST");
            var buffer = new byte[1024];
            for (int i = 0; i < 1023; i++)
            {
                buffer[i] = 0;
            }
            bool running = true;
            // wait for data to be received
            int bytesRead;
            do
            {
                bytesRead = this.socketStream.Read(buffer, 0, buffer.Length);
                var r = System.Text.Encoding.UTF8.GetString(buffer);
                r.Split('\n');
               string[] array = r.Split('\n');
                for (int i = 0; i < array.Length; i++)
                {
                    string line = array[i];
                    if (line.StartsWith("OK", StringComparison.OrdinalIgnoreCase)) running = false;
                    else
                    {
                        try
                        {
                            PropertyClass current = new PropertyClass(line.Split(' ')[0])
                            {
                                propertyType = line.Split(' ')[1]
                            };
                            string range = line.Substring(line.IndexOf('[')+1, line.IndexOf(']')-line.IndexOf('[')-1);
                            current.ParseRange(range);
                            string value = line.Substring(line.IndexOf('=')+1, line.Length - line.IndexOf('=') - 1);
                            current.ParseValue(value);
                            properties.Add(current);
                        }
                        catch (Exception e)
                        {
                            running = false;
                            Console.WriteLine(e.StackTrace);
                        }
                    }
                }
            } while (running);

        }

        public string ReadLine()
        {
            var buffer = new byte[1024];
            for (int i = 0; i<1023; i++) {
                buffer[i] = 0;
            }
            // wait for data to be received
            var bytesRead = this.socketStream.Read(buffer, 0, buffer.Length);
            var r = System.Text.Encoding.UTF8.GetString(buffer);
            return r;
        }
        public bool TestAnswer()
        {
            SendLine("OK");
            return ReadLine().StartsWith("OK", StringComparison.OrdinalIgnoreCase);
        }
        //return status bool of connection availability
        public bool GetStatus()
        {
            try
            {
                return !(socket.Client.Poll(1, SelectMode.SelectRead) && socket.Client.Available == 0);
            }
            catch (SocketException) { return false; }
        }

        public void Disconnect()
        {
            this.socket.Close();
        }

        public void Connect()
        {
            if (this.socket == null)
            {
                this.socket = new TcpClient();
            }
            if (this.socket.Connected)
            {
                this.socket.Close();
            }
            try
            {
                this.socket.Connect(this.Host, this.port);
                this.socketStream = this.socket.GetStream();


            }
            catch (Exception e)
            { Console.WriteLine(e.ToString()); }
        }
    }
}
