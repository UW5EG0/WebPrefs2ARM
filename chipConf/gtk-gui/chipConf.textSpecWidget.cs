
// This file has been generated by the GUI designer. Do not modify.
namespace chipConf
{
	public partial class textSpecWidget
	{
		private global::Gtk.VBox vbox1;

		private global::Gtk.Label labelName;

		private global::Gtk.Entry entryValue;

		private global::Gtk.Label labelCharCounter;

		protected virtual void Build()
		{
			global::Stetic.Gui.Initialize(this);
			// Widget chipConf.textSpecWidget
			global::Stetic.BinContainer.Attach(this);
			this.Name = "chipConf.textSpecWidget";
			// Container child chipConf.textSpecWidget.Gtk.Container+ContainerChild
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
			this.entryValue = new global::Gtk.Entry();
			this.entryValue.CanFocus = true;
			this.entryValue.Name = "entryValue";
			this.entryValue.IsEditable = true;
			this.entryValue.InvisibleChar = '•';
			this.vbox1.Add(this.entryValue);
			global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.vbox1[this.entryValue]));
			w2.Position = 1;
			w2.Expand = false;
			w2.Fill = false;
			// Container child vbox1.Gtk.Box+BoxChild
			this.labelCharCounter = new global::Gtk.Label();
			this.labelCharCounter.Name = "labelCharCounter";
			this.labelCharCounter.LabelProp = global::Mono.Unix.Catalog.GetString("Chars: 0/32");
			this.vbox1.Add(this.labelCharCounter);
			global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.vbox1[this.labelCharCounter]));
			w3.Position = 2;
			w3.Expand = false;
			w3.Fill = false;
			this.Add(this.vbox1);
			if ((this.Child != null))
			{
				this.Child.ShowAll();
			}
			this.Hide();
			this.entryValue.Changed += new global::System.EventHandler(this.EntryValueChanged);
		}
	}
}
