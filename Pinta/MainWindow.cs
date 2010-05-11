// 
// MainWindow.cs
//  
// Author:
//       Jonathan Pobst <monkey@jpobst.com>
// 
// Copyright (c) 2010 Jonathan Pobst
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using Gdk;
using Gtk;
using Pinta.Core;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition;
using System.Collections.Generic;
using System.Linq;

namespace Pinta
{
	public partial class MainWindow : Gtk.Window
	{
		DialogHandlers dialog_handler;

		ProgressDialog progress_dialog;

		[ImportMany]
		public IEnumerable<BaseTool> Tools { get; set; }

		public MainWindow () : base(Gtk.WindowType.Toplevel)
		{
			DateTime start = DateTime.Now;
			Build ();
			
			// Initialize interface things
			PintaCore.Actions.AccelGroup = new AccelGroup ();
			this.AddAccelGroup (PintaCore.Actions.AccelGroup);
			
			progress_dialog = new ProgressDialog ();
			
			PintaCore.Initialize (tooltoolbar, label5, drawingarea1, history_treeview, this, progress_dialog);
			colorpalettewidget1.Initialize ();
			
			Compose ();
			
			PintaCore.Chrome.StatusBarTextChanged += new EventHandler<TextChangedEventArgs> (Chrome_StatusBarTextChanged);
			CreateToolBox ();
			
			PintaCore.Actions.CreateMainMenu (menubar1);
			PintaCore.Actions.CreateToolBar (toolbar1);
			PintaCore.Actions.Layers.CreateLayerWindowToolBar (toolbar4);
			PintaCore.Actions.Edit.CreateHistoryWindowToolBar (toolbar2);
			
			Gtk.Image i = new Gtk.Image (PintaCore.Resources.GetIcon ("StatusBar.CursorXY.png"));
			i.Show ();
			
			statusbar1.Add (i);
			Gtk.Box.BoxChild box = (Gtk.Box.BoxChild)statusbar1[i];
			box.Position = 2;
			box.Fill = false;
			box.Expand = false;
			
			this.Icon = PintaCore.Resources.GetIcon ("Pinta.png");
			
			dialog_handler = new DialogHandlers (this);
			
			// Create a blank document
			Layer background = PintaCore.Layers.AddNewLayer ("Background");
			
			using (Cairo.Context g = new Cairo.Context (background.Surface)) {
				g.SetSourceRGB (255, 255, 255);
				g.Paint ();
			}
			
			PintaCore.Workspace.Filename = "Untitled1";
			PintaCore.History.PushNewItem (new BaseHistoryItem ("gtk-new", "New Image"));
			PintaCore.Workspace.IsDirty = false;
			PintaCore.Workspace.Invalidate ();
			
			//History
			history_treeview.Model = PintaCore.History.ListStore;
			history_treeview.HeadersVisible = false;
			history_treeview.Selection.Mode = SelectionMode.Single;
			history_treeview.Selection.SelectFunction = HistoryItemSelected;
			
			Gtk.TreeViewColumn icon_column = new Gtk.TreeViewColumn ();
			Gtk.CellRendererPixbuf icon_cell = new Gtk.CellRendererPixbuf ();
			icon_column.PackStart (icon_cell, true);
			
			Gtk.TreeViewColumn text_column = new Gtk.TreeViewColumn ();
			Gtk.CellRendererText text_cell = new Gtk.CellRendererText ();
			text_column.PackStart (text_cell, true);
			
			text_column.SetCellDataFunc (text_cell, new Gtk.TreeCellDataFunc (HistoryRenderText));
			icon_column.SetCellDataFunc (icon_cell, new Gtk.TreeCellDataFunc (HistoryRenderIcon));
			
			history_treeview.AppendColumn (icon_column);
			history_treeview.AppendColumn (text_column);
			
			PintaCore.History.HistoryItemAdded += new EventHandler<HistoryItemAddedEventArgs> (OnHistoryItemsChanged);
			PintaCore.History.ActionUndone += new EventHandler (OnHistoryItemsChanged);
			PintaCore.History.ActionRedone += new EventHandler (OnHistoryItemsChanged);
			
			PintaCore.Actions.View.ZoomToWindow.Activated += new EventHandler (ZoomToWindow_Activated);
			DeleteEvent += new DeleteEventHandler (MainWindow_DeleteEvent);
			
			PintaCore.LivePreview.RenderUpdated += LivePreview_RenderUpdated;
			
			WindowAction.Visible = false;
			
			if (Platform.GetOS () == Platform.OS.Mac) {
				try {
					//enable the global key handler for keyboard shortcuts
					IgeMacMenu.GlobalKeyHandlerEnabled = true;
					
					//Tell the IGE library to use your GTK menu as the Mac main menu
					IgeMacMenu.MenuBar = menubar1;
					/*
					//tell IGE which menu item should be used for the app menu's quit item
					IgeMacMenu.QuitMenuItem = yourQuitMenuItem;
					*/					
					//add a new group to the app menu, and add some items to it
					var appGroup = IgeMacMenu.AddAppMenuGroup ();
					MenuItem aboutItem = (MenuItem)PintaCore.Actions.Help.About.CreateMenuItem ();
					appGroup.AddMenuItem (aboutItem, Mono.Unix.Catalog.GetString ("About"));
					
					menubar1.Hide ();
				} catch {
					// If things don't work out, just use a normal menu.
				}
			}
			
			Console.WriteLine ("Total: {0}", DateTime.Now - start);
		}

