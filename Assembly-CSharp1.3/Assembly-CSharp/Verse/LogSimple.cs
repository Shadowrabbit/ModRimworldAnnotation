using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000497 RID: 1175
	public static class LogSimple
	{
		// Token: 0x060023D6 RID: 9174 RVA: 0x000DF3DC File Offset: 0x000DD5DC
		public static void Message(string text)
		{
			for (int i = 0; i < LogSimple.tabDepth; i++)
			{
				text = "  " + text;
			}
			LogSimple.messages.Add(text);
		}

		// Token: 0x060023D7 RID: 9175 RVA: 0x000DF411 File Offset: 0x000DD611
		public static void BeginTabMessage(string text)
		{
			LogSimple.Message(text);
			LogSimple.tabDepth++;
		}

		// Token: 0x060023D8 RID: 9176 RVA: 0x000DF425 File Offset: 0x000DD625
		public static void EndTab()
		{
			LogSimple.tabDepth--;
		}

		// Token: 0x060023D9 RID: 9177 RVA: 0x000DF434 File Offset: 0x000DD634
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

		// Token: 0x060023DA RID: 9178 RVA: 0x000DF4C8 File Offset: 0x000DD6C8
		public static void FlushToStandardLog()
		{
			if (LogSimple.messages.Count == 0)
			{
				return;
			}
			Log.Message(LogSimple.CompiledLog());
			LogSimple.messages.Clear();
		}

		// Token: 0x060023DB RID: 9179 RVA: 0x000DF4EC File Offset: 0x000DD6EC
		private static string CompiledLog()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string value in LogSimple.messages)
			{
				stringBuilder.AppendLine(value);
			}
			return stringBuilder.ToString().TrimEnd(Array.Empty<char>());
		}

		// Token: 0x0400169F RID: 5791
		private static List<string> messages = new List<string>();

		// Token: 0x040016A0 RID: 5792
		private static int tabDepth = 0;
	}
}
