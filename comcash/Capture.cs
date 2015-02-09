using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

//Capture block

namespace comcash
{
	partial class TestData
	{
		public void Capture()
		{
			var bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
			var gfx = Graphics.FromImage (bmpScreenshot);
			gfx.CopyFromScreen (Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);
			string screenPath = partPath + @"\Screen_" + Errors + ".png";
			bmpScreenshot.Save (screenPath, ImageFormat.Png);
			Logger ("<td>Error screen: <a href=\"" + screenPath + "\"> Screen_" + Errors + "</a></td></tr>");
		}
	
	}
}

