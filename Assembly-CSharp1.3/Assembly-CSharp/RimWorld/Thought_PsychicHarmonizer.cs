using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001571 RID: 5489
	public class Thought_PsychicHarmonizer : Thought_Memory
	{
		// Token: 0x170015F6 RID: 5622
		// (get) Token: 0x060081D6 RID: 33238 RVA: 0x002DE4F0 File Offset: 0x002DC6F0
		public override string LabelCap
		{
			get
			{
				return base.CurStage.label.Formatted(this.harmonizer.pawn.Named("HARMONIZER")).CapitalizeFirst();
			}
		}

		// Token: 0x060081D7 RID: 33239 RVA: 0x002DE52F File Offset: 0x002DC72F
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Hediff>(ref this.harmonizer, "harmonizer", false);
		}

		// Token: 0x060081D8 RID: 33240 RVA: 0x002DE548 File Offset: 0x002DC748
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

		// Token: 0x060081D9 RID: 33241 RVA: 0x001DE61B File Offset: 0x001DC81B
		public override bool TryMergeWithExistingMemory(out bool showBubble)
		{
			showBubble = false;
			return false;
		}

		// Token: 0x060081DA RID: 33242 RVA: 0x002DE5C8 File Offset: 0x002DC7C8
		public override bool GroupsWith(Thought other)
		{
			Thought_PsychicHarmonizer thought_PsychicHarmonizer = other as Thought_PsychicHarmonizer;
			return thought_PsychicHarmonizer != null && base.GroupsWith(other) && thought_PsychicHarmonizer.harmonizer == this.harmonizer;
		}

		// Token: 0x170015F7 RID: 5623
		// (get) Token: 0x060081DB RID: 33243 RVA: 0x002DE5FC File Offset: 0x002DC7FC
		public override bool ShouldDiscard
		{
			get
			{
				Pawn pawn = this.harmonizer.pawn;
				return pawn.health.Dead || pawn.needs == null || pawn.needs.mood == null || this.pawn.health.hediffSet.HasHediff(HediffDefOf.PsychicHarmonizer, false) || ((pawn.Spawned || this.pawn.Spawned || pawn.GetCaravan() != this.pawn.GetCaravan()) && (!pawn.Spawned || !this.pawn.Spawned || pawn.Map != this.pawn.Map || pawn.Position.DistanceTo(this.pawn.Position) > this.harmonizer.TryGetComp<HediffComp_PsychicHarmonizer>().Props.range));
			}
		}

		// Token: 0x040050CD RID: 20685
		public Hediff harmonizer;
	}
}
