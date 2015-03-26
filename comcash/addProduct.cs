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
		public TestStack.White.Application AddProd(TestStack.White.Application comcash, string arg){

			Window win = comcash.GetWindow(SearchCriteria.ByAutomationId("Window"),TestStack.White.Factory.InitializeOption.NoCache);

			try{
				string prod, qty = null;

				if (arg.Contains(",")){
					var str = arg.Split(new Char [] {','});
					prod = str[0].Trim();
					qty = str[1].Trim();
				} else {
					prod = arg;
				}

				var SearchField = win.Get<TestStack.White.UIItems.TextBox> (SearchCriteria.ByAutomationId ("SearchTextBox"));
				var ClearBut = win.Get<TestStack.White.UIItems.Button> (SearchCriteria.ByAutomationId ("ClearSearchButton"));
				ClearBut.Click ();
				win.WaitWhileBusy();
				Thread.Sleep(500);
				SearchField.Enter(prod);
				Thread.Sleep(1000);
				var listBox = win.Get<TestStack.White.UIItems.ListBoxItems.ListBox>(SearchCriteria.ByAutomationId("ProductsListBox"));
				var listItem = listBox.Items.Find(item=>item.Text.ToLower().StartsWith(prod));
				Thread.Sleep(1000);
				listItem.Click();
				win.WaitWhileBusy();
				Thread.Sleep(1000);

				if (qty != null)
					AddQTY(comcash, prod + "," + qty);

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

