using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000DA1 RID: 3489
	public class WorkGiver_TakeBeerOutOfFermentingBarrel : WorkGiver_Scanner
	{
		// Token: 0x17000C31 RID: 3121
		// (get) Token: 0x06004F8E RID: 20366 RVA: 0x00037742 File Offset: 0x00035942
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForDef(ThingDefOf.FermentingBarrel);
			}
		}

		// Token: 0x06004F8F RID: 20367 RVA: 0x001B4EA8 File Offset: 0x001B30A8
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			List<Thing> list = pawn.Map.listerThings.ThingsOfDef(ThingDefOf.FermentingBarrel);
			for (int i = 0; i < list.Count; i++)
			{
				if (((Building_FermentingBarrel)list[i]).Fermented)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x17000C32 RID: 3122
		// (get) Token: 0x06004F90 RID: 20368 RVA: 0x0001B6B4 File Offset: 0x000198B4
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x06004F91 RID: 20369 RVA: 0x001B4EF4 File Offset: 0x001B30F4
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Building_FermentingBarrel building_FermentingBarrel = t as Building_FermentingBarrel;
			return building_FermentingBarrel != null && building_FermentingBarrel.Fermented && !t.IsBurning() && !t.IsForbidden(pawn) && pawn.CanReserve(t, 1, -1, null, forced);
		}

		// Token: 0x06004F92 RID: 20370 RVA: 0x00037F0A File Offset: 0x0003610A
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.TakeBeerOutOfFermentingBarrel, t);
		}
	}
}
