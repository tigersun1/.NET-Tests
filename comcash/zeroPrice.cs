using System;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems;
using System.Threading;
using System.Diagnostics;

namespace comcash
{
	partial class TestData
	{
		public TestStack.White.Application zeroPrice (TestStack.White.Application comcash, string value)
		{
			try{
				var win = comcash.GetWindow (SearchCriteria.ByAutomationId ("Window"), TestStack.White.Factory.InitializeOption.NoCache);
				EnterAmount(win, value);
				SaveProdDetails(win);
				return comcash;
			}catch (Exception e){
				Logger ("<td><font color=\"red\">ERROR: " + e + "</font></td></tr>");
				SetFail (true);
				return comcash;
			}
		}
	}
}

