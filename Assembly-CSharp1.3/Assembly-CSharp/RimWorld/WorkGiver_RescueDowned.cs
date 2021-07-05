using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200085C RID: 2140
	public class WorkGiver_RescueDowned : WorkGiver_TakeToBed
	{
		// Token: 0x17000A12 RID: 2578
		// (get) Token: 0x06003882 RID: 14466 RVA: 0x000126F5 File Offset: 0x000108F5
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.OnCell;
			}
		}

		// Token: 0x06003883 RID: 14467 RVA: 0x00034716 File Offset: 0x00032916
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x17000A13 RID: 2579
		// (get) Token: 0x06003884 RID: 14468 RVA: 0x00138ACD File Offset: 0x00136CCD
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Pawn);
			}
		}

		// Token: 0x06003885 RID: 14469 RVA: 0x0013D64B File Offset: 0x0013B84B
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.mapPawns.SpawnedDownedPawns;
		}

		// Token: 0x06003886 RID: 14470 RVA: 0x0013D660 File Offset: 0x0013B860
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			List<Pawn> list = pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].Downed && !list[i].InBed())
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06003887 RID: 14471 RVA: 0x0013D6B4 File Offset: 0x0013B8B4
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			if (pawn2 == null || !pawn2.Downed || pawn2.Faction != pawn.Faction || pawn2.InBed() || !pawn.CanReserve(pawn2, 1, -1, null, forced) || GenAI.EnemyIsNear(pawn2, 40f))
			{
				return false;
			}
			Thing thing = base.FindBed(pawn, pawn2);
			return thing != null && pawn2.CanReserve(thing, 1, -1, null, false);
		}

		// Token: 0x06003888 RID: 14472 RVA: 0x0013D72C File Offset: 0x0013B92C
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			Thing t2 = base.FindBed(pawn, pawn2);
			Job job = JobMaker.MakeJob(JobDefOf.Rescue, pawn2, t2);
			job.count = 1;
			return job;
		}

		// Token: 0x04001F33 RID: 7987
		private const float MinDistFromEnemy = 40f;
	}
}
