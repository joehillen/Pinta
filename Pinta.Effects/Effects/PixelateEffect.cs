/////////////////////////////////////////////////////////////////////////////////
// Paint.NET                                                                   //
// Copyright (C) dotPDN LLC, Rick Brewster, Tom Jackson, and contributors.     //
// Portions Copyright (C) Microsoft Corporation. All Rights Reserved.          //
// See license-pdn.txt for full licensing and attribution details.             //
//                                                                             //
// Ported to Pinta by: Marco Rolappe <m_rolappe@gmx.net>                       //
/////////////////////////////////////////////////////////////////////////////////

using System;
using Cairo;
using Pinta.Gui.Widgets;
using Pinta.Core;
using Mono.Unix;

namespace Pinta.Effects
{
	[System.ComponentModel.Composition.Export (typeof (BaseEffect))]
	public class PixelateEffect : BaseEffect
	{
		public override string Icon {
			get { return "Menu.Effects.Distort.Pixelate.png"; }
		}

		public override string Text {
			get { return Catalog.GetString ("Pixelate"); }
		}

		public override bool IsConfigurable {
			get { return true; }
		}

		public PixelateData Data {
			get { return EffectData as PixelateData; }
		}

		public override string EffectMenuCategory {
			get { return Catalog.GetString ("Distort"); }
		}

		public PixelateEffect () {
			EffectData = new PixelateData ();
		}

		public override bool LaunchConfiguration () {
			return EffectHelper.LaunchSimpleEffectDialog (this);
		}

		#region Algorithm Code Ported From PDN
		private ColorBgra ComputeCellColor (int x, int y, ImageSurface src, int cellSize, Gdk.Rectangle srcBounds) {
			Gdk.Rectangle cell = GetCellBox (x, y, cellSize);
			cell.Intersect (srcBounds);
			
			int left = cell.Left;
			int right = cell.Right - 1;
			int bottom = cell.Bottom - 1;
			int top = cell.Top;
			
			ColorBgra colorTopLeft = src.GetColorBgra (left, top);
			ColorBgra colorTopRight = src.GetColorBgra (right, top);
			ColorBgra colorBottomLeft = src.GetColorBgra (left, bottom);
			ColorBgra colorBottomRight = src.GetColorBgra (right, bottom);
			
			ColorBgra c = ColorBgra.BlendColors4W16IP (colorTopLeft, 16384, colorTopRight, 16384, colorBottomLeft, 16384, colorBottomRight, 16384);
			
			return c;
		}

		private Gdk.Rectangle GetCellBox (int x, int y, int cellSize) {
			int widthBoxNum = x % cellSize;
			int heightBoxNum = y % cellSize;
			var leftUpper = new Gdk.Point (x - widthBoxNum, y - heightBoxNum);
			
			var returnMe = new Gdk.Rectangle (leftUpper, new Gdk.Size (cellSize, cellSize));
			
			return returnMe;
		}


		unsafe public override void RenderEffect (ImageSurface src, ImageSurface dest, Gdk.Rectangle[] rois) {
			var cellSize = Data.CellSize;
			
			Gdk.Rectangle src_bounds = src.GetBounds ();
			Gdk.Rectangle dest_bounds = dest.GetBounds ();
			
			foreach (var rect in rois) {
				for (int y = rect.Top; y < rect.Bottom; ++y) {
					int yEnd = y + 1;
					
					for (int x = rect.Left; x < rect.Right; ++x) {
						var cellRect = GetCellBox (x, y, cellSize);
						cellRect.Intersect (dest_bounds);
						var color = ComputeCellColor (x, y, src, cellSize, src_bounds);
						
						int xEnd = Math.Min (rect.Right, cellRect.Right);
						yEnd = Math.Min (rect.Bottom, cellRect.Bottom);
						
						for (int y2 = y; y2 < yEnd; ++y2) {
							ColorBgra* ptr = dest.GetPointAddressUnchecked (x, y2);
							
							for (int x2 = x; x2 < xEnd; ++x2) {
								ptr->Bgra = color.Bgra;
								++ptr;
							}
						}
						
						x = xEnd - 1;
					}
					
					y = yEnd - 1;
				}
			}
		}
		#endregion
	}


	public class PixelateData : EffectData
	{
		[MinimumValue(1), MaximumValue(100)]
		public int CellSize = 2;
	}
}
