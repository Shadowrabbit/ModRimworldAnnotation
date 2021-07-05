using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Verse
{
	// Token: 0x02000320 RID: 800
	public static class SaveGameFilesUtility
	{
		// Token: 0x060016E6 RID: 5862 RVA: 0x00086DE0 File Offset: 0x00084FE0
		public static bool IsAutoSave(string fileName)
		{
			return fileName.Length >= 8 && fileName.Substring(0, 8) == "Autosave";
		}

		// Token: 0x060016E7 RID: 5863 RVA: 0x00086E00 File Offset: 0x00085000
		public static bool SavedGameNamedExists(string fileName)
		{
			using (IEnumerator<string> enumerator = (from f in GenFilePaths.AllSavedGameFiles
			select Path.GetFileNameWithoutExtension(f.Name)).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current == fileName)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060016E8 RID: 5864 RVA: 0x00086E78 File Offset: 0x00085078
		public static string UnusedDefaultFileName(string factionLabel)
		{
			int num = 1;
			string text;
			do
			{
				text = factionLabel + num.ToString();
				num++;
			}
			while (SaveGameFilesUtility.SavedGameNamedExists(text));
			return text;
		}

		// Token: 0x060016E9 RID: 5865 RVA: 0x00086EA8 File Offset: 0x000850A8
		public static FileInfo GetAutostartSaveFile()
		{
			if (!Prefs.DevMode)
			{
				return null;
			}
			return GenFilePaths.AllSavedGameFiles.FirstOrDefault((FileInfo x) => Path.GetFileNameWithoutExtension(x.Name).ToLower() == "autostart".ToLower());
		}
	}
}
