using System;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems;
using System.Threading;
using System.Diagnostics;

namespace comcash
{
	partial class TestData
	{
		public TestStack.White.Application VoidSale (TestStack.White.Application comcash)
		{
			try{

				var win = comcash.GetWindow (SearchCriteria.ByAutomationId (Variables.MainWindowId), TestStack.White.Factory.InitializeOption.NoCache);
				var CancelButton = win.Get<TestStack.White.UIItems.Button>(SearchCriteria.ByAutomationId(Variables.CancelButtonId));
				CancelButton.Click();
				Thread.Sleep(1500);

				var stopwatch = new Stopwatch();
				stopwatch.Start();
				while (stopwatch.ElapsedMilliseconds < 5000){
					var b = win.Items.Exists(obj=>obj.Name.Contains(Variables.ResetText));
					if(b){
						var DialogWindow = win.MdiChild(SearchCriteria.ByText(Variables.DialogWindowText));
						var YesButton = DialogWindow.Get<TestStack.White.UIItems.Button>(SearchCriteria.ByAutomationId(Variables.ButtonYESId));
						YesButton.Click();
						Thread.Sleep(300);
						break;
					}
				}
					

				stopwatch.Restart();
				while (stopwatch.ElapsedMilliseconds < 300000){
					var c = win.Items.Exists(obj=>obj.Name.Contains(Variables.TendersForReturnText));
					if(c){
						var ReturnWindow = win.MdiChild(SearchCriteria.ByText(Variables.ReturnPaymentWindowId));
						var ContButton = ReturnWindow.Get <TestStack.White.UIItems.Button> (SearchCriteria.ByAutomationId(Variables.ContinueButtonId));
								ContButton.Click();
								
								stopwatch.Restart();
								while(stopwatch.ElapsedMilliseconds < 6000){
							var t = win.Items.Exists(obj=>obj.Name.Contains(Variables.TendersForReturnText));
									if (!t)
										break;
								}

								return comcash;
					}
				}
					
				Log.Error("No Tenders for return window", true);
				return comcash;

			}catch (Exception e){
				Log.Error(e.ToString(), true);
				return comcash;
			}
		}
	}
}

