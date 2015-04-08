using System;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems;
using System.Threading;
using System.Collections.Generic;
using System.Diagnostics;

namespace comcash
{
	partial class TestData
	{
		public TestStack.White.Application VoidSale (TestStack.White.Application comcash)
		{
			try{

				var win = comcash.GetWindow (SearchCriteria.ByAutomationId ("Window"), TestStack.White.Factory.InitializeOption.NoCache);
				var CancelButton = win.Get<TestStack.White.UIItems.Button>(SearchCriteria.ByAutomationId("CancelButton"));
				CancelButton.Click();
				Thread.Sleep(1500);

				var stopwatch = new Stopwatch();
				stopwatch.Start();
				while (stopwatch.ElapsedMilliseconds < 5000){
					var b = win.Items.Exists(obj=>obj.Name.Contains("Reset"));
					if(b){
						var DialogWindow = win.MdiChild(SearchCriteria.ByText("DialogWindow"));
						var YesButton = DialogWindow.Get<TestStack.White.UIItems.Button>(SearchCriteria.ByAutomationId("ButtonYES"));
						YesButton.Click();
						Thread.Sleep(300);
						break;
					}
				}
					

				stopwatch.Restart();
				while (stopwatch.ElapsedMilliseconds < 300000){
					var c = win.Items.Exists(obj=>obj.Name.Contains("Tenders for return"));
					if(c){
								var ReturnWindow = win.MdiChild(SearchCriteria.ByText("ReturnPaymentWindow"));
								var ContButton = ReturnWindow.Get <TestStack.White.UIItems.Button> (SearchCriteria.ByAutomationId("ContinueButton"));
								ContButton.Click();
								Thread.Sleep(300);

								return comcash;
					}
				}


				SetFail(true);
				Logger("<td><font color=\"red\">ERROR: No Tenders for return window</font></td></tr>");
				return comcash;

			}catch (Exception e){
				Logger ("<td><font color=\"red\">ERROR: " + e + "</font></td></tr>");
				SetFail (true);
				return comcash;
			}
		}
	}
}

