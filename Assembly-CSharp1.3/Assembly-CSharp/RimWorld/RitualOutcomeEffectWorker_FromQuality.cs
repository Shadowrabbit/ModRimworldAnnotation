using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F70 RID: 3952
	public class RitualOutcomeEffectWorker_FromQuality : RitualOutcomeEffectWorker
	{
		// Token: 0x17001034 RID: 4148
		// (get) Token: 0x06005DB1 RID: 23985 RVA: 0x002019D0 File Offset: 0x001FFBD0
		public override bool SupportsAttachableOutcomeEffect
		{
			get
			{
				return this.def.allowAttachableOutcome;
			}
		}

		// Token: 0x06005DB2 RID: 23986 RVA: 0x0020193C File Offset: 0x001FFB3C
		public RitualOutcomeEffectWorker_FromQuality()
		{
		}

		// Token: 0x06005DB3 RID: 23987 RVA: 0x00201944 File Offset: 0x001FFB44
		public RitualOutcomeEffectWorker_FromQuality(RitualOutcomeEffectDef def) : base(def)
		{
		}

		// Token: 0x06005DB4 RID: 23988 RVA: 0x00201C20 File Offset: 0x001FFE20
		public virtual OutcomeChance GetOutcome(float quality, LordJob_Ritual ritual)
		{
			return (from o in this.def.outcomeChances
			where this.OutcomePossible(o, ritual)
			select o).RandomElementByWeight(delegate(OutcomeChance c)
			{
				if (!c.Positive)
				{
					return c.chance;
				}
				return Mathf.Max(c.chance * quality, 0f);
			});
		}

		// Token: 0x06005DB5 RID: 23989 RVA: 0x00201C78 File Offset: 0x001FFE78
		protected virtual void ApplyAttachableOutcome(Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, OutcomeChance outcomeChance, out string extraLetterText, ref LookTargets letterLookTargets)
		{
			extraLetterText = null;
			if (jobRitual.Ritual.attachableOutcomeEffect != null && jobRitual.Ritual.attachableOutcomeEffect.AppliesToOutcome(jobRitual.Ritual.outcomeEffect.def, outcomeChance))
			{
				jobRitual.Ritual.attachableOutcomeEffect.Worker.Apply(totalPresence, jobRitual, outcomeChance, out extraLetterText, ref letterLookTargets);
			}
		}

		// Token: 0x06005DB6 RID: 23990 RVA: 0x00201CD8 File Offset: 0x001FFED8
		public override void Apply(float progress, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual)
		{
			float quality = this.GetQuality(jobRitual, progress);
			OutcomeChance outcome = this.GetOutcome(quality, jobRitual);
			LookTargets lookTargets = jobRitual.selectedTarget;
			string text;
			this.ApplyExtraOutcome(totalPresence, jobRitual, outcome, out text, ref lookTargets);
			string text2 = null;
			if (jobRitual.Ritual != null)
			{
				this.ApplyAttachableOutcome(totalPresence, jobRitual, outcome, out text2, ref lookTargets);
			}
			string text3 = outcome.description.Formatted(jobRitual.Ritual.Label).CapitalizeFirst();
			string text4 = this.def.OutcomeMoodBreakdown(outcome);
			if (!text4.NullOrEmpty())
			{
				text3 = text3 + "\n\n" + text4;
			}
			if (text != null)
			{
				text3 = text3 + "\n\n" + text;
			}
			if (text2 != null)
			{
				text3 = text3 + "\n\n" + text2;
			}
			text3 = text3 + "\n\n" + this.OutcomeQualityBreakdownDesc(quality, progress, jobRitual);
			Find.LetterStack.ReceiveLetter("OutcomeLetterLabel".Translate(outcome.label.Named("OUTCOMELABEL"), jobRitual.Ritual.Label.Named("RITUALLABEL")), text3, outcome.Positive ? LetterDefOf.RitualOutcomePositive : LetterDefOf.RitualOutcomeNegative, lookTargets, null, null, null, null);
			foreach (KeyValuePair<Pawn, int> keyValuePair in totalPresence)
			{
				if (!outcome.roleIdsNotGainingMemory.NullOrEmpty<string>())
				{
					RitualRole ritualRole = jobRitual.assignments.RoleForPawn(keyValuePair.Key, true);
					if (ritualRole != null && outcome.roleIdsNotGainingMemory.Contains(ritualRole.id))
					{
						continue;
					}
				}
				this.GiveMemoryToPawn(keyValuePair.Key, outcome.memory, jobRitual);
			}
		}

		// Token: 0x06005DB7 RID: 23991 RVA: 0x00201E9C File Offset: 0x0020009C
		protected void GiveMemoryToPawn(Pawn pawn, ThoughtDef memory, LordJob_Ritual jobRitual)
		{
			Thought_AttendedRitual newThought = (Thought_AttendedRitual)base.MakeMemory(pawn, jobRitual, memory);
			pawn.needs.mood.thoughts.memories.TryGainMemory(newThought, null);
		}

		// Token: 0x06005DB8 RID: 23992 RVA: 0x00201ED4 File Offset: 0x002000D4
		protected virtual void ApplyExtraOutcome(Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, OutcomeChance outcome, out string extraOutcomeDesc, ref LookTargets letterLookTargets)
		{
			extraOutcomeDesc = null;
		}

		// Token: 0x06005DB9 RID: 23993 RVA: 0x00201EDC File Offset: 0x002000DC
		protected float GetQuality(LordJob_Ritual jobRitual, float progress)
		{
			float num = this.def.startingQuality;
			foreach (RitualOutcomeComp ritualOutcomeComp in this.def.comps)
			{
				if (ritualOutcomeComp is RitualOutcomeComp_Quality && ritualOutcomeComp.Applies(jobRitual))
				{
					num += ritualOutcomeComp.QualityOffset(jobRitual, base.DataForComp(ritualOutcomeComp));
				}
			}
			if (jobRitual.repeatPenalty && jobRitual.Ritual != null)
			{
				num += jobRitual.Ritual.RepeatQualityPenalty;
			}
			Map map = jobRitual.Map;
			Precept_Ritual ritual = jobRitual.Ritual;
			Tuple<ExpectationDef, float> expectationsOffset = RitualOutcomeEffectWorker_FromQuality.GetExpectationsOffset(map, (ritual != null) ? ritual.def : null);
			if (expectationsOffset != null)
			{
				num += expectationsOffset.Item2;
			}
			return Mathf.Clamp(num * Mathf.Lerp(RitualOutcomeEffectWorker_FromQuality.ProgressToQualityMapping.min, RitualOutcomeEffectWorker_FromQuality.ProgressToQualityMapping.max, progress), this.def.minQuality, this.def.maxQuality);
		}

		// Token: 0x06005DBA RID: 23994 RVA: 0x00201FDC File Offset: 0x002001DC
		public static Tuple<ExpectationDef, float> GetExpectationsOffset(Map map, PreceptDef ritual)
		{
			if (ritual == null || !ritual.receivesExpectationsQualityOffset)
			{
				return null;
			}
			ExpectationDef expectationDef = ExpectationsUtility.CurrentExpectationFor(map);
			if (Math.Abs(expectationDef.ritualQualityOffset) > 1E-45f)
			{
				return new Tuple<ExpectationDef, float>(expectationDef, expectationDef.ritualQualityOffset);
			}
			return null;
		}

		// Token: 0x06005DBB RID: 23995 RVA: 0x00202020 File Offset: 0x00200220
		protected virtual string OutcomeQualityBreakdownDesc(float quality, float progress, LordJob_Ritual jobRitual)
		{
			TaggedString taggedString = "RitualOutcomeQualitySpecific".Translate(jobRitual.Ritual.Label, quality.ToStringPercent()).CapitalizeFirst() + ":\n";
			if (this.def.startingQuality > 0f)
			{
				taggedString += "\n  - " + "StartingRitualQuality".Translate(this.def.startingQuality.ToStringPercent()) + ".";
			}
			foreach (RitualOutcomeComp ritualOutcomeComp in this.def.comps)
			{
				if (ritualOutcomeComp is RitualOutcomeComp_Quality && ritualOutcomeComp.Applies(jobRitual) && Mathf.Abs(ritualOutcomeComp.QualityOffset(jobRitual, base.DataForComp(ritualOutcomeComp))) >= 1E-45f)
				{
					taggedString += "\n  - " + ritualOutcomeComp.GetDesc(jobRitual, base.DataForComp(ritualOutcomeComp)).CapitalizeFirst();
				}
			}
			if (jobRitual.repeatPenalty && jobRitual.Ritual != null)
			{
				taggedString += "\n  - " + "RitualOutcomePerformedRecently".Translate() + ": " + jobRitual.Ritual.RepeatQualityPenalty.ToStringPercent();
			}
			Map map = jobRitual.Map;
			Precept_Ritual ritual = jobRitual.Ritual;
			Tuple<ExpectationDef, float> expectationsOffset = RitualOutcomeEffectWorker_FromQuality.GetExpectationsOffset(map, (ritual != null) ? ritual.def : null);
			if (expectationsOffset != null)
			{
				taggedString += "\n  - " + "RitualQualityExpectations".Translate(expectationsOffset.Item1.LabelCap) + ": " + expectationsOffset.Item2.ToStringPercent();
			}
			if (progress < 1f)
			{
				taggedString += "\n  - " + "RitualOutcomeProgress".Translate(jobRitual.Ritual.Label).CapitalizeFirst() + ": x" + Mathf.Lerp(RitualOutcomeEffectWorker_FromQuality.ProgressToQualityMapping.min, RitualOutcomeEffectWorker_FromQuality.ProgressToQualityMapping.max, progress).ToStringPercent();
			}
			return taggedString;
		}

		// Token: 0x06005DBC RID: 23996 RVA: 0x000126F5 File Offset: 0x000108F5
		protected virtual bool OutcomePossible(OutcomeChance chance, LordJob_Ritual ritual)
		{
			return true;
		}

		// Token: 0x06005DBD RID: 23997 RVA: 0x0020226C File Offset: 0x0020046C
		public override string ExtraAlertParagraph(Precept_Ritual ritual)
		{
			string text = "";
			foreach (RitualOutcomeComp ritualOutcomeComp in this.def.comps)
			{
				if (ritualOutcomeComp is RitualOutcomeComp_Quality)
				{
					text = text + "\n  - " + ritualOutcomeComp.GetDesc(null, null).CapitalizeFirst();
				}
			}
			string text2 = ("RitualOutcomeQualityAbstract".Translate(ritual.Label).Resolve().CapitalizeFirst() + ":").Colorize(ColoredText.TipSectionTitleColor) + text;
			return string.Concat(new string[]
			{
				text2,
				"\n  - ",
				"RitualOutcomeProgress".Translate(ritual.Label).Resolve().CapitalizeFirst(),
				": ",
				"OutcomeBonusDesc_QualitySingleOffset".Translate(string.Concat(new object[]
				{
					"x",
					RitualOutcomeEffectWorker_FromQuality.ProgressToQualityMapping.min * 100f,
					"-",
					RitualOutcomeEffectWorker_FromQuality.ProgressToQualityMapping.max.ToStringPercent()
				})).Resolve(),
				"."
			});
		}

		// Token: 0x0400362B RID: 13867
		public static FloatRange ProgressToQualityMapping = new FloatRange(0.25f, 1f);
	}
}
