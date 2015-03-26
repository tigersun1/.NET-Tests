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
		//Parsing Test Cases
		TestData newTest = new TestData ();
		newTest.Logger ("<td>Test Execution Started</td></tr>");

		//Counting Test Cases
		string[] TestCases = newTest.GetTestCases ();
		if (TestCases.Length == 0) {
			newTest.Logger ("<td><font color=\"red\">ERROR: Can't find Test Cases</font></td></tr>");
			return;
		}

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
					bool f = valueTestCases [x].StartsWith ("!");
					app = newTest.LogIn (app, pass, f);
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
				} else if (valueTestCases [x].Contains ("qty")) {
					string mod = newTest.GetArgument (valueTestCases [x]);
					if (mod == "") {
						newTest.Logger ("<td><font color=\"red\">ERROR: Empty argument</font></td></tr>");
						newTest.SetFail (true);
						continue;
					}
					app = newTest.AddQTY (app, mod);
					continue;
				} else if (valueTestCases [x].Contains ("return")) {
					string ar = newTest.GetArgument (valueTestCases [x]);
					app = newTest.Return (app, ar);
					continue;
				} else if (valueTestCases [x].Contains ("recall")) {
					string arg = newTest.GetArgument (valueTestCases [x]);
					app = newTest.recallSuspend (app, arg);
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
		}

	}




	
