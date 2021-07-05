using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F68 RID: 3944
	public class RitualOutcomeComp_PawnStatScaled : RitualOutcomeComp_QualitySingleOffset
	{
		// Token: 0x06005D83 RID: 23939 RVA: 0x00200FD3 File Offset: 0x001FF1D3
		public override float QualityOffset(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
		{
			return this.Count(ritual, data);
		}

		// Token: 0x06005D84 RID: 23940 RVA: 0x00200FDD File Offset: 0x001FF1DD
		protected float StatValue(Pawn pawn)
		{
			if (this.curve == null)
			{
				return pawn.GetStatValue(this.statDef, true);
			}
			return this.curve.Evaluate(pawn.GetStatValue(this.statDef, true));
		}

		// Token: 0x06005D85 RID: 23941 RVA: 0x00201010 File Offset: 0x001FF210
		public override float Count(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
		{
			Pawn pawn = ritual.PawnWithRole(this.roleId);
			if (pawn == null)
			{
				return 0f;
			}
			if (this.statDef.Worker.IsDisabledFor(pawn))
			{
				return 0f;
			}
			return this.StatValue(pawn) * this.scaledBy;
		}

		// Token: 0x06005D86 RID: 23942 RVA: 0x0020105C File Offset: 0x001FF25C
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
			float num = 0f;
			if (!this.statDef.Worker.IsDisabledFor(pawn))
			{
				num = this.StatValue(pawn);
			}
			float num2 = num * this.scaledBy;
			string str = (num2 < 0f) ? "" : "+";
			return this.LabelForDesc.Formatted(pawn.Named("PAWN")) + ": " + "OutcomeBonusDesc_QualitySingleOffset".Translate(str + num2.ToStringPercent()) + ".";
		}

		// Token: 0x06005D87 RID: 23943 RVA: 0x0020111C File Offset: 0x001FF31C
		public override ExpectedOutcomeDesc GetExpectedOutcomeDesc(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments)
		{
			Pawn pawn = assignments.FirstAssignedPawn(this.roleId);
			if (pawn == null)
			{
				return null;
			}
			float f = 0f;
			float num = 0f;
			if (!this.statDef.Worker.IsDisabledFor(pawn))
			{
				f = pawn.GetStatValue(this.statDef, true);
				num = this.StatValue(pawn) * this.scaledBy;
			}
			return new ExpectedOutcomeDesc
			{
				label = this.label.Formatted(pawn.Named("PAWN")),
				count = f.ToStringPercent(),
				effect = ((Math.Abs(num) > float.Epsilon) ? "OutcomeBonusDesc_QualitySingleOffset".Translate(num.ToStringWithSign("0.#%")).Resolve() : " - "),
				positive = (num >= 0f),
				quality = num,
				priority = 0f
			};
		}

		// Token: 0x06005D88 RID: 23944 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool Applies(LordJob_Ritual ritual)
		{
			return true;
		}

		// Token: 0x04003608 RID: 13832
		public string roleId;

		// Token: 0x04003609 RID: 13833
		public StatDef statDef;

		// Token: 0x0400360A RID: 13834
		public float scaledBy = 1f;
	}
}
