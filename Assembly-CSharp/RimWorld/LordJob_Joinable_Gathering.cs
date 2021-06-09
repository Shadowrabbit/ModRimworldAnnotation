using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000DD7 RID: 3543
	public abstract class LordJob_Joinable_Gathering : LordJob_VoluntarilyJoinable
	{
		// Token: 0x17000C62 RID: 3170
		// (get) Token: 0x060050BF RID: 20671 RVA: 0x00038A38 File Offset: 0x00036C38
		public Pawn Organizer
		{
			get
			{
				return this.organizer;
			}
		}

		// Token: 0x060050C0 RID: 20672 RVA: 0x00038A40 File Offset: 0x00036C40
		public LordJob_Joinable_Gathering()
		{
		}

		// Token: 0x060050C1 RID: 20673 RVA: 0x00038A48 File Offset: 0x00036C48
		public LordJob_Joinable_Gathering(IntVec3 spot, Pawn organizer, GatheringDef gatheringDef)
		{
			this.spot = spot;
			this.organizer = organizer;
			this.gatheringDef = gatheringDef;
		}

		// Token: 0x060050C2 RID: 20674
		protected abstract LordToil CreateGatheringToil(IntVec3 spot, Pawn organizer, GatheringDef gatheringDef);

		// Token: 0x060050C3 RID: 20675 RVA: 0x00038A65 File Offset: 0x00036C65
		protected virtual bool ShouldBeCalledOff()
		{
			return !GatheringsUtility.PawnCanStartOrContinueGathering(this.organizer) || !GatheringsUtility.AcceptableGameConditionsToContinueGathering(base.Map);
		}

		// Token: 0x060050C4 RID: 20676 RVA: 0x001B9174 File Offset: 0x001B7374
		public override float VoluntaryJoinPriorityFor(Pawn p)
		{
			if (!this.IsInvited(p))
			{
				return 0f;
			}
			if (!GatheringsUtility.ShouldPawnKeepGathering(p, this.gatheringDef))
			{
				return 0f;
			}
			if (this.spot.IsForbidden(p))
			{
				return 0f;
			}
			if (!this.lord.ownedPawns.Contains(p) && this.IsGatheringAboutToEnd())
			{
				return 0f;
			}
			return VoluntarilyJoinableLordJobJoinPriorities.SocialGathering;
		}

		// Token: 0x060050C5 RID: 20677 RVA: 0x001B91E0 File Offset: 0x001B73E0
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.spot, "spot", default(IntVec3), false);
			Scribe_References.Look<Pawn>(ref this.organizer, "organizer", false);
			Scribe_Defs.Look<GatheringDef>(ref this.gatheringDef, "gatheringDef");
		}

		// Token: 0x060050C6 RID: 20678 RVA: 0x00038A86 File Offset: 0x00036C86
		private bool IsGatheringAboutToEnd()
		{
			return this.timeoutTrigger.TicksLeft < 1200;
		}

		// Token: 0x060050C7 RID: 20679 RVA: 0x00038A9D File Offset: 0x00036C9D
		private bool IsInvited(Pawn p)
		{
			return this.lord.faction != null && p.Faction == this.lord.faction;
		}

		// Token: 0x0400340D RID: 13325
		protected IntVec3 spot;

		// Token: 0x0400340E RID: 13326
		protected Pawn organizer;

		// Token: 0x0400340F RID: 13327
		protected GatheringDef gatheringDef;

		// Token: 0x04003410 RID: 13328
		protected Trigger_TicksPassed timeoutTrigger;
	}
}
