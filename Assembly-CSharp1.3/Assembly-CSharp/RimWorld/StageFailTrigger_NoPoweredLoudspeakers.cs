using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F17 RID: 3863
	public class StageFailTrigger_NoPoweredLoudspeakers : StageFailTrigger
	{
		// Token: 0x06005BFA RID: 23546 RVA: 0x001FC2F4 File Offset: 0x001FA4F4
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
			if (thing2 == null || thing2.Destroyed)
			{
				return true;
			}
			foreach (Thing thing3 in thing2.Map.listerBuldingOfDefInProximity.GetForCell(thing2.Position, 12f, ThingDefOf.Loudspeaker, null))
			{
				CompPowerTrader compPowerTrader = thing3.TryGetComp<CompPowerTrader>();
				if (thing3.GetRoom(RegionType.Set_All) == thing2.GetRoom(RegionType.Set_All) && compPowerTrader.PowerNet != null && compPowerTrader.PowerNet.HasActivePowerSource)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06005BFB RID: 23547 RVA: 0x001FC3D0 File Offset: 0x001FA5D0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.onlyIfTargetIsOfDef, "onlyIfTargetIsOfDef");
		}

		// Token: 0x0400358D RID: 13709
		public ThingDef onlyIfTargetIsOfDef;

		// Token: 0x0400358E RID: 13710
		public const int maxDistance = 12;
	}
}
