using System;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;


namespace comcash
{
	public static class LogInOut
	{
		//logs in
		//comcash - application var, pin - pin code
		public static TestStack.White.Application LogIn(TestStack.White.Application comcash, string pin)
		{

			Window pinWindow = comcash.GetWindow (SearchCriteria.ByAutomationId(Variables.PinWindowId),TestStack.White.Factory.InitializeOption.NoCache);

			try{
				TestStack.White.UIItems.Button LogInBut = pinWindow.Get<TestStack.White.UIItems.Button> (SearchCriteria.ByAutomationId (Variables.LogInButtonId));
				TestStack.White.UIItems.TextBox textBox = pinWindow.Get<TestStack.White.UIItems.TextBox> (SearchCriteria.ByAutomationId(Variables.PinPasswordBoxId));
				textBox.Enter (pin);
				pinWindow.WaitWhileBusy();
				LogInBut.Click ();
				pinWindow.WaitWhileBusy();
				Thread.Sleep(3000);

				var stopwatch = new Stopwatch();
				stopwatch.Start();
				while (stopwatch.ElapsedMilliseconds < 300000){
					List<Window> list = comcash.GetWindows();
					if(list.Exists(obj=>obj.Id.StartsWith( Variables.MainWindowId))){ 
						if(!list.Find(obj=>obj.Id.StartsWith(Variables.MainWindowId)).IsOffScreen){
							Fiddler.checkResponse("authorization/login");
							return comcash;
						}
						else {						
							continue;
						}
						}

					var c = pinWindow.Exists(SearchCriteria.ByAutomationId(Variables.ErrorMessageLabelId));
					if(c){
						
						var b = pinWindow.Get<TestStack.White.UIItems.Label>(SearchCriteria.ByAutomationId(Variables.ErrorMessageLabelId));
							Log.Error(b.Text, true);
						Fiddler.checkResponse("authorization/login");
							return comcash;
					}
					}


				Log.Error("POS is hangs, wait " + stopwatch.ElapsedMilliseconds + " ms", true);
				return comcash;	
			}

			catch (Exception e){
				Log.Error(e.ToString(), true);
				return comcash;
			}
		}


		//logouts
		//comcash - application var
		public static TestStack.White.Application LogOut(TestStack.White.Application comcash){

			Window mainWindow = comcash.GetWindow (SearchCriteria.ByAutomationId(Variables.MainWindowId),TestStack.White.Factory.InitializeOption.NoCache);
			try{
				TestStack.White.UIItems.Button LogOut = mainWindow.Get<TestStack.White.UIItems.Button>(SearchCriteria.ByAutomationId(Variables.LogoutButtonId));

				Thread.Sleep(1000);

				if (LogOut.Enabled){
					LogOut.Click();
					mainWindow.WaitWhileBusy();
					Thread.Sleep(2000);
				}
				else{
					Log.Error("LogOut button is disabled", true);
					return comcash;
				}

				var stopwatch = new Stopwatch();
				stopwatch.Start();
				for ( int x = 0; stopwatch.ElapsedMilliseconds < 60000; x++){
					List<Window> list = comcash.GetWindows();
					if(list.Exists(obj=>obj.Id.StartsWith(Variables.PinWindowId))){
						Fiddler.checkResponse("authorization/logout");
						return comcash;
					}
				}
					
				Log.Error("Can't LogOut", true);
				Fiddler.checkResponse("authorization/logout");
				return comcash;

			}


			catch (Exception e){
				Log.Error(e.ToString(), true);
				return comcash;
			}

		}


	}
}

