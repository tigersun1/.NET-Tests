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
		public Window tenderOthers (Window comWin, string value)
		{
			try{
				Thread.Sleep (1000);

				var homeButt = comWin.Get<TestStack.White.UIItems.RadioButton> (SearchCriteria.ByAutomationId ("HomeNavButton"));
				homeButt.Click();
				Thread.Sleep(1000);

				if (!value.Equals("")){
					double couponCash = double.Parse(value);
					double actCash = double.Parse (comWin.Get<TestStack.White.UIItems.Label> (SearchCriteria.ByAutomationId ("BalanceDueLabel")).Name);
					if (couponCash > actCash){
						Logger("<td><font color=\"red\">ERROR: Entered amount more than balance amount</font></td></tr>");
						SetFail(true);
						return comWin;
					}
					else
						EnterAmount(comWin, value);
				}

				var tenderOtherButton = comWin.Get<TestStack.White.UIItems.Button> (SearchCriteria.ByAutomationId ("TenderOtherButton"));
				tenderOtherButton.Click ();
				Thread.Sleep (2000);
				return comWin;
			}

			catch (Exception e){
				Logger ("<td><font color=\"red\">ERROR: " + e + "</font></td></tr>");
				SetFail (true);
				return comWin;
			}
		}
	}
}

