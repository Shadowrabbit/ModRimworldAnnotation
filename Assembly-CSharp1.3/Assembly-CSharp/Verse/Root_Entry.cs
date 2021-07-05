using System;
using System.IO;
using RimWorld;

namespace Verse
{
	// Token: 0x02000146 RID: 326
	public class Root_Entry : Root
	{
		// Token: 0x06000907 RID: 2311 RVA: 0x00029D1C File Offset: 0x00027F1C
		public override void Start()
		{
			base.Start();
			try
			{
				Current.Game = null;
				this.musicManagerEntry = new MusicManagerEntry();
				FileInfo fileInfo = Root.checkedAutostartSaveFile ? null : SaveGameFilesUtility.GetAutostartSaveFile();
				Root.checkedAutostartSaveFile = true;
				if (fileInfo != null)
				{
					GameDataSaveLoader.LoadGame(fileInfo);
				}
			}
			catch (Exception arg)
			{
				Log.Error("Critical error in root Start(): " + arg);
			}
		}

		// Token: 0x06000908 RID: 2312 RVA: 0x00029D84 File Offset: 0x00027F84
		public override void Update()
		{
			base.Update();
			if (LongEventHandler.ShouldWaitForEvent || this.destroyed)
			{
				return;
			}
			try
			{
				this.musicManagerEntry.MusicManagerEntryUpdate();
				if (Find.World != null)
				{
					Find.World.WorldUpdate();
				}
				if (Current.Game != null)
				{
					Current.Game.UpdateEntry();
				}
			}
			catch (Exception arg)
			{
				Log.Error("Root level exception in Update(): " + arg);
			}
		}

		// Token: 0x0400083F RID: 2111
		public MusicManagerEntry musicManagerEntry;
	}
}
