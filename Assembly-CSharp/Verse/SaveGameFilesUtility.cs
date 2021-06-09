using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Verse
{
	// Token: 0x020004A3 RID: 1187
	public static class SaveGameFilesUtility
	{
		// Token: 0x06001DA8 RID: 7592 RVA: 0x0001A8A0 File Offset: 0x00018AA0
		public static bool IsAutoSave(string fileName)
		{
			return fileName.Length >= 8 && fileName.Substring(0, 8) == "Autosave";
		}

		// Token: 0x06001DA9 RID: 7593 RVA: 0x000F6E08 File Offset: 0x000F5008
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

		// Token: 0x06001DAA RID: 7594 RVA: 0x000F6E80 File Offset: 0x000F5080
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

		// Token: 0x06001DAB RID: 7595 RVA: 0x0001A8BF File Offset: 0x00018ABF
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
