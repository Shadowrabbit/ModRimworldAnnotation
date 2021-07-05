using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Profile;

namespace RimWorld
{
	// Token: 0x020013E4 RID: 5092
	public sealed class Autosaver
	{
		// Token: 0x170015AE RID: 5550
		// (get) Token: 0x06007BDA RID: 31706 RVA: 0x002BAE10 File Offset: 0x002B9010
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

		// Token: 0x170015AF RID: 5551
		// (get) Token: 0x06007BDB RID: 31707 RVA: 0x002BAE43 File Offset: 0x002B9043
		private int AutosaveIntervalTicks
		{
			get
			{
				return Mathf.RoundToInt(this.AutosaveIntervalDays * 60000f);
			}
		}

		// Token: 0x06007BDC RID: 31708 RVA: 0x002BAE58 File Offset: 0x002B9058
		public void AutosaverTick()
		{
			this.ticksSinceSave++;
			if (this.ticksSinceSave >= this.AutosaveIntervalTicks && !GameDataSaveLoader.SavingIsTemporarilyDisabled)
			{
				LongEventHandler.QueueLongEvent(new Action(this.DoAutosave), "Autosaving", false, null, true);
				this.ticksSinceSave = 0;
			}
		}

		// Token: 0x06007BDD RID: 31709 RVA: 0x002BAEA8 File Offset: 0x002B90A8
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

		// Token: 0x06007BDE RID: 31710 RVA: 0x0009F206 File Offset: 0x0009D406
		private void DoMemoryCleanup()
		{
			MemoryUtility.UnloadUnusedUnityAssets();
		}

		// Token: 0x06007BDF RID: 31711 RVA: 0x002BAEE8 File Offset: 0x002B90E8
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

		// Token: 0x06007BE0 RID: 31712 RVA: 0x002BAF54 File Offset: 0x002B9154
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

		// Token: 0x04004494 RID: 17556
		private int ticksSinceSave;

		// Token: 0x04004495 RID: 17557
		private const int NumAutosaves = 5;

		// Token: 0x04004496 RID: 17558
		public const float MaxPermadeathModeAutosaveInterval = 1f;
	}
}
