#define LOGGING_INNER_FUNCTIONS

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
            LogWindow logScreen = new LogWindow();
            configConnection = new PropertiesSocket("10.10.10.104", 3324);
            configConnection.SetLogWindow(logScreen);
            configConnection.AttachWindowEventOnDisconnect(win.ForceDisconnect);
            logScreen.AttachSocket(configConnection);
            win.attachLogWindowToggler(logScreen);

            win.Show();
            Application.Run();
        }
    }


}
