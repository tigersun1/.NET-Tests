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
		public TestStack.White.Application Launch()
		{
			TestStack.White.Application x = TestStack.White.Application.Launch (appPath);

			try{

				List<Window> list = x.GetWindows();
				var t = list.Exists(obj=>obj.Title.Contains("Instance issue"));
				if (t){
					Logger("<td><font color=\"red\">ERROR: POS is already running</font></td></tr>");
					SetFail(true);
					return x;
				}


				Window window = x.GetWindow(SearchCriteria.ByAutomationId("PINWindow"), TestStack.White.Factory.InitializeOption.NoCache);
				if (window == null){
					Logger("<td><font color=\"red\">ERROR: POS is not opened</font></td></tr>");
					SetFail(true);
					return x;
				}

				else{
					var b = window.Items.Exists(obj=>obj.Name.Contains("Config file not found"));
					if (b){
						Logger("<td><font color=\"red\">ERROR: No config file</font></td></tr>");
						SetFail(true);
						return x;
					}

					return x;
				}
			}
			catch (Exception e){
				Logger ("<td><font color=\"red\">ERROR: " + e + "</font></td></tr>");
				SetFail (true);
				return x;
			}
		}
	}
}

