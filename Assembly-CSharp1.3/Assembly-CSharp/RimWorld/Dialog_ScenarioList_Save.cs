using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001304 RID: 4868
	public class Dialog_ScenarioList_Save : Dialog_ScenarioList
	{
		// Token: 0x17001484 RID: 5252
		// (get) Token: 0x06007505 RID: 29957 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool ShouldDoTypeInField
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06007506 RID: 29958 RVA: 0x0027DE4C File Offset: 0x0027C04C
		public Dialog_ScenarioList_Save(Scenario scen)
		{
			this.interactButLabel = "OverwriteButton".Translate();
			this.typingName = scen.name;
			this.savingScen = scen;
		}

		// Token: 0x06007507 RID: 29959 RVA: 0x0027DE7C File Offset: 0x0027C07C
		protected override void DoFileInteraction(string fileName)
		{
			fileName = GenFile.SanitizedFileName(fileName);
			string absPath = GenFilePaths.AbsPathForScenario(fileName);
			LongEventHandler.QueueLongEvent(delegate()
			{
				GameDataSaveLoader.SaveScenario(this.savingScen, absPath);
			}, "SavingLongEvent", false, null, true);
			Messages.Message("SavedAs".Translate(fileName), MessageTypeDefOf.SilentInput, false);
			this.Close(true);
		}

		// Token: 0x0400408C RID: 16524
		private Scenario savingScen;
	}
}
