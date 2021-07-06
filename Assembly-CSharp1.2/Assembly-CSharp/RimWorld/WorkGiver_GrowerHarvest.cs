using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000D7E RID: 3454
	public class WorkGiver_GrowerHarvest : WorkGiver_Grower
	{
		// Token: 0x17000C0F RID: 3087
		// (get) Token: 0x06004ED1 RID: 20177 RVA: 0x0001B6B4 File Offset: 0x000198B4
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x06004ED2 RID: 20178 RVA: 0x001B309C File Offset: 0x001B129C
		public override bool HasJobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			Plant plant = c.GetPlant(pawn.Map);
			return plant != null && !plant.IsForbidden(pawn) && plant.HarvestableNow && plant.LifeStage == PlantLifeStage.Mature && plant.CanYieldNow() && pawn.CanReserve(plant, 1, -1, null, forced);
		}

		// Token: 0x06004ED3 RID: 20179 RVA: 0x00037866 File Offset: 0x00035A66
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return pawn.GetLord() != null || base.ShouldSkip(pawn, forced);
		}

		// Token: 0x06004ED4 RID: 20180 RVA: 0x001B30F8 File Offset: 0x001B12F8
		public override Job JobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			Job job = JobMaker.MakeJob(JobDefOf.Harvest);
			Map map = pawn.Map;
			Room room = c.GetRoom(map, RegionType.Set_Passable);
			float num = 0f;
			for (int i = 0; i < 40; i++)
			{
				IntVec3 intVec = c + GenRadial.RadialPattern[i];
				if (intVec.GetRoom(map, RegionType.Set_Passable) == room && this.HasJobOnCell(pawn, intVec, false))
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
