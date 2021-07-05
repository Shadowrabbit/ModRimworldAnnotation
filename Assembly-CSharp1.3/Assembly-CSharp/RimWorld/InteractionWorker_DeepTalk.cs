using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DF4 RID: 3572
	public class InteractionWorker_DeepTalk : InteractionWorker
	{
		// Token: 0x060052C7 RID: 21191 RVA: 0x001BEF59 File Offset: 0x001BD159
		public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
		{
			return 0.075f * this.CompatibilityFactorCurve.Evaluate(initiator.relations.CompatibilityWith(recipient));
		}

		// Token: 0x040030D3 RID: 12499
		private const float BaseSelectionWeight = 0.075f;

		// Token: 0x040030D4 RID: 12500
		private SimpleCurve CompatibilityFactorCurve = new SimpleCurve
		{
			{
				new CurvePoint(-1.5f, 0f),
				true
			},
			{
				new CurvePoint(-0.5f, 0.1f),
				true
			},
			{
				new CurvePoint(0.5f, 1f),
				true
			},
			{
				new CurvePoint(1f, 1.8f),
				true
			},
			{
				new CurvePoint(2f, 3f),
				true
			}
		};
	}
}
