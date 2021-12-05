using System;
namespace chipConf
{
    public partial class LogWindow : Gtk.Window
    {
        private PropertiesSocket propertiesSocket;
        public LogWindow() :
                base(Gtk.WindowType.Toplevel)
        {
            this.Build();
            this.textview3.SizeAllocated += HandleSizeAllocatedHandler;
        }

        void HandleSizeAllocatedHandler(object o, Gtk.SizeAllocatedArgs args)
        {
            textview3.ScrollToIter(textview3.Buffer.EndIter, 0, false, 0, 0);
        }


        internal void AppendTransmitted(string v)
        {
            // throw new NotImplementedException();
            this.textview3.Buffer.Text += "TX>" + v + "<ETX\n";
        }

        internal void AttachSocket(PropertiesSocket configConnection)
        {
            this.propertiesSocket = configConnection;
        }

        internal void AppendToLog(string v)
        {
            // throw new NotImplementedException();
            this.textview3.Buffer.Text += "LOG>" + v + "<ELOG\n";
        }

        internal void AppendReceived(string answer)
        {
            this.textview3.Buffer.Text += "RX>" + answer + "<ERX\n";
        }

        internal void AppendToERR(string line)
        {
            this.textview3.Buffer.Text += "\nERROR:" + line + "\n";
        }

        protected void EventSendCommand(object sender, EventArgs e)
        {
            String ans = "(empty)";
            try
            {
                ans = this.propertiesSocket.ReadWrite(entryCommand.Text);
                entryCommand.Text = "";
            }
            catch (Exception ex)
            {
                this.AppendToERR(ex.Message + "\nans=" + ans + "\n" + ex.StackTrace);
            }
        }

        protected void OnEnter(object o, Gtk.KeyPressEventArgs args)
        {
            this.AppendToLog("key="+args.Event.Key.ToString());
            if (args.Event.Key == Gdk.Key.Return)
            {
                EventSendCommand(this, null);
            }
        }

        protected void EventOnDestroy(object o, Gtk.DestroyEventArgs args)
        {
            this.Visible = false;
        }

        protected void EventClearLog(object sender, EventArgs e)
        {
            this.textview3.Buffer.Text = "";
        }
    }
}
