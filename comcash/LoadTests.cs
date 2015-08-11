using System;
using System.Threading;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using System.Windows.Automation;
using System.Diagnostics;



namespace comcash
{
	public class LoadTests
	{
		readonly string[] products = {"gin", "st_4", "good_ebt", "block", "pack"};
		static Random rnd = new Random();
		readonly TestStack.White.Application comcash;

		public LoadTests(TestStack.White.Application arg){
			comcash = arg;
		}
			
		public void Prod (){
			int lastIndex = rnd.Next (1, 4);
			for (int x = 1; x <= lastIndex; x++) {
				Product.AddProd (comcash, products [rnd.Next (0, 4)]);
			}
		}

		public void Tenders(){
			if (Payments.getEBTDue (ConfigTest.getWindow (comcash)) > 0) 
				Payments.PayByTender (comcash, Variables.EBTText);
			if (Payments.getBalanceDue(ConfigTest.getWindow(comcash)) > 0){
				int val = rnd.Next (1, 6);
				if (val < 5)
					Payments.PayByCash (comcash, "");
				else if (val == 5)
					Payments.PayByCard (comcash, "");
				else if (val == 6)
					Payments.PayByTender (comcash, Variables.CouponText);
				
			}
		}
			


	}
}

