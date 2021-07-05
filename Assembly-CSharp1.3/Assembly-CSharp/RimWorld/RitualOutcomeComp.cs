using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F55 RID: 3925
	public abstract class RitualOutcomeComp
	{
		// Token: 0x1700101E RID: 4126
		// (get) Token: 0x06005D1D RID: 23837 RVA: 0x000126F5 File Offset: 0x000108F5
		public virtual bool DataRequired
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005D1E RID: 23838 RVA: 0x000682C5 File Offset: 0x000664C5
		public virtual float QualityOffset(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
		{
			return 0f;
		}

		// Token: 0x06005D1F RID: 23839 RVA: 0x00002688 File Offset: 0x00000888
		public virtual RitualOutcomeComp_Data MakeData()
		{
			return null;
		}

		// Token: 0x06005D20 RID: 23840
		public abstract bool Applies(LordJob_Ritual ritual);

		// Token: 0x06005D21 RID: 23841 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Tick(LordJob_Ritual ritual, RitualOutcomeComp_Data data, float progressAmount)
		{
		}

		// Token: 0x06005D22 RID: 23842 RVA: 0x001FF79A File Offset: 0x001FD99A
		public virtual string GetDesc(LordJob_Ritual ritual = null, RitualOutcomeComp_Data data = null)
		{
			return this.label;
		}

		// Token: 0x06005D23 RID: 23843 RVA: 0x001FF79A File Offset: 0x001FD99A
		public virtual string GetBonusDescShort()
		{
			return this.label;
		}

		// Token: 0x06005D24 RID: 23844 RVA: 0x00002688 File Offset: 0x00000888
		public virtual ExpectedOutcomeDesc GetExpectedOutcomeDesc(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments)
		{
			return null;
		}

		// Token: 0x06005D25 RID: 23845 RVA: 0x00014F75 File Offset: 0x00013175
		protected virtual string ExpectedOffsetDesc(bool positive, float quality = 0f)
		{
			return "";
		}

		// Token: 0x040035F4 RID: 13812
		[MustTranslate]
		protected string label;

		// Token: 0x040035F5 RID: 13813
		[MustTranslate]
		protected string labelAbstract;

		// Token: 0x040035F6 RID: 13814
		protected float qualityOffset;
	}
}
