using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000897 RID: 2199
	public class LordJob_Joinable_Speech : LordJob_Ritual
	{
		// Token: 0x17000A6A RID: 2666
		// (get) Token: 0x06003A4F RID: 14927 RVA: 0x00146DB2 File Offset: 0x00144FB2
		protected override int MinTicksToFinish
		{
			get
			{
				return base.DurationTicks / 2;
			}
		}

		// Token: 0x17000A6B RID: 2667
		// (get) Token: 0x06003A50 RID: 14928 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool AllowStartNewGatherings
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000A6C RID: 2668
		// (get) Token: 0x06003A51 RID: 14929 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool OrganizerIsStartingPawn
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06003A52 RID: 14930 RVA: 0x001435B7 File Offset: 0x001417B7
		public LordJob_Joinable_Speech()
		{
		}

		// Token: 0x06003A53 RID: 14931 RVA: 0x00146DBC File Offset: 0x00144FBC
		public LordJob_Joinable_Speech(TargetInfo spot, Pawn organizer, Precept_Ritual ritual, List<RitualStage> stages, RitualRoleAssignments assignments, bool titleSpeech) : base(spot, ritual, null, stages, assignments, organizer)
		{
			Building_Throne firstThing = spot.Cell.GetFirstThing(organizer.Map);
			if (firstThing != null)
			{
				this.selectedTarget = firstThing;
			}
			this.titleSpeech = titleSpeech;
		}

		// Token: 0x06003A54 RID: 14932 RVA: 0x00146E01 File Offset: 0x00145001
		protected override LordToil_Ritual MakeToil(RitualStage stage)
		{
			if (stage == null)
			{
				return new LordToil_Speech(this.spot, this.ritual, this, this.organizer);
			}
			return new LordToil_Ritual(this.spot, this, stage, this.organizer);
		}

		// Token: 0x06003A55 RID: 14933 RVA: 0x00146E34 File Offset: 0x00145034
		public override string GetReport(Pawn pawn)
		{
			return ((pawn != this.organizer) ? "LordReportListeningSpeech".Translate(this.organizer.Named("ORGANIZER")) : "LordReportGivingSpeech".Translate()) + " " + base.TimeLeftPostfix;
		}

		// Token: 0x06003A56 RID: 14934 RVA: 0x00146E8C File Offset: 0x0014508C
		protected override void ApplyOutcome(float progress, List<LordToil_Ritual> toils, bool showFinishedMessage = true, bool showFailedMessage = true, bool cancelled = false)
		{
			if (this.ticksPassed < this.MinTicksToFinish || cancelled)
			{
				Find.LetterStack.ReceiveLetter("LetterLabelSpeechCancelled".Translate(), "LetterSpeechCancelled".Translate(this.titleSpeech ? GrammarResolverSimple.PawnResolveBestRoyalTitle(this.organizer) : this.organizer.LabelShort).CapitalizeFirst(), LetterDefOf.NegativeEvent, this.organizer, null, null, null, null);
				return;
			}
			base.ApplyOutcome(progress, toils, false, true, false);
		}

		// Token: 0x04001FF4 RID: 8180
		private bool titleSpeech;
	}
}
