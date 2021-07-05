using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000814 RID: 2068
	public class WorkGiver_ConstructDeliverResourcesToFrames : WorkGiver_ConstructDeliverResources
	{
		// Token: 0x170009D0 RID: 2512
		// (get) Token: 0x06003713 RID: 14099 RVA: 0x00137833 File Offset: 0x00135A33
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.BuildingFrame);
			}
		}

		// Token: 0x06003714 RID: 14100 RVA: 0x0013783C File Offset: 0x00135A3C
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (t.Faction != pawn.Faction)
			{
				return null;
			}
			Frame frame = t as Frame;
			if (frame == null)
			{
				return null;
			}
			if (GenConstruct.FirstBlockingThing(frame, pawn) != null)
			{
				return GenConstruct.HandleBlockingThingJob(frame, pawn, forced);
			}
			if (!GenConstruct.CanConstruct(frame, pawn, this.def.workType, forced))
			{
				return null;
			}
			return base.ResourceDeliverJobFor(pawn, frame, true);
		}
	}
}
