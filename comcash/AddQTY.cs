using System;
using System.Threading;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;

namespace comcash
{
	partial class TestData
	{
		public TestStack.White.Application AddQTY (TestStack.White.Application comcash, string prod, string qty)
		{
			try{

			var win = OpenProduct(comcash, prod);
			EnterAmount (win, qty);
			SaveProdDetails (win); 

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

