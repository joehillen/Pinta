// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------

namespace Pinta {
    
    
    public partial class LayerPropertiesDialog {
        
        private Gtk.VBox vbox2;
        
        private Gtk.HBox hbox1;
        
        private Gtk.Label label1;
        
        private Gtk.Entry entry1;
        
        private Gtk.CheckButton checkbutton1;
        
        private Gtk.HSeparator hseparator2;
        
        private Gtk.HBox hbox2;
        
        private Gtk.Label label2;
        
        private Gtk.SpinButton spinbutton1;
        
        private Gtk.HScale hscale1;
        
        private Gtk.Button buttonCancel;
        
        private Gtk.Button buttonOk;
        
        protected virtual void Build() {
            Stetic.Gui.Initialize(this);
            // Widget Pinta.LayerPropertiesDialog
            this.Name = "Pinta.LayerPropertiesDialog";
            this.Title = Mono.Unix.Catalog.GetString("Layer Properties");
            this.WindowPosition = ((Gtk.WindowPosition)(4));
            // Internal child Pinta.LayerPropertiesDialog.VBox
            Gtk.VBox w1 = this.VBox;
            w1.Name = "dialog1_VBox";
            w1.BorderWidth = ((uint)(2));
            // Container child dialog1_VBox.Gtk.Box+BoxChild
            this.vbox2 = new Gtk.VBox();
            this.vbox2.Name = "vbox2";
            this.vbox2.Spacing = 10;
            this.vbox2.BorderWidth = ((uint)(9));
            // Container child vbox2.Gtk.Box+BoxChild
            this.hbox1 = new Gtk.HBox();
            this.hbox1.Name = "hbox1";
            this.hbox1.Spacing = 6;
            // Container child hbox1.Gtk.Box+BoxChild
            this.label1 = new Gtk.Label();
            this.label1.Name = "label1";
            this.label1.LabelProp = Mono.Unix.Catalog.GetString("Name:");
            this.hbox1.Add(this.label1);
            Gtk.Box.BoxChild w2 = ((Gtk.Box.BoxChild)(this.hbox1[this.label1]));
            w2.Position = 0;
            w2.Expand = false;
            w2.Fill = false;
            // Container child hbox1.Gtk.Box+BoxChild
            this.entry1 = new Gtk.Entry();
            this.entry1.CanFocus = true;
            this.entry1.Name = "entry1";
            this.entry1.IsEditable = true;
            this.entry1.InvisibleChar = '●';
            this.hbox1.Add(this.entry1);
            Gtk.Box.BoxChild w3 = ((Gtk.Box.BoxChild)(this.hbox1[this.entry1]));
            w3.Position = 1;
            this.vbox2.Add(this.hbox1);
            Gtk.Box.BoxChild w4 = ((Gtk.Box.BoxChild)(this.vbox2[this.hbox1]));
            w4.Position = 0;
            w4.Expand = false;
            w4.Fill = false;
            // Container child vbox2.Gtk.Box+BoxChild
            this.checkbutton1 = new Gtk.CheckButton();
            this.checkbutton1.CanFocus = true;
            this.checkbutton1.Name = "checkbutton1";
            this.checkbutton1.Label = Mono.Unix.Catalog.GetString("Visible");
            this.checkbutton1.DrawIndicator = true;
            this.checkbutton1.UseUnderline = true;
            this.vbox2.Add(this.checkbutton1);
            Gtk.Box.BoxChild w5 = ((Gtk.Box.BoxChild)(this.vbox2[this.checkbutton1]));
            w5.Position = 1;
            w5.Expand = false;
            w5.Fill = false;
            // Container child vbox2.Gtk.Box+BoxChild
            this.hseparator2 = new Gtk.HSeparator();
            this.hseparator2.Name = "hseparator2";
            this.vbox2.Add(this.hseparator2);
            Gtk.Box.BoxChild w6 = ((Gtk.Box.BoxChild)(this.vbox2[this.hseparator2]));
            w6.Position = 2;
            w6.Expand = false;
            w6.Fill = false;
            // Container child vbox2.Gtk.Box+BoxChild
            this.hbox2 = new Gtk.HBox();
            this.hbox2.Name = "hbox2";
            this.hbox2.Spacing = 16;
            // Container child hbox2.Gtk.Box+BoxChild
            this.label2 = new Gtk.Label();
            this.label2.Name = "label2";
            this.label2.LabelProp = Mono.Unix.Catalog.GetString("Opacity:");
            this.hbox2.Add(this.label2);
            Gtk.Box.BoxChild w7 = ((Gtk.Box.BoxChild)(this.hbox2[this.label2]));
            w7.Position = 0;
            w7.Expand = false;
            w7.Fill = false;
            // Container child hbox2.Gtk.Box+BoxChild
            this.spinbutton1 = new Gtk.SpinButton(0, 100, 1);
            this.spinbutton1.CanFocus = true;
            this.spinbutton1.Name = "spinbutton1";
            this.spinbutton1.Adjustment.PageIncrement = 10;
            this.spinbutton1.ClimbRate = 1;
            this.spinbutton1.Numeric = true;
            this.hbox2.Add(this.spinbutton1);
            Gtk.Box.BoxChild w8 = ((Gtk.Box.BoxChild)(this.hbox2[this.spinbutton1]));
            w8.Position = 1;
            w8.Expand = false;
            w8.Fill = false;
            // Container child hbox2.Gtk.Box+BoxChild
            this.hscale1 = new Gtk.HScale(null);
            this.hscale1.CanFocus = true;
            this.hscale1.Name = "hscale1";
            this.hscale1.Adjustment.Upper = 100;
            this.hscale1.Adjustment.PageIncrement = 10;
            this.hscale1.Adjustment.StepIncrement = 1;
            this.hscale1.DrawValue = true;
            this.hscale1.Digits = 0;
            this.hscale1.ValuePos = ((Gtk.PositionType)(2));
            this.hbox2.Add(this.hscale1);
            Gtk.Box.BoxChild w9 = ((Gtk.Box.BoxChild)(this.hbox2[this.hscale1]));
            w9.Position = 2;
            this.vbox2.Add(this.hbox2);
            Gtk.Box.BoxChild w10 = ((Gtk.Box.BoxChild)(this.vbox2[this.hbox2]));
            w10.Position = 3;
            w10.Expand = false;
            w10.Fill = false;
            w1.Add(this.vbox2);
            Gtk.Box.BoxChild w11 = ((Gtk.Box.BoxChild)(w1[this.vbox2]));
            w11.Position = 0;
            w11.Expand = false;
            w11.Fill = false;
            // Internal child Pinta.LayerPropertiesDialog.ActionArea
            Gtk.HButtonBox w12 = this.ActionArea;
            w12.Name = "dialog1_ActionArea";
            w12.Spacing = 10;
            w12.BorderWidth = ((uint)(5));
            w12.LayoutStyle = ((Gtk.ButtonBoxStyle)(4));
            // Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
            this.buttonCancel = new Gtk.Button();
            this.buttonCancel.CanDefault = true;
            this.buttonCancel.CanFocus = true;
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.UseStock = true;
            this.buttonCancel.UseUnderline = true;
            this.buttonCancel.Label = "gtk-cancel";
            this.AddActionWidget(this.buttonCancel, -6);
            Gtk.ButtonBox.ButtonBoxChild w13 = ((Gtk.ButtonBox.ButtonBoxChild)(w12[this.buttonCancel]));
            w13.Expand = false;
            w13.Fill = false;
            // Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
            this.buttonOk = new Gtk.Button();
            this.buttonOk.CanDefault = true;
            this.buttonOk.CanFocus = true;
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.UseStock = true;
            this.buttonOk.UseUnderline = true;
            this.buttonOk.Label = "gtk-ok";
            this.AddActionWidget(this.buttonOk, -5);
            Gtk.ButtonBox.ButtonBoxChild w14 = ((Gtk.ButtonBox.ButtonBoxChild)(w12[this.buttonOk]));
            w14.Position = 1;
            w14.Expand = false;
            w14.Fill = false;
            if ((this.Child != null)) {
                this.Child.ShowAll();
            }
            this.DefaultWidth = 349;
            this.DefaultHeight = 224;
            this.Show();
        }
    }
}
