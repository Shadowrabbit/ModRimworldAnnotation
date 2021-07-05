using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020011CE RID: 4558
	public class CompProperties_WakeUpDormant : CompProperties
	{
		// Token: 0x06006DFD RID: 28157 RVA: 0x0024DF70 File Offset: 0x0024C170
		public CompProperties_WakeUpDormant()
		{
			this.compClass = typeof(CompWakeUpDormant);
		}

		// Token: 0x04003D10 RID: 15632
		public string wakeUpSignalTag = "CompCanBeDormant.WakeUp";

		// Token: 0x04003D11 RID: 15633
		public float anyColonistCloseCheckRadius;

		// Token: 0x04003D12 RID: 15634
		public float wakeUpOnThingConstructedRadius = 3f;

		// Token: 0x04003D13 RID: 15635
		public bool wakeUpOnDamage = true;

		// Token: 0x04003D14 RID: 15636
		public bool onlyWakeUpSameFaction = true;

		// Token: 0x04003D15 RID: 15637
		public SoundDef wakeUpSound;

		// Token: 0x04003D16 RID: 15638
		public bool wakeUpIfAnyColonistClose;
	}
}
