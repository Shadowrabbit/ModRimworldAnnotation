using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F15 RID: 3861
	public class StageFailTrigger_TargetUnpowered : StageFailTrigger
	{
		// Token: 0x06005BF4 RID: 23540 RVA: 0x001FC20C File Offset: 0x001FA40C
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
			CompPowerTrader compPowerTrader = (thing2 != null) ? thing2.TryGetComp<CompPowerTrader>() : null;
			return compPowerTrader == null || !compPowerTrader.PowerOn;
		}

		// Token: 0x06005BF5 RID: 23541 RVA: 0x001FC268 File Offset: 0x001FA468
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.onlyIfTargetIsOfDef, "onlyIfTargetIsOfDef");
		}

		// Token: 0x0400358B RID: 13707
		public ThingDef onlyIfTargetIsOfDef;
	}
}
