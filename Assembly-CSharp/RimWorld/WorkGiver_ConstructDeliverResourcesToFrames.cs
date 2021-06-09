using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D53 RID: 3411
	public class WorkGiver_ConstructDeliverResourcesToFrames : WorkGiver_ConstructDeliverResources
	{
		// Token: 0x17000BE4 RID: 3044
		// (get) Token: 0x06004DF3 RID: 19955 RVA: 0x00037175 File Offset: 0x00035375
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.BuildingFrame);
			}
		}

		// Token: 0x06004DF4 RID: 19956 RVA: 0x001B01E0 File Offset: 0x001AE3E0
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
			bool checkSkills = this.def.workType == WorkTypeDefOf.Construction;
			if (!GenConstruct.CanConstruct(frame, pawn, checkSkills, forced))
			{
				return null;
			}
			return base.ResourceDeliverJobFor(pawn, frame, true);
		}
	}
}
