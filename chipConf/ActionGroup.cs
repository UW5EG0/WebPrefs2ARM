using System;
namespace chipConf
{
    public partial class ActionGroup : Gtk.ActionGroup
    {
        public ActionGroup() :
                base("chipConf.ActionGroup")
        {
            this.Build();
        }
    }
}
