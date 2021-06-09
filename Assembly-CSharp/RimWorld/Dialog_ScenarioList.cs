using System;
using System.IO;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A1C RID: 6684
	public abstract class Dialog_ScenarioList : Dialog_FileList
	{
		// Token: 0x060093A9 RID: 37801 RVA: 0x002A8F4C File Offset: 0x002A714C
		protected override void ReloadFiles()
		{
			this.files.Clear();
			foreach (FileInfo fileInfo in GenFilePaths.AllCustomScenarioFiles)
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
	}
}
