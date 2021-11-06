using System;
namespace chipConf
{
    [System.ComponentModel.ToolboxItem(true)]
    public partial class textSpecsWidget : Gtk.Bin
    {
        private string name = "";
        private int charCount = 0;

        public textSpecsWidget(string propertyName, int charCount, string startValue)
        {
            Build();
            this.name = propertyName;
            this.charCount = charCount;
            this.entryValue.Text = startValue;
            this.entryValue.MaxLength = this.charCount;
            this.labelName.Text = this.name.Replace('_', ' ');

        }
        private void updateData()
        {
            this.labelName.Text = this.name.Replace('_', ' ');
        }

        protected void EntryValueChanged(object sender, EventArgs e)
        {
            this.labelHint.Text = "Chars: " + this.entryValue.Text.Length + '/' + this.entryValue.MaxLength;
        }
    }

}
