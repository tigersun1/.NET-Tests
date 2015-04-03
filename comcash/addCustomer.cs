using System;
using TestStack;
using TestStack.White.Recording;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems;
using TestStack.White.UIItems.MenuItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.TreeItems;
using TestStack.White.UIItems.WindowStripControls;
using System.Threading;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Forms;
using System.ComponentModel;
using System.Reflection;

namespace comcash
{
	partial class TestData
	{
		public TestStack.White.Application AddCustomer( TestStack.White.Application comcash, string customer){

			if (!connectStatus || !PingInternet()) {
				Logger("<td><font color=\"red\">ERROR: Can't add customer, no internet connection</font></td></tr>");
				SetFail (true);
				return comcash;
			}

			Window win = comcash.GetWindow(SearchCriteria.ByAutomationId("Window"),TestStack.White.Factory.InitializeOption.NoCache);


			try{
				var CustButton = win.Get<TestStack.White.UIItems.Button> (SearchCriteria.ByAutomationId ("CustomerButton"));

				if (!CustButton.Enabled) {
					Logger ("<td><font color=\"red\">ERROR: Customer button is disabled</font></td></tr>");
					SetFail (true);
					return comcash;
				}

				CustButton.Click ();
			
				Thread.Sleep (1000);

				var searchField = win.Get<TestStack.White.UIItems.TextBox> (SearchCriteria.ByAutomationId ("SearchCustomerTextBox"));
				var searchBut = win.Get<TestStack.White.UIItems.Button> (SearchCriteria.ByAutomationId ("SearchCustomerButton"));
				searchField.Enter (customer);
				Thread.Sleep (500);
				searchBut.Click ();
				Thread.Sleep (500);

				var stopwatch = new Stopwatch();
				stopwatch.Start();
				for (int i = 0; ; i++) {
					var errlabel = win.Get<TestStack.White.UIItems.Label>(SearchCriteria.ByAutomationId("ErrorMessageLabel"));
					if (stopwatch.ElapsedMilliseconds > 300000){
						Logger("<td><font color=\"red\">ERROR: server doesn't return customer list</font></td></tr>");
						SetFail(true);
						return comcash;
					}
					else if (errlabel.Name.StartsWith("Operation is"))
						continue;
					else 
						break;
				}

				if (!checkResponse("customers"))
					return comcash;

				var label = win.Get<TestStack.White.UIItems.Label>(SearchCriteria.ByAutomationId("CustomerNotFoundLabel"));
				if (!label.IsOffScreen){
					Logger("<td><font color=\"red\">ERROR: Customer Not Found</font></td></tr>");
					SetFail(true);
					return comcash;
				}

				var listBox = win.Get<TestStack.White.UIItems.ListBoxItems.ListBox> (SearchCriteria.ByAutomationId("CustomersListBox"));
				var item = listBox.AutomationElement.FindFirst (TreeScope.Descendants, new PropertyCondition (AutomationElement.ClassNameProperty, "ListBoxItem"));

				SelectionItemPattern sel = item.GetCurrentPattern (SelectionItemPattern.Pattern) as SelectionItemPattern;
				sel.Select();
				Thread.Sleep(1000);

				var continueButton = item.FindFirst (TreeScope.Descendants, new PropertyCondition (AutomationElement.NameProperty, "Continue"));

				InvokePattern patt = (InvokePattern)continueButton.GetCurrentPattern (InvokePattern.Pattern);
				patt.Invoke ();
				Thread.Sleep(1000);
				checkResponse("customer/loyaltyproducts");
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

