using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F5B RID: 3931
	public class RitualOutcomeComp_RolePresentNotSubstituted : RitualOutcomeComp_QualitySingleOffset
	{
		// Token: 0x17001026 RID: 4134
		// (get) Token: 0x06005D45 RID: 23877 RVA: 0x001FF79A File Offset: 0x001FD99A
		protected override string LabelForDesc
		{
			get
			{
				return this.label;
			}
		}

		// Token: 0x06005D46 RID: 23878 RVA: 0x001FFD4E File Offset: 0x001FDF4E
		public override bool Applies(LordJob_Ritual ritual)
		{
			return ritual.RoleFilled(this.roleId) && !ritual.assignments.RoleSubstituted(this.roleId);
		}

		// Token: 0x06005D47 RID: 23879 RVA: 0x001FFD74 File Offset: 0x001FDF74
		public override ExpectedOutcomeDesc GetExpectedOutcomeDesc(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments)
		{
			bool flag = assignments.AnyPawnAssigned(this.roleId) && !assignments.RoleSubstituted(this.roleId);
			return new ExpectedOutcomeDesc
			{
				label = this.LabelForDesc.CapitalizeFirst(),
				present = flag,
				effect = this.ExpectedOffsetDesc(flag, -1f),
				quality = (flag ? this.qualityOffset : 0f),
				positive = flag,
				priority = 3f
			};
		}

		// Token: 0x040035FA RID: 13818
		public string roleId;
	}
}
