using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020003F3 RID: 1011
	public class HediffComp_SkillDecay : HediffComp
	{
		// Token: 0x17000485 RID: 1157
		// (get) Token: 0x06001898 RID: 6296 RVA: 0x00017585 File Offset: 0x00015785
		public HediffCompProperties_SkillDecay Props
		{
			get
			{
				return (HediffCompProperties_SkillDecay)this.props;
			}
		}

		// Token: 0x06001899 RID: 6297 RVA: 0x000DF9E8 File Offset: 0x000DDBE8
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
