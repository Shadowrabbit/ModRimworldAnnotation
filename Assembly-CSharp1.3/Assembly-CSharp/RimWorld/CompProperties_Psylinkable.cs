using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001175 RID: 4469
	public class CompProperties_Psylinkable : CompProperties
	{
		// Token: 0x06006B65 RID: 27493 RVA: 0x00240EF9 File Offset: 0x0023F0F9
		public CompProperties_Psylinkable()
		{
			this.compClass = typeof(CompPsylinkable);
		}

		// Token: 0x04003BBC RID: 15292
		public List<int> requiredSubplantCountPerPsylinkLevel;

		// Token: 0x04003BBD RID: 15293
		public MeditationFocusDef requiredFocus;

		// Token: 0x04003BBE RID: 15294
		public SoundDef linkSound;

		// Token: 0x04003BBF RID: 15295
		public string enoughPlantsLetterLabel;

		// Token: 0x04003BC0 RID: 15296
		public string enoughPlantsLetterText;
	}
}
