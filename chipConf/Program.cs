using System;
using Gtk;

namespace chipConf
{
    class MainClass
    {
        public static PropertiesSocket
         configConnection;
        public static void Main(string[] args)
        {
            Application.Init();
            MainWindow win = new MainWindow();
            configConnection = new PropertiesSocket("127.0.0.1", 8080);
            win.Show();
            Application.Run();
        }
    }


}
