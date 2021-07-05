using System;
using System.IO;
using Verse;

namespace RimWorld
{
	// Token: 0x02001303 RID: 4867
	public abstract class Dialog_ScenarioList : Dialog_FileList
	{
		// Token: 0x06007503 RID: 29955 RVA: 0x0027DDB0 File Offset: 0x0027BFB0
		protected override void ReloadFiles()
		{
			this.files.Clear();
			foreach (FileInfo fileInfo in GenFilePaths.AllCustomScenarioFiles)
			{
				try
				{
					SaveFileInfo saveFileInfo = new SaveFileInfo(fileInfo);
					saveFileInfo.LoadData();
					this.files.Add(saveFileInfo);
				}
				catch (Exception ex)
				{
					Log.Error("Exception loading " + fileInfo.Name + ": " + ex.ToString());
				}
			}
		}
	}
}
