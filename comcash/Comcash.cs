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
			
			//Check if there is a fail or empty/commentet line

				 if (newTest.GetFail ()) {
					app.Close ();
					newTest.setKilled (true);
					newTest.SetFail (false);
					newTest.Logger ("<td><font color=\"red\">Error in the line:" + x + " -- " + valueTestCases[x-1] + "</font></td></tr>");
					break;
				} else if (valueTestCases[x].StartsWith ("//") || valueTestCases[x] == "")
					continue;



				string [] use = newTest.GetArgument (valueTestCases [x]);
				string command = use [0];
				string arg = use [1];

				if (command == "replay") {
					if (arg == "") {
						newTest.ErrorEmptyArgument ();
						continue;
					}
					int val = int.Parse (arg);
					newTest.setReplay (val);
					newTest.setReplayPoint (x);
					continue;
				} else if (command == "endreplay") {
					newTest.setReplay (newTest.getReplay () - 1);
					if (newTest.getReplay () > 0) {
						x = newTest.getReplayPoint ();
						continue;
					} else
						continue;
				} else if (command == "login") {
					if (arg == "") 
						newTest.ErrorEmptyArgument();
					else
						app = newTest.LogIn (app, arg);
					continue;
				} else if (command == "logout") {
					app = newTest.LogOut (app);
					continue;
				} else if (command == "addprod") {
					if (arg == "") 
						newTest.ErrorEmptyArgument ();
					else
						app = newTest.AddProd (app, arg);
					continue;
				} else if (command == "paycash") {
					app = newTest.payByCash (app, arg);
					continue;
				} else if (command == "paycard") {
					app = newTest.payByCard (app, arg);
					continue;
				} else if (command == "paycoupon") {
					app = newTest.payByCoupon (app, arg);
					continue;
				} else if (command == "payar") {
					app = newTest.payByAR (app, arg);
					continue;
				} else if (command == "paystore") {
					app = newTest.payByCoupon (app, arg);
					continue;
				} else if (command == "paygift") {
					app = newTest.payByGift (app, arg);
					continue;
				} else if (command =="addcustomer") {
					if (arg == "") 
						newTest.ErrorEmptyArgument ();
					else
						app = newTest.AddCustomer (app, arg);
					continue;
				} else if (command == "internet") {
					if (arg == "") 
						newTest.ErrorEmptyArgument ();
					else
						newTest.Internet (arg);
					continue;
				} else if (command == "addmod") {
					if (arg == "") 
						newTest.ErrorEmptyArgument ();
					else
						app = newTest.addModifiers (app, arg);
					continue;
				} else if (command == "suspend") {
					app = newTest.Suspend (app);
					continue;
				} else if (command == "return") {
					app = newTest.Return (app, arg);
					continue;
				} else if (command == "recall") {
					app = newTest.recallSuspend (app, arg);
					continue;
				} else if (command == "checkresp") {
					newTest.checkResponse (arg);
					continue;
				} else if (command == "notax") {
					if (arg == "") 
						newTest.ErrorEmptyArgument ();
					else
						app = newTest.NoTax (app, arg);
					continue;
				} else if (command == "removeprod") {
					if (arg == "") 
						newTest.ErrorEmptyArgument ();
					else
						app = newTest.RemoveProd (app, arg);
					continue;
				} else if (command == "prodprice") {
					if (arg == "") 
						newTest.ErrorEmptyArgument ();
					else
						app = newTest.ChangeProdPrice (app, arg);
					continue;
				} else if (command == "proddiscount") {
					if (arg == "") 
						newTest.ErrorEmptyArgument ();
					else
						app = newTest.SetProdDiscount (app, arg);
					continue;
				} else if (command == "alldiscount") {
					if (arg == "") 
						newTest.ErrorEmptyArgument ();
					else
						app = newTest.SetAllDiscount (app, arg);
					continue;
				} else if (command == "addbyplu") {
					if (arg == "" || arg.Length > 4) 
						newTest.ErrorEmptyArgument ();
					else
						app = newTest.PLU (app, arg);
					continue;
				} else if (command == "checkamount") {
					if (arg == "") 
						newTest.ErrorEmptyArgument ();
					else
						app = newTest.CheckAmount (app, arg);
					continue;
				} else if (command == "voidsale") {
					app = newTest.VoidSale (app);
					continue;
				} else if (command == "closeout") {
					app = newTest.CloseOut (app);
					continue;
				} else if (command == "simplereturn") {
					app = newTest.SimpleReturn (app);
					continue;
				} else if (command == "continue") {
					app = newTest.Continue (app);
					continue;
				} else if (command == "zeroprice") {
					if (arg == "") 
						newTest.ErrorEmptyArgument ();
					else
						app = newTest.zeroPrice (app, arg);
					continue;
				} else if (command == "returnprod") {
					if (arg == "") 
						newTest.ErrorEmptyArgument ();
					else
						app = newTest.ReturnProd (app, arg);
					continue;
				} else if (command == "qty") {
					if (arg == "") {
						newTest.ErrorEmptyArgument ();
						continue;
					}
					var str = arg.Split (new Char [] { ',' });
					var prod = str [0].Trim ().ToLower ();
					var val = str [1].Trim ().ToLower ();
					app = newTest.AddQTY (app, prod, val);
					continue;
				} else if (command == "qt") {
					if (arg == "") 
						newTest.ErrorEmptyArgument ();
					else
						app = newTest.AddQTYbyButton (app, arg);
					continue;
				}

				else {
					newTest.Logger ("<td><font color=\"red\">Unknown command: <" + valueTestCases[x] + ">, test case stopped</font></td></tr>");
					break;
				}



			}

			if (newTest.GetFail ()) {
				app.Close ();
				newTest.setKilled (true);
				newTest.Logger ("<td><font color=\"red\">Error in the last line</font></td></tr>");

			}

			newTest.Logger ("<td>End case execution</td></tr>");
			Thread.Sleep (1000);
			}
		newTest.Logger ("<td><font color=\"red\">Errors: " + newTest.Errors + "</font></td></tr>");
	    newTest.messBox ("Test Execution end \rErrors: " + newTest.Errors);
		newTest.deleteListener ();

		}

	}




	
