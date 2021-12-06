using System;
namespace chipConf
{
    [System.ComponentModel.ToolboxItem(true)]
    public partial class textSpecWidget : Gtk.Bin
    {
        public string SpecName;
        public int charCount;
        public PropertyClass property;
        public string SpecValue;
        public textSpecWidget() => Build();

        public void setName(string newName)
        {
            this.SpecName = newName;
            this.labelName.Text = this.SpecName.Replace('_', ' ');
        }

        internal
        void setPointer(PropertyClass propertyClass)
        {
            this.property = propertyClass;

        }
        internal void setValue(string v)
        {
            this.SpecValue = v;
            this.entryValue.Text = this.SpecValue;
        }

        internal void setCharsCount(int newCharCount)
        {
            this.entryValue.MaxLength = newCharCount;
            this.charCount = newCharCount;
            this.EntryValueChanged(this, null);
        }


        protected void EntryValueChanged(object sender, EventArgs e)
        {
           try {
                this.SpecValue = this.entryValue.Text;
                this.property.ParseValue(this.SpecValue);
                this.property.isChanged = true;
               } catch (Exception ex) {
                this.property.isChanged = false;
                PropertiesSocket.Alert(ex.Message);
               }

            this.labelCharCounter.Text = "Chars: "+this.entryValue.Text.Length.ToString()+"/" + this.charCount.ToString();
        }
    }
}
