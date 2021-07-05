using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020002B8 RID: 696
	public class HediffComp_SkillDecay : HediffComp
	{
		// Token: 0x170003AE RID: 942
		// (get) Token: 0x060012D3 RID: 4819 RVA: 0x0006BABD File Offset: 0x00069CBD
		public HediffCompProperties_SkillDecay Props
		{
			get
			{
				return (HediffCompProperties_SkillDecay)this.props;
			}
		}

		// Token: 0x060012D4 RID: 4820 RVA: 0x0006BACC File Offset: 0x00069CCC
		public override void CompPostTick(ref float severityAdjustment)
		{
			Pawn_SkillTracker skills = base.Pawn.skills;
			if (skills == null)
			{
				return;
			}
			for (int i = 0; i < skills.skills.Count; i++)
			{
				SkillRecord skillRecord = skills.skills[i];
				float num = this.parent.Severity * this.Props.decayPerDayPercentageLevelCurve.Evaluate((float)skillRecord.Level);
				float num2 = skillRecord.XpRequiredForLevelUp * num / 60000f;
				skillRecord.Learn(-num2, false);
			}
		}
	}
}
