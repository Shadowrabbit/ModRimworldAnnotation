using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F18 RID: 3864
	public class StageFailTrigger_NoThingPresent : StageFailTrigger
	{
		// Token: 0x06005BFD RID: 23549 RVA: 0x001FC3E8 File Offset: 0x001FA5E8
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
			using (List<Thing>.Enumerator enumerator = thing2.Map.listerBuldingOfDefInProximity.GetForCell(thing2.Position, (float)this.maxDistance, this.thingDef, null).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.GetRoom(RegionType.Set_All) == thing2.GetRoom(RegionType.Set_All))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06005BFE RID: 23550 RVA: 0x001FC4AC File Offset: 0x001FA6AC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.onlyIfTargetIsOfDef, "onlyIfTargetIsOfDef");
			Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
			Scribe_Values.Look<int>(ref this.maxDistance, "maxDistance", 0, false);
		}

		// Token: 0x0400358F RID: 13711
		public ThingDef onlyIfTargetIsOfDef;

		// Token: 0x04003590 RID: 13712
		public int maxDistance = 12;

		// Token: 0x04003591 RID: 13713
		public ThingDef thingDef;
	}
}
