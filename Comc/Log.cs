using System;
using System.IO; 
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace comcash
{
	public static class Log
	{
		static StreamWriter log;
		static bool Fail;
		static public int Errors;
		static public int SessionErrors;

	    static Log(){
			Fail = false;
			Errors = 0;
			SessionErrors = 0;

			if (!Directory.Exists(ConfigTest.partPath))
				Directory.CreateDirectory(ConfigTest.partPath);

			if(!File.Exists(ConfigTest.LogPath))
			{
				log = new StreamWriter(ConfigTest.LogPath,true);
				log.WriteLine("<!DOCTYPE HTML> <html> <head> <meta charset=\"utf-8\"> <title>Test log</title>");
				log.WriteLine ("</head><body><table border=\"1\"><caption>Test log " + DateTime.Now + "</caption>");
				log.Close ();
			}
		}

		public static bool getFail(){
			return Fail;
		}

		public static void ResetFail(){
			Fail = false;
		}

		public static void Link (string Message, string Name, string Link){
			log = File.AppendText (ConfigTest.LogPath);
			log.WriteLine ("<tr><td>" + DateTime.Now.ToString("T") + "</td>" + "<td>" + Message + "<a href=\"" + Link + "\">" + Name + "</a></td></tr>");
			log.Close ();
		}

		public static void Info (string Message){
			log = File.AppendText (ConfigTest.LogPath);
			log.WriteLine ("<tr><td>" + DateTime.Now.ToString("T") + "</td>" + "<td>" + Message + "</td></tr>");
			log.Close ();
		}

		public static void Error(string Error, bool Capture){
			log = File.AppendText (ConfigTest.LogPath);
			log.WriteLine ("<tr><td>" + DateTime.Now.ToString("T") + "<td><font color=\"red\">ERROR: " + Error + "</font></td></tr>");
			log.Close ();

			Fail = true;
			Errors = Errors + 1;
			if (Capture) {
				var bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
				var gfx = Graphics.FromImage (bmpScreenshot);
				gfx.CopyFromScreen (Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);
				string screenPath = ConfigTest.partPath + @"\Screen_" + Errors + ".png";
				bmpScreenshot.Save (screenPath, ImageFormat.Png);
				Link ("Error screen:", "Screen_" + Errors, screenPath);   
			}
		}
	}
}

