using System;
using System.IO;
using RimWorld;

namespace Verse
{
	// Token: 0x020001EC RID: 492
	public class Root_Entry : Root
	{
		// Token: 0x06000CC9 RID: 3273 RVA: 0x000A5944 File Offset: 0x000A3B44
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
				Log.Error("Critical error in root Start(): " + arg, false);
			}
		}

		// Token: 0x06000CCA RID: 3274 RVA: 0x000A59B0 File Offset: 0x000A3BB0
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
				Log.Error("Root level exception in Update(): " + arg, false);
			}
		}

		// Token: 0x04000B06 RID: 2822
		public MusicManagerEntry musicManagerEntry;
	}
}
