using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000893 RID: 2195
	public abstract class LordJob_Joinable_Gathering : LordJob_VoluntarilyJoinable
	{
		// Token: 0x17000A5F RID: 2655
		// (get) Token: 0x06003A1A RID: 14874 RVA: 0x00145BA6 File Offset: 0x00143DA6
		public Pawn Organizer
		{
			get
			{
				return this.organizer;
			}
		}

		// Token: 0x17000A60 RID: 2656
		// (get) Token: 0x06003A1B RID: 14875 RVA: 0x00145BAE File Offset: 0x00143DAE
		public int DurationTicks
		{
			get
			{
				return this.durationTicks;
			}
		}

		// Token: 0x17000A61 RID: 2657
		// (get) Token: 0x06003A1C RID: 14876 RVA: 0x00145BB6 File Offset: 0x00143DB6
		public virtual int TicksLeft
		{
			get
			{
				return this.timeoutTrigger.TicksLeft;
			}
		}

		// Token: 0x17000A62 RID: 2658
		// (get) Token: 0x06003A1D RID: 14877 RVA: 0x00145BC3 File Offset: 0x00143DC3
		public virtual IntVec3 Spot
		{
			get
			{
				return this.spot;
			}
		}

		// Token: 0x06003A1E RID: 14878 RVA: 0x00145BCB File Offset: 0x00143DCB
		public LordJob_Joinable_Gathering()
		{
		}

		// Token: 0x06003A1F RID: 14879 RVA: 0x00145BD3 File Offset: 0x00143DD3
		public LordJob_Joinable_Gathering(IntVec3 spot, Pawn organizer, GatheringDef gatheringDef)
		{
			this.spot = spot;
			this.organizer = organizer;
			this.gatheringDef = gatheringDef;
		}

		// Token: 0x06003A20 RID: 14880
		protected abstract LordToil CreateGatheringToil(IntVec3 spot, Pawn organizer, GatheringDef gatheringDef);

		// Token: 0x06003A21 RID: 14881 RVA: 0x00145BF0 File Offset: 0x00143DF0
		protected virtual bool ShouldBeCalledOff()
		{
			return (this.organizer != null && !GatheringsUtility.PawnCanStartOrContinueGathering(this.organizer)) || !GatheringsUtility.AcceptableGameConditionsToContinueGathering(base.Map);
		}

		// Token: 0x06003A22 RID: 14882 RVA: 0x00145C1C File Offset: 0x00143E1C
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
			List<Hediff> hediffs = p.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (hediffs[i].def.blocksSocialInteraction)
				{
					return 0f;
				}
			}
			return VoluntarilyJoinableLordJobJoinPriorities.SocialGathering;
		}

		// Token: 0x06003A23 RID: 14883 RVA: 0x00145CC4 File Offset: 0x00143EC4
		protected virtual Trigger_TicksPassed GetTimeoutTrigger()
		{
			return new Trigger_TicksPassed(this.durationTicks);
		}

		// Token: 0x06003A24 RID: 14884 RVA: 0x00145CD4 File Offset: 0x00143ED4
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.spot, "spot", default(IntVec3), false);
			Scribe_References.Look<Pawn>(ref this.organizer, "organizer", false);
			Scribe_Defs.Look<GatheringDef>(ref this.gatheringDef, "gatheringDef");
		}

		// Token: 0x06003A25 RID: 14885 RVA: 0x00145D1C File Offset: 0x00143F1C
		protected bool IsGatheringAboutToEnd()
		{
			return this.TicksLeft < 1200;
		}

		// Token: 0x06003A26 RID: 14886 RVA: 0x00145D2E File Offset: 0x00143F2E
		protected virtual bool IsInvited(Pawn p)
		{
			return this.lord.faction != null && p.Faction == this.lord.faction;
		}

		// Token: 0x04001FEB RID: 8171
		protected IntVec3 spot;

		// Token: 0x04001FEC RID: 8172
		protected Pawn organizer;

		// Token: 0x04001FED RID: 8173
		protected GatheringDef gatheringDef;

		// Token: 0x04001FEE RID: 8174
		protected int durationTicks;

		// Token: 0x04001FEF RID: 8175
		protected Trigger_TicksPassed timeoutTrigger;
	}
}
