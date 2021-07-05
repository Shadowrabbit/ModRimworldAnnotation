using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000DA3 RID: 3491
	public class WorkGiver_TakeToBedToOperate : WorkGiver_TakeToBed
	{
		// Token: 0x17000C33 RID: 3123
		// (get) Token: 0x06004F96 RID: 20374 RVA: 0x0003738E File Offset: 0x0003558E
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Pawn);
			}
		}

		// Token: 0x17000C34 RID: 3124
		// (get) Token: 0x06004F97 RID: 20375 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.OnCell;
			}
		}

		// Token: 0x06004F98 RID: 20376 RVA: 0x0002EB44 File Offset: 0x0002CD44
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x06004F99 RID: 20377 RVA: 0x001B4F40 File Offset: 0x001B3140
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			List<Pawn> allPawnsSpawned = pawn.Map.mapPawns.AllPawnsSpawned;
			for (int i = 0; i < allPawnsSpawned.Count; i++)
			{
				if (HealthAIUtility.ShouldHaveSurgeryDoneNow(allPawnsSpawned[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06004F9A RID: 20378 RVA: 0x00037F35 File Offset: 0x00036135
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.mapPawns.SpawnedPawnsWhoShouldHaveSurgeryDoneNow;
		}

		// Token: 0x06004F9B RID: 20379 RVA: 0x001B4F80 File Offset: 0x001B3180
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			if (pawn2 == null || pawn2 == pawn || pawn2.InBed() || !pawn2.RaceProps.IsFlesh || !HealthAIUtility.ShouldHaveSurgeryDoneNow(pawn2) || !pawn.CanReserve(pawn2, 1, -1, null, forced) || (pawn2.InMentalState && pawn2.MentalStateDef.IsAggro))
			{
				return false;
			}
			if (!pawn2.Downed)
			{
				if (pawn2.IsColonist)
				{
					return false;
				}
				if (!pawn2.IsPrisonerOfColony && pawn2.Faction != Faction.OfPlayer)
				{
					return false;
				}
				if (pawn2.guest != null && pawn2.guest.Released)
				{
					return false;
				}
			}
			Building_Bed building_Bed = base.FindBed(pawn, pawn2);
			return building_Bed != null && pawn2.CanReserve(building_Bed, building_Bed.SleepingSlotsCount, -1, null, false);
		}

		// Token: 0x06004F9C RID: 20380 RVA: 0x001B5048 File Offset: 0x001B3248
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			Building_Bed t2 = base.FindBed(pawn, pawn2);
			Job job = JobMaker.MakeJob(JobDefOf.TakeToBedToOperate, pawn2, t2);
			job.count = 1;
			return job;
		}
	}
}
