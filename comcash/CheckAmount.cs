using System;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;

namespace comcash
{
	partial class TestData
	{
		public TestStack.White.Application CheckAmount (TestStack.White.Application comcash, string arg)
		{
			try{
				var win = comcash.GetWindow (SearchCriteria.ByAutomationId (Variables.MainWindowId), TestStack.White.Factory.InitializeOption.NoCache);

				decimal value;
				Decimal.TryParse(arg, out value);
				var dueLabel = win.Get<TestStack.White.UIItems.Label>(SearchCriteria.ByAutomationId(Variables.BalanceDueLabelId));
				string due = dueLabel.Name;
				var index = due.IndexOf("$");
				due = due.Remove(index, 1);
				decimal amount;
				Decimal.TryParse(due, out amount);

				if (value != amount){
					Log.Error("actual amount " + amount + " is not equal to expected amount " + value, true);
					return comcash;
				}

				return comcash;
			}catch (Exception e){
				Log.Error(e.ToString(), true);
				return comcash;
			}
		}
	}
}

