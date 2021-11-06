using System;
using System.IO;
namespace testSocketServer
{
    public class PropertyClass
    {
        public String Type;
        public Object Value;
        public Object MinValue;
        public Object MaxValue;
        public Object DefaultValue;
        public string name;
        private int CharCapacity = 0;
        public PropertyClass(string PropertyName)
        {
            this.name = PropertyName;
        }

        public void InitAsInt(Int64 minVal, Int64 maxVal, Int64 defVal)
        {
            this.Type = "int";
            this.Value = new Int64();
            this.MinValue = new Int64();
            this.MaxValue = new Int64();
            this.DefaultValue = new Int64();
            this.MinValue = minVal;
            this.MaxValue = maxVal;
            this.Value = this.DefaultValue = defVal;
        }
        public void InitAsFloat(float minVal, float maxVal, float defVal)
        {
            this.Type = "flt";
            this.Value = new float();
            this.MinValue = new float();
            this.MaxValue = new float();
            this.DefaultValue = new float();
            this.MinValue = minVal;
            this.MaxValue = maxVal;
            this.Value = this.DefaultValue = defVal;
        }
        public void InitAsString(int length, string defVal) {
            this.Type = "str";
            this.CharCapacity = length;
            this.Value = new String((char)0, length);
            this.Value = defVal;
        }


        public string toString()
        {
            switch (this.Type)
            {
                case "flt":
                case "int": return this.name + ' ' + this.Type + " [" + this.MinValue.ToString() + ':' + this.MaxValue.ToString() + "]=" + this.Value.ToString();
                case "str": return this.name + ' ' + this.Type + " ["+this.CharCapacity.ToString()+"]=" + this.Value.ToString();
                default: return "ERROR: Not init";
            }
        }

        internal void set(string value)
        {  
          switch (this.Type) {
                case "str": this.Value = value; break;
                case "int": this.Value = Int16.Parse(value); break;
                case "flt": this.Value = Double.Parse(value.Replace('.',',')); break;
            }
        }
    }
}
