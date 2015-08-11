using System;
using System.Threading;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;

namespace comcash
{
	public static class Product
	{

		//Opens product from the sale list
		//comcash - application var, ProdName - product name
		public static Window OpenProduct (TestStack.White.Application comcash, string ProdName)
		{
			
			var win = ConfigTest.getWindow(comcash);
			try{
				var listBox = win.Get<TestStack.White.UIItems.ListBoxItems.ListBox>(SearchCriteria.ByAutomationId(Variables.BuyHomeListBoxId));
				var listItem = listBox.Items.Find(item=>item.Text.ToLower().StartsWith(ProdName));
				Thread.Sleep(1000);
				listItem.Click();
				win.WaitWhileBusy();
				Thread.Sleep(1000);

				return win;

			}catch (Exception e){
				Log.Error(e.ToString(), true);
				return win;
			}
		}
			
		//Add product quantity through product menu
		//comcash - application var, prod - product name, qty - product quantity
		public static TestStack.White.Application AddQTY (TestStack.White.Application comcash, string prod, string qty)
		{
			try{

				var win = OpenProduct(comcash, prod);
				Payments.EnterAmount (win, qty);
				SaveProdDetails (win); 

				return comcash;

			}
			catch (Exception e){
				Log.Error(e.ToString(), true);
				return comcash;
			}

		}

		//Adds product by searching 
		//comcash - application var, arg - product name
		public static TestStack.White.Application AddProd(TestStack.White.Application comcash, string arg){

			var win = ConfigTest.getWindow (comcash);

			try {
				string prod, qty = null;

				if (arg.Contains (",")) {
					var str = arg.Split (new Char [] { ',' });
					prod = str [0].Trim ().ToLower ();
					qty = str [1].Trim ().ToLower ();
				} else {
					prod = arg;
				}

				var SearchField = win.Get<TestStack.White.UIItems.TextBox> (SearchCriteria.ByAutomationId (Variables.SearchTextBoxId));
				var ClearBut = win.Get<TestStack.White.UIItems.Button> (SearchCriteria.ByAutomationId (Variables.ClearSearchButtonId));
				ClearBut.Click ();
				win.WaitWhileBusy ();
				Thread.Sleep (500);
				SearchField.Enter (prod);
				Thread.Sleep (1000);
				var listBox = win.Get<TestStack.White.UIItems.ListBoxItems.ListBox> (SearchCriteria.ByAutomationId (Variables.ProductsListBoxId));
				var listItem = listBox.Items.Find (item => item.Text.ToLower ().StartsWith (prod));
				Thread.Sleep (1000);
				listItem.Click ();
				win.WaitWhileBusy ();
				Thread.Sleep (1000);

				if (qty != null)
					AddQTY (comcash, prod, qty);

				return comcash;
			} catch (Exception e) {
				Log.Error (e.ToString (), true);
				return comcash;
			}
		}


		//Clicks on 'Save' button in the product menu 
		//win - window var
		public static Window SaveProdDetails (Window win)
			{
				var saveButton = win.Get<TestStack.White.UIItems.Button> (SearchCriteria.ByAutomationId (Variables.SaveProductDetailsButtonId));
				saveButton.Click();
				Thread.Sleep(1000);
				return win;
			}


		//Adds product in the calculator vidget
		//comcash - application var, arg - product quantity
		public static TestStack.White.Application AddQTYbyButton (TestStack.White.Application comcash, string arg)
		{
			try{

				var win = ConfigTest.getWindow(comcash);
				Payments.ClickOnHomeButton(win);
				Payments.EnterAmount(win, arg);

				var pluButton = win.Get<TestStack.White.UIItems.Button>(SearchCriteria.ByAutomationId(Variables.QuantityButtonId));
				pluButton.Click();
				Thread.Sleep(300);

				return comcash;

			}catch (Exception e){
				Log.Error(e.ToString(), true);
				return comcash;
			}
		}

		//Changes product price in product menu
		//comcash - application var, args - product name and new price
		public static TestStack.White.Application ChangeProdPrice (TestStack.White.Application comcash, string args)
		{
			try{
				//prod - product name
				//val - new price
				var str = args.Split(new Char [] {','});
				var prod = str[0].Trim().ToLower();
				var val = str[1].Trim().ToLower();

				var win = OpenProduct (comcash, prod);
				var Button = win.Get<TestStack.White.UIItems.Button>(SearchCriteria.ByAutomationId(Variables.ChangeProductPriceButtonId));
				Button.Click();
				Thread.Sleep(300);
				Payments.EnterAmount(win, val);
				SaveProdDetails(win);

				return comcash;

			} catch (Exception e){
				Log.Error(e.ToString(), true);
				return comcash;
			}

		}

