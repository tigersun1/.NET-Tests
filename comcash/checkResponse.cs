using System;
using System.IO;

namespace comcash
{
	partial class TestData
	{
		//Returns true if checked session is OK, false if error in session
		public bool checkResponse (string arg)
		{
			if (!PingInternet () || !connectStatus)
				return true;

			Fiddler ("\"request " + arg + "\"");

			if (!File.Exists (listenerPath)) {
				SetFail (true);
				Logger ("No listener file in the folder");
				return false;
			}

			var res = File.ReadAllLines (listenerPath);
			string str;
			if (res.Length > 0)
				str = res [res.Length - 1];
			else {
				SetFail (true);
				Logger ("<td><font color=\"red\">ERROR: Fiddler doesn't respond</font></td></tr>");
				return false;
			}
				
			
			str = str.ToLower ();
			str = str.Trim ();

			if (str == "true")
				return true;
			if (str == "false") {
				SessionErrors = SessionErrors + 1;
				Logger ("<td><font color=\"red\">ERROR: Session error <a href=\"" + partPath + "\\error(" + SessionErrors + ").txt\">" + partPath + "\\error(" + SessionErrors + ").txt</a></font></td></tr>");
				SetFail (true);
				return false;
			} else
				return false;

		}
	}
}

