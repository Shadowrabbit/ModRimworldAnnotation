using System;
using System.IO;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A18 RID: 6680
	public abstract class Dialog_SaveFileList : Dialog_FileList
	{
		// Token: 0x0600939D RID: 37789 RVA: 0x00062DF1 File Offset: 0x00060FF1
		protected override Color FileNameColor(SaveFileInfo sfi)
		{
			if (SaveGameFilesUtility.IsAutoSave(Path.GetFileNameWithoutExtension(sfi.FileInfo.Name)))
			{
				GUI.color = Dialog_SaveFileList.AutosaveTextColor;
			}
			return base.FileNameColor(sfi);
		}

		// Token: 0x0600939E RID: 37790 RVA: 0x002A8DD0 File Offset: 0x002A6FD0
		protected override void ReloadFiles()
		{
			this.files.Clear();
			foreach (FileInfo fileInfo in GenFilePaths.AllSavedGameFiles)
			{
				try
				{
					this.files.Add(new SaveFileInfo(fileInfo));
				}
				catch (Exception ex)
				{
					Log.Error("Exception loading " + fileInfo.Name + ": " + ex.ToString(), false);
				}
			}
		}

		// Token: 0x0600939F RID: 37791 RVA: 0x00062E1C File Offset: 0x0006101C
		public override void PostClose()
		{
			if (Current.ProgramState == ProgramState.Playing)
			{
				Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Menu, true);
			}
		}

		// Token: 0x04005D8F RID: 23951
		private static readonly Color AutosaveTextColor = new Color(0.75f, 0.75f, 0.75f);
	}
}
