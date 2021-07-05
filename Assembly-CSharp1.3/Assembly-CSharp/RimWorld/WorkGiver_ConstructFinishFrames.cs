using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000818 RID: 2072
	public class WorkGiver_ConstructFinishFrames : WorkGiver_Scanner
	{
		// Token: 0x170009D2 RID: 2514
		// (get) Token: 0x06003729 RID: 14121 RVA: 0x0009007E File Offset: 0x0008E27E
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x0600372A RID: 14122 RVA: 0x00034716 File Offset: 0x00032916
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x170009D3 RID: 2515
		// (get) Token: 0x0600372B RID: 14123 RVA: 0x00137833 File Offset: 0x00135A33
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.BuildingFrame);
			}
		}

		// Token: 0x0600372C RID: 14124 RVA: 0x00138244 File Offset: 0x00136444
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
