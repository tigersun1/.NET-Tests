using System;
using TestStack.White.UIItems.Finders;
using System.Threading;
using System.Diagnostics;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems;

using System.IO; 
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;


namespace comcash
{
	partial class TestData
	{
		public TestStack.White.Application CloseOut (TestStack.White.Application comcash)
		{
			try{

				var win = comcash.GetWindow(SearchCriteria.ByAutomationId(Variables.MainWindowId), TestStack.White.Factory.InitializeOption.NoCache);
				var adminButton = win.Get<TestStack.White.UIItems.RadioButton>(SearchCriteria.ByAutomationId(Variables.AdminNavButtonId));
				adminButton.Click();
				Thread.Sleep(1000);

				var CloseOutButton = win.Get<TestStack.White.UIItems.Button>(SearchCriteria.ByText(Variables.CloseOutText));
				CloseOutButton.Click();
				Thread.Sleep(300);



				var stopwatch = new Stopwatch();
				stopwatch.Start();

				while (stopwatch.ElapsedMilliseconds < 300000) {
					var label = win.Get<TestStack.White.UIItems.Label> (SearchCriteria.ByAutomationId (Variables.ErrorMessageLabelId));
					if (label.IsOffScreen){
						Fiddler.checkResponse("closeout/get");
						var SubmitButton = win.Get<TestStack.White.UIItems.Button>(SearchCriteria.ByAutomationId(Variables.SubmitCloseOutReportId));
						//Delete
						var bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
						var gfx = Graphics.FromImage (bmpScreenshot);
						gfx.CopyFromScreen (Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);
						string screenPath = ConfigTest.partPath + @"\Screen.png";
						bmpScreenshot.Save (screenPath, ImageFormat.Png);
						//Delete
						SubmitButton.Click();
						ConfigTest.AcceptPayment(win);
						Fiddler.checkResponse("closeout/submit");
						return comcash;
					} else if (label.Name == "" | label.Name.StartsWith("Operation is"))
						continue; 
					else 
					{
						Log.Error(label.Name, true);
						return comcash;
					}
				}

				return comcash;

			}catch (Exception e){
				Log.Error(e.ToString(), true);
				return comcash;
			}
		}
	}
}

