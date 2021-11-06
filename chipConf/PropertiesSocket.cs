using System;
using System.Collections.Generic;
using System.Net.Sockets;
using Gtk;

namespace chipConf
{

    public class PropertiesSocket
    {
        private string host;
        private UInt16 port;
        private TcpClient socket;
        private NetworkStream socketStream;
        public List<PropertyClass> properties = new List<PropertyClass>();
        public string Host { get => host; set => host = value; }
        public ushort Port { get => port; set => port = value; }

        public PropertiesSocket(string host, UInt16 port)
        {
            try { 
            this.socket = new TcpClient();
            this.Host = host;
            this.port = port;
            this.socketStream = null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }
        /*
        Проверка на наличие ответа.
        */
        internal bool TestAnswer()
        {
            SendLine("OK");
            return ReadLine().Equals("OK", StringComparison.OrdinalIgnoreCase);

        }
        /*
        Читать один буфер до переноса на новую строку или окончания чтения
        */
        private string ReadLine()
        {
            string answer = "";
            try
            {
                int currentByte = -1;
                do
                {
                    if (currentByte != -1 && currentByte != '\n')
                    {
                        byte[] bytes = new byte[2];
                        bytes[0] = (byte) (currentByte & 0xFF);
                        bytes[1] = (byte) (currentByte >> 8); 
                        answer += BitConverter.ToChar(bytes, 0);
                    }
                    currentByte = this.socketStream.ReadByte();
                }
                while (currentByte != -1 && currentByte != '\n');
            } catch (Exception e) {
               alert(e.ToString() + '\n' + e.StackTrace);
            }
            return answer;

        }

        internal bool GetStatus()
        {
            try
            {
                return (socket.Client.Poll(1, SelectMode.SelectRead) && socket.Client.Available == 0);
            }
            catch (SocketException) { return false; }
        }

        internal void GetSpecs()
        {
           properties.Clear();
           SendLine("LIST");
            string line = "";
            do
            {
                line = ReadLine();
                if (!line.Equals("ok", StringComparison.OrdinalIgnoreCase)) {
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
                        alert(e.Message+"\nline:"+line+"\n"+ e.StackTrace);
                        line = "ok";
                    }
                }
            }
            while (!line.Equals("ok", StringComparison.OrdinalIgnoreCase));
        }

        internal void LoadValues()
        {
            properties.ForEach(currentProperty =>
            {
                SendLine("GET " + currentProperty.propertyName);
                string line = "";
                do
                {
                    line = ReadLine();
                    if (!line.Equals("ok", StringComparison.OrdinalIgnoreCase))
                    {
                        try
                        {
                            currentProperty.ParseValue(line);
                        }
                        catch (Exception e)
                        {
                            alert(e.Message + "\nline:" + line + "\n" + e.StackTrace);
                        }
                    }
                }
                while (!line.Equals("ok", StringComparison.OrdinalIgnoreCase));
            });
        }

           public static void alert(string message) {
            MessageDialog md = new Gtk.MessageDialog(null, DialogFlags.Modal,
                                                       MessageType.Warning,
                                                       ButtonsType.Ok,
                                                       "Alert!");
            md.SecondaryText = message;
            md.Run();
            md.Destroy();
        }
   
        internal void SaveValues()
        {
            string line;
            properties.ForEach(currentProperty =>
            {
                SendLine("SET " + currentProperty.propertyName + "=" +currentProperty.ToString());
                do {
                    line = ReadLine();
                } while (!line.Equals("ok", StringComparison.OrdinalIgnoreCase));


            });
        }

        internal void ResetDevice()
        {
            SendLine("RESET");
            this.Disconnect();
            this.Connect();

        }

        internal void RebootDevice()
        {
            SendLine("REBOOT");
        }


        internal void Disconnect()
        {
            this.socket.Close();
        }

        internal bool Connect()
        {
            bool answer = false;
            try
            {
                if(this.socket != null)
                this.socket.Close();
                this.socket = new TcpClient();
                this.socket.Connect(this.Host, this.port);
                this.socketStream = this.socket.GetStream();
                if (this.socket.Connected)
                {
                    this.socketStream = this.socket.GetStream();
                }
                answer = true;
            }
            catch (SocketException e)
            { alert(e.Message); }
            return answer;
        }

        public void SendLine(String message)
        {
            this.socketStream.Write(System.Text.Encoding.UTF8.GetBytes(message + "\n"), 0, message.Length + 1);
        }

    }
}
