using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F5A RID: 3930
	public class RitualOutcomeComp_WeaponUsed : RitualOutcomeComp_QualitySingleOffset
	{
		// Token: 0x17001024 RID: 4132
		// (get) Token: 0x06005D40 RID: 23872 RVA: 0x001FF79A File Offset: 0x001FD99A
		protected override string LabelForDesc
		{
			get
			{
				return this.label;
			}
		}

		// Token: 0x17001025 RID: 4133
		// (get) Token: 0x06005D41 RID: 23873 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool DataRequired
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06005D42 RID: 23874 RVA: 0x001FFCD8 File Offset: 0x001FDED8
		public override bool Applies(LordJob_Ritual ritual)
		{
			LordJob_Ritual_Duel lordJob_Ritual_Duel;
			return (lordJob_Ritual_Duel = (ritual as LordJob_Ritual_Duel)) != null && lordJob_Ritual_Duel.usedWeapon;
		}

		// Token: 0x06005D43 RID: 23875 RVA: 0x001FFCF8 File Offset: 0x001FDEF8
		public override ExpectedOutcomeDesc GetExpectedOutcomeDesc(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments)
		{
			return new ExpectedOutcomeDesc
			{
				label = this.LabelForDesc.CapitalizeFirst(),
				present = false,
				uncertainOutcome = true,
				effect = this.ExpectedOffsetDesc(true, -1f),
				quality = this.qualityOffset,
				positive = true
			};
		}
	}
}
