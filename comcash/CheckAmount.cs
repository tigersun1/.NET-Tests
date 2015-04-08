using System;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using System.Threading;

namespace comcash
{
	partial class TestData
	{
		public TestStack.White.Application CheckAmount (TestStack.White.Application comcash, string arg)
		{
			try{
				var win = comcash.GetWindow (SearchCriteria.ByAutomationId ("Window"), TestStack.White.Factory.InitializeOption.NoCache);

				var value = double.Parse(arg);
				var dueLabel = win.Get<TestStack.White.UIItems.Label>(SearchCriteria.ByAutomationId("BalanceDueLabel"));
				string due = dueLabel.Name;
				var index = due.IndexOf("$");
				due = due.Remove(index,1);
				double amount = double.Parse(due);

				if (value != amount){
					Logger ("<td><font color=\"red\">ERROR: actual amount " + amount + " is not equal to expected amount " + value+ " </font></td></tr>");
					SetFail(true);
					return comcash;
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

