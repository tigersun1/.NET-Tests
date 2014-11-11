using System;
using System.IO; 


namespace comcash
{
	partial class TestData
	{
		static string partPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "TestLog-" + DateTime.Now.ToString("D") );
		private string LogPath = System.IO.Path.Combine(partPath, "testlogfile.html");


		public void Logger(string Message)
		{
			StreamWriter log;

			if (!System.IO.Directory.Exists(partPath))
				System.IO.Directory.CreateDirectory(partPath);

			if(!File.Exists(LogPath))
			{
				log = new StreamWriter(LogPath,true);
				log.WriteLine("<!DOCTYPE HTML> <html> <head> <meta charset=\"utf-8\"> <title>Test log</title>");
				log.WriteLine ("</head><body><table border=\"1\"><caption>Test log " + DateTime.Now + "</caption>");
			}
			else 
			{
				log = File.AppendText(LogPath);
			}

			log.WriteLine("<tr><td>" + DateTime.Now.ToString("T") + " </td>" + Message);
			log.Close();
		}
	
	}
}

