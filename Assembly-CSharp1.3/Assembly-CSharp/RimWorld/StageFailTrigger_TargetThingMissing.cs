using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F14 RID: 3860
	public class StageFailTrigger_TargetThingMissing : StageFailTrigger
	{
		// Token: 0x06005BF1 RID: 23537 RVA: 0x001FC19C File Offset: 0x001FA39C
		public override bool Failed(LordJob_Ritual ritual, TargetInfo spot, TargetInfo focus)
		{
			if (this.onlyIfTargetIsOfDef != null)
			{
				Thing thing = ritual.selectedTarget.Thing;
				if (((thing != null) ? thing.def : null) != this.onlyIfTargetIsOfDef)
				{
					return false;
				}
			}
			return ritual.selectedTarget.Thing == null || ritual.selectedTarget.ThingDestroyed;
		}

		// Token: 0x06005BF2 RID: 23538 RVA: 0x001FC1EC File Offset: 0x001FA3EC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.onlyIfTargetIsOfDef, "onlyIfTargetIsOfDef");
		}

		// Token: 0x0400358A RID: 13706
		public ThingDef onlyIfTargetIsOfDef;
	}
}
