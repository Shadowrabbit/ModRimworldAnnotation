using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DFF RID: 3583
	public class InteractionWorker_Suppress : InteractionWorker
	{
		// Token: 0x060052E9 RID: 21225 RVA: 0x001C0EA8 File Offset: 0x001BF0A8
		public override void Interacted(Pawn initiator, Pawn recipient, List<RulePackDef> extraSentencePacks, out string letterText, out string letterLabel, out LetterDef letterDef, out LookTargets lookTargets)
		{
			letterText = null;
			letterLabel = null;
			letterDef = null;
			lookTargets = null;
			Pawn_NeedsTracker needs = recipient.needs;
			Need_Suppression need_Suppression = (needs != null) ? needs.TryGetNeed<Need_Suppression>() : null;
			if (need_Suppression == null)
			{
				return;
			}
			float statValue = initiator.GetStatValue(StatDefOf.SuppressionPower, true);
			float num = InteractionWorker_Suppress.CurrentSuppressionFactorCurve.Evaluate(need_Suppression.CurLevel);
			need_Suppression.CurLevelPercentage += statValue * num * need_Suppression.MaxLevel;
			TaggedString taggedString = "TextMote_SuppressionIncreased".Translate(need_Suppression.CurLevel.ToStringPercent());
			MoteMaker.ThrowText((initiator.DrawPos + recipient.DrawPos) / 2f, initiator.Map, taggedString, 8f);
		}

		// Token: 0x040030F0 RID: 12528
		private static readonly SimpleCurve CurrentSuppressionFactorCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 2f),
				true
			},
			{
				new CurvePoint(0.5f, 1f),
				true
			},
			{
				new CurvePoint(1f, 0.5f),
				true
			}
		};
	}
}
