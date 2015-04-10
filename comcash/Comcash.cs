using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Forms;
using System.Diagnostics;
using System.ComponentModel;
using System.Reflection;
using System.Threading;
using System.IO; 
using TestStack;
using TestStack.White.Recording;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems;
using TestStack.White.UIItems.MenuItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.TreeItems;
using TestStack.White.UIItems.WindowStripControls;
using comcash;



class Start
{


	static void Main()
	{
		//Initialization TestData
		var newTest = new TestData ();
		newTest.PingInternet ();
		if (!newTest.messBox ("Start Test Execution?")) 
			return;

		//Counting Test Cases
		string[] TestCases = newTest.GetTestCases ();
		if (TestCases.Length == 0) {
			newTest.messBox ("ERROR: Can't find Test Cases");
			newTest.Logger ("<td><font color=\"red\">ERROR: Can't find Test Cases</font></td></tr>");
			System.Environment.Exit(1);
		}

		newTest.Logger ("<td>Test Execution Started</td></tr>");

		//app initialization
		TestStack.White.Application app;
		app = newTest.Launch ();
		if (newTest.GetFail ()) {
			app.Close ();
			newTest.setKilled (true);
		}

		//Parsing Test Cases and perform them
		for (int i = 0; i < TestCases.Length; i++) {

			string[] valueTestCases = newTest.ParseTS (TestCases [i]);

			if (newTest.getKilled()) {
				app = newTest.Launch ();
				newTest.setKilled (true);
				if (newTest.GetFail ()) {
					app.Close ();
				    newTest.setKilled (true);
					break;
				}
			}

			//Do commands from the test case;
			newTest.Logger("<td>Start execute: <a href=\"" + TestCases[i] + "\">" + TestCases[i] + "</a></td></tr>");
			for (int x = 0; x < valueTestCases.Length; x++) {
			     
				valueTestCases [x] = valueTestCases [x].ToLower ();
				valueTestCases [x] = valueTestCases [x].Trim ();

				if (newTest.GetFail ()) {
					app.Close ();
					newTest.setKilled (true);
					newTest.SetFail (false);
					newTest.Logger ("<td><font color=\"red\">Error in the line: " + x + "</font></td></tr>");
					break;
				} else if (valueTestCases [x].StartsWith ("//") | valueTestCases [x].ToString () == "")
					continue;
				else if (valueTestCases [x].StartsWith ("replay")) {
					string arg = newTest.GetArgument (valueTestCases [x]);
					if (arg == "") {
						newTest.Logger ("<td><font color=\"red\">ERROR: Empty argument</font></td></tr>");
						newTest.SetFail (true);
						continue;
					}
					int val = int.Parse (arg);
					newTest.setReplay (val);
					newTest.setReplayPoint (x);
					continue;
				} else if (valueTestCases [x].StartsWith ("endreplay")) {
					newTest.setReplay (newTest.getReplay () - 1);
					if (newTest.getReplay () > 0) {
						x = newTest.getReplayPoint ();
						continue;
					} else
						continue;
				} else if (valueTestCases [x].Contains ("login")) {
					string pass = newTest.GetArgument (valueTestCases [x]);
					if (pass == "") {
						newTest.Logger ("<td><font color=\"red\">ERROR: Empty argument</font></td></tr>");
						newTest.SetFail (true);
						continue;
					}
					app = newTest.LogIn (app, pass);
					continue;
				} else if (valueTestCases [x].Contains ("logout")) {
					app = newTest.LogOut (app);
				} else if (valueTestCases [x].Contains ("addprod")) {
					string pr = newTest.GetArgument (valueTestCases [x]);
					if (pr == "") {
						newTest.Logger ("<td><font color=\"red\">ERROR: Empty argument</font></td></tr>");
						newTest.SetFail (true);
						continue;
					}
					app = newTest.AddProd (app, pr);
				} else if (valueTestCases [x].Contains ("paycash")) {
					string value = newTest.GetArgument (valueTestCases [x]);
					app = newTest.payByCash (app, value);
					continue;
				} else if (valueTestCases [x].Contains ("paycard")) {
					string value = newTest.GetArgument (valueTestCases [x]);
					app = newTest.payByCard (app, value);
					continue;
				} else if (valueTestCases [x].Contains ("paycoupon")) {
					string value = newTest.GetArgument (valueTestCases [x]);
					app = newTest.payByCoupon (app, value);
					continue;
				} else if (valueTestCases [x].Contains ("payar")) {
					string value = newTest.GetArgument (valueTestCases [x]);
					app = newTest.payByAR (app, value);
					continue;
				} else if (valueTestCases [x].Contains ("paystore")) {
					string value = newTest.GetArgument (valueTestCases [x]);
					app = newTest.payByCoupon (app, value);
					continue;
				} else if (valueTestCases [x].Contains ("paygift")) {
					string value = newTest.GetArgument (valueTestCases [x]);
					app = newTest.payByGift (app, value);
					continue;
				} else if (valueTestCases [x].Contains ("addcustomer")) {
					string cust = newTest.GetArgument (valueTestCases [x]);
					if (cust == "") {
						newTest.Logger ("<td><font color=\"red\">ERROR: Empty argument</font></td></tr>");
						newTest.SetFail (true);
						continue;
					}
					app = newTest.AddCustomer (app, cust);
					continue;
				
				} else if (valueTestCases [x].Contains ("internet")) {
					string status = newTest.GetArgument (valueTestCases [x]);
					if (status == "") {
						newTest.Logger ("<td><font color=\"red\">ERROR: Empty argument</font></td></tr>");
						newTest.SetFail (true);
						continue;
					}
					newTest.Internet (status);
					continue;
				} else if (valueTestCases [x].Contains ("addmod")) {
					string mod = newTest.GetArgument (valueTestCases [x]);
					if (mod == "") {
						newTest.Logger ("<td><font color=\"red\">ERROR: Empty argument</font></td></tr>");
						newTest.SetFail (true);
						continue;
					}
					app = newTest.addModifiers (app, mod);
					continue;
				} else if (valueTestCases [x].Contains ("suspend")) {
					app = newTest.Suspend (app);
					continue;
				} else if (valueTestCases [x].StartsWith ("return")) {
					string ar = newTest.GetArgument (valueTestCases [x]);
					app = newTest.Return (app, ar);
					continue;
				} else if (valueTestCases [x].Contains ("recall")) {
					string arg = newTest.GetArgument (valueTestCases [x]);
					app = newTest.recallSuspend (app, arg);
					continue;
				} else if (valueTestCases [x].Contains ("checkresp")) {
					string ar = newTest.GetArgument (valueTestCases [x]);
					newTest.checkResponse (ar);
					continue;
				} else if (valueTestCases [x].Contains ("notax")) {
					string arg = newTest.GetArgument (valueTestCases [x]);
					if (arg == "") {
						newTest.Logger ("<td><font color=\"red\">ERROR: Empty argument</font></td></tr>");
						newTest.SetFail (true);
						continue;
					}
					app = newTest.NoTax (app, arg);
					continue;
				} else if (valueTestCases [x].Contains ("removeprod")) {
					string arg = newTest.GetArgument (valueTestCases [x]);
					if (arg == "") {
						newTest.Logger ("<td><font color=\"red\">ERROR: Empty argument</font></td></tr>");
						newTest.SetFail (true);
						continue;
					}
					app = newTest.RemoveProd (app, arg);
					continue;
				} else if (valueTestCases [x].Contains ("prodprice")) {
					string arg = newTest.GetArgument (valueTestCases [x]);
					if (arg == "") {
						newTest.Logger ("<td><font color=\"red\">ERROR: Empty argument</font></td></tr>");
						newTest.SetFail (true);
						continue;
					}
					app = newTest.ChangeProdPrice (app, arg);
					continue;
				} else if (valueTestCases [x].Contains ("proddiscount")) {
					string arg = newTest.GetArgument (valueTestCases [x]);
					if (arg == "") {
						newTest.Logger ("<td><font color=\"red\">ERROR: Empty argument</font></td></tr>");
						newTest.SetFail (true);
						continue;
					}
					app = newTest.SetProdDiscount (app, arg);
					continue;
				} else if (valueTestCases [x].Contains ("alldiscount")) {
					string arg = newTest.GetArgument (valueTestCases [x]);
					if (arg == "") {
						newTest.Logger ("<td><font color=\"red\">ERROR: Empty argument</font></td></tr>");
						newTest.SetFail (true);
						continue;
					}
					app = newTest.SetAllDiscount (app, arg);
					continue;
				} else if (valueTestCases [x].Contains ("plu")) {
					string arg = newTest.GetArgument (valueTestCases [x]);
					if (arg == "" || arg.Length > 4) {
						newTest.Logger ("<td><font color=\"red\">ERROR: Incorrect argument in PLU command</font></td></tr>");
						newTest.SetFail (true);
						continue;
					}
					app = newTest.PLU (app, arg);
					continue;
				} else if (valueTestCases [x].Contains ("checkamount")) {
					string arg = newTest.GetArgument (valueTestCases [x]);
					if (arg == "") {
						newTest.Logger ("<td><font color=\"red\">ERROR: Empty argument</font></td></tr>");
						newTest.SetFail (true);
						continue;
					}
					app = newTest.CheckAmount (app, arg);
					continue;
				} else if (valueTestCases [x].Contains ("voidsale")) {
					app = newTest.VoidSale (app);
					continue;
				} else if (valueTestCases [x].Contains ("closeout")) {
					app = newTest.CloseOut (app);
					continue;
				} else if (valueTestCases [x].Contains ("simplereturn")) {
					app = newTest.SimpleReturn (app);
					continue;
				} else if (valueTestCases [x].StartsWith ("continue")) {
					app = newTest.Continue (app);
					continue;
				} else if (valueTestCases [x].StartsWith ("zeroprice")) {
					string arg = newTest.GetArgument (valueTestCases [x]);
					if (arg == "") {
						newTest.Logger ("<td><font color=\"red\">ERROR: Empty argument</font></td></tr>");
						newTest.SetFail (true);
						continue;
					}
					app = newTest.zeroPrice (app, arg);
					continue;
				} else if (valueTestCases [x].StartsWith ("retprod")) {
					string arg = newTest.GetArgument (valueTestCases [x]);
					if (arg == "") {
						newTest.Logger ("<td><font color=\"red\">ERROR: Empty argument</font></td></tr>");
						newTest.SetFail (true);
						continue;
					}
					app = newTest.ReturnProd (app, arg);
					continue;
				} else if (valueTestCases [x].StartsWith ("qty")) {
					string arg = newTest.GetArgument (valueTestCases [x]);
					if (arg == "") {
						newTest.Logger ("<td><font color=\"red\">ERROR: Empty argument</font></td></tr>");
						newTest.SetFail (true);
						continue;
					}
					var str = arg.Split (new Char [] { ',' });
					var prod = str [0].Trim ().ToLower ();
					var val = str [1].Trim ().ToLower ();
					app = newTest.AddQTY (app, prod, val);
					continue;
				} else if (valueTestCases [x].StartsWith ("qt")) {
					string arg = newTest.GetArgument (valueTestCases [x]);
					if (arg == "") {
						newTest.Logger ("<td><font color=\"red\">ERROR: Empty argument</font></td></tr>");
						newTest.SetFail (true);
						continue;
					}
					app = newTest.AddQTYbyButton (app, arg);
					continue;
				}

				else {
					newTest.Logger ("<td><font color=\"red\">Unknown command: <" + valueTestCases[x] + ">, test case stopped</font></td></tr>");
					break;
				}

				}
			newTest.Logger ("<td>End case execution</td></tr>");
			Thread.Sleep (1000);
			}
		newTest.Logger ("<td><font color=\"red\">Errors: " + newTest.Errors + "</font></td></tr>");
	    newTest.messBox ("Test Execution end \rErrors: " + newTest.Errors);
		newTest.deleteListener ();

		}

	}




	
