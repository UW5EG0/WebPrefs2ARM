using System;
using chipConf;
using Gtk;
public partial class MainWindow : Gtk.Window
{
    private LogWindow log;
    public MainWindow() : base(Gtk.WindowType.Toplevel)
    {
        Build();
    }

    protected void OnDeleteEvent(object sender, DeleteEventArgs a)
    {
        Application.Quit();
        a.RetVal = true;
    }

    protected void EventButtonConnectClicked(object sender, EventArgs e)
    {
        this.labelStatus.LabelProp = "Connecting";
        chipConf.MainClass.configConnection.Host = entryHost.Text;
        chipConf.MainClass.configConnection.Port = (ushort)portSelector.ValueAsInt;
        if (chipConf.MainClass.configConnection.Connect()) {
            this.labelStatus.LabelProp = "Connected";
            if (chipConf.MainClass.configConnection.TestAnswer())
            { 
                ActivateButtons(true);
                EventReadSpecsClicked(sender, e);
            }
            else {
                chipConf.MainClass.configConnection.Disconnect();
                this.labelStatus.LabelProp = "Test Answer failed";

                ActivateButtons(false);
            }
        } else {
            this.labelStatus.LabelProp = "Failed";
            ActivateButtons(false);

        }
    }

    internal bool ForceDisconnect()
    {
        OnButtonDisconnectClicked(this, null);
        return true;
    }

    internal void attachLogWindowToggler(LogWindow logScreen)
    {
        this.log = logScreen; 
    }

    protected void ActivateButtons(bool activate) {
        buttonConnect.Sensitive = !activate;
        buttonDisconnect.Sensitive = activate;
        buttonReadSpecs.Sensitive = activate;
        buttonWriteSpecs.Sensitive = activate;
        buttonRestartDevice.Sensitive = activate;
        buttonReset.Sensitive = activate;
    }

    protected void OnButtonDisconnectClicked(object sender, EventArgs e)
    {
        foreach (chipConf.PropertyClass property in chipConf.MainClass.configConnection.properties)
        {
            property.widget.Destroy();
        }
        while (table4.Children.Length > 0)
        {
            table4.Remove(table4.Children[0]);
        }

        chipConf.MainClass.configConnection.properties.Clear();
        chipConf.MainClass.configConnection.Disconnect();
        this.labelStatus.LabelProp = "Disconnected";
        ActivateButtons(false);

    }

    protected void EventReadSpecsClicked(object sender, EventArgs e)
    {
        try
        {
            this.labelStatus.LabelProp = "Get specs";
            chipConf.MainClass.configConnection.GetSpecs();
            this.labelStatus.LabelProp = "Load values";
            chipConf.MainClass.configConnection.LoadValues();
            this.labelStatus.LabelProp = "Create stickers";

            while (table4.Children.Length > 0) {
                table4.Remove(table4.Children[0]);
            }
            uint i = 0;
            foreach (chipConf.PropertyClass property in chipConf.MainClass.configConnection.properties)
            {
                property.MakeWidget();
                table4.Attach(property.widget, i % 3, i % 3 + 1, i / 3, i / 3 + 1);
                i++;
            }
            this.labelStatus.LabelProp = "Ready";

        }
        catch (Exception ex) {
            alert(ex.Message + "\n" + ex.StackTrace);
        }
    }
    public static void alert(string message)
    {
        MessageDialog md = new Gtk.MessageDialog(null, DialogFlags.Modal,
                                                   MessageType.Warning,
                                                   ButtonsType.Ok,
                                                   "Alert!")
        {
            SecondaryText = message
        };
        md.Run();
        md.Destroy();
    }

    protected void EventWriteSpecsClicked(object sender, EventArgs e)
    {
     
         chipConf.MainClass.configConnection.SaveValues();
     }

    protected void EventResetDeviceClicked(object sender, EventArgs e)
    {
        chipConf.MainClass.configConnection.ResetDevice();
        EventReadSpecsClicked(sender, e);
    }

    protected void EventRebootDeviceClicked(object sender, EventArgs e)
    {
        chipConf.MainClass.configConnection.RebootDevice();
        while (table4.Children.Length > 0)
        {
            table4.Remove(table4.Children[0]);
        }
        chipConf.MainClass.configConnection.Disconnect();
        ActivateButtons(false);
    }

    protected void eventLogDisplayingToggle(object sender, EventArgs e)
    {
        log.Visible = !log.Visible;
    }
}
