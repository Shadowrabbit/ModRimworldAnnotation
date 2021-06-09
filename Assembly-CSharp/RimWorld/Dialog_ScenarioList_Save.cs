using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A1D RID: 6685
	public class Dialog_ScenarioList_Save : Dialog_ScenarioList
	{
		// Token: 0x17001772 RID: 6002
		// (get) Token: 0x060093AB RID: 37803 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool ShouldDoTypeInField
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060093AC RID: 37804 RVA: 0x00062E8B File Offset: 0x0006108B
		public Dialog_ScenarioList_Save(Scenario scen)
		{
			this.interactButLabel = "OverwriteButton".Translate();
			this.typingName = scen.name;
			this.savingScen = scen;
		}

		// Token: 0x060093AD RID: 37805 RVA: 0x002A8FE0 File Offset: 0x002A71E0
		protected override void DoFileInteraction(string fileName)
		{
			string absPath = GenFilePaths.AbsPathForScenario(fileName);
			LongEventHandler.QueueLongEvent(delegate()
			{
				GameDataSaveLoader.SaveScenario(this.savingScen, absPath);
			}, "SavingLongEvent", false, null, true);
			Messages.Message("SavedAs".Translate(fileName), MessageTypeDefOf.SilentInput, false);
			this.Close(true);
		}

		// Token: 0x04005D91 RID: 23953
		private Scenario savingScen;
	}
}