		private void Compose ()
		{
			DateTime start = DateTime.Now;
			string ext_dir = System.IO.Path.Combine (System.IO.Path.GetDirectoryName (System.Reflection.Assembly.GetEntryAssembly ().Location), "Extensions");
			var catalog = new DirectoryCatalog (ext_dir);
			var container = new CompositionContainer (catalog);
			container.ComposeParts (this);
			Console.WriteLine ("Compose: {0}", DateTime.Now - start);
		}

		private void MainWindow_DeleteEvent (object o, DeleteEventArgs args)
		{
			// leave window open so user can cancel quitting
			args.RetVal = true;
			
			PintaCore.Actions.File.Exit.Activate ();
		}

		private void ZoomToWindow_Activated (object sender, EventArgs e)
		{
			// The image is small enough to fit in the window
			if (PintaCore.Workspace.ImageFitsInWindow) {
				PintaCore.Actions.View.ActualSize.Activate ();
				return;
			}
			
			int image_x = PintaCore.Workspace.ImageSize.Width;
			int image_y = PintaCore.Workspace.ImageSize.Height;
			
			int window_x = GtkScrolledWindow.Children[0].Allocation.Width;
			int window_y = GtkScrolledWindow.Children[0].Allocation.Height;
			
			// The image is more constrained by width than height
			if ((double)image_x / (double)window_x >= (double)image_y / (double)window_y) {
				double ratio = (double)(window_x - 20) / (double)image_x;
				PintaCore.Workspace.Scale = ratio;
				PintaCore.Actions.View.SuspendZoomUpdate ();
				(PintaCore.Actions.View.ZoomComboBox.ComboBox as ComboBoxEntry).Entry.Text = string.Format ("{0}%", (int)(PintaCore.Workspace.Scale * 100));
				PintaCore.Actions.View.ResumeZoomUpdate ();
			} else {
				double ratio2 = (double)(window_y - 20) / (double)image_y;
				PintaCore.Workspace.Scale = ratio2;
				PintaCore.Actions.View.SuspendZoomUpdate ();
				(PintaCore.Actions.View.ZoomComboBox.ComboBox as ComboBoxEntry).Entry.Text = string.Format ("{0}%", (int)(PintaCore.Workspace.Scale * 100));
				PintaCore.Actions.View.ResumeZoomUpdate ();
			}
		}

		private void Chrome_StatusBarTextChanged (object sender, TextChangedEventArgs e)
		{
			label5.Text = e.Text;
		}

		#region History
		public bool HistoryItemSelected (TreeSelection selection, TreeModel model, TreePath path, bool path_currently_selected)
		{
			int current = path.Indices[0];
			if (!path_currently_selected) {
				while (PintaCore.History.Pointer < current) {
					PintaCore.History.Redo ();
				}
				while (PintaCore.History.Pointer > current) {
					PintaCore.History.Undo ();
				}
			}
			return true;
		}

