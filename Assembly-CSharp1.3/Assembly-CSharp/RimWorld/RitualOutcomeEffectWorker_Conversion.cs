using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F74 RID: 3956
	public class RitualOutcomeEffectWorker_Conversion : RitualOutcomeEffectWorker_FromQuality
	{
		// Token: 0x17001037 RID: 4151
		// (get) Token: 0x06005DCF RID: 24015 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool SupportsAttachableOutcomeEffect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06005DD0 RID: 24016 RVA: 0x002019DD File Offset: 0x001FFBDD
		public RitualOutcomeEffectWorker_Conversion()
		{
		}

		// Token: 0x06005DD1 RID: 24017 RVA: 0x002019E5 File Offset: 0x001FFBE5
		public RitualOutcomeEffectWorker_Conversion(RitualOutcomeEffectDef def) : base(def)
		{
		}

		// Token: 0x06005DD2 RID: 24018 RVA: 0x00203128 File Offset: 0x00201328
		public override void Apply(float progress, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual)
		{
			float quality = base.GetQuality(jobRitual, progress);
			OutcomeChance outcome = this.GetOutcome(quality, jobRitual);
			LookTargets lookTargets = jobRitual.selectedTarget;
			string text = null;
			if (jobRitual.Ritual != null)
			{
				this.ApplyAttachableOutcome(totalPresence, jobRitual, outcome, out text, ref lookTargets);
			}
			Pawn pawn = jobRitual.PawnWithRole("moralist");
			Pawn pawn2 = jobRitual.PawnWithRole("convertee");
			float ideoCertaintyOffset = outcome.ideoCertaintyOffset;
			if (ideoCertaintyOffset <= -1f)
			{
				pawn2.ideo.SetIdeo(pawn.Ideo);
			}
			else
			{
				pawn2.ideo.OffsetCertainty(ideoCertaintyOffset);
			}
			foreach (Pawn pawn3 in totalPresence.Keys)
			{
				if (pawn3 != pawn && pawn3 != pawn2 && outcome.memory != null)
				{
					Thought_AttendedRitual newThought = (Thought_AttendedRitual)base.MakeMemory(pawn3, jobRitual, outcome.memory);
					pawn3.needs.mood.thoughts.memories.TryGainMemory(newThought, null);
				}
			}
			TaggedString t = outcome.description.Formatted(jobRitual.Ritual.Label).CapitalizeFirst();
			string text2 = this.def.OutcomeMoodBreakdown(outcome);
			if (!text2.NullOrEmpty())
			{
				t += "\n\n" + text2;
			}
			if (text != null)
			{
				t += "\n\n" + text;
			}
			Find.LetterStack.ReceiveLetter("OutcomeLetterLabel".Translate(outcome.label.Named("OUTCOMELABEL"), jobRitual.Ritual.Label.Named("RITUALLABEL")), t + "\n\n" + this.OutcomeQualityBreakdownDesc(quality, progress, jobRitual), outcome.Positive ? LetterDefOf.RitualOutcomePositive : LetterDefOf.RitualOutcomeNegative, lookTargets, null, null, null, null);
		}
	}
}
