using System;
using Gdk;

namespace chipConf
{
    [System.ComponentModel.ToolboxItem(true)]
    public partial class digitSpecWidget : Gtk.Bin
    {
        public string SpecName;
        private bool isInteger;
        public object SpecValue;
        public object SpecMinValue;
        public object SpecMaxValue;
        public object SpecStepValue;
        public PropertyClass property;
        public digitSpecWidget()
        {
            this.Build();
        }

        public void setMode(string propertyType)
        {
            if (propertyType == "int")
            {
                this.isInteger = true;
                this.SpecValue = new Int64();
                this.SpecMinValue = new Int64();
                this.SpecMaxValue = new Int64();
            }

            if (propertyType == "flt")
            {
                this.isInteger = false;
                this.SpecValue = new Double();
                this.SpecMinValue = new Double();
                this.SpecMaxValue = new Double();
            }
        }

        public void setName(string newName)
        {
            this.SpecName = newName;
            this.labelName.Text = this.SpecName.Replace('_', ' ');
        }

        protected void EntryValueChanged(object sender, EventArgs e)
        {
            try {
                this.SpecValue = (this.isInteger) ? (Int64.Parse(this.entryValue.Text)) : (Double.Parse(this.entryValue.Text));
                this.property.ParseValue(this.SpecValue.ToString());
                this.entryValue.Text = this.SpecValue.ToString();
                Gdk.Color clr = new Gdk.Color();
                Gdk.Color.Parse("white", ref clr);
                this.entryValue.ModifyBg(Gtk.StateType.Normal, clr);
                ValueToSlider();
            } catch (Exception ex) {
                Gdk.Color clr = new Gdk.Color();
                Gdk.Color.Parse("red", ref clr);
                this.entryValue.ModifyBg(Gtk.StateType.Normal, clr);
            }
        }
        internal void ValueToSlider()
        {
            Double up = (this.isInteger) ? ((Int64)this.SpecValue) : ((Double)this.SpecValue);
            up -= (this.isInteger) ? ((Int64)this.SpecMinValue) : ((Double)this.SpecMinValue);
            Double dn = (this.isInteger) ? ((Int64)this.SpecMaxValue) : ((Double)this.SpecMaxValue);
            dn -= (this.isInteger) ? ((Int64)this.SpecMinValue) : ((Double)this.SpecMinValue);
            this.hscrollbar3.Adjustment.Value = up *100.0 / dn;

        }
        internal void sliderToValue() {
            Double min = (this.isInteger) ? ((Int64)this.SpecMinValue) : ((Double)this.SpecMinValue);
            Double range = (this.isInteger) ? ((Int64)this.SpecMaxValue) : ((Double)this.SpecMaxValue);
            range -= (this.isInteger) ? ((Int64)this.SpecMinValue) : ((Double)this.SpecMinValue);
            Double val = min + range * this.hscrollbar3.Adjustment.Value/100;
            Double step = (this.isInteger) ? ((Int64)this.SpecStepValue) : ((Double)this.SpecStepValue);
            this.SpecValue = (this.isInteger)?(Math.Round(val)):(Math.Round(val/step)*step);
            this.entryValue.Text = this.SpecValue.ToString();
        }

        internal void setValue(string v)
        {
            try
            {
                this.SpecValue = (this.isInteger)?(Int64.Parse(v)):(Double.Parse(v));
                this.entryValue.Text = this.SpecValue.ToString();
                this.ValueToSlider();
            }
            catch (Exception ex)
            {

            }
        }
        internal
        void SetPointer(PropertyClass propertyClass)
        {
            property = propertyClass;
        }

        internal void setStep(object step)
        {
            try
            {
                this.SpecStepValue = (this.isInteger) ? (Int64.Parse(step.ToString())) : (Double.Parse(step.ToString()));
                this.ValueToSlider();
            }
            catch (Exception ex) { 
            
            }
        }

        internal void setMinValue(object minValue)
        {
            this.SpecMinValue = (this.isInteger) ? (Int64.Parse(minValue.ToString())) : (Double.Parse(minValue.ToString()));
            labelMinValue.Text = this.SpecMinValue.ToString();

        }

        internal void setMaxValue(object maxValue)
        {
            this.SpecMaxValue = (this.isInteger) ? (Int64.Parse(maxValue.ToString())) : (Double.Parse(maxValue.ToString()));
            labelMaxValue.Text = this.SpecMaxValue.ToString();
            }

        protected void EventScrollBarDND(object o, Gtk.ChangeValueArgs args)
        {
            this.sliderToValue();
        }

        protected void eventDecrement(object sender, EventArgs e)
        {
            SpecValue = isInteger ? (long.Parse(SpecValue.ToString()) - long.Parse(SpecStepValue.ToString())) : (double.Parse(SpecValue.ToString()) - double.Parse(SpecStepValue.ToString()));
            if (double.Parse(SpecValue.ToString()) < double.Parse(SpecMinValue.ToString())) {
                setValue(SpecMinValue.ToString());
            }
            this.entryValue.Text = this.SpecValue.ToString();

            this.ValueToSlider();
        }

        protected void EventIncrement(object sender, EventArgs e)
        {
            SpecValue = isInteger ? (long.Parse(SpecValue.ToString()) + long.Parse(SpecStepValue.ToString())) : (double.Parse(SpecValue.ToString()) + double.Parse(SpecStepValue.ToString()));
            if (double.Parse(SpecValue.ToString()) > double.Parse(SpecMaxValue.ToString()))
            {
                setValue(SpecMaxValue.ToString());
            }

            this.entryValue.Text = this.SpecValue.ToString(); 
            this.ValueToSlider();
        }
        override
        public string ToString()
        {
            return this.SpecName.ToString();
        }
    }
}