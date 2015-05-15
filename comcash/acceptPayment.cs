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
		public bool AcceptPayment(Window x){

			try{
				var stopwatch = new Stopwatch();
				stopwatch.Start();
				while (stopwatch.ElapsedMilliseconds < 300000) {
					var label = x.Get<TestStack.White.UIItems.Label> (SearchCriteria.ByAutomationId (Variables.ErrorMessageLabelId));
					var c = x.Items.Exists(obj=>obj.Name.Contains("sure"));
					if (c){
						var WarWind = x.MdiChild(SearchCriteria.ByText(Variables.ReturnOptionsWindowText));
						var ApplyButt = WarWind.Get<TestStack.White.UIItems.Button>(SearchCriteria.ByAutomationId(Variables.ApplyReturnOptionsButtonId));
						ApplyButt.Click();
						continue;
					}
					else if (label.Name == "" | label.Name.StartsWith("Operation is"))
						continue;
					else if (label.Name.Contains ("Complete") || label.Name.ToLower().Contains("return") || label.Name.ToLower().Contains("completed")) {
						var okButt = x.Get<TestStack.White.UIItems.Button> (SearchCriteria.ByAutomationId (Variables.CloseMessageButtonId));
						okButt.Click ();
						Thread.Sleep (1000);
						return true;
					} else {
						Log.Error(label.Name, true);
						return false;
					}
				}
				return false;
			}
			catch (Exception e){
				Log.Error(e.ToString(), true);
				return false;
			}
		}
	}
}

