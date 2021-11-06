using System;
using Gtk;

namespace chipConf
{
    public class PropertyClass
    {
        public string propertyName;
        public string propertyType;
        public object value;
        public object defaultValue;
        public object minValue;
        public object maxValue;
        public object step;
        public int charCount;
        public Widget widget;

        public PropertyClass(string name) {
            this.propertyName = name;
        }

        public void SetType(string type) {
            this.propertyType = type;
            if (this.propertyType == "int") { this.step = (int)1; }
            if (this.propertyType == "flt") { this.step = (double) 1.0; }

        }

        internal void ParseValue(string newValue)
        {
            switch (this.propertyType)
            {
                case "int":
                    this.value = Int64.Parse(newValue);
                    if ((Int64)this.value > (Int64)this.maxValue) this.value = this.maxValue;
                    if ((Int64)this.value < (Int64)this.minValue) this.value = this.minValue;
                    break;
                case "flt":
                    this.value = Double.Parse(newValue);
                    if ((double)this.value > (double)this.maxValue) this.value = this.maxValue;
                    if ((double)this.value < (double)this.minValue) this.value = this.minValue;
                    break;
                case "str":
                    this.value = newValue.Substring(0, (newValue.Length > this.charCount + 1) ? (this.charCount) : (newValue.Length));
                    break;
                default:
                    this.value = newValue.Substring(0, (newValue.Length > this.charCount+1)?(this.charCount): (newValue.Length));
                    break;

            }
        }
        internal void ParseRange(string range)
        {
            var indexes = range.Split(':');
            switch (this.propertyType) {
                case "int": 
                    minValue = Int64.Parse(indexes[0]);
                    maxValue = Int64.Parse(indexes[1]);
                    step = 1;
                    break;
                case "flt": 
                    minValue = Double.Parse(indexes[0]);
                    maxValue = Double.Parse(indexes[1]);
                    if (indexes.Length == 3)
                        step = Double.Parse(indexes[2]);
                    else step = 0.0001;
                    break;
                case "str":
                    charCount = UInt16.Parse(indexes[0]);
                    break;
                default:
                    charCount = 256; break;
            }
        }

        internal void MakeWidget()
        {
            if (this.propertyType != "str")
            {
            
            digitSpecWidget tempWidget = new chipConf.digitSpecWidget();
                tempWidget.setName(this.propertyName);
                tempWidget.setValue(this.ToString());
                tempWidget.setMinValue(this.minValue);
                tempWidget.setMaxValue(this.maxValue);
                tempWidget.setStep(this.step);
                tempWidget.SetPointer(this);

                this.widget = tempWidget;
            } else {
                textSpecWidget tempWidget = new chipConf.textSpecWidget();
                tempWidget.setName(this.propertyName);
                tempWidget.SetPointer(this);
                tempWidget.setValue(this.ToString());
                tempWidget.setCharsCount(this.charCount);
                tempWidget.SetPointer(this);

                this.widget = tempWidget;
            }

            this.widget.Realize();
            this.widget.ShowAll();
        }

        override
        public string ToString() {
        if (this.value is string) {
                return (string) this.value;
        } else {
                return value.ToString();
        }
        }
    }
}
