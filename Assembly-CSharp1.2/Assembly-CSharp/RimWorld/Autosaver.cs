using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Profile;

namespace RimWorld
{
	// Token: 0x02001BF3 RID: 7155
	public sealed class Autosaver
	{
		// Token: 0x170018B6 RID: 6326
		// (get) Token: 0x06009D7B RID: 40315 RVA: 0x002E1384 File Offset: 0x002DF584
		private float AutosaveIntervalDays
		{
			get
			{
				float num = Prefs.AutosaveIntervalDays;
				if (Current.Game.Info.permadeathMode && num > 1f)
				{
					num = 1f;
				}
				return num;
			}
		}

		// Token: 0x170018B7 RID: 6327
		// (get) Token: 0x06009D7C RID: 40316 RVA: 0x00068DDA File Offset: 0x00066FDA
		private int AutosaveIntervalTicks
		{
			get
			{
				return Mathf.RoundToInt(this.AutosaveIntervalDays * 60000f);
			}
		}

		// Token: 0x06009D7D RID: 40317 RVA: 0x00068DED File Offset: 0x00066FED
		public void AutosaverTick()
		{
			this.ticksSinceSave++;
			if (this.ticksSinceSave >= this.AutosaveIntervalTicks)
			{
				LongEventHandler.QueueLongEvent(new Action(this.DoAutosave), "Autosaving", false, null, true);
				this.ticksSinceSave = 0;
			}
		}

		// Token: 0x06009D7E RID: 40318 RVA: 0x002E13B8 File Offset: 0x002DF5B8
		public void DoAutosave()
		{
			string fileName;
			if (Current.Game.Info.permadeathMode)
			{
				fileName = Current.Game.Info.permadeathModeUniqueName;
			}
			else
			{
				fileName = this.NewAutosaveFileName();
			}
			GameDataSaveLoader.SaveGame(fileName);
		}

		// Token: 0x06009D7F RID: 40319 RVA: 0x0001E036 File Offset: 0x0001C236
		private void DoMemoryCleanup()
		{
			MemoryUtility.UnloadUnusedUnityAssets();
		}

		// Token: 0x06009D80 RID: 40320 RVA: 0x002E13F8 File Offset: 0x002DF5F8
		private string NewAutosaveFileName()
		{
			string text = (from name in this.AutoSaveNames()
			where !SaveGameFilesUtility.SavedGameNamedExists(name)
			select name).FirstOrDefault<string>();
			if (text != null)
			{
				return text;
			}
			return this.AutoSaveNames().MinBy((string name) => new FileInfo(GenFilePaths.FilePathForSavedGame(name)).LastWriteTime);
		}

		// Token: 0x06009D81 RID: 40321 RVA: 0x00068E2B File Offset: 0x0006702B
		private IEnumerable<string> AutoSaveNames()
		{
			int num;
			for (int i = 1; i <= 5; i = num + 1)
			{
				yield return "Autosave-" + i;
				num = i;
			}
			yield break;
		}

		// Token: 0x0400643B RID: 25659
		private int ticksSinceSave;

		// Token: 0x0400643C RID: 25660
		private const int NumAutosaves = 5;

		// Token: 0x0400643D RID: 25661
		public const float MaxPermadeathModeAutosaveInterval = 1f;
	}
}
