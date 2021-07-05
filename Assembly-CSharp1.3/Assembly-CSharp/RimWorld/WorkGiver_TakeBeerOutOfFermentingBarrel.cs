using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000860 RID: 2144
	public class WorkGiver_TakeBeerOutOfFermentingBarrel : WorkGiver_Scanner
	{
		// Token: 0x17000A19 RID: 2585
		// (get) Token: 0x0600389F RID: 14495 RVA: 0x0013B271 File Offset: 0x00139471
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForDef(ThingDefOf.FermentingBarrel);
			}
		}

		// Token: 0x060038A0 RID: 14496 RVA: 0x0013D9A0 File Offset: 0x0013BBA0
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

		// Token: 0x17000A1A RID: 2586
		// (get) Token: 0x060038A1 RID: 14497 RVA: 0x0009007E File Offset: 0x0008E27E
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x060038A2 RID: 14498 RVA: 0x0013D9EC File Offset: 0x0013BBEC
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Building_FermentingBarrel building_FermentingBarrel = t as Building_FermentingBarrel;
			return building_FermentingBarrel != null && building_FermentingBarrel.Fermented && !t.IsBurning() && !t.IsForbidden(pawn) && pawn.CanReserve(t, 1, -1, null, forced);
		}

		// Token: 0x060038A3 RID: 14499 RVA: 0x0013DA35 File Offset: 0x0013BC35
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.TakeBeerOutOfFermentingBarrel, t);
		}
	}
}
