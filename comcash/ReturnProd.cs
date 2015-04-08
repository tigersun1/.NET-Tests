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
		public TestStack.White.Application ReturnProd (TestStack.White.Application comcash, string args)
		{
			try{
				Window win = comcash.GetWindow(SearchCriteria.ByAutomationId("Window"),TestStack.White.Factory.InitializeOption.NoCache);

				string prod, qty = null;

				if (args.Contains(",")){
					var str = args.Split(new Char [] {','});
					prod = str[0].Trim().ToLower();
					qty = str[1].Trim().ToLower();
				} else {
					prod = args;
				}

				Thread.Sleep(300);
				OpenProduct(comcash, prod);
				var saveButton = win.Get<TestStack.White.UIItems.Button> (SearchCriteria.ByAutomationId ("SaveProductDetailsButton"));
				if (!saveButton.IsOffScreen){
					if (qty != null)
						EnterAmount(win, qty);
					saveButton.Click();
				}
				
				return comcash;
			}catch (Exception e){
				Logger ("<td><font color=\"red\">ERROR: " + e + "</font></td></tr>");
				SetFail (true);
				return comcash;
			}

		}
	}
}

