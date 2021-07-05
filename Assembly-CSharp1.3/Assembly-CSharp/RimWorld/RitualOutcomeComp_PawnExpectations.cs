using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F6A RID: 3946
	public class RitualOutcomeComp_PawnExpectations : RitualOutcomeComp_QualitySingleOffset
	{
		// Token: 0x06005D8C RID: 23948 RVA: 0x00200FD3 File Offset: 0x001FF1D3
		public override float QualityOffset(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
		{
			return this.Count(ritual, data);
		}

		// Token: 0x06005D8D RID: 23949 RVA: 0x00201248 File Offset: 0x001FF448
		protected PawnExpectationsQualityOffset ExpectationOffset(Pawn pawn)
		{
			ExpectationDef expectations = ExpectationsUtility.CurrentExpectationFor(pawn);
			return this.offsetPerExpectation.FirstOrDefault((PawnExpectationsQualityOffset e) => e.expectation == expectations);
		}

		// Token: 0x06005D8E RID: 23950 RVA: 0x00201280 File Offset: 0x001FF480
		public override float Count(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
		{
			Pawn pawn = ritual.assignments.FirstAssignedPawn(this.roleId);
			if (pawn == null)
			{
				return 0f;
			}
			PawnExpectationsQualityOffset pawnExpectationsQualityOffset = this.ExpectationOffset(pawn);
			if (pawnExpectationsQualityOffset == null)
			{
				return 0f;
			}
			return pawnExpectationsQualityOffset.offset;
		}

		// Token: 0x06005D8F RID: 23951 RVA: 0x002012C0 File Offset: 0x001FF4C0
		public override string GetDesc(LordJob_Ritual ritual = null, RitualOutcomeComp_Data data = null)
		{
			if (ritual == null)
			{
				return this.labelAbstract;
			}
			Pawn pawn = (ritual != null) ? ritual.PawnWithRole(this.roleId) : null;
			if (pawn == null)
			{
				return null;
			}
			PawnExpectationsQualityOffset pawnExpectationsQualityOffset = this.ExpectationOffset(pawn);
			string str = (pawnExpectationsQualityOffset.offset < 0f) ? "" : "+";
			return this.LabelForDesc.Formatted(pawn.Named("PAWN")) + ": " + "OutcomeBonusDesc_QualitySingleOffset".Translate(str + pawnExpectationsQualityOffset.offset.ToStringPercent()) + ".";
		}

		// Token: 0x06005D90 RID: 23952 RVA: 0x00201368 File Offset: 0x001FF568
		public override ExpectedOutcomeDesc GetExpectedOutcomeDesc(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments)
		{
			Pawn pawn = assignments.FirstAssignedPawn(this.roleId);
			if (pawn == null)
			{
				return null;
			}
			PawnExpectationsQualityOffset pawnExpectationsQualityOffset = this.ExpectationOffset(pawn);
			if (pawnExpectationsQualityOffset != null)
			{
				return new ExpectedOutcomeDesc
				{
					label = "RitualPredictedOutcomeDescPawnExpectations".Translate(pawn.Named("PAWN")),
					count = pawnExpectationsQualityOffset.expectation.LabelCap,
					effect = ((Math.Abs(pawnExpectationsQualityOffset.offset) > float.Epsilon) ? "OutcomeBonusDesc_QualitySingleOffset".Translate(pawnExpectationsQualityOffset.offset.ToStringWithSign("0.#%")).Resolve() : " - "),
					positive = (pawnExpectationsQualityOffset.offset >= 0f),
					quality = pawnExpectationsQualityOffset.offset,
					priority = 0f
				};
			}
			return null;
		}

		// Token: 0x0400360D RID: 13837
		[NoTranslate]
		public string roleId;

		// Token: 0x0400360E RID: 13838
		public List<PawnExpectationsQualityOffset> offsetPerExpectation = new List<PawnExpectationsQualityOffset>();
	}
}
