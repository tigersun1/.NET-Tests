using System;
using System.Threading;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;

namespace comcash
{
	partial class TestData
	{
		public Window tenderOthers (Window comWin, string value)
		{
			try{
				Thread.Sleep (1000);

				var homeButt = comWin.Get<TestStack.White.UIItems.RadioButton> (SearchCriteria.ByAutomationId (Variables.HomeNavButtonId));
				homeButt.Click();
				Thread.Sleep(1000);

				if (!value.Equals("")){
					decimal couponCash;
					Decimal.TryParse(value, out couponCash);
					var label = comWin.Get<TestStack.White.UIItems.Label> (SearchCriteria.ByAutomationId (Variables.BalanceDueLabelId)).Name;
					var index = label.IndexOf("$");
					label = label.Remove(index, 1);
					decimal actCash;
					Decimal.TryParse (label, out actCash);
					if (couponCash > actCash){
						Log.Error("Entered amount more than balance amount", true);
						return comWin;
					}
					else
						EnterAmount(comWin, value);
				}

				var tenderOtherButton = comWin.Get<TestStack.White.UIItems.Button> (SearchCriteria.ByAutomationId (Variables.TenderOtherButtonId));
				tenderOtherButton.Click ();
				Thread.Sleep (2000);
				return comWin;
			}

			catch (Exception e){
				Log.Error(e.ToString(), true);
				return comWin;
			}
		}
	}
}

