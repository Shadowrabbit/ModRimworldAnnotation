using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000862 RID: 2146
	public class WorkGiver_TakeToBedToOperate : WorkGiver_TakeToBed
	{
		// Token: 0x17000A1B RID: 2587
		// (get) Token: 0x060038A7 RID: 14503 RVA: 0x00138ACD File Offset: 0x00136CCD
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Pawn);
			}
		}

		// Token: 0x17000A1C RID: 2588
		// (get) Token: 0x060038A8 RID: 14504 RVA: 0x000126F5 File Offset: 0x000108F5
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.OnCell;
			}
		}

		// Token: 0x060038A9 RID: 14505 RVA: 0x00034716 File Offset: 0x00032916
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x060038AA RID: 14506 RVA: 0x0013DA58 File Offset: 0x0013BC58
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

		// Token: 0x060038AB RID: 14507 RVA: 0x0013DA98 File Offset: 0x0013BC98
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.mapPawns.SpawnedPawnsWhoShouldHaveSurgeryDoneNow;
		}

		// Token: 0x060038AC RID: 14508 RVA: 0x0013DAAC File Offset: 0x0013BCAC
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			if (pawn2 == null || pawn2 == pawn || pawn2.InBed() || !pawn2.RaceProps.IsFlesh || !HealthAIUtility.ShouldHaveSurgeryDoneNow(pawn2) || !pawn.CanReserve(pawn2, 1, -1, null, forced) || (pawn2.InMentalState && pawn2.MentalStateDef.IsAggro))
			{
				return false;
			}
			if (!pawn2.Downed)
			{
				if (pawn2.IsColonist || pawn2.CurJobDef == JobDefOf.LayDown)
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

		// Token: 0x060038AD RID: 14509 RVA: 0x0013DB80 File Offset: 0x0013BD80
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
