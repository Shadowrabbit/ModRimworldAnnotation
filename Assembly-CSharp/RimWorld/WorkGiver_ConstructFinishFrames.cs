using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D54 RID: 3412
	public class WorkGiver_ConstructFinishFrames : WorkGiver_Scanner
	{
		// Token: 0x17000BE5 RID: 3045
		// (get) Token: 0x06004DF6 RID: 19958 RVA: 0x0001B6B4 File Offset: 0x000198B4
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x06004DF7 RID: 19959 RVA: 0x0002EB44 File Offset: 0x0002CD44
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x17000BE6 RID: 3046
		// (get) Token: 0x06004DF8 RID: 19960 RVA: 0x00037175 File Offset: 0x00035375
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.BuildingFrame);
			}
		}

		// Token: 0x06004DF9 RID: 19961 RVA: 0x001B0244 File Offset: 0x001AE444
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
			if (frame.MaterialsNeeded().Count > 0)
			{
				return null;
			}
			if (GenConstruct.FirstBlockingThing(frame, pawn) != null)
			{
				return GenConstruct.HandleBlockingThingJob(frame, pawn, forced);
			}
			if (!GenConstruct.CanConstruct(frame, pawn, true, forced))
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.FinishFrame, frame);
		}
	}
}
