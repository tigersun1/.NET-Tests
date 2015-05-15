using System;
using TestStack.White.UIItems.WindowItems;


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
				Log.Error(e.ToString(), true);
				return comcash;
			}

		}
	}
}

