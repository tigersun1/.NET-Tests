﻿using System;
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
		public TestStack.White.Application payByGift (TestStack.White.Application comcash, string str)
		{
			try{

				if (!PingInternet()){
					Logger("<td><font color=\"red\">ERROR: Can't pay by gift card, no internet connection</font></td></tr>");
					SetFail(true);
					return comcash;
				}

				Window win = comcash.GetWindow(SearchCriteria.ByAutomationId("Window"), TestStack.White.Factory.InitializeOption.NoCache);
				string code, value;

				if (str.Contains(",")){
					var args = str.Split(new Char [] {','});
					code = args[0].Trim();
					value = args[1].Trim();
				} else {
					code = str;
					value= "";
				}

				ClickOnHomeButton(win);
				tenderOthers(win, value);

				var tenderOtherWin = win.MdiChild(SearchCriteria.ByText("TenderOtherWindow"));
				var button = tenderOtherWin.Get<TestStack.White.UIItems.Button>(SearchCriteria.ByText("Gift Card"));
				button.Click();
				Thread.Sleep(500);

				var applyButton = win.Get<TestStack.White.UIItems.Button>(SearchCriteria.ByAutomationId("ApplyBarcodeButton"));
				var errLable = win.Get<TestStack.White.UIItems.Label>(SearchCriteria.ByAutomationId("ErrorMessageLabel"));

				SendKeys.SendWait(code);
				SendKeys.SendWait("{ENTER}");

				var stopwatch = new Stopwatch();
				stopwatch.Start();
				while(!applyButton.IsOffScreen){
					if (stopwatch.ElapsedMilliseconds > 300000){
						Logger("<td><font color=\"red\">ERROR: POS can't accept gift card payment</font></td></tr>");
						SetFail(true);
						return comcash;
					}
				}

				Thread.Sleep(1000);

				if (!errLable.IsOffScreen){
					AcceptPayment(win);
				}

				var dueLabel = win.Get<TestStack.White.UIItems.Label>(SearchCriteria.ByAutomationId("BalanceDueLabel"));
				string due = dueLabel.Name;
				var index = due.IndexOf("$");
				due = due.Remove(index,1);
				double amount = double.Parse(due);
				if (amount > 0)
					return comcash;

				var noReceiptButton = win.Get<TestStack.White.UIItems.Button>(SearchCriteria.ByAutomationId("NoReceiptButton"));
				noReceiptButton.Click();
				Thread.Sleep(1000);
				AcceptPayment(win);
				checkResponse("sale/perform");

				return comcash;
			}
			catch (Exception e){
				Logger ("<td><font color=\"red\">ERROR: " + e + "</font></td></tr>");
				SetFail (true);
				return comcash;
			}
		}
	}
}

