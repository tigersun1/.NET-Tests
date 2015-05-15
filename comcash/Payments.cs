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


		public static void EndSale(Window win){
			NoReceiptButtonClick(win);
			ConfigTest.AcceptPayment(win);
			Fiddler.checkResponse("sale");
		}


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
					else if (String.IsNullOrEmpty(args[2])){
						Log.Error("No gift card barcode in the statement", false);
						return comcash;
					}
					args[0] = Variables.GiftCardText;
					break;
				
				case "coupon":
					args[0] = Variables.CouponText;
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

				else if (!String.IsNullOrEmpty(args[1])){
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

				if (getBalanceDue(win) > 0)
					return comcash;

				EndSale(win);

				return comcash;
				
			}catch (Exception e){
				Log.Error(e.ToString(), true);
				return comcash;
			}
		}

	}
}

