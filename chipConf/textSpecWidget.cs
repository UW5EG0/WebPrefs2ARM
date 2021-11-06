using System;
namespace chipConf
{
    [System.ComponentModel.ToolboxItem(true)]
    public partial class textSpecWidget : Gtk.Bin
    {
        public string SpecName;
        public int charCount;
        public PropertyClass property;

        public textSpecWidget()
        {
            this.Build();
        }
        public void setName(string newName)
        {
            this.SpecName = newName;
            this.labelName.Text = this.SpecName.Replace('_', ' ');
        }

        internal
        void SetPointer(PropertyClass propertyClass)
        {
            this.property = propertyClass;

        }
        internal void setValue(string v)
        {
            this.entryValue.Text = v;
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
               this.property.ParseValue(this.entryValue.Text); 
               } catch (Exception ex) {
                PropertiesSocket.alert(ex.Message);
               }

            this.labelCharCounter.Text = "Chars: "+this.entryValue.Text.Length.ToString()+"/" + this.charCount.ToString();
        }
    }
}
