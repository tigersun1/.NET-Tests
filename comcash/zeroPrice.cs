using System;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;

namespace comcash
{
	partial class TestData
	{
		public TestStack.White.Application zeroPrice (TestStack.White.Application comcash, string value)
		{
			try{
				var win = comcash.GetWindow (SearchCriteria.ByAutomationId (Variables.MainWindowId), TestStack.White.Factory.InitializeOption.NoCache);
				EnterAmount(win, value);
				SaveProdDetails(win);
				return comcash;
			}catch (Exception e){
				Log.Error(e.ToString(), true);
				return comcash;
			}
		}
	}
}

