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
using System.Collections.Generic;

namespace comcash
{
	partial class TestData
	{
		public void Internet(string st)
		{
			System.Diagnostics.ProcessStartInfo InOff = new System.Diagnostics.ProcessStartInfo ("netsh", "wlan disconnect");
			System.Diagnostics.ProcessStartInfo InOn = new System.Diagnostics.ProcessStartInfo ("netsh", "wlan connect name = MD");
			System.Diagnostics.Process p = new System.Diagnostics.Process ();

			if (st.StartsWith ("off") && PingInternet ()) {
				p.StartInfo = InOff;
				p.Start ();
				Thread.Sleep (1000);
				if (PingInternet ()) {
					SetFail (true);
					Logger ("<td><font color=\"red\">ERROR: can't switch off the Internet</font></td></tr>");
				}
				Thread.Sleep (1000);

			} else if (st.StartsWith ("on") && !PingInternet ()) {
				p.StartInfo = InOn;
				p.Start ();
				Thread.Sleep (1000);
				if (!PingInternet ()) {
					SetFail (true);
					Logger ("<td><font color=\"red\">ERROR: can't switch on the Internet</font></td></tr>");
				}
				Thread.Sleep (1000);
			}
		}


		public bool PingInternet()
		{
			try{
				return System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
			}
			catch (Exception e){
				Logger (e.Message);
				return false;
			}
		}
	}
}

