using System;
using Gtk;
public partial class MainWindow : Gtk.Window
{

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
            };
        } else {
            this.labelStatus.LabelProp = "Failed";
            ActivateButtons(false);

        }
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
                table4.Attach(property.widget, i % 3, i%3+1, i/3, i / 3+1);
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
                                                   "Alert!");
        md.SecondaryText = message;
        md.Run();
        md.Destroy();
    }

    protected void EventWriteSpecsClicked(object sender, EventArgs e)
    {
        foreach (chipConf.PropertyClass property in chipConf.MainClass.configConnection.properties)
        {
            chipConf.MainClass.configConnection.SaveValues();
        }
    }

    protected void EventResetDeviceClicked(object sender, EventArgs e)
    {
        chipConf.MainClass.configConnection.ResetDevice();
        EventReadSpecsClicked(this, null);
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
}
