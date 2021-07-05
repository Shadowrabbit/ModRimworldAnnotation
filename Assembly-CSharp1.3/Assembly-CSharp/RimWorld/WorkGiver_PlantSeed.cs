using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000853 RID: 2131
	public class WorkGiver_PlantSeed : WorkGiver_Scanner
	{
		// Token: 0x17000A08 RID: 2568
		// (get) Token: 0x06003853 RID: 14419 RVA: 0x0013CCF6 File Offset: 0x0013AEF6
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Seed);
			}
		}

		// Token: 0x06003854 RID: 14420 RVA: 0x0013CD00 File Offset: 0x0013AF00
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!ModsConfig.IdeologyActive)
			{
				return false;
			}
			CompPlantable compPlantable = t.TryGetComp<CompPlantable>();
			if (compPlantable == null || !compPlantable.PlantCell.IsValid)
			{
				return false;
			}
			if (!pawn.CanReserve(t, 1, 1, null, forced) || !pawn.CanReach(compPlantable.PlantCell, PathEndMode.Touch, pawn.NormalMaxDanger(), false, false, TraverseMode.ByPawn))
			{
				return false;
			}
			if (PlantUtility.AdjacentSowBlocker(compPlantable.Props.plantDefToSpawn, compPlantable.PlantCell, pawn.Map) != null)
			{
				return false;
			}
			if (!compPlantable.Props.plantDefToSpawn.CanEverPlantAt(compPlantable.PlantCell, pawn.Map, true))
			{
				return false;
			}
			Plant plant = compPlantable.PlantCell.GetPlant(t.Map);
			return plant == null || this.CanDoCutJob(pawn, plant, forced);
		}

		// Token: 0x06003855 RID: 14421 RVA: 0x0013CDC8 File Offset: 0x0013AFC8
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			CompPlantable compPlantable = t.TryGetComp<CompPlantable>();
			if (compPlantable == null || !compPlantable.PlantCell.IsValid)
			{
				return null;
			}
			Plant plant = compPlantable.PlantCell.GetPlant(t.Map);
			if (plant == null)
			{
				Job job = JobMaker.MakeJob(JobDefOf.PlantSeed, t, compPlantable.PlantCell);
				job.playerForced = forced;
				job.plantDefToSow = compPlantable.Props.plantDefToSpawn;
				job.count = 1;
				return job;
			}
			if (!this.CanDoCutJob(pawn, plant, forced))
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.CutPlant, plant);
		}

		// Token: 0x06003856 RID: 14422 RVA: 0x0013CE5E File Offset: 0x0013B05E
		private bool CanDoCutJob(Pawn pawn, Thing plant, bool forced)
		{
			return pawn.CanReserve(plant, 1, -1, null, forced) && !plant.IsForbidden(pawn) && PlantUtility.PawnWillingToCutPlant_Job(plant, pawn);
		}
	}
}
