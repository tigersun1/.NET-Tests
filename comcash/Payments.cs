using System;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems.Finders;


namespace comcash
{
	public static class Payments
	{
		
		//Enters data in the amount calculator
		//x - window var, amount - new amount
		public static void EnterAmount(Window x, string amount){

			try{
				var field = x.Get<TestStack.White.UIItems.TextBox> (SearchCriteria.ByAutomationId (Variables.CalculatorTextBoxId));
				field.BulkText = "";
				field.Enter (amount);
				x.WaitWhileBusy ();

				} catch (Exception e){		
				Log.Error(e.ToString(), true);
				return;
				}
		}


		//returns BalanceDue in the decimal format
		//win - window var
		public static decimal getBalanceDue (Window win){
			try{

				string label = (win.Get<TestStack.White.UIItems.Label> (SearchCriteria.ByAutomationId (Variables.BalanceDueLabelId)).Name);
				label = label.Remove(0,1);
				decimal actCash;
				Decimal.TryParse(label, out actCash);
				return actCash;

				} catch (Exception e){		
				Log.Error(e.ToString(), true);
				return 0;
				}
		}

		//returns EBT due in the decimal format
		//win - windows var
		public static decimal getEBTDue (Window win){
			try{

				//string label = win.Get<TestStack.White.UIItems.Label> (SearchCriteria.ByAutomationId (Variables.TotalLabelId)).Name;
				string label = ConfigTest.returnTotalLabel(win);
				label = label.Remove(label.IndexOf("$"), label.LastIndexOf("$")+1);
				label = label.Remove(label.Length-1, 1);
				decimal actCash;
				Decimal.TryParse(label, out actCash);
				return actCash;

			} catch (Exception e){		
				Log.Error(e.ToString(), true);
				return 0;
			}
		}

		//Clicks on Home button
		//win - window var
		public static void ClickOnHomeButton (Window win){
			try{
				var homeButt = win.Get<TestStack.White.UIItems.RadioButton> (SearchCriteria.ByAutomationId (Variables.HomeNavButtonId));
					homeButt.Click();
					win.WaitWhileBusy();
				}catch (Exception e){
				Log.Error(e.ToString(), true);
				return;
				}
		}


		//Clicks on NoReceipt button
		//win - window var
		public static void NoReceiptButtonClick (Window win){
			try{

				var noReceiptButton = win.Get<TestStack.White.UIItems.Button>(SearchCriteria.ByAutomationId(Variables.NoReceiptButtonId));
				noReceiptButton.Click();
				win.WaitWhileBusy();
			}catch (Exception e){
				Log.Error(e.ToString(), true);
				return;
			}
		}


		//Ends sale - clicks on NoReceipt button and accept payment
		//win - window var
		public static void EndSale(Window win){
			NoReceiptButtonClick(win);
			ConfigTest.AcceptPayment(win);
			Fiddler.checkResponse("sale");
		}


		//Pays by cash
		//comcash - application var, payType - cashh amount
		public static TestStack.White.Application PayByCash (TestStack.White.Application comcash, string payType){
			try{

				var win = ConfigTest.getWindow(comcash);
				ClickOnHomeButton (win);

				if (!payType.Equals(""))
					EnterAmount(win, payType);

				var cashBut = win.Get<TestStack.White.UIItems.Button>(SearchCriteria.ByAutomationId(Variables.TenderCashButtonId));
				cashBut.Click();
				win.WaitWhileBusy();

				if (getBalanceDue(win) > 0)
					return comcash;

				EndSale(win);

				return comcash;

			}catch (Exception e){
				Log.Error(e.ToString(), true);
				return comcash;
			}

		}



		//Pays by card
		//comcash - application var, value - card amount
		public static TestStack.White.Application PayByCard(TestStack.White.Application comcash, string value){
			try{

				if (!Inet.PingInternet() || !ConfigTest.connectStatus){
					Log.Error("Can't pay by card, no internet connection", false);
					return comcash;
				}

				var win = ConfigTest.getWindow(comcash);
				ClickOnHomeButton(win);

				if (!value.Equals("")){
					decimal cardCash;
					Decimal.TryParse(value, out cardCash);
					if (cardCash > getBalanceDue(win)){
						Log.Error("Card amount more than balance amount", true);
						return comcash;
					} 
					else
					EnterAmount(win, value);
				}

				var cardButton = win.Get<TestStack.White.UIItems.Button>(SearchCriteria.ByAutomationId(Variables.TenderCardButtonId));
				cardButton.Click();
				win.WaitWhileBusy();

				var childWin = win.MdiChild(SearchCriteria.ByText(Variables.PayWithCardText));

				SendKeys.SendWait(Variables.CardKey);
				SendKeys.SendWait("{ENTER}");


				var stopwatch = new Stopwatch();
				stopwatch.Start();
				while(!childWin.IsOffScreen){
					if (stopwatch.ElapsedMilliseconds > 120000){
						Log.Error("POS can't accept card payment", true);
						return comcash;
					}
				}

				if (getBalanceDue(win) > 0)
					return comcash;

				EndSale(win);

				return comcash;
				
			}catch (Exception e){
				Log.Error(e.ToString(), true);
				return comcash;
			}
		}


