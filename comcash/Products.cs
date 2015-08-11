using System;
using System.Threading;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;

namespace comcash
{
	public static class Products
	{

		public static TestStack.White.Application AddProd(TestStack.White.Application comcash, string arg){

			Window win = comcash.GetWindow(SearchCriteria.ByAutomationId(Variables.MainWindowId),TestStack.White.Factory.InitializeOption.NoCache);

			try{
				string prod, qty = null;

				if (arg.Contains(",")){
					var str = arg.Split(new Char [] {','});
					prod = str[0].Trim().ToLower();
					qty = str[1].Trim().ToLower();
				} else {
					prod = arg;
				}

				var SearchField = win.Get<TestStack.White.UIItems.TextBox> (SearchCriteria.ByAutomationId (Variables.SearchTextBoxId));
				var ClearBut = win.Get<TestStack.White.UIItems.Button> (SearchCriteria.ByAutomationId (Variables.ClearSearchButtonId));
				ClearBut.Click ();
				win.WaitWhileBusy();
				Thread.Sleep(500);
				SearchField.Enter(prod);
				Thread.Sleep(1000);
				var listBox = win.Get<TestStack.White.UIItems.ListBoxItems.ListBox>(SearchCriteria.ByAutomationId(Variables.ProductsListBoxId));
				var listItem = listBox.Items.Find(item=>item.Text.ToLower().StartsWith(prod));
				Thread.Sleep(1000);
				listItem.Click();
				win.WaitWhileBusy();
				Thread.Sleep(1000);

				if (qty != null)
					AddQTY(comcash, prod, qty);

				return comcash;
			}

			catch (Exception e){
				Log.Error(e.ToString(), true);
				return comcash;
			}


		}


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

		public static Window OpenProduct (TestStack.White.Application comcash, string ProdName)
		{
			Window win = comcash.GetWindow (SearchCriteria.ByAutomationId (Variables.MainWindowId), TestStack.White.Factory.InitializeOption.NoCache);
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

		public static Window SaveProdDetails (Window win)
		{
			var saveButton = win.Get<TestStack.White.UIItems.Button> (SearchCriteria.ByAutomationId (Variables.SaveProductDetailsButtonId));
			saveButton.Click();
			Thread.Sleep(1000);
			return win;
		}

	}
}

