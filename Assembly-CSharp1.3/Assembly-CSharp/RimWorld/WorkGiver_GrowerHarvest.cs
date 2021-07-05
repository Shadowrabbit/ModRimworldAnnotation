using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000841 RID: 2113
	public class WorkGiver_GrowerHarvest : WorkGiver_Grower
	{
		// Token: 0x170009FB RID: 2555
		// (get) Token: 0x060037FD RID: 14333 RVA: 0x0009007E File Offset: 0x0008E27E
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x060037FE RID: 14334 RVA: 0x0013B63C File Offset: 0x0013983C
		public override bool HasJobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			Plant plant = c.GetPlant(pawn.Map);
			if (plant == null)
			{
				return false;
			}
			if (plant.IsForbidden(pawn))
			{
				return false;
			}
			if (!plant.HarvestableNow || plant.LifeStage != PlantLifeStage.Mature)
			{
				return false;
			}
			if (!plant.CanYieldNow())
			{
				return false;
			}
			if (!PlantUtility.PawnWillingToCutPlant_Job(plant, pawn))
			{
				return false;
			}
			Zone_Growing zone_Growing = c.GetZone(pawn.Map) as Zone_Growing;
			return (zone_Growing == null || zone_Growing.allowCut) && pawn.CanReserve(plant, 1, -1, null, forced);
		}

		// Token: 0x060037FF RID: 14335 RVA: 0x0013B6C2 File Offset: 0x001398C2
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return pawn.GetLord() != null || base.ShouldSkip(pawn, forced);
		}

		// Token: 0x06003800 RID: 14336 RVA: 0x0013B6D8 File Offset: 0x001398D8
		public override Job JobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			Job job = JobMaker.MakeJob(JobDefOf.Harvest);
			Map map = pawn.Map;
			Room room = c.GetRoom(map);
			float num = 0f;
			for (int i = 0; i < 40; i++)
			{
				IntVec3 intVec = c + GenRadial.RadialPattern[i];
				if (intVec.GetRoom(map) == room && this.HasJobOnCell(pawn, intVec, false))
				{
					Plant plant = intVec.GetPlant(map);
					if (!(intVec != c) || plant.def == WorkGiver_Grower.CalculateWantedPlantDef(intVec, map))
					{
						num += plant.def.plant.harvestWork;
						if (intVec != c && num > 2400f)
						{
							break;
						}
						job.AddQueuedTarget(TargetIndex.A, plant);
					}
				}
			}
			if (job.targetQueueA != null && job.targetQueueA.Count >= 3)
			{
				job.targetQueueA.SortBy((LocalTargetInfo targ) => targ.Cell.DistanceToSquared(pawn.Position));
			}
			return job;
		}
	}
}
