using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Forms;
using System.Diagnostics;
using System.ComponentModel;
using System.Reflection;
using System.Threading;
using System.IO; 
using TestStack;
using TestStack.White.Recording;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems;
using TestStack.White.UIItems.MenuItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.TreeItems;
using TestStack.White.UIItems.WindowStripControls;
using System.Collections.Generic;


namespace comcash
{
	partial class TestData
	{

		public TestStack.White.Application payByCard (TestStack.White.Application comcash, string value)
		{
			Window win = comcash.GetWindow (SearchCriteria.ByAutomationId ("Window"), TestStack.White.Factory.InitializeOption.NoCache);

			try{

				if (!PingInternet() || !connectStatus){
					Logger("<td><font color=\"red\">ERROR: Can't pay by card, no internet connection</font></td></tr>");
					SetFail(true);
					return comcash;
				}

				ClickOnHomeButton(win);

				if (!value.Equals("")){
					//double cardCash = double.Parse(value);
					double cardCash; 
					Double.TryParse(value, out cardCash);
					string x = (win.Get<TestStack.White.UIItems.Label> (SearchCriteria.ByAutomationId ("BalanceDueLabel")).Name);
					x = x.Remove(0,1);
					double actCash;
					Double.TryParse(x, out actCash); 

					if (cardCash > actCash){
						Logger("<td><font color=\"red\">ERROR: Card amount more than balance amount</font></td></tr>");
						SetFail(true);
						return comcash;
					}
					else
						EnterAmount(win, value);
				}

				var cardButton = win.Get<TestStack.White.UIItems.Button>(SearchCriteria.ByAutomationId("TenderCardButton"));
				cardButton.Click();
				Thread.Sleep(500);

				var childWin = win.MdiChild(SearchCriteria.ByText("Pay with Card"));

				SendKeys.SendWait("<DvcMsg Ver=\"1.1\"><Dvc App=\"SecureKey Software\" AppVer=\"1.0\" DvcType=\"M130-IDTECH\" DvcSN=\"54132607651\" Entry=\"SWIPE\"></Dvc><Card CEncode=\"0\" ETrk1=\"54B326F9996A81B8CB9F85F6E2AE1E43CCCEAB102B03EF88A870678CD7F5277D42AE67666194A1A85FF2C03386987C6E32DD955F71EA0184CF34FE2679EA582F321A0A70ED106DB10A9AD81660FF17E5\" ETrk2=\"3CC2E80D702DD70366C5F1E350D916369FC875E0D043C6C306D8BF669FFC8A6E523A07FC37FA0CA0\" CDataKSN=\"9010500326012E60019A\" Exp=\"1512\" MskPAN=\"5499********6781\" CHolder=\"GLOBAL PAYMENTS TEST CARD/\" EFormat=\"4\"></Card><Addr></Addr><Tran TranType=\"CREDIT\"></Tran></DvcMsg>");
				SendKeys.SendWait("{ENTER}");


				var stopwatch = new Stopwatch();
				stopwatch.Start();
				while(!childWin.IsOffScreen){
					if (stopwatch.ElapsedMilliseconds > 120000){
						Logger("<td><font color=\"red\">ERROR: POS can't accept card payment</font></td></tr>");
						SetFail(true);
						return comcash;
					}
				}

				var dueLabel = win.Get<TestStack.White.UIItems.Label>(SearchCriteria.ByAutomationId("BalanceDueLabel"));
				string due = dueLabel.Name;
				var index = due.IndexOf("$");
				due = due.Remove(index,1);
				double amount;
				Double.TryParse(due, out amount);
				if (amount > 0)
					return comcash;


				var noReceiptButton = win.Get<TestStack.White.UIItems.Button>(SearchCriteria.ByAutomationId("NoReceiptButton"));
				noReceiptButton.Click();
				Thread.Sleep(1000);

				AcceptPayment(win);
				checkResponse("sale");

				return comcash;
			}

			catch(Exception e){
				Logger ("<td><font color=\"red\">ERROR: " + e + "</font></td></tr>");
				SetFail (true);
				return comcash;
			}

		}
	}
}

