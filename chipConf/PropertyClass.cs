using System;
using System.Globalization;
using Gtk;

namespace chipConf
{
    public class PropertyClass
    {
        public string propertyName;
        public string propertyType;
        public bool isChanged;
        public object value;
        public object defaultValue;
        public object minValue;
        public object maxValue;
        public object step;
        public UInt16 charCount;
        public Widget widget;
        private LogWindow log;

        public PropertyClass(string name) {
            this.propertyName = name;
            this.isChanged = false;
        }


        public void SetType(string type) {
            this.propertyType = type;
            this.step = (double) 1.0;
            switch (this.propertyType)
            {
                case "int":
                    this.value = new Int64();
                    this.minValue = new Int64();
                    this.maxValue = new Int64();
                    this.step = new Int64();
                    break;
                case "flt":
                    this.value = new Double();
                    this.minValue = new Double();
                    this.maxValue = new Double();
                    this.step = new Double();
                    break;
                default:
                    this.value = "";
                    break;
            }
                
        }
        /*Задолбало - штатная функция при классе требует ей подтереться
        Считываем знакоместа и рассчитываем число
        */
        public Int64 ParseInt(String input) {
            Int64 result = 0;
            int digitLength = input.Length;
            //от наименьшего до наибольшего знакоместа
            for (int idx = input.Length - 1; idx >= 0; idx--) { 
            if (input.ToCharArray()[idx] != '-') {
                    if (input.ToCharArray()[idx] >= '0' && input.ToCharArray()[idx] <= '9') {
                        int Num = (int)(input.ToCharArray()[idx] - '0');
                        result += Num * (long) Math.Round(Math.Pow(10, digitLength - idx- 1));
                    }
                }
            }
            return (input.ToCharArray()[0] != '-')? result : -result;
        }

        public Double ParseDouble(String input)
        {
            Int64 intMantiss = 0;
            int dotPos = input.Length - input.Replace(',','.').IndexOf('.') - 1; // положение десятичного разделителя ( 
            String mantiss = input.Replace(".", "").Replace("-","");
            for (uint idx = 0; idx < mantiss.Length; idx++)
            {
                if (mantiss.ToCharArray()[idx] != '-')
                {
                    if (mantiss.ToCharArray()[idx] >= '0' && mantiss.ToCharArray()[idx] <= '9')
                    {
                        Int64 Num = (Int64)(mantiss.ToCharArray()[idx] - '0');
                        intMantiss = intMantiss * 10 + Num;
                    }
                }
            }
            intMantiss = (input.ToCharArray()[0] != '-') ? intMantiss : -intMantiss;
         //   log.AppendToLog(input + "=>" + intMantiss + "E" + (-dotPos).ToString());


            return intMantiss * Math.Pow(10, -dotPos);
            }

        internal void ParseValue(string newValue)
        {
          //  PropertiesSocket.Alert(newValue);
            switch (this.propertyType)
            {
                case "int":
                    this.value = ParseInt(newValue);
                    Int64 tempVal = (Int64) this.value;
                    Int64 tempMinVal = (Int64)this.minValue;
                    Int64 tempMaxVal = (Int64)this.maxValue;

                    if (tempVal > tempMaxVal) this.value = this.maxValue;
                    if (tempVal < tempMinVal) this.value = this.minValue;
                    break;
                case "flt":
                    this.value = ParseDouble(newValue);
                    if ((double)this.value > (double)this.maxValue) this.value = this.maxValue;
                    if ((double)this.value < (double)this.minValue) this.value = this.minValue;
                    break;
                case "str":
                    if ((newValue.Length > this.charCount + 1)) {
                        this.value = (string) newValue.Substring(0, this.charCount);
                    } else {
                        this.value = (string) newValue;
                    }

                    break;
                default:
                    if (newValue.Split('\n').Length!=1) {
                        newValue = newValue.Split('\n')[0];
                    }
                    if ((newValue.Length > this.charCount + 1))
                    {
                        this.value = (string) newValue.Substring(0, this.charCount);
                    }
                    else
                    {
                        this.value = (string) newValue;
                    }

                    break;

            }
        }
        internal void ParseRange(string range)
        {
            var indexes = range.Split(':');
            switch (this.propertyType) {
                case "int":
               try
                    {
                     //   log.appendToLog("Property " + propertyName +" from "+ range+" parse minval");
                        this.minValue = ParseInt(indexes[0]);
                     //   log.appendToLog(indexes[0] + " converted to "+ this.minValue.ToString());
                    }
                    catch (Exception e){
                        PropertiesSocket.Alert(indexes[0]+'\n'+e.Message);
                        }
                    try {
                      //  log.appendToLog("Property " + propertyName + " from " + range + " parse maxval");
                        this.maxValue = ParseInt(indexes[1]);
                      //  log.appendToLog(indexes[1] + " converted to " + this.maxValue.ToString());
                    }
                    catch (Exception e) {
                        PropertiesSocket.Alert(indexes[1]+'\n'+e.Message);
                    }
                    step = 1;
                    break;
                case "flt":
                   
                    try
                    {
                     //   log.appendToLog("Property " + propertyName + " from " + range + " parse minval");
                        this.minValue = ParseDouble(indexes[0]);
                     //  log.appendToLog(indexes[0] + " converted to " + this.minValue.ToString());
                    }
                    catch (Exception e)
                    {
                        PropertiesSocket.Alert(indexes[0] + '\n' + e.Message);
                    }
                    try
                    {
                     //   log.appendToLog("Property " + propertyName + " from " + range + " parse maxval");
                        this.maxValue = ParseDouble(indexes[1]);
                     //   log.appendToLog(indexes[1] + " converted to " + this.maxValue.ToString());
                    }
                    catch (Exception e)
                    {
                        PropertiesSocket.Alert(indexes[1] + '\n' + e.Message);
                    }

                    if (indexes.Length == 3)
                        try
                        {
                          //  log.appendToLog("Property " + propertyName + " from " + range + " parse step");
                            this.step = ParseDouble(indexes[2]);
                          // log.appendToLog(indexes[2] + " converted to " + this.step.ToString());
                        }
                        catch (Exception e)
                        {
                            PropertiesSocket.Alert(indexes[2] + '\n' + e.Message);
                        }
                    else step = 0.0001;
                    break;
                case "str":
                    try
                    {
                        charCount = 256; 
                        UInt16.TryParse(indexes[0], NumberStyles.Integer , CultureInfo.InvariantCulture, out charCount);
                    }
                    catch (Exception e)
                    {
                        PropertiesSocket.Alert(indexes[0] + '\n' + e.Message);
                    }
                    break;
                default:
                    charCount = 256; break;
            }
        }

        internal void linkToLog(LogWindow logwnd)
        {
            this.log = logwnd;
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
                tempWidget.setPointer(this);
                tempWidget.setValue(this.ToString());
                tempWidget.setCharsCount(this.charCount);
                tempWidget.setPointer(this);

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
                return value.ToString().Replace(',','.');
        }
        }
    }
}