		private void HistoryRenderText (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			BaseHistoryItem item = (BaseHistoryItem)model.GetValue (iter, 0);
			if (item.State == HistoryItemState.Undo) {
				(cell as Gtk.CellRendererText).Style = Pango.Style.Normal;
				(cell as Gtk.CellRendererText).Foreground = "black";
				(cell as Gtk.CellRendererText).Text = item.Text;
			} else if (item.State == HistoryItemState.Redo) {
				(cell as Gtk.CellRendererText).Style = Pango.Style.Oblique;
				(cell as Gtk.CellRendererText).Foreground = "gray";
				(cell as Gtk.CellRendererText).Text = item.Text;
			}
			
		}

		private void HistoryRenderIcon (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			BaseHistoryItem item = (BaseHistoryItem)model.GetValue (iter, 0);
			(cell as Gtk.CellRendererPixbuf).Pixbuf = PintaCore.Resources.GetIcon (item.Icon);
		}

		private void OnHistoryItemsChanged (object o, EventArgs args)
		{
			if (PintaCore.History.Current != null) {
				history_treeview.Selection.SelectIter (PintaCore.History.Current.Id);
				history_treeview.ScrollToCell (history_treeview.Model.GetPath (PintaCore.History.Current.Id), history_treeview.Columns[1], true, (float)0.9, 0);
			}
			
		}
		#endregion

		private void CreateToolBox ()
		{
			// Create our tools
			foreach (BaseTool tool in Tools.OrderBy (t => t.Priority))
				PintaCore.Tools.AddTool (tool);
			
			// Try to set the paint brush as the default tool, if that
			// fails, set the first thing we can find.
			if (!PintaCore.Tools.SetCurrentTool ("PaintBrush"))
				PintaCore.Tools.SetCurrentTool (Tools.First ());
			
			bool even = true;
			
			foreach (var tool in PintaCore.Tools) {
				if (even)
					toolbox1.Insert (tool.ToolItem, toolbox1.NItems);
				else
					toolbox2.Insert (tool.ToolItem, toolbox2.NItems);
				
				even = !even;
			}
		}

		void LivePreview_RenderUpdated (object o, LivePreviewRenderUpdatedEventArgs args)
		{
			double scale = PintaCore.Workspace.Scale;
			var offset = PintaCore.Workspace.Offset;
			
			var bounds = args.Bounds;
			
			// Transform bounds (Image -> Canvas -> Window)
			
			// Calculate canvas bounds.
			double x1 = bounds.Left * scale;
			double y1 = bounds.Top * scale;
			double x2 = bounds.Right * scale;
			double y2 = bounds.Bottom * scale;
			
			// TODO Figure out why when scale > 1 that I need add on an
			// extra pixel of padding.
			// I must being doing something wrong here.
			if (scale > 1.0) {
				//x1 = (bounds.Left-1) * scale;
				y1 = (bounds.Top - 1) * scale;
				//x2 = (bounds.Right+1) * scale;
				//y2 = (bounds.Bottom+1) * scale;
			}
			
			// Calculate window bounds.
			x1 += offset.X;
			y1 += offset.Y;
			x2 += offset.X;
			y2 += offset.Y;
			
			// Convert to integer, carefull not to miss paritally covered
			// pixels by rounding incorrectly.
			int x = (int)Math.Floor (x1);
			int y = (int)Math.Floor (y1);
			int width = (int)Math.Ceiling (x2) - x;
			int height = (int)Math.Ceiling (y2) - y;
			
			// Tell GTK to expose the drawing area.			
			drawingarea1.QueueDrawArea (x, y, width, height);
		}

		#region Drawing Area
		private void OnDrawingarea1MotionNotifyEvent (object o, Gtk.MotionNotifyEventArgs args)
		{
			Cairo.PointD point = PintaCore.Workspace.WindowPointToCanvas (args.Event.X, args.Event.Y);
			
			if (PintaCore.Workspace.PointInCanvas (point))
				CursorPositionLabel.Text = string.Format ("{0}, {1}", (int)point.X, (int)point.Y);
		}
		#endregion
	}
}
