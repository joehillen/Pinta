﻿// 
// SettingsManager.cs
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace Pinta.Core
{
	public class SettingsManager
	{
		private Dictionary<string, object> settings;
		
		public SettingsManager ()
		{
			LoadSettings ();
		}
		
		public string GetUserSettingsDirectory ()
		{
			return Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.ApplicationData), "Pinta");
		}
		
		public T GetSetting<T> (string key, T defaultValue)
		{
			if (!settings.ContainsKey (key))
				return defaultValue;
				
			return (T)settings[key];
		}
		
		public void PutSetting (string key, object value)
		{
			settings[key] = value;
		}

		private static Dictionary<string, object> Deserialize (string filename)
		{
			Dictionary<string, object> properties = new Dictionary<string,object> ();

			if (!File.Exists (filename))
				return properties;
				
			XmlDocument doc = new XmlDocument ();
			doc.Load (filename);
			
			// Kinda cheating for now because I know there is only one thing stored in here
			foreach (XmlElement setting in doc.DocumentElement.ChildNodes) {
				switch (setting.GetAttribute ("type")) {
					case "System.Int32":
						properties[setting.GetAttribute ("name")] = int.Parse (setting.InnerText);
						break;
				}
			
			}

			return properties;
		}

		private static void Serialize (string filename, Dictionary<string, object> settings)
		{
			using (XmlWriter xw = XmlWriter.Create (filename)) {
				xw.WriteStartElement ("settings");
				
				foreach (var item in settings) {
					xw.WriteStartElement ("setting");
					xw.WriteAttributeString ("name", item.Key);
					xw.WriteAttributeString ("type", item.Value.GetType ().ToString ());
					xw.WriteValue (item.Value.ToString ());
					xw.WriteEndElement ();
				}
				
				xw.WriteEndElement ();
			}
		}
		
		private void LoadSettings ()
		{
			string settings_file = Path.Combine (GetUserSettingsDirectory (), "settings.xml");
			
			settings = Deserialize (settings_file);
		}
		
		public void SaveSettings ()
		{
			string settings_file = Path.Combine (GetUserSettingsDirectory (), "settings.xml");

			Serialize (settings_file, settings);
		}
	}
}
