using System;
using System.Linq;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x0200028E RID: 654
	public class HediffComp_Disorientation : HediffComp
	{
		// Token: 0x17000391 RID: 913
		// (get) Token: 0x06001258 RID: 4696 RVA: 0x00069FEB File Offset: 0x000681EB
		private HediffCompProperties_Disorientation Props
		{
			get
			{
				return (HediffCompProperties_Disorientation)this.props;
			}
		}

		// Token: 0x06001259 RID: 4697 RVA: 0x00069FF8 File Offset: 0x000681F8
		public override void CompPostTick(ref float severityAdjustment)
		{
			if (this.Props.wanderMtbHours > 0f && base.Pawn.Spawned && !base.Pawn.Downed && base.Pawn.Awake() && base.Pawn.CurJobDef.suspendable && this.Props.wanderMtbHours > 0f && base.Pawn.IsHashIntervalTick(60) && Rand.MTBEventOccurs(this.Props.wanderMtbHours, 2500f, 60f) && base.Pawn.CurJob.def != JobDefOf.GotoMindControlled)
			{
				IntVec3 c2 = (from c in GenRadial.RadialCellsAround(base.Pawn.Position, this.Props.wanderRadius, false)
				where c.Standable(base.Pawn.MapHeld) && base.Pawn.CanReach(c, PathEndMode.OnCell, Danger.Unspecified, false, false, TraverseMode.ByPawn)
				select c).RandomElementWithFallback(IntVec3.Invalid);
				if (c2.IsValid)
				{
					MoteMaker.MakeThoughtBubble(base.Pawn, "Things/Mote/Disoriented", false);
					Job job = JobMaker.MakeJob(JobDefOf.GotoMindControlled, c2);
					job.expiryInterval = this.Props.singleWanderDurationTicks;
					base.Pawn.jobs.StartJob(job, JobCondition.InterruptForced, null, true, true, null, null, false, false);
				}
			}
		}

		// Token: 0x04000DE7 RID: 3559
		private const string moteTexPath = "Things/Mote/Disoriented";
	}
}
