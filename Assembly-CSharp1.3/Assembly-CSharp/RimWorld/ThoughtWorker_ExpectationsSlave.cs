using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009A1 RID: 2465
	public class ThoughtWorker_ExpectationsSlave : ThoughtWorker
	{
		// Token: 0x06003DCC RID: 15820 RVA: 0x00153640 File Offset: 0x00151840
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!p.IsSlave)
			{
				return ThoughtState.Inactive;
			}
			ExpectationDef expectationDef = ExpectationsUtility.CurrentExpectationFor(p);
			if (expectationDef == null)
			{
				return ThoughtState.Inactive;
			}
			return ThoughtState.ActiveAtStage(this.GetThoughtStageForExpectation(expectationDef));
		}

		// Token: 0x06003DCD RID: 15821 RVA: 0x00153677 File Offset: 0x00151877
		private int GetThoughtStageForExpectation(ExpectationDef expectation)
		{
			if (expectation == ExpectationDefOf.ExtremelyLow)
			{
				return 0;
			}
			if (expectation == ExpectationDefOf.VeryLow)
			{
				return 1;
			}
			if (expectation == ExpectationDefOf.Low)
			{
				return 2;
			}
			return 3;
		}
	}
}
