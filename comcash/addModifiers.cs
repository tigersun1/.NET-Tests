using System;
using System.Threading;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;

namespace comcash
{
	partial class TestData
	{
		public TestStack.White.Application addModifiers (TestStack.White.Application comcash, string arg)
		{
			Window win = comcash.GetWindow(SearchCriteria.ByAutomationId(Variables.MainWindowId), TestStack.White.Factory.InitializeOption.NoCache);

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
	}
}

