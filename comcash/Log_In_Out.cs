using System;
using TestStack;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems;
using TestStack.White.UIItems.MenuItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.TreeItems;
using TestStack.White.UIItems.WindowStripControls;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;


namespace comcash
{
	partial class TestData
	{
		public TestStack.White.Application LogIn(TestStack.White.Application comcash,string pin,bool fail)
		{

			Window pinWindow = comcash.GetWindow (SearchCriteria.ByAutomationId("PINWindow"),TestStack.White.Factory.InitializeOption.NoCache);

			try{
				TestStack.White.UIItems.Button LogInBut = pinWindow.Get<TestStack.White.UIItems.Button> (SearchCriteria.ByAutomationId("LogInButton"));
				TestStack.White.UIItems.TextBox textBox = pinWindow.Get<TestStack.White.UIItems.TextBox> (SearchCriteria.ByAutomationId("PinPasswordBox"));
				textBox.Enter (pin);
				pinWindow.WaitWhileBusy();
				LogInBut.Click ();
				pinWindow.WaitWhileBusy();
				Thread.Sleep(3000);

				var stopwatch = new Stopwatch();
				stopwatch.Start();
				while (stopwatch.ElapsedMilliseconds < 300000){
					List<Window> list = comcash.GetWindows();
					if(list.Exists(obj=>obj.Id.StartsWith("Window"))){ 
						if(fail == true){
							SetFail(true);
							Logger("<td><font color=\"red\">ERROR: User can LogIn with incorrect pin</font></td></tr>");
							return comcash;
						}
						else{
							checkResponse("authorization/login");
							return comcash;
						}
					}

					var c = pinWindow.Exists(SearchCriteria.ByAutomationId("ErrorMessageLabel"));
					if(c){
						if (fail == false){
							var b = pinWindow.Get<TestStack.White.UIItems.Label>(SearchCriteria.ByAutomationId("ErrorMessageLabel"));
							Logger("<td><font color=\"red\">ERROR: " + b.Text + "</font></td></tr>");
							SetFail(true);
							return comcash;
						}
						else {
							return comcash;
						}
					}
				}

				Logger("<td><font color=\"red\">ERROR: POS is hangs, wait " + stopwatch.ElapsedMilliseconds + " ms</font></td></tr>");
				SetFail(true);
				return comcash;	
			}

			catch (Exception e){
				Logger ("<td><font color=\"red\">ERROR: " + e + "</font></td></tr>");
				SetFail (true);
				return comcash;
			}
		}


		public TestStack.White.Application LogOut(TestStack.White.Application comcash){

			Window mainWindow = comcash.GetWindow (SearchCriteria.ByAutomationId("Window"),TestStack.White.Factory.InitializeOption.NoCache);
			try{
				TestStack.White.UIItems.Button LogOut = mainWindow.Get<TestStack.White.UIItems.Button>(SearchCriteria.ByAutomationId("LogoutButton"));

				Thread.Sleep(1000);

				if (LogOut.Enabled){
					LogOut.Click();
					mainWindow.WaitWhileBusy();
					Thread.Sleep(2000);
				}
				else{
					Logger("<td><font color=\"red\">ERROR: LogOut button is disabled</font></td></tr>");
					SetFail(true);
					return comcash;
				}

				var stopwatch = new Stopwatch();
				stopwatch.Start();
				for ( int x = 0; stopwatch.ElapsedMilliseconds < 60000; x++){
					List<Window> list = comcash.GetWindows();
					if(list.Exists(obj=>obj.Id.StartsWith("PINWindow"))){
						checkResponse("authorization/logout");
						return comcash;
					}
				}

				Logger("<td><font color=\"red\">ERROR: Can't LogOut</font></td></tr>");
				SetFail(true);
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

