using System;
using System.Collections.Generic;
using System.IO;
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
        private LogWindow log;
        private Func<bool> EventDisconnectHandler;
        public List<PropertyClass> properties = new List<PropertyClass>();
        public string Host { get => host; set => host = value; }

        internal void SetLogWindow(LogWindow logScreen)
        {
            this.log = logScreen;
        }

        internal void AttachWindowEventOnDisconnect(Func<bool> forceDisconnect)
        {
            EventDisconnectHandler = forceDisconnect;

        }

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
            return ReadWrite("OK").Equals("OK", StringComparison.OrdinalIgnoreCase);
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
                        if ((currentByte != -1) && currentByte != 0 && currentByte != '\n' && currentByte != '\r')
                        {
                            byte[] bytes = new byte[2];
                            bytes[0] = (byte) (currentByte & 0xFF);
                            bytes[1] = (byte) (currentByte >> 8); 
                            answer += BitConverter.ToChar(bytes, 0);
                        }
                        currentByte = this.socketStream.ReadByte();
                    }
                    while (currentByte != -1 && currentByte != 0 && currentByte != '\n' && currentByte != '\r');
                 } catch (Exception e) {
                   Alert(e.ToString() + '\n' + e.StackTrace);
                }
            log.AppendReceived(answer);
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

        internal string ReadWrite(String command)
        {
            String answer = "";
            log.AppendTransmitted(command + "\n");
            //this.socketStream;
            this.socketStream.Write(System.Text.Encoding.UTF8.GetBytes(command + "\n"), 0, command.Length + 1);
             try
            {
                int currentByte = -1;
                do
                {
                    if ((currentByte != -1) && currentByte != 0 && currentByte != '\r')
                    {
                        byte[] bytes = new byte[2];
                        bytes[0] = (byte)(currentByte & 0xFF);
                        bytes[1] = (byte)(currentByte >> 8);
                        answer += BitConverter.ToChar(bytes, 0);
                    }

                    currentByte = this.socketStream.ReadByte();
                }
                while (!answer.EndsWith("OK", StringComparison.InvariantCultureIgnoreCase));
                //while (currentByte != -1 && currentByte != 0 && currentByte != '\n' && currentByte != '\r');
            }
            catch (Exception e)
            {
                Alert(e.ToString() + '\n' + e.StackTrace);
            }
            log.AppendReceived(answer);
            return answer;
        }

        internal void GetSpecs()
        {
            properties.Clear();
            String ans = ReadWrite("LIST");
            String[] lines = ans.Split('\n');
            foreach (String line in lines)
            {
                if (line != "OK" && line.Length > 0 && line.Split(' ').Length >= 2)
                {
                    log.AppendToLog(line);
                    try
                    {
                           String var_name = line.Split(' ')[0];
                            String type = line.Split(' ')[1];
                            string value = line.Substring(line.IndexOf('=') + 1, line.Length - line.IndexOf('=')-1);
                            string range = line.Substring(line.IndexOf('[') + 1, line.IndexOf(']') - line.IndexOf('[') - 1);
                            log.AppendToLog("varName:" + var_name);
                            log.AppendToLog("type:" + type);
                            log.AppendToLog("value:" + value);
                            log.AppendToLog("range:" + range);

                            PropertyClass current = new PropertyClass(var_name)
                            {
                                propertyType = type
                            };
                            current.linkToLog(log);
                            current.ParseRange(range);
                            current.ParseValue(value);
                            properties.Add(current);
                    }
                    catch (Exception e)
                    {
                        log.AppendToERR(e.Message + "\non [" + line + "]\n" + "trace" + e.StackTrace);
                        Alert(e.Message + "\nline:" + line + "\n" + e.StackTrace);
                    }
                }
            }
        }
       

        internal void LoadValues()
        {
            properties.ForEach(currentProperty =>
            {
                string[] lines = ReadWrite("GET " + currentProperty.propertyName).Split('\n');
                foreach (String line in lines)
                {
                    if (line != "OK" && line.Length > 0)
                    {
                        currentProperty.ParseValue(line);
                    }
                }
            });
        }

           public static void Alert(string message) {
            MessageDialog md = new MessageDialog(null, flags: DialogFlags.Modal,
                                                       type: MessageType.Warning,
                                                       bt: ButtonsType.Ok,
                                                       format: "Alert!")
            {
                SecondaryText = message
            };
            md.Run();
            md.Destroy();
        }
   
        internal void SaveValues()
        {
            properties.ForEach(currentProperty =>
            {
                if (currentProperty.isChanged == true)
                {
                    string[] lines = ReadWrite("SET " + currentProperty.propertyName + "=" + currentProperty.ToString()).Split('\n');
                    foreach (String line in lines)
                    {
                        if (line != "OK" && line.Length > 0)
                        {
                            currentProperty.ParseValue(line);
                        }
                    }
                }
            });
           }

        internal void ResetDevice()
        {
            string lines = ReadWrite("RUN RESET");
            this.Disconnect();
            }

        internal void RebootDevice()
        {
            string lines = ReadWrite("RUN REBOOT");
            this.Disconnect();
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
            {
             log.AppendToERR(e.Message);
             Alert(e.Message); 
             }
            return answer;
        }
    }
}
