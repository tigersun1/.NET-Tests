using System;
using System.Windows.Automation;
using System.Diagnostics;
using System.Threading;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;

namespace comcash
{
	partial class TestData
	{
		public TestStack.White.Application Return (TestStack.White.Application comcash, string args)
		{
			Window win = comcash.GetWindow(SearchCriteria.ByAutomationId(Variables.MainWindowId), TestStack.White.Factory.InitializeOption.NoCache);

			try{
				var adminButton = win.Get<TestStack.White.UIItems.RadioButton>(SearchCriteria.ByAutomationId(Variables.AdminNavButtonId));
				Thread.Sleep(500);
				adminButton.Click();
				Thread.Sleep(1000);

				var journalButton = win.Get<TestStack.White.UIItems.Button>(SearchCriteria.ByAutomationId(Variables.JournalButtonId));
				Thread.Sleep(500);
				journalButton.Click();
				Thread.Sleep(1000);

				var ListBox = win.Get<TestStack.White.UIItems.ListBoxItems.ListBox>(SearchCriteria.ByAutomationId(Variables.JournalListBoxId));

				var stopWatch = new Stopwatch();
				stopWatch.Start();
				while(stopWatch.ElapsedMilliseconds < 300000){
					if (ListBox.Enabled)						
						break;
					if(stopWatch.ElapsedMilliseconds > 250000){
						Log.Error("Can't get list of transactions", true);
						return comcash;
					}
				}

				var item = ListBox.AutomationElement.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.ClassNameProperty, "ListBoxItem"));
				SelectionItemPattern sel = item.GetCurrentPattern(SelectionItemPattern.Pattern) as SelectionItemPattern;
				sel.Select();

				if (args.Contains("void")){
					var voidButton = win.Get<TestStack.White.UIItems.Button>(SearchCriteria.ByAutomationId(Variables.VoidInReturnButtonId));
					Thread.Sleep(500);
					voidButton.Click();
					Thread.Sleep(500);
					var noReceiptButton = win.Get<TestStack.White.UIItems.Button>(SearchCriteria.ByAutomationId(Variables.NoReceiptButtonId));
					noReceiptButton.Click();

					var stopwatch = new Stopwatch();
					stopwatch.Start();
					while (stopwatch.ElapsedMilliseconds < 5000){
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

							break;
						}
						else if (stopwatch.ElapsedMilliseconds >5000){
							Log.Error("No Tenders for return window", true);
							return comcash;
						}
					}

					//Thread.Sleep(500);

					ConfigTest.AcceptPayment(win);
					Fiddler.checkResponse("/sale/void");
					Payments.ClickOnHomeButton(win);
					return comcash;

				} else{
					var returnButton = win.Get<TestStack.White.UIItems.Button>(SearchCriteria.ByAutomationId(Variables.ReturnButtonId));
					Thread.Sleep(500);
					returnButton.Click();
					Thread.Sleep(1000);

					var stopwatch = new Stopwatch();
					stopwatch.Start();
					while (stopwatch.ElapsedMilliseconds < 300000) {

						var label = win.Get<TestStack.White.UIItems.Label> (SearchCriteria.ByAutomationId (Variables.ErrorMessageLabelId));
						if (label.Name == "")
							break;
						if (stopWatch.ElapsedMilliseconds > 250000){
							Log.Error("Timeout", true);
							return comcash;
						}

					}

					if (args.Contains("all")){ 
						var listBox = win.Get<TestStack.White.UIItems.ListBoxItems.ListBox>(SearchCriteria.ByAutomationId(Variables.BuyHomeListBoxId));
						var list = listBox.Items;
						foreach (var s in list)
							ReturnProd(comcash, s.Text.ToLower());
						}

					return comcash;
				}

			}

			catch (Exception e){
				Log.Error(e.ToString(), true);
				return comcash;
			}
		}
	}
}