		//Pays by tenders other
		//comcash - application var, str - tender type + tender amount
		static public TestStack.White.Application PayByTender (TestStack.White.Application comcash, string str){
			try{

				//args[0] = TypeOfTender, args[1] = Value or gift card code, args[2] = gift card value

				var args = new string[3];

				if (str.Contains(",")){
					string[] arr = str.Split(new Char [] {','});
					for (int x = 0; x <= arr.Length - 1; x++){
						args[x] = arr[x].Trim();
					}
				} else if (!String.IsNullOrEmpty(str))
					args[0] = str;
				else {
					Log.Error("No argumets in the statement", false);
					return comcash;
				}


				switch (args[0]){
				case "gift":
					if (!Inet.PingInternet() || !ConfigTest.connectStatus){
						Log.Error("Can't pay by " + args[0] + ", no internet connection", false);
						return comcash;
					}
					else if (String.IsNullOrEmpty(args[1])){
						Log.Error("No gift card barcode in the statement", false);
						return comcash;
					}
					args[0] = Variables.GiftCardText;
					break;
				
				case "coupon":
					args[0] = Variables.CouponText;
					break;

				case "ebt":
					args[0] = Variables.EBTText;
					break;

				case "ar":
					if (!Inet.PingInternet() || !ConfigTest.connectStatus){
						Log.Error("Can't pay by " + args[0] + ", no internet connection", false);
						return comcash;
					}
					args[0] = Variables.AccountsReceivableText;
					break;

				case "store":
					if (!Inet.PingInternet() || !ConfigTest.connectStatus){
						Log.Error("Can't pay by " + args[0] + ", no internet connection", false);
						return comcash;
					}
					args[0] = Variables.StoreCreditText;
					break;

				default:
					Log.Error("Incorrect tender in the statement", false);
					return comcash;
				}

				var win = ConfigTest.getWindow(comcash);
				ClickOnHomeButton(win);

				if (!String.IsNullOrEmpty(args[2])){
					decimal tenderCash;
					Decimal.TryParse(args[2], out tenderCash);
					if (tenderCash > getBalanceDue(win)){
						Log.Error("Tender amount more than balance amount", true);
						return comcash;
					} 
					else
						EnterAmount(win, args[2]);
				}

				else if (!String.IsNullOrEmpty(args[1]) && args[0] != Variables.GiftCardText){
					decimal tenderCash;
					Decimal.TryParse(args[1], out tenderCash);
					if (tenderCash > getBalanceDue(win)){
						Log.Error("Tender amount more than balance amount", true);
						return comcash;
					} 
					else
						EnterAmount(win, args[1]);
				}

				var tenderOtherButton = win.Get<TestStack.White.UIItems.Button> (SearchCriteria.ByAutomationId (Variables.TenderOtherButtonId));
				tenderOtherButton.Click ();
				win.WaitWhileBusy();

				var tenderOtherWin = win.MdiChild(SearchCriteria.ByClassName(Variables.MainWindowId));
				var TextButton = tenderOtherWin.Get<TestStack.White.UIItems.Label> (SearchCriteria.ByText(args[0]));
				TextButton.Click();
				win.WaitWhileBusy();

				if (args[0] == Variables.GiftCardText){
					var applyButton = win.Get<TestStack.White.UIItems.Button>(SearchCriteria.ByAutomationId(Variables.ApplyBarcodeButtonId));
					var errLable = win.Get<TestStack.White.UIItems.Label>(SearchCriteria.ByAutomationId(Variables.ErrorMessageLabelId));
				
					SendKeys.SendWait(args[1]);
					SendKeys.SendWait("{ENTER}");


					var stopwatch = new Stopwatch();
					stopwatch.Start();
					while(!applyButton.IsOffScreen){
						if (stopwatch.ElapsedMilliseconds > 120000){
							Log.Error("POS can't accept gift card payment", true);
							return comcash;
						}
					}
				}

				Thread.Sleep(1000);
				if (getBalanceDue(win) > 0)
					return comcash;

				EndSale(win);

				return comcash;
				
			}catch (Exception e){
				Log.Error(e.ToString(), true);
				return comcash;
			}
		}


		//sets sale discount
		//comcash - application var, arg - discount amount
		public static TestStack.White.Application SetAllDiscount (TestStack.White.Application comcash, string arg)
		{

			try{

				var win = ConfigTest.getWindow(comcash);
				ClickOnHomeButton(win);
				EnterAmount(win, arg);

				var DiscountButton = win.Get<TestStack.White.UIItems.Button>(SearchCriteria.ByAutomationId(Variables.TotalDiscountButtonId));
				DiscountButton.Click();
				Thread.Sleep(300);

				var label = win.Get<TestStack.White.UIItems.Label>(SearchCriteria.ByAutomationId(Variables.ErrorMessageLabelId));
				if(!label.IsOffScreen){
					var str = label.Text;
					Log.Error(str, true);
					return comcash;
				}

				return comcash;

			}catch (Exception e){
				Log.Error(e.ToString(), true);
				return comcash;
			}
		}


		//compares BalanceDue expected and actual amounts
		//comcash - application var, arg - expected amount 
		public static TestStack.White.Application CheckAmount (TestStack.White.Application comcash, string arg)
		{
			try{

				var win = ConfigTest.getWindow(comcash);

				decimal value;
				Decimal.TryParse(arg, out value);
				var amount = getBalanceDue(win);

				if (value != amount){
					Log.Error("actual amount " + amount + " is not equal to expected amount " + value, true);
					return comcash;
				}

				return comcash;
			}catch (Exception e){
				Log.Error(e.ToString(), true);
				return comcash;
			}
		}

		//cancell sale if some amount were paid
		//comcash - application var
		public static TestStack.White.Application VoidSale (TestStack.White.Application comcash)
		{
			try{

				var win = ConfigTest.getWindow(comcash);
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

