using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F77 RID: 3959
	public class RitualOutcomeEffectWorker_AnimaTreeLinking : RitualOutcomeEffectWorker_FromQuality
	{
		// Token: 0x1700103A RID: 4154
		// (get) Token: 0x06005DDB RID: 24027 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool SupportsAttachableOutcomeEffect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06005DDC RID: 24028 RVA: 0x002019DD File Offset: 0x001FFBDD
		public RitualOutcomeEffectWorker_AnimaTreeLinking()
		{
		}

		// Token: 0x06005DDD RID: 24029 RVA: 0x002019E5 File Offset: 0x001FFBE5
		public RitualOutcomeEffectWorker_AnimaTreeLinking(RitualOutcomeEffectDef def) : base(def)
		{
		}

		// Token: 0x06005DDE RID: 24030 RVA: 0x00203604 File Offset: 0x00201804
		public override void Apply(float progress, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual)
		{
			float quality = base.GetQuality(jobRitual, progress);
			Pawn pawn = jobRitual.PawnWithRole("organizer");
			Thing thing = jobRitual.selectedTarget.Thing;
			object obj = (thing != null) ? thing.TryGetComp<CompPsylinkable>() : null;
			int num = (int)RitualOutcomeEffectWorker_AnimaTreeLinking.RestoredGrassFromQuality.Evaluate(quality);
			object obj2 = obj;
			if (obj2 != null)
			{
				obj2.FinishLinkingRitual(pawn, num);
			}
			string text = "LetterTextLinkingRitualCompleted".Translate(pawn.Named("PAWN"), jobRitual.selectedTarget.Thing.Named("LINKABLE"));
			if (num > 0)
			{
				text += " " + "LetterTextLinkingRitualCompletedAnimaGrass".Translate(num);
			}
			text = text + "\n\n" + this.OutcomeQualityBreakdownDesc(quality, progress, jobRitual);
			Find.LetterStack.ReceiveLetter("LetterLabelLinkingRitualCompleted".Translate(), text, LetterDefOf.RitualOutcomePositive, new LookTargets(new TargetInfo[]
			{
				pawn,
				jobRitual.selectedTarget.Thing
			}), null, null, null, null);
		}

		// Token: 0x04003630 RID: 13872
		public static readonly SimpleCurve RestoredGrassFromQuality = new SimpleCurve
		{
			{
				new CurvePoint(0.2f, 0f),
				true
			},
			{
				new CurvePoint(0.4f, 1f),
				true
			},
			{
				new CurvePoint(0.6f, 3f),
				true
			},
			{
				new CurvePoint(0.8f, 5f),
				true
			},
			{
				new CurvePoint(1f, 8f),
				true
			}
		};
	}
}
