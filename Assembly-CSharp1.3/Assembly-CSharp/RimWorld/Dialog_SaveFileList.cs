using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001300 RID: 4864
	public abstract class Dialog_SaveFileList : Dialog_FileList
	{
		// Token: 0x060074F7 RID: 29943 RVA: 0x0027DB3D File Offset: 0x0027BD3D
		protected override Color FileNameColor(SaveFileInfo sfi)
		{
			if (SaveGameFilesUtility.IsAutoSave(Path.GetFileNameWithoutExtension(sfi.FileName)))
			{
				GUI.color = Dialog_SaveFileList.AutosaveTextColor;
			}
			return base.FileNameColor(sfi);
		}

		// Token: 0x060074F8 RID: 29944 RVA: 0x0027DB62 File Offset: 0x0027BD62
		private void ReloadFilesTask()
		{
			Parallel.ForEach<SaveFileInfo>(this.files, delegate(SaveFileInfo file)
			{
				try
				{
					file.LoadData();
				}
				catch (Exception ex)
				{
					Log.Error("Exception loading " + file.FileInfo.Name + ": " + ex.ToString());
				}
			});
		}

		// Token: 0x060074F9 RID: 29945 RVA: 0x0027DB90 File Offset: 0x0027BD90
		protected override void ReloadFiles()
		{
			if (this.loadSavesTask != null && this.loadSavesTask.Status != TaskStatus.RanToCompletion)
			{
				this.loadSavesTask.Wait();
			}
			this.files.Clear();
			foreach (FileInfo fileInfo in GenFilePaths.AllSavedGameFiles)
			{
				try
				{
					SaveFileInfo item = new SaveFileInfo(fileInfo);
					this.files.Add(item);
				}
				catch (Exception ex)
				{
					Log.Error("Exception loading " + fileInfo.Name + ": " + ex.ToString());
				}
			}
			this.loadSavesTask = Task.Run(new Action(this.ReloadFilesTask));
		}

		// Token: 0x060074FA RID: 29946 RVA: 0x0027DC5C File Offset: 0x0027BE5C
		public override void DoWindowContents(Rect inRect)
		{
			base.DoWindowContents(inRect);
		}

		// Token: 0x060074FB RID: 29947 RVA: 0x0027DC65 File Offset: 0x0027BE65
		public override void PostClose()
		{
			if (Current.ProgramState == ProgramState.Playing)
			{
				Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Menu, true);
			}
		}

		// Token: 0x0400408A RID: 16522
		private static readonly Color AutosaveTextColor = new Color(0.75f, 0.75f, 0.75f);

		// Token: 0x0400408B RID: 16523
		private Task loadSavesTask;
	}
}
