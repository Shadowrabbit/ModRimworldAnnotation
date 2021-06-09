using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000DD6 RID: 3542
	public class LordJob_Joinable_Concert : LordJob_Joinable_Party
	{
		// Token: 0x17000C5E RID: 3166
		// (get) Token: 0x060050B5 RID: 20661 RVA: 0x00038992 File Offset: 0x00036B92
		protected override ThoughtDef AttendeeThought
		{
			get
			{
				return ThoughtDefOf.AttendedConcert;
			}
		}

		// Token: 0x17000C5F RID: 3167
		// (get) Token: 0x060050B6 RID: 20662 RVA: 0x00038999 File Offset: 0x00036B99
		protected override TaleDef AttendeeTale
		{
			get
			{
				return TaleDefOf.AttendedConcert;
			}
		}

		// Token: 0x17000C60 RID: 3168
		// (get) Token: 0x060050B7 RID: 20663 RVA: 0x000389A0 File Offset: 0x00036BA0
		protected override ThoughtDef OrganizerThought
		{
			get
			{
				return ThoughtDefOf.HeldConcert;
			}
		}

		// Token: 0x17000C61 RID: 3169
		// (get) Token: 0x060050B8 RID: 20664 RVA: 0x000389A7 File Offset: 0x00036BA7
		protected override TaleDef OrganizerTale
		{
			get
			{
				return TaleDefOf.HeldConcert;
			}
		}

		// Token: 0x060050B9 RID: 20665 RVA: 0x000389AE File Offset: 0x00036BAE
		public LordJob_Joinable_Concert()
		{
		}

		// Token: 0x060050BA RID: 20666 RVA: 0x000389B6 File Offset: 0x00036BB6
		public LordJob_Joinable_Concert(IntVec3 spot, Pawn organizer, GatheringDef gatheringDef) : base(spot, organizer, gatheringDef)
		{
		}

		// Token: 0x060050BB RID: 20667 RVA: 0x000389C1 File Offset: 0x00036BC1
		public override string GetReport(Pawn pawn)
		{
			if (pawn != this.organizer)
			{
				return "LordReportAttendingConcert".Translate();
			}
			return "LordReportHoldingConcert".Translate();
		}

		// Token: 0x060050BC RID: 20668 RVA: 0x000389EB File Offset: 0x00036BEB
		protected override LordToil CreateGatheringToil(IntVec3 spot, Pawn organizer, GatheringDef gatheringDef)
		{
			return new LordToil_Concert(spot, organizer, gatheringDef, 3.5E-05f);
		}

		// Token: 0x060050BD RID: 20669 RVA: 0x000389FA File Offset: 0x00036BFA
		protected override Trigger_TicksPassed GetTimeoutTrigger()
		{
			return new Trigger_TicksPassedAfterConditionMet(base.DurationTicks, () => GatheringsUtility.InGatheringArea(this.organizer.Position, this.spot, this.organizer.Map), 60);
		}
	}
}
