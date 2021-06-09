using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A1A RID: 6682
	public class Dialog_SaveFileList_Save : Dialog_SaveFileList
	{
		// Token: 0x17001771 RID: 6001
		// (get) Token: 0x060093A4 RID: 37796 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool ShouldDoTypeInField
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060093A5 RID: 37797 RVA: 0x002A8E64 File Offset: 0x002A7064
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

		// Token: 0x060093A6 RID: 37798 RVA: 0x002A8ED4 File Offset: 0x002A70D4
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
