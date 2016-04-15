using System;
using System.Threading;
using System.Collections.Generic;
using Fiddler;

namespace comcash
{
	public class FiddlerCore
	{
		public static List<Session> oFiddlerSessions = new List<Session>();

		public static void StartFiddlerCore(){

			try{
				if (!CertMaker.rootCertExists())
				{
					if (!CertMaker.createRootCert())
					{
						Log.Error("Unable to create cert for FiddlerCore.", false);
					}
				}

				if (!CertMaker.rootCertIsTrusted())
				{
					if (!CertMaker.trustRootCert())
					{
						Log.Error("Unable to install FiddlerCore's cert.", false);
					}
				}

				FiddlerApplication.BeforeRequest += delegate(Session oS) {

					if (oS.HostnameIs(ConfigTest.serverName) && !ConfigTest.connectStatus){
						oS.oRequest.pipeClient.End();
					}
				};

				FiddlerApplication.AfterSessionComplete += delegate(Session oSes) {
					if(oSes.uriContains(ConfigTest.serverName)){
						Monitor.Enter(oFiddlerSessions);
						oFiddlerSessions.Add(oSes);
						Monitor.Exit(oFiddlerSessions);
					}
				};

				FiddlerApplication.Startup(8888, true, true);

				}

				catch (Exception e){
				Log.Error(e.ToString(), false);
				}
			}

		public static void CheckFiddlerCore (string link){
			if (!ConfigTest.connectStatus)
				return;
			
			Session session = oFiddlerSessions.Find (x => x.fullUrl.Contains (link));

			if (session.responseCode != 200)
				ErrorSession (session);
			else if (String.IsNullOrEmpty (session.GetResponseBodyAsString ()))
				ErrorSession (session);
			else if (session.GetResponseBodyAsString ().Contains ("\"error\""))
				ErrorSession (session);

			oFiddlerSessions.Clear ();
		}

		public static void ErrorSession (Session ses){
			Log.Error ("Session error", false);
			Log.Info ("Request\t" + ses.GetRequestBodyAsString ());
			Log.Info ("Response\t" + ses.GetResponseBodyAsString ());
		}

		public static void StopFiddlerCore(){
			FiddlerApplication.Shutdown ();
		}

		}
}

