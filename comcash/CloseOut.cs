using System;
using TestStack.White.UIItems.Finders;
using System.Threading;
using System.Diagnostics;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems;

namespace comcash
{
	partial class TestData
	{
		public TestStack.White.Application CloseOut (TestStack.White.Application comcash)
		{
			try{

				var win = comcash.GetWindow(SearchCriteria.ByAutomationId("Window"), TestStack.White.Factory.InitializeOption.NoCache);
				var adminButton = win.Get<TestStack.White.UIItems.RadioButton>(SearchCriteria.ByAutomationId("AdminNavButton"));
				adminButton.Click();
				Thread.Sleep(1000);

				var CloseOutButton = win.Get<TestStack.White.UIItems.Button>(SearchCriteria.ByText("Close Out"));
				CloseOutButton.Click();
				Thread.Sleep(300);

				var stopwatch = new Stopwatch();
				stopwatch.Start();

				while (stopwatch.ElapsedMilliseconds < 300000) {
					var label = win.Get<TestStack.White.UIItems.Label> (SearchCriteria.ByAutomationId ("ErrorMessageLabel"));
					if (label.IsOffScreen){
						checkResponse("closeout/get");
						var SubmitButton = win.Get<TestStack.White.UIItems.Button>(SearchCriteria.ByAutomationId("SubmitCloseOutReport"));
						SubmitButton.Click();
						AcceptPayment(win);
						checkResponse("closeout/submit");
						return comcash;
					} else if (label.Name == "" | label.Name.StartsWith("Operation is"))
						continue; 
					else 
					{
						Logger("<td><font color=\"red\">ERROR: " + label.Name + "</font></td></tr>");
						SetFail(true);
						return comcash;
					}
				}

				return comcash;

			}catch (Exception e){
				Logger ("<td><font color=\"red\">ERROR: " + e + "</font></td></tr>");
				SetFail (true);
				return comcash;
			}
		}
	}
}

