using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DFA RID: 7674
	public class Thought_PsychicHarmonizer : Thought_Memory
	{
		// Token: 0x17001964 RID: 6500
		// (get) Token: 0x0600A63E RID: 42558 RVA: 0x00303444 File Offset: 0x00301644
		public override string LabelCap
		{
			get
			{
				return base.CurStage.label.Formatted(this.harmonizer.pawn.Named("HARMONIZER")).CapitalizeFirst();
			}
		}

		// Token: 0x0600A63F RID: 42559 RVA: 0x0006DFB7 File Offset: 0x0006C1B7
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Hediff>(ref this.harmonizer, "harmonizer", false);
		}

		// Token: 0x0600A640 RID: 42560 RVA: 0x00303484 File Offset: 0x00301684
		public override float MoodOffset()
		{
			if (ThoughtUtility.ThoughtNullified(this.pawn, this.def))
			{
				return 0f;
			}
			if (this.ShouldDiscard)
			{
				return 0f;
			}
			float num = base.MoodOffset();
			float num2 = Mathf.Lerp(-1f, 1f, this.harmonizer.pawn.needs.mood.CurLevel);
			float statValue = this.harmonizer.pawn.GetStatValue(StatDefOf.PsychicSensitivity, true);
			return num * num2 * statValue;
		}

		// Token: 0x0600A641 RID: 42561 RVA: 0x00050420 File Offset: 0x0004E620
		public override bool TryMergeWithExistingMemory(out bool showBubble)
		{
			showBubble = false;
			return false;
		}

		// Token: 0x0600A642 RID: 42562 RVA: 0x00303504 File Offset: 0x00301704
		public override bool GroupsWith(Thought other)
		{
			Thought_PsychicHarmonizer thought_PsychicHarmonizer = other as Thought_PsychicHarmonizer;
			return thought_PsychicHarmonizer != null && base.GroupsWith(other) && thought_PsychicHarmonizer.harmonizer == this.harmonizer;
		}

		// Token: 0x17001965 RID: 6501
		// (get) Token: 0x0600A643 RID: 42563 RVA: 0x00303538 File Offset: 0x00301738
		public override bool ShouldDiscard
		{
			get
			{
				Pawn pawn = this.harmonizer.pawn;
				return pawn.health.Dead || pawn.needs == null || pawn.needs.mood == null || this.pawn.health.hediffSet.HasHediff(HediffDefOf.PsychicHarmonizer, false) || ((pawn.Spawned || this.pawn.Spawned || pawn.GetCaravan() != this.pawn.GetCaravan()) && (!pawn.Spawned || !this.pawn.Spawned || pawn.Map != this.pawn.Map || pawn.Position.DistanceTo(this.pawn.Position) > this.harmonizer.TryGetComp<HediffComp_PsychicHarmonizer>().Props.range));
			}
		}

		// Token: 0x040070B3 RID: 28851
		public Hediff harmonizer;
	}
}
