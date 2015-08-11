using System;
using System.Threading;

namespace comcash {

class Start
{


	static void Main()
	{
			Fiddler.RunFiddler ();
			if (!ConfigTest.messBox ("Start Test Execution?")) 
			return;

			//Counting Test Cases
			string[] TestCases = ConfigTest.GetTestCases ();
			if (TestCases.Length == 0) {
				ConfigTest.messBox ("ERROR: Can't find Test Cases");
				Log.Error("Can't find Test Cases", false);
				Environment.Exit(1);
			}
				
			Log.Info("Test Execution Started");

			//app initialization
			TestStack.White.Application app;
			app = ConfigTest.Launch ();
			if(Log.getFail()){
				app.Close ();
				ConfigTest.setKilled (true);
			}

		
//			int caunter = 0;
//			DateTime stop = new DateTime (2015, 6, 22, 10, 0, 0);
//			LoadTests Test = new LoadTests (app);
//			app = newTest.LogIn (app, "7777");
//
//			app = newTest.CloseOut (app);
//
//			app = Customer.AddCustomer (app, "john");
//			app = Products.AddProd (app, "cedar");
//			app = Payments.PayByTender (app, "ar, 700");
//			app = Payments.PayByTender (app, "gift, 12345qwert");
//			caunter++;
//
//			Inet.Internet ("off");
//			for (int x = 1; x < 3; x++) {
//				Test.Prod ();
//				Payments.PayByCash (app, "");
//				caunter++;
//			}
//			app = newTest.LogOut (app);
//			Inet.Internet ("on");
//			app = newTest.LogIn (app, "7777");
//
//			for (int x = 1; x < 500; ) {
//				Test.Prod ();
//				Test.Tenders ();
//				caunter++;
//				if (DateTime.Compare (DateTime.Now, stop) > 0)
//					break;
//			}
//
//			newTest.CloseOut (app);
//			Log.Info ("Sales = " + caunter.ToString ());


		//Parsing Test Cases and perform them
		for (int i = 0; i < TestCases.Length; i++) {

				string[] valueTestCases = ConfigTest.ParseTS (TestCases [i]);

			if (ConfigTest.getKilled()) {
				app = ConfigTest.Launch ();
				ConfigTest.setKilled (true);
				if (Log.getFail()){ 
					app.Close ();
					ConfigTest.setKilled (true);
					break;
				}
			}

			//Do commands from the test case;
			Log.Link("Start execute: ", TestCases[i], TestCases[i]);
			for (int x = 0; x < valueTestCases.Length; x++) {
			
			//Check if there is a fail or empty/commentet line

				if(Log.getFail()){
					app.Close ();
					ConfigTest.setKilled (true);
					Log.ResetFail ();
					Log.Info("Error in the line:" + x + " -- " + valueTestCases[x-1]);
					break;
				} else if (valueTestCases[x].StartsWith ("//") || valueTestCases[x] == "")
					continue;



					string [] use = ConfigTest.GetArgument (valueTestCases [x]);
				string command = use [0];
				string arg = use [1];

					if (command == "replay") {
						if (arg == "") {
							ConfigTest.ErrorEmptyArgument ();
							continue;
						}
						int val = int.Parse (arg);
						ConfigTest.setReplay (val);
						ConfigTest.setReplayPoint (x);
						continue;
					} else if (command == "endreplay") {
						ConfigTest.setReplay (ConfigTest.getReplay () - 1);
						if (ConfigTest.getReplay () > 0) {
							x = ConfigTest.getReplayPoint ();
							continue;
						} else
							continue;
					} else if (command == "login") {
						if (arg == "")
							ConfigTest.ErrorEmptyArgument ();
						else
							app = LogInOut.LogIn (app, arg);
						continue;
					} else if (command == "logout") {
						app = LogInOut.LogOut (app);
						continue;
					} else if (command == "addprod") {
						if (arg == "")
							ConfigTest.ErrorEmptyArgument ();
						else
							app = Product.AddProd (app, arg);
						continue;
					} else if (command == "paycash") {
						app = Payments.PayByCash (app, arg);
						continue;
					} else if (command == "paycard") {
						app = Payments.PayByCard (app, arg);
						continue;
					} else if (command == "addcustomer") {
						if (arg == "")
							ConfigTest.ErrorEmptyArgument ();
						else
							app = Customer.AddCustomer(app, arg);
						continue;
					} else if (command == "internet") {
						Inet.Internet (arg);
						continue;
					} else if (command == "addmod") {
						if (arg == "")
							ConfigTest.ErrorEmptyArgument ();
						else
							app = Product.addModifiers (app, arg);
						continue;
					} else if (command == "suspend") {
						app = Suspends.Suspend (app);
						continue;
					} else if (command == "return") {
						app = AdminTab.Return (app, arg);
						continue;
					} else if (command == "recall") {
						app = Suspends.recallSuspend (app, arg);
						continue;
					} else if (command == "checkresp") {
						Fiddler.checkResponse (arg);
						continue;
					} else if (command == "notax") {
						if (arg == "")
							ConfigTest.ErrorEmptyArgument ();
						else
							app = Product.NoTax (app, arg);
						continue;
					} else if (command == "removeprod") {
						if (arg == "")
							ConfigTest.ErrorEmptyArgument ();
						else
							app = Product.RemoveProd (app, arg);
						continue;
					} else if (command == "prodprice") {
						if (arg == "")
							ConfigTest.ErrorEmptyArgument ();
						else
							app = Product.ChangeProdPrice (app, arg);
						continue;
					} else if (command == "proddiscount") {
						if (arg == "")
							ConfigTest.ErrorEmptyArgument ();
						else
							app = Product.SetProdDiscount (app, arg);
						continue;
					} else if (command == "alldiscount") {
						if (arg == "")
							ConfigTest.ErrorEmptyArgument ();
						else
							app = Payments.SetAllDiscount (app, arg);
						continue;
					} else if (command == "addbyplu") {
						if (arg == "" || arg.Length > 4)
							ConfigTest.ErrorEmptyArgument ();
						else
							app = Product.PLU (app, arg);
						continue;
					} else if (command == "checkamount") {
						if (arg == "")
							ConfigTest.ErrorEmptyArgument ();
						else
							app = Payments.CheckAmount (app, arg);
						continue;
					} else if (command == "voidsale") {
						app = Payments.VoidSale (app);
						continue;
					} else if (command == "closeout") {
						app = AdminTab.CloseOut (app);
						continue;
					} else if (command == "simplereturn") {
						app = AdminTab.SimpleReturn (app);
						continue;
					} else if (command == "continue") {
						app = AdminTab.Continue (app);
						continue;
					} else if (command == "zeroprice") {
						if (arg == "")
							ConfigTest.ErrorEmptyArgument ();
						else
							app = Product.zeroPrice (app, arg);
						continue;
					} else if (command == "returnprod") {
						if (arg == "")
							ConfigTest.ErrorEmptyArgument ();
						else
							app = AdminTab.ReturnProd (app, arg);
						continue;
					} else if (command == "qty") {
						if (arg == "") {
							ConfigTest.ErrorEmptyArgument ();
							continue;
						}
						var str = arg.Split (new Char [] { ',' });
						var prod = str [0].Trim ().ToLower ();
						var val = str [1].Trim ().ToLower ();
						app = Product.AddQTY (app, prod, val);
						continue;
					} else if (command == "qt") {
						if (arg == "")
							ConfigTest.ErrorEmptyArgument ();
						else
							app = Product.AddQTYbyButton (app, arg);
						continue;
					} else if (command == "paytender") {
						app = Payments.PayByTender (app, arg);
						continue;
					}

				else {
					Log.Error("Unknown command: <" + valueTestCases[x] + ">, test case stopped", false);
					break;
				}



			}

			if(Log.getFail()){
				app.Close ();
				ConfigTest.setKilled (true);
				Log.ResetFail ();
				Log.Info("Error in the line:" + valueTestCases.Length + " -- " + valueTestCases[valueTestCases.Length-1]);

			}
					
			Log.Info("End case execution");
			Thread.Sleep (1000);
			}
		Log.Info("Errors: " + Log.Errors);
		Fiddler.CloseFiddler ();
			ConfigTest.messBox ("Test Execution end \rErrors: " + Log.Errors);
			ConfigTest.deleteListener ();

		}

	}

}


	
