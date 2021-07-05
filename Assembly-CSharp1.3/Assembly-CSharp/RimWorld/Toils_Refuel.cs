using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000740 RID: 1856
	public class Toils_Refuel
	{
		// Token: 0x06003373 RID: 13171 RVA: 0x00125414 File Offset: 0x00123614
		public static Toil FinalizeRefueling(TargetIndex refuelableInd, TargetIndex fuelInd)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Job curJob = toil.actor.CurJob;
				Thing thing = curJob.GetTarget(refuelableInd).Thing;
				if (toil.actor.CurJob.placedThings.NullOrEmpty<ThingCountClass>())
				{
					thing.TryGetComp<CompRefuelable>().Refuel(new List<Thing>
					{
						curJob.GetTarget(fuelInd).Thing
					});
					return;
				}
				thing.TryGetComp<CompRefuelable>().Refuel((from p in toil.actor.CurJob.placedThings
				select p.thing).ToList<Thing>());
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			return toil;
		}
	}
}
