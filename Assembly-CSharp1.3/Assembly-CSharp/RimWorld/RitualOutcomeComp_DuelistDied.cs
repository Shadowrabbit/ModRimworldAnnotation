using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F59 RID: 3929
	public class RitualOutcomeComp_DuelistDied : RitualOutcomeComp_QualitySingleOffset
	{
		// Token: 0x17001022 RID: 4130
		// (get) Token: 0x06005D3B RID: 23867 RVA: 0x001FF79A File Offset: 0x001FD99A
		protected override string LabelForDesc
		{
			get
			{
				return this.label;
			}
		}

		// Token: 0x17001023 RID: 4131
		// (get) Token: 0x06005D3C RID: 23868 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool DataRequired
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06005D3D RID: 23869 RVA: 0x001FFC3C File Offset: 0x001FDE3C
		public override bool Applies(LordJob_Ritual ritual)
		{
			LordJob_Ritual_Duel lordJob_Ritual_Duel;
			if ((lordJob_Ritual_Duel = (ritual as LordJob_Ritual_Duel)) != null)
			{
				return lordJob_Ritual_Duel.duelists.Any((Pawn d) => d.Dead);
			}
			return false;
		}

		// Token: 0x06005D3E RID: 23870 RVA: 0x001FFC80 File Offset: 0x001FDE80
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
