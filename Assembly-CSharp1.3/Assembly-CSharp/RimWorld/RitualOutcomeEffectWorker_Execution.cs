using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F75 RID: 3957
	public class RitualOutcomeEffectWorker_Execution : RitualOutcomeEffectWorker_FromQuality
	{
		// Token: 0x17001038 RID: 4152
		// (get) Token: 0x06005DD3 RID: 24019 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool SupportsAttachableOutcomeEffect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06005DD4 RID: 24020 RVA: 0x002019DD File Offset: 0x001FFBDD
		public RitualOutcomeEffectWorker_Execution()
		{
		}

		// Token: 0x06005DD5 RID: 24021 RVA: 0x002019E5 File Offset: 0x001FFBE5
		public RitualOutcomeEffectWorker_Execution(RitualOutcomeEffectDef def) : base(def)
		{
		}

		// Token: 0x06005DD6 RID: 24022 RVA: 0x00203318 File Offset: 0x00201518
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
			bool flag = false;
			foreach (Pawn pawn in totalPresence.Keys)
			{
				if (pawn.IsSlave)
				{
					Need_Suppression need_Suppression = pawn.needs.TryGetNeed<Need_Suppression>();
					if (need_Suppression != null)
					{
						need_Suppression.CurLevel = 1f;
					}
					flag = true;
				}
				else
				{
					base.GiveMemoryToPawn(pawn, outcome.memory, jobRitual);
				}
			}
			string text2 = outcome.description.Formatted(jobRitual.Ritual.Label).CapitalizeFirst() + "\n\n" + this.OutcomeQualityBreakdownDesc(quality, progress, jobRitual);
			string text3 = this.def.OutcomeMoodBreakdown(outcome);
			if (!text3.NullOrEmpty())
			{
				text2 = text2 + "\n\n" + text3;
			}
			if (flag)
			{
				text2 += "\n\n" + "RitualOutcomeExtraDesc_Execution".Translate();
			}
			if (text != null)
			{
				text2 = text2 + "\n\n" + text;
			}
			Find.LetterStack.ReceiveLetter("OutcomeLetterLabel".Translate(outcome.label.Named("OUTCOMELABEL"), jobRitual.Ritual.Label.Named("RITUALLABEL")), text2, outcome.Positive ? LetterDefOf.RitualOutcomePositive : LetterDefOf.RitualOutcomeNegative, lookTargets, null, null, null, null);
		}
	}
}
