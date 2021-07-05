using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F16 RID: 3862
	public class StageFailTrigger_TargetNotLit : StageFailTrigger
	{
		// Token: 0x06005BF7 RID: 23543 RVA: 0x001FC280 File Offset: 0x001FA480
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
			Thing thing2 = ritual.selectedTarget.Thing;
			CompRefuelable compRefuelable = (thing2 != null) ? thing2.TryGetComp<CompRefuelable>() : null;
			return compRefuelable == null || !compRefuelable.HasFuel;
		}

		// Token: 0x06005BF8 RID: 23544 RVA: 0x001FC2DC File Offset: 0x001FA4DC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.onlyIfTargetIsOfDef, "onlyIfTargetIsOfDef");
		}

		// Token: 0x0400358C RID: 13708
		public ThingDef onlyIfTargetIsOfDef;
	}
}
