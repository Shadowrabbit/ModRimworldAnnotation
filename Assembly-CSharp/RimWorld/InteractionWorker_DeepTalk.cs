using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200147F RID: 5247
	public class InteractionWorker_DeepTalk : InteractionWorker
	{
		// Token: 0x06007140 RID: 28992 RVA: 0x0004C3CF File Offset: 0x0004A5CF
		public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
		{
			return 0.075f * this.CompatibilityFactorCurve.Evaluate(initiator.relations.CompatibilityWith(recipient));
		}

		// Token: 0x04004ABC RID: 19132
		private const float BaseSelectionWeight = 0.075f;

		// Token: 0x04004ABD RID: 19133
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
