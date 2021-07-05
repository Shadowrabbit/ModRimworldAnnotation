using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001302 RID: 4866
	public class Dialog_SaveFileList_Save : Dialog_SaveFileList
	{
		// Token: 0x17001483 RID: 5251
		// (get) Token: 0x06007500 RID: 29952 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool ShouldDoTypeInField
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06007501 RID: 29953 RVA: 0x0027DCC8 File Offset: 0x0027BEC8
		public Dialog_SaveFileList_Save()
		{
			this.interactButLabel = "OverwriteButton".Translate();
			this.bottomAreaHeight = 85f;
			if (Faction.OfPlayer.HasName)
			{
				this.typingName = Faction.OfPlayer.Name;
				return;
			}
			this.typingName = SaveGameFilesUtility.UnusedDefaultFileName(Faction.OfPlayer.def.LabelCap);
		}

		// Token: 0x06007502 RID: 29954 RVA: 0x0027DD38 File Offset: 0x0027BF38
		protected override void DoFileInteraction(string mapName)
		{
			mapName = GenFile.SanitizedFileName(mapName);
			LongEventHandler.QueueLongEvent(delegate()
			{
				GameDataSaveLoader.SaveGame(mapName);
			}, "SavingLongEvent", false, null, true);
			Messages.Message("SavedAs".Translate(mapName), MessageTypeDefOf.SilentInput, false);
			PlayerKnowledgeDatabase.Save();
			this.Close(true);
		}
	}
}
