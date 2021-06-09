using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200180E RID: 6158
	public class CompProperties_Psylinkable : CompProperties
	{
		// Token: 0x06008849 RID: 34889 RVA: 0x0005B83B File Offset: 0x00059A3B
		public CompProperties_Psylinkable()
		{
			this.compClass = typeof(CompPsylinkable);
		}

		// Token: 0x04005772 RID: 22386
		public List<int> requiredSubplantCountPerPsylinkLevel;

		// Token: 0x04005773 RID: 22387
		public MeditationFocusDef requiredFocus;

		// Token: 0x04005774 RID: 22388
		public SoundDef linkSound;

		// Token: 0x04005775 RID: 22389
		public string enoughPlantsLetterLabel;

		// Token: 0x04005776 RID: 22390
		public string enoughPlantsLetterText;
	}
}
