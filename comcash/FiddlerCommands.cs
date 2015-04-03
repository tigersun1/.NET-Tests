using System.Diagnostics;
using System.IO;

namespace comcash
{
	partial class TestData
	{
		public void Fiddler(string arg)
		{
			if (!Directory.Exists(FiddlerPath)) {
				messBox("ERROR: Incorrect fiddler path");
				System.Environment.Exit(1);
			}

			string file = Path.Combine (FiddlerPath, "ExecAction.exe");
			Process proc = Process.Start (file, arg);
			proc.WaitForExit ();
		}
	}
	
}
