﻿using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems.Finders;
using System;

namespace comcash
{
	partial class TestData
	{
		public TestStack.White.Application NoTax (TestStack.White.Application comcash, string prod)
		{
			try {
				var win = OpenProduct (comcash, prod);
				var Button = win.Get<TestStack.White.UIItems.Button> (SearchCriteria.ByAutomationId(Variables.NoTaxProductButtonId));
				Button.Click();
				SaveProdDetails(win);
				return comcash;
				
				} catch (Exception e){
					Log.Error(e.ToString(), true);
					return comcash;
				}
			
		}
	}
}
