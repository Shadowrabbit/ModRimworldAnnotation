using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001142 RID: 4418
	public class CompProperties_Hackable : CompProperties
	{
		// Token: 0x06006A12 RID: 27154 RVA: 0x0023B3EC File Offset: 0x002395EC
		public CompProperties_Hackable()
		{
			this.compClass = typeof(CompHackable);
		}

		// Token: 0x04003B2D RID: 15149
		public float defence;

		// Token: 0x04003B2E RID: 15150
		public EffecterDef effectHacking;

		// Token: 0x04003B2F RID: 15151
		public QuestScriptDef completedQuest;

		// Token: 0x04003B30 RID: 15152
		public bool glowIfHacked;

		// Token: 0x04003B31 RID: 15153
		public SoundDef hackingCompletedSound;
	}
}
