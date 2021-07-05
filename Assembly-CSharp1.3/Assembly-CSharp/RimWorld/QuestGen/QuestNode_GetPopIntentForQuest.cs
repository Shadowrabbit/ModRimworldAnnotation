using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001684 RID: 5764
	public class QuestNode_GetPopIntentForQuest : QuestNode
	{
		// Token: 0x0600861E RID: 34334 RVA: 0x00301E22 File Offset: 0x00300022
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x0600861F RID: 34335 RVA: 0x00301E2C File Offset: 0x0030002C
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x06008620 RID: 34336 RVA: 0x00301E3C File Offset: 0x0030003C
		private void SetVars(Slate slate)
		{
			float populationIntentForQuest = StorytellerUtilityPopulation.PopulationIntentForQuest;
			slate.Set<float>(this.storeAs.GetValue(slate), populationIntentForQuest, false);
		}

		// Token: 0x040053F4 RID: 21492
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