		//Removes product from the sales list
		//comcash - application var, prod - product name
		public static TestStack.White.Application RemoveProd (TestStack.White.Application comcash, string prod)
		{
			try{

				var win = OpenProduct(comcash, prod);
				var Button = win.Get<TestStack.White.UIItems.Button> (SearchCriteria.ByAutomationId(Variables.RemoveProductButtonId));
				Button.Click();
				Thread.Sleep(300);
				return comcash;

			} catch (Exception e){
				Log.Error(e.ToString(), true);
				return comcash;
			}
		}

		//sets product discount
		//comcash - application var, args - product name and discount amount
		public static TestStack.White.Application SetProdDiscount (TestStack.White.Application comcash, string args)
		{
			try{
				var str = args.Split(new Char [] {','});
				var prod = str[0].Trim().ToLower();
				var val = str[1].Trim().ToLower();

				var win = OpenProduct(comcash, prod);
				var Button = win.Get<TestStack.White.UIItems.Button>(SearchCriteria.ByAutomationId(Variables.DiscountProductButtonId));
				Button.Click();
				Thread.Sleep(300);
				Payments.EnterAmount(win, val);
				SaveProdDetails(win);

				return comcash;

			}catch (Exception e){
				Log.Error(e.ToString(), true);
				return comcash;
			}
		}

		//enters price for zero-price products
		//comcash - application var, value - product price
		public static TestStack.White.Application zeroPrice (TestStack.White.Application comcash, string value)
		{
			try{
				var win = ConfigTest.getWindow(comcash);
				Payments.EnterAmount(win, value);
				SaveProdDetails(win);
				return comcash;
			}catch (Exception e){
				Log.Error(e.ToString(), true);
				return comcash;
			}
		}

		//adds modifiers
		//comcash - application var, arg - modifiers list
		public static TestStack.White.Application addModifiers (TestStack.White.Application comcash, string arg)
		{
			var win = ConfigTest.getWindow(comcash);

			try{
				string[] args = arg.Split (new Char[] {','}, StringSplitOptions.RemoveEmptyEntries);
				foreach (string s in args) {
					string str = s.Trim ();
					str = str.ToLower ();
					if (str == "save") {
						var saveButton = win.Get<TestStack.White.UIItems.Button> (SearchCriteria.ByAutomationId (Variables.SaveProductDetailsButtonId));
						if (saveButton.Enabled) {
							saveButton.Click ();
							Thread.Sleep (500);
							return comcash;
						} else {
							Log.Error("Can't submit modifiers, SAVE button is disabled", true);
							return comcash;
						}
					}
					else {
						var listBox = win.Get<TestStack.White.UIItems.ListBoxItems.ListBox>(SearchCriteria.ByAutomationId(Variables.ModifiersListBoxId));
						var listItem = listBox.Items.Find(item=>item.Text.ToLower().StartsWith(str));
						Thread.Sleep(500);
						listItem.Click();
						Thread.Sleep(500);
					}
				}
				return comcash;
			}

			catch (Exception e){
				Log.Error(e.ToString(), true);
				return comcash;
			}
		}


		//sets NoTax in the product menu
		//comcash - application var, prod - products name
		public static TestStack.White.Application NoTax (TestStack.White.Application comcash, string prod)
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

		//Adds product by PLU
		//comcash - application var, arg - PLU code
		public static TestStack.White.Application PLU (TestStack.White.Application comcash, string arg)
		{
			try{

				var win = ConfigTest.getWindow(comcash);
				Payments.ClickOnHomeButton(win);
				Payments.EnterAmount(win, arg);

				var pluButton = win.Get<TestStack.White.UIItems.Button>(SearchCriteria.ByAutomationId(Variables.PLUButtonId));
				pluButton.Click();
				Thread.Sleep(300);

				return comcash;

			}catch (Exception e){
				Log.Error(e.ToString(), true);
				return comcash;
			}
		}


	}
}

