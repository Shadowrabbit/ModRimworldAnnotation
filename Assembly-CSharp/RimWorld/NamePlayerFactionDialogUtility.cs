using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001A05 RID: 6661
	public static class NamePlayerFactionDialogUtility
	{
		// Token: 0x06009349 RID: 37705 RVA: 0x00062A72 File Offset: 0x00060C72
		public static bool IsValidName(string s)
		{
			return s.Length != 0 && s.Length <= 64 && GenText.IsValidFilename(s) && !GrammarResolver.ContainsSpecialChars(s);
		}

		// Token: 0x0600934A RID: 37706 RVA: 0x002A6DF8 File Offset: 0x002A4FF8
		public static void Named(string s)
		{
			Faction.OfPlayer.Name = s;
			if (Find.GameInfo.permadeathMode)
			{
				string oldSavefileName = Find.GameInfo.permadeathModeUniqueName;
				string newSavefileName = PermadeathModeUtility.GeneratePermadeathSaveNameBasedOnPlayerInput(s, oldSavefileName);
				if (oldSavefileName != newSavefileName)
				{
					Func<FileInfo, bool> <>9__1;
					LongEventHandler.QueueLongEvent(delegate()
					{
						Find.GameInfo.permadeathModeUniqueName = newSavefileName;
						Find.Autosaver.DoAutosave();
						IEnumerable<FileInfo> allSavedGameFiles = GenFilePaths.AllSavedGameFiles;
						Func<FileInfo, bool> predicate;
						if ((predicate = <>9__1) == null)
						{
							predicate = (<>9__1 = ((FileInfo x) => Path.GetFileNameWithoutExtension(x.Name) == oldSavefileName));
						}
						FileInfo fileInfo = allSavedGameFiles.FirstOrDefault(predicate);
						if (fileInfo != null)
						{
							fileInfo.Delete();
						}
					}, "Autosaving", false, null, true);
				}
			}
		}
	}
}
