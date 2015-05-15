using System;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems.Finders;
using System.Collections.Generic;

namespace comcash
{
	partial class TestData
	{
		public TestStack.White.Application Launch()
		{
			TestStack.White.Application x = TestStack.White.Application.Launch (ConfigTest.appPath);

			try{

				List<Window> list = x.GetWindows();
				var t = list.Exists(obj=>obj.Title.Contains("Instance issue"));
				if (t){
					Log.Error("POS is already running", false);
					return x;
				}


				Window window = x.GetWindow(SearchCriteria.ByAutomationId(Variables.PinWindowId), TestStack.White.Factory.InitializeOption.NoCache);
				if (window == null){
					Log.Error("POS is not opened", true);
					return x;
				}

				else{
					var b = window.Items.Exists(obj=>obj.Name.Contains("Config file not found"));
					if (b){
						Log.Error("No config file", true);
						return x;
					}

					return x;
				}
			}
			catch (Exception e){
				Log.Error(e.ToString(), true);
				return x;
			}
		}
	}
}

