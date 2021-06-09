using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Verse
{
	// Token: 0x0200083D RID: 2109
	public static class GenCommandLine
	{
		// Token: 0x060034D0 RID: 13520 RVA: 0x001550C8 File Offset: 0x001532C8
		public static bool CommandLineArgPassed(string key)
		{
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			for (int i = 0; i < commandLineArgs.Length; i++)
			{
				if (string.Compare(commandLineArgs[i], key, true) == 0 || string.Compare(commandLineArgs[i], "-" + key, true) == 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060034D1 RID: 13521 RVA: 0x00155110 File Offset: 0x00153310
		public static bool TryGetCommandLineArg(string key, out string value)
		{
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			for (int i = 0; i < commandLineArgs.Length; i++)
			{
				if (commandLineArgs[i].Contains('='))
				{
					string[] array = commandLineArgs[i].Split(new char[]
					{
						'='
					});
					if (array.Length == 2 && (string.Compare(array[0], key, true) == 0 || string.Compare(array[0], "-" + key, true) == 0))
					{
						value = array[1];
						return true;
					}
				}
			}
			value = null;
			return false;
		}

		// Token: 0x060034D2 RID: 13522 RVA: 0x00155184 File Offset: 0x00153384
		public static void Restart()
		{
			try
			{
				string[] commandLineArgs = Environment.GetCommandLineArgs();
				string fileName = commandLineArgs[0];
				string text = "";
				for (int i = 1; i < commandLineArgs.Length; i++)
				{
					if (!text.NullOrEmpty())
					{
						text += " ";
					}
					text = text + "\"" + commandLineArgs[i].Replace("\"", "\\\"") + "\"";
				}
				new Process
				{
					StartInfo = new ProcessStartInfo(fileName, text)
				}.Start();
				Root.Shutdown();
				LongEventHandler.QueueLongEvent(delegate()
				{
					Thread.Sleep(10000);
				}, "Restarting", true, null, true);
			}
			catch (Exception arg)
			{
				Log.Error("Error restarting: " + arg, false);
				Find.WindowStack.Add(new Dialog_MessageBox("FailedToRestart".Translate(), null, null, null, null, null, false, null, null));
			}
		}
	}
}
