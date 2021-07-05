using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000892 RID: 2194
	public class LordJob_Joinable_Concert : LordJob_Joinable_Party
	{
		// Token: 0x17000A5B RID: 2651
		// (get) Token: 0x06003A10 RID: 14864 RVA: 0x00145B00 File Offset: 0x00143D00
		protected override ThoughtDef AttendeeThought
		{
			get
			{
				return ThoughtDefOf.AttendedConcert;
			}
		}

		// Token: 0x17000A5C RID: 2652
		// (get) Token: 0x06003A11 RID: 14865 RVA: 0x00145B07 File Offset: 0x00143D07
		protected override TaleDef AttendeeTale
		{
			get
			{
				return TaleDefOf.AttendedConcert;
			}
		}

		// Token: 0x17000A5D RID: 2653
		// (get) Token: 0x06003A12 RID: 14866 RVA: 0x00145B0E File Offset: 0x00143D0E
		protected override ThoughtDef OrganizerThought
		{
			get
			{
				return ThoughtDefOf.HeldConcert;
			}
		}

		// Token: 0x17000A5E RID: 2654
		// (get) Token: 0x06003A13 RID: 14867 RVA: 0x00145B15 File Offset: 0x00143D15
		protected override TaleDef OrganizerTale
		{
			get
			{
				return TaleDefOf.HeldConcert;
			}
		}

		// Token: 0x06003A14 RID: 14868 RVA: 0x00145B1C File Offset: 0x00143D1C
		public LordJob_Joinable_Concert()
		{
		}

		// Token: 0x06003A15 RID: 14869 RVA: 0x00145B24 File Offset: 0x00143D24
		public LordJob_Joinable_Concert(IntVec3 spot, Pawn organizer, GatheringDef gatheringDef) : base(spot, organizer, gatheringDef)
		{
		}

		// Token: 0x06003A16 RID: 14870 RVA: 0x00145B2F File Offset: 0x00143D2F
		public override string GetReport(Pawn pawn)
		{
			if (pawn != this.organizer)
			{
				return "LordReportAttendingConcert".Translate();
			}
			return "LordReportHoldingConcert".Translate();
		}

		// Token: 0x06003A17 RID: 14871 RVA: 0x00145B59 File Offset: 0x00143D59
		protected override LordToil CreateGatheringToil(IntVec3 spot, Pawn organizer, GatheringDef gatheringDef)
		{
			return new LordToil_Concert(spot, organizer, gatheringDef, 3.5E-05f);
		}

		// Token: 0x06003A18 RID: 14872 RVA: 0x00145B68 File Offset: 0x00143D68
		protected override Trigger_TicksPassed GetTimeoutTrigger()
		{
			return new Trigger_TicksPassedAfterConditionMet(base.DurationTicks, () => GatheringsUtility.InGatheringArea(this.organizer.Position, this.spot, this.organizer.Map), 60);
		}
	}
}
