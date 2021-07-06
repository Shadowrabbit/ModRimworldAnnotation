using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001896 RID: 6294
	public class CompProperties_WakeUpDormant : CompProperties
	{
		// Token: 0x06008BB0 RID: 35760 RVA: 0x0005DA01 File Offset: 0x0005BC01
		public CompProperties_WakeUpDormant()
		{
			this.compClass = typeof(CompWakeUpDormant);
		}

		// Token: 0x04005983 RID: 22915
		public string wakeUpSignalTag = "CompCanBeDormant.WakeUp";

		// Token: 0x04005984 RID: 22916
		public float anyColonistCloseCheckRadius;

		// Token: 0x04005985 RID: 22917
		public float wakeUpOnThingConstructedRadius = 3f;

		// Token: 0x04005986 RID: 22918
		public bool wakeUpOnDamage = true;

		// Token: 0x04005987 RID: 22919
		public bool onlyWakeUpSameFaction = true;

		// Token: 0x04005988 RID: 22920
		public SoundDef wakeUpSound;

		// Token: 0x04005989 RID: 22921
		public bool wakeUpIfAnyColonistClose;
	}
}
