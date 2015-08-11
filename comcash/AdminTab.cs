using System;
using System.Windows.Automation;
using System.Diagnostics;
using System.Threading;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;

namespace comcash
{
	public static class AdminTab
	{
		//Clicks on Continue button in returns and ends returning
		//comcash - application var
		public static TestStack.White.Application Continue (TestStack.White.Application comcash)
		{
			try{

				var win = ConfigTest.getWindow(comcash);
				Payments.ClickOnHomeButton(win);
				var ContButt = win.Get<TestStack.White.UIItems.Button>(SearchCriteria.ByAutomationId(Variables.ContinueSaleButtonId));
				ContButt.Click();
				Payments.NoReceiptButtonClick(win);

				ConfigTest.AcceptPayment(win);
				Fiddler.checkResponse("return");

				return comcash;

			}catch (Exception e){
				Log.Error(e.ToString(), true);
				return comcash;
			}
		}


		//Select the first sale in the sale journal, opens it or void it or returns all products in the sale
		//comcash - application var, args - void/all - voids the sale/returns all products in the sale
		public static TestStack.White.Application Return (TestStack.White.Application comcash, string args)
		{
			Window win = ConfigTest.getWindow(comcash);

			try{

				clickOnAdminButton(win);

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
					Payments.NoReceiptButtonClick(win);

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


		//returns product by name
		//comcash - application var, args - product name and quantity for return
		public static TestStack.White.Application ReturnProd (TestStack.White.Application comcash, string args)
		{
			try{
				var win = ConfigTest.getWindow(comcash);

				string prod, qty = null;

				if (args.Contains(",")){
					var str = args.Split(new Char [] {','});
					prod = str[0].Trim().ToLower();
					qty = str[1].Trim().ToLower();
				} else {
					prod = args;
				}

				Thread.Sleep(300);
				Product.OpenProduct(comcash, prod);
				var saveButton = win.Get<TestStack.White.UIItems.Button> (SearchCriteria.ByAutomationId (Variables.SaveProductDetailsButtonId));
				if (!saveButton.IsOffScreen){
					if (qty != null)
						Payments.EnterAmount(win, qty);
					saveButton.Click();
				}

				return comcash;
			}catch (Exception e){
				Log.Error(e.ToString(), true);
				return comcash;
			}
		}

		//Simple return
		//comcash - application var
		public static TestStack.White.Application SimpleReturn (TestStack.White.Application comcash)
		{
			try{

				var win = ConfigTest.getWindow(comcash);
				clickOnAdminButton(win);

				var ReturnButt = win.Get<TestStack.White.UIItems.Button>(SearchCriteria.ByAutomationId(Variables.SimpleReturnButtonId));
				ReturnButt.Click();
				Thread.Sleep(300);

				return comcash;

			}catch (Exception e){
				Log.Error(e.ToString(), true);
				return comcash;
			}
		}

		//Opens admin tab
		//win - windows var
		public static void clickOnAdminButton(Window win)
		{
			try{
				var adminButton = win.Get<TestStack.White.UIItems.RadioButton>(SearchCriteria.ByAutomationId(Variables.AdminNavButtonId));
				adminButton.Click();
				win.WaitWhileBusy();
			}catch (Exception e){
				Log.Error(e.ToString(), true);
				return;
			}
		}


		//makes close out
		//comcash - application var
		public static TestStack.White.Application CloseOut (TestStack.White.Application comcash)
		{
			try{

				var win = ConfigTest.getWindow(comcash);
				clickOnAdminButton(win);

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

