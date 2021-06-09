using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F47 RID: 8007
	public class QuestNode_GetPopIntentForQuest : QuestNode
	{
		// Token: 0x0600AAF7 RID: 43767 RVA: 0x0006FE7A File Offset: 0x0006E07A
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x0600AAF8 RID: 43768 RVA: 0x0006FE84 File Offset: 0x0006E084
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x0600AAF9 RID: 43769 RVA: 0x0031DE00 File Offset: 0x0031C000
		private void SetVars(Slate slate)
		{
			float populationIntentForQuest = StorytellerUtilityPopulation.PopulationIntentForQuest;
			slate.Set<float>(this.storeAs.GetValue(slate), populationIntentForQuest, false);
		}

		// Token: 0x04007455 RID: 29781
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
