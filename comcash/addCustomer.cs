﻿using System;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using System.Threading;
using System.Diagnostics;
using System.Windows.Automation;

namespace comcash
{
	partial class TestData
	{
		public TestStack.White.Application AddCustomer( TestStack.White.Application comcash, string customer){

			if (!ConfigTest.connectStatus || !Inet.PingInternet()) {
				Log.Error("Can't add customer, no internet connection", false);
				return comcash;
			}

			Window win = comcash.GetWindow(SearchCriteria.ByAutomationId(Variables.MainWindowId),TestStack.White.Factory.InitializeOption.NoCache);


			try{
				var CustButton = win.Get<TestStack.White.UIItems.Button> (SearchCriteria.ByAutomationId (Variables.CustomerButtonId));

				if (!CustButton.Enabled) {
					Log.Error("Customer button is disabled", true);
					return comcash;
				}

				CustButton.Click ();
			
				Thread.Sleep (1000);

				var searchField = win.Get<TestStack.White.UIItems.TextBox> (SearchCriteria.ByAutomationId (Variables.SearchCustomerTextBoxId));
				var searchBut = win.Get<TestStack.White.UIItems.Button> (SearchCriteria.ByAutomationId (Variables.SearchCustomerButtonId));
				searchField.Enter (customer);
				Thread.Sleep (500);
				searchBut.Click ();
				Thread.Sleep (500);

				var stopwatch = new Stopwatch();
				stopwatch.Start();

				for (int i = 0; ; i++) {
					var errlabel = win.Get<TestStack.White.UIItems.Label>(SearchCriteria.ByAutomationId(Variables.ErrorMessageLabelId));
					if (stopwatch.ElapsedMilliseconds > 120000) {
						Log.Error("POS hangs", true);
						return comcash;
					}
					if (!errlabel.Name.StartsWith("Operation is")){
						break;
					}
				}

				if (!Fiddler.checkResponse("customers"))
					return comcash;

				var label = win.Get<TestStack.White.UIItems.Label>(SearchCriteria.ByAutomationId("CustomerNotFoundLabel"));
				if (!label.IsOffScreen){
					Log.Error("Customer Not Found", true);
					return comcash;
				}

				var listBox = win.Get<TestStack.White.UIItems.ListBoxItems.ListBox> (SearchCriteria.ByAutomationId(Variables.CustomersListBoxId));
				var item = listBox.AutomationElement.FindFirst (TreeScope.Descendants, new PropertyCondition (AutomationElement.ClassNameProperty, Variables.ListBoxItemName));

				SelectionItemPattern sel = item.GetCurrentPattern (SelectionItemPattern.Pattern) as SelectionItemPattern;
				sel.Select();

				var continueButton = item.FindFirst (TreeScope.Descendants, new PropertyCondition (AutomationElement.NameProperty, Variables.ContinueName));

				InvokePattern patt = (InvokePattern)continueButton.GetCurrentPattern (InvokePattern.Pattern);
				patt.Invoke ();
				Thread.Sleep(1000);
				Fiddler.checkResponse("customer/loyaltyproducts");
				return comcash;
			}

			catch (Exception e){
				Log.Error(e.ToString(), true);
				return comcash;
			}
		}

	}
}

