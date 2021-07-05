using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000817 RID: 2071
	public class WorkGiver_ConstructDeliverResourcesToBlueprints : WorkGiver_ConstructDeliverResources
	{
		// Token: 0x170009D1 RID: 2513
		// (get) Token: 0x06003725 RID: 14117 RVA: 0x00138149 File Offset: 0x00136349
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Blueprint);
			}
		}

		// Token: 0x06003726 RID: 14118 RVA: 0x00138154 File Offset: 0x00136354
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (t.Faction != pawn.Faction)
			{
				return null;
			}
			Blueprint blueprint = t as Blueprint;
			if (blueprint == null)
			{
				return null;
			}
			if (GenConstruct.FirstBlockingThing(blueprint, pawn) != null)
			{
				return GenConstruct.HandleBlockingThingJob(blueprint, pawn, forced);
			}
			if (!GenConstruct.CanConstruct(blueprint, pawn, this.def.workType, forced))
			{
				return null;
			}
			if (this.def.workType != WorkTypeDefOf.Construction && WorkGiver_ConstructDeliverResources.ShouldRemoveExistingFloorFirst(pawn, blueprint))
			{
				return null;
			}
			Job job = base.RemoveExistingFloorJob(pawn, blueprint);
			if (job != null)
			{
				return job;
			}
			Job job2 = base.ResourceDeliverJobFor(pawn, blueprint, true);
			if (job2 != null)
			{
				return job2;
			}
			if (this.def.workType != WorkTypeDefOf.Hauling)
			{
				Job job3 = this.NoCostFrameMakeJobFor(pawn, blueprint);
				if (job3 != null)
				{
					return job3;
				}
			}
			return null;
		}

		// Token: 0x06003727 RID: 14119 RVA: 0x00138203 File Offset: 0x00136403
		private Job NoCostFrameMakeJobFor(Pawn pawn, IConstructible c)
		{
			if (c is Blueprint_Install)
			{
				return null;
			}
			if (c is Blueprint && c.MaterialsNeeded().Count == 0)
			{
				Job job = JobMaker.MakeJob(JobDefOf.PlaceNoCostFrame);
				job.targetA = (Thing)c;
				return job;
			}
			return null;
		}
	}
}
