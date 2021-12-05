
// This file has been generated by the GUI designer. Do not modify.
namespace chipConf
{
	public partial class digitSpecWidget
	{
		private global::Gtk.VBox vbox1;

		private global::Gtk.Label labelName;

		private global::Gtk.HBox hbox1;

		private global::Gtk.Button buttonDec;

		private global::Gtk.Entry entryValue;

		private global::Gtk.Button buttonInc;

		private global::Gtk.HButtonBox hbuttonbox1;

		private global::Gtk.HBox hbox2;

		private global::Gtk.Label labelMinValue;

		private global::Gtk.HScrollbar hscrollbar3;

		private global::Gtk.Label labelMaxValue;

		protected virtual void Build()
		{
			global::Stetic.Gui.Initialize(this);
			// Widget chipConf.digitSpecWidget
			global::Stetic.BinContainer.Attach(this);
			this.Name = "chipConf.digitSpecWidget";
			// Container child chipConf.digitSpecWidget.Gtk.Container+ContainerChild
			this.vbox1 = new global::Gtk.VBox();
			this.vbox1.Name = "vbox1";
			this.vbox1.Spacing = 6;
			// Container child vbox1.Gtk.Box+BoxChild
			this.labelName = new global::Gtk.Label();
			this.labelName.Name = "labelName";
			this.labelName.LabelProp = global::Mono.Unix.Catalog.GetString("Name");
			this.vbox1.Add(this.labelName);
			global::Gtk.Box.BoxChild w1 = ((global::Gtk.Box.BoxChild)(this.vbox1[this.labelName]));
			w1.Position = 0;
			w1.Expand = false;
			w1.Fill = false;
			// Container child vbox1.Gtk.Box+BoxChild
			this.hbox1 = new global::Gtk.HBox();
			this.hbox1.Spacing = 6;
			// Container child hbox1.Gtk.Box+BoxChild
			this.buttonDec = new global::Gtk.Button();
			this.buttonDec.CanFocus = true;
			this.buttonDec.Events = ((global::Gdk.EventMask)(256));
			this.buttonDec.Name = "buttonDec";
			this.buttonDec.UseUnderline = true;
			global::Gtk.Image w2 = new global::Gtk.Image();
			w2.Pixbuf = global::Stetic.IconLoader.LoadIcon(this, "gtk-remove", global::Gtk.IconSize.Menu);
			this.buttonDec.Image = w2;
			this.hbox1.Add(this.buttonDec);
			global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.hbox1[this.buttonDec]));
			w3.Position = 0;
			w3.Expand = false;
			w3.Fill = false;
			// Container child hbox1.Gtk.Box+BoxChild
			this.entryValue = new global::Gtk.Entry();
			this.entryValue.CanFocus = true;
			this.entryValue.Name = "entryValue";
			this.entryValue.Text = global::Mono.Unix.Catalog.GetString("0.000");
			this.entryValue.IsEditable = true;
			this.entryValue.InvisibleChar = '•';
			this.hbox1.Add(this.entryValue);
			global::Gtk.Box.BoxChild w4 = ((global::Gtk.Box.BoxChild)(this.hbox1[this.entryValue]));
			w4.Position = 1;
			// Container child hbox1.Gtk.Box+BoxChild
			this.buttonInc = new global::Gtk.Button();
			this.buttonInc.CanFocus = true;
			this.buttonInc.Events = ((global::Gdk.EventMask)(256));
			this.buttonInc.Name = "buttonInc";
			this.buttonInc.UseUnderline = true;
			global::Gtk.Image w5 = new global::Gtk.Image();
			w5.Pixbuf = global::Stetic.IconLoader.LoadIcon(this, "gtk-add", global::Gtk.IconSize.Menu);
			this.buttonInc.Image = w5;
			this.hbox1.Add(this.buttonInc);
			global::Gtk.Box.BoxChild w6 = ((global::Gtk.Box.BoxChild)(this.hbox1[this.buttonInc]));
			w6.Position = 2;
			w6.Expand = false;
			w6.Fill = false;
			this.vbox1.Add(this.hbox1);
			global::Gtk.Box.BoxChild w7 = ((global::Gtk.Box.BoxChild)(this.vbox1[this.hbox1]));
			w7.Position = 1;
			w7.Expand = false;
			w7.Fill = false;
			// Container child vbox1.Gtk.Box+BoxChild
			this.hbuttonbox1 = new global::Gtk.HButtonBox();
			this.hbuttonbox1.Name = "hbuttonbox1";
			this.vbox1.Add(this.hbuttonbox1);
			global::Gtk.Box.BoxChild w8 = ((global::Gtk.Box.BoxChild)(this.vbox1[this.hbuttonbox1]));
			w8.Position = 2;
			w8.Expand = false;
			w8.Fill = false;
			// Container child vbox1.Gtk.Box+BoxChild
			this.hbox2 = new global::Gtk.HBox();
			this.hbox2.Name = "hbox2";
			this.hbox2.Spacing = 6;
			// Container child hbox2.Gtk.Box+BoxChild
			this.labelMinValue = new global::Gtk.Label();
			this.labelMinValue.Name = "labelMinValue";
			this.labelMinValue.LabelProp = global::Mono.Unix.Catalog.GetString("-1000");
			this.hbox2.Add(this.labelMinValue);
			global::Gtk.Box.BoxChild w9 = ((global::Gtk.Box.BoxChild)(this.hbox2[this.labelMinValue]));
			w9.Position = 0;
			w9.Expand = false;
			w9.Fill = false;
			// Container child hbox2.Gtk.Box+BoxChild
			this.hscrollbar3 = new global::Gtk.HScrollbar(null);
			this.hscrollbar3.Name = "hscrollbar3";
			this.hscrollbar3.Adjustment.Upper = 110D;
			this.hscrollbar3.Adjustment.PageIncrement = 10D;
			this.hscrollbar3.Adjustment.PageSize = 10D;
			this.hscrollbar3.Adjustment.StepIncrement = 0.01D;
			this.hscrollbar3.Adjustment.Value = 49.6D;
			this.hbox2.Add(this.hscrollbar3);
			global::Gtk.Box.BoxChild w10 = ((global::Gtk.Box.BoxChild)(this.hbox2[this.hscrollbar3]));
			w10.Position = 1;
			// Container child hbox2.Gtk.Box+BoxChild
			this.labelMaxValue = new global::Gtk.Label();
			this.labelMaxValue.Name = "labelMaxValue";
			this.labelMaxValue.LabelProp = global::Mono.Unix.Catalog.GetString("1000");
			this.hbox2.Add(this.labelMaxValue);
			global::Gtk.Box.BoxChild w11 = ((global::Gtk.Box.BoxChild)(this.hbox2[this.labelMaxValue]));
			w11.Position = 2;
			w11.Expand = false;
			w11.Fill = false;
			this.vbox1.Add(this.hbox2);
			global::Gtk.Box.BoxChild w12 = ((global::Gtk.Box.BoxChild)(this.vbox1[this.hbox2]));
			w12.Position = 3;
			w12.Expand = false;
			w12.Fill = false;
			this.Add(this.vbox1);
			if ((this.Child != null))
			{
				this.Child.ShowAll();
			}
			this.Hide();
			this.buttonDec.Clicked += new global::System.EventHandler(this.eventDecrement);
			this.entryValue.Changed += new global::System.EventHandler(this.EntryValueChanged);
			this.buttonInc.Clicked += new global::System.EventHandler(this.EventIncrement);
			this.hscrollbar3.ChangeValue += new global::Gtk.ChangeValueHandler(this.EventScrollBarDND);
		}
	}
}
