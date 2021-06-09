using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000808 RID: 2056
	public static class LogSimple
	{
		// Token: 0x060033D0 RID: 13264 RVA: 0x00150D18 File Offset: 0x0014EF18
		public static void Message(string text)
		{
			for (int i = 0; i < LogSimple.tabDepth; i++)
			{
				text = "  " + text;
			}
			LogSimple.messages.Add(text);
		}

		// Token: 0x060033D1 RID: 13265 RVA: 0x00028A2E File Offset: 0x00026C2E
		public static void BeginTabMessage(string text)
		{
			LogSimple.Message(text);
			LogSimple.tabDepth++;
		}

		// Token: 0x060033D2 RID: 13266 RVA: 0x00028A42 File Offset: 0x00026C42
		public static void EndTab()
		{
			LogSimple.tabDepth--;
		}

		// Token: 0x060033D3 RID: 13267 RVA: 0x00150D50 File Offset: 0x0014EF50
		public static void FlushToFileAndOpen()
		{
			if (LogSimple.messages.Count == 0)
			{
				return;
			}
			string value = LogSimple.CompiledLog();
			string path = GenFilePaths.SaveDataFolderPath + Path.DirectorySeparatorChar.ToString() + "LogSimple.txt";
			using (StreamWriter streamWriter = new StreamWriter(path, false))
			{
				streamWriter.Write(value);
			}
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				Application.OpenURL(path);
			});
			LogSimple.messages.Clear();
		}

		// Token: 0x060033D4 RID: 13268 RVA: 0x00028A50 File Offset: 0x00026C50
		public static void FlushToStandardLog()
		{
			if (LogSimple.messages.Count == 0)
			{
				return;
			}
			Log.Message(LogSimple.CompiledLog(), false);
			LogSimple.messages.Clear();
		}

		// Token: 0x060033D5 RID: 13269 RVA: 0x00150DE4 File Offset: 0x0014EFE4
		private static string CompiledLog()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string value in LogSimple.messages)
			{
				stringBuilder.AppendLine(value);
			}
			return stringBuilder.ToString().TrimEnd(Array.Empty<char>());
		}

		// Token: 0x040023F1 RID: 9201
		private static List<string> messages = new List<string>();

		// Token: 0x040023F2 RID: 9202
		private static int tabDepth = 0;
	}
}
