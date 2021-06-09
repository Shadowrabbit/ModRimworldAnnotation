using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D9D RID: 3485
	public class WorkGiver_RescueDowned : WorkGiver_TakeToBed
	{
		// Token: 0x17000C2A RID: 3114
		// (get) Token: 0x06004F6F RID: 20335 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.OnCell;
			}
		}

		// Token: 0x06004F70 RID: 20336 RVA: 0x0002EB44 File Offset: 0x0002CD44
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x17000C2B RID: 3115
		// (get) Token: 0x06004F71 RID: 20337 RVA: 0x0003738E File Offset: 0x0003558E
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Pawn);
			}
		}

		// Token: 0x06004F72 RID: 20338 RVA: 0x00037DE0 File Offset: 0x00035FE0
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.mapPawns.SpawnedDownedPawns;
		}

		// Token: 0x06004F73 RID: 20339 RVA: 0x001B4BF8 File Offset: 0x001B2DF8
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

		// Token: 0x06004F74 RID: 20340 RVA: 0x001B4C4C File Offset: 0x001B2E4C
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

		// Token: 0x06004F75 RID: 20341 RVA: 0x001B4CC4 File Offset: 0x001B2EC4
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			Thing t2 = base.FindBed(pawn, pawn2);
			Job job = JobMaker.MakeJob(JobDefOf.Rescue, pawn2, t2);
			job.count = 1;
			return job;
		}

		// Token: 0x04003387 RID: 13191
		private const float MinDistFromEnemy = 40f;
	}
}
