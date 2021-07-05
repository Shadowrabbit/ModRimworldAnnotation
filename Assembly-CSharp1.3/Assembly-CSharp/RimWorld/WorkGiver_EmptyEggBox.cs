using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000846 RID: 2118
	public class WorkGiver_EmptyEggBox : WorkGiver_Scanner
	{
		// Token: 0x170009FD RID: 2557
		// (get) Token: 0x06003811 RID: 14353 RVA: 0x0013BCEA File Offset: 0x00139EEA
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForDef(ThingDefOf.EggBox);
			}
		}

		// Token: 0x06003812 RID: 14354 RVA: 0x0013BCF8 File Offset: 0x00139EF8
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!t.Spawned || t.IsForbidden(pawn))
			{
				return false;
			}
			if (!pawn.CanReserve(t, 1, -1, null, forced))
			{
				return false;
			}
			CompEggContainer compEggContainer = t.TryGetComp<CompEggContainer>();
			return compEggContainer != null && compEggContainer.ContainedThing != null && (compEggContainer.CanEmpty || forced);
		}

		// Token: 0x06003813 RID: 14355 RVA: 0x0013BD50 File Offset: 0x00139F50
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			CompEggContainer compEggContainer = t.TryGetComp<CompEggContainer>();
			if (compEggContainer == null || compEggContainer.ContainedThing == null)
			{
				return null;
			}
			if (!compEggContainer.CanEmpty && !forced)
			{
				return null;
			}
			IntVec3 c;
			IHaulDestination haulDestination;
			if (!StoreUtility.TryFindBestBetterStorageFor(compEggContainer.ContainedThing, pawn, pawn.Map, StoragePriority.Unstored, pawn.Faction, out c, out haulDestination, true))
			{
				JobFailReason.Is(HaulAIUtility.NoEmptyPlaceLowerTrans, null);
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.EmptyThingContainer, t, compEggContainer.ContainedThing, c);
			job.count = compEggContainer.ContainedThing.stackCount;
			return job;
		}
	}
}
