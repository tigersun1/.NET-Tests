using System;
using System.IO;

namespace comcash
{
	partial class TestData
	{
		public bool checkResponse ()
		{
			if (File.Exists (partPath + "\\validError.txt")) {
				SetFail (true);
				Logger ("<td><font color=\"red\">ERROR: Server validation error</font></td></tr>");
				var body = File.ReadAllLines (partPath + "\\validError.txt");
				string data = "";;
				foreach (string elem in body)
					data += elem;
				Logger ("<td>" + data + "</td></tr>");
				File.Delete (partPath + "\\validError.txt");
				return true;
			} 
			if (File.Exists (partPath + "\\server Error.txt")) {
				File.Delete (partPath + "\\server Error.txt");
				if (!PingInternet ())
					return false;
				SetFail (true);
				Logger ("ERROR: Server response error");
				return true;
			} 
			if (File.Exists (partPath + "\\emptyError.txt")) {
				File.Delete (LogPath + "\\emptyError.txt");
				if (!PingInternet ())
					return false;
				SetFail (true);
				Logger ("ERROR: Empty response");
				return true;
			}

			return false;
		}
	}
}

