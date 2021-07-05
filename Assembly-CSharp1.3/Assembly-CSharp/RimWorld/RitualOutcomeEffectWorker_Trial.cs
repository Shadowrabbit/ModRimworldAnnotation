using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F73 RID: 3955
	public class RitualOutcomeEffectWorker_Trial : RitualOutcomeEffectWorker_FromQuality
	{
		// Token: 0x17001036 RID: 4150
		// (get) Token: 0x06005DC9 RID: 24009 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool SupportsAttachableOutcomeEffect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06005DCA RID: 24010 RVA: 0x002019DD File Offset: 0x001FFBDD
		public RitualOutcomeEffectWorker_Trial()
		{
		}

		// Token: 0x06005DCB RID: 24011 RVA: 0x002019E5 File Offset: 0x001FFBE5
		public RitualOutcomeEffectWorker_Trial(RitualOutcomeEffectDef def) : base(def)
		{
		}

		// Token: 0x06005DCC RID: 24012 RVA: 0x00202F34 File Offset: 0x00201134
		public override void Apply(float progress, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual)
		{
			float quality = base.GetQuality(jobRitual, progress);
			OutcomeChance outcome = this.GetOutcome(quality, jobRitual);
			Pawn pawn = jobRitual.PawnWithRole("leader");
			Pawn pawn2 = jobRitual.PawnWithRole("convict");
			LookTargets lookTargets = pawn2;
			string text = null;
			if (jobRitual.Ritual != null)
			{
				this.ApplyAttachableOutcome(totalPresence, jobRitual, outcome, out text, ref lookTargets);
			}
			string str = pawn2.LabelShort + " " + outcome.label;
			TaggedString taggedString = outcome.description.Formatted(pawn2.Named("PAWN"), pawn.Named("PROSECUTOR"));
			string text2 = this.def.OutcomeMoodBreakdown(outcome);
			if (!text2.NullOrEmpty())
			{
				taggedString += "\n\n" + text2;
			}
			taggedString += "\n\n" + this.OutcomeQualityBreakdownDesc(quality, progress, jobRitual);
			if (text != null)
			{
				taggedString += "\n\n" + text;
			}
			if (outcome.Positive)
			{
				ChoiceLetter let = LetterMaker.MakeLetter(str, taggedString, LetterDefOf.RitualOutcomePositive, lookTargets, null, null, null);
				Find.LetterStack.ReceiveLetter(let, null);
				pawn2.guilt.Notify_Guilty(900000);
				pawn2.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.TrialConvicted, null, null);
				return;
			}
			Find.LetterStack.ReceiveLetter(str, taggedString, LetterDefOf.RitualOutcomeNegative, lookTargets, null, null, null, null);
			pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.TrialFailed, null, null);
			pawn2.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.TrialExonerated, null, null);
		}

		// Token: 0x06005DCD RID: 24013 RVA: 0x002030E8 File Offset: 0x002012E8
		public override OutcomeChance GetOutcome(float quality, LordJob_Ritual ritual)
		{
			if (!Rand.Chance(quality))
			{
				return this.def.outcomeChances[0];
			}
			return this.def.outcomeChances[1];
		}

		// Token: 0x06005DCE RID: 24014 RVA: 0x00203115 File Offset: 0x00201315
		public override string ExpectedQualityLabel()
		{
			return "ExpectedConvictionChance".Translate();
		}

		// Token: 0x0400362E RID: 13870
		public const int ConvictGuiltyForDays = 15;
	}
}
