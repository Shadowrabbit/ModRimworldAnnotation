using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A33 RID: 2611
	public class SkillNeed_Direct : SkillNeed
	{
		// Token: 0x06003F4F RID: 16207 RVA: 0x00158A8C File Offset: 0x00156C8C
		public override float ValueFor(Pawn pawn)
		{
			if (pawn.skills == null)
			{
				return 1f;
			}
			int level = pawn.skills.GetSkill(this.skill).Level;
			if (this.valuesPerLevel.Count > level)
			{
				return this.valuesPerLevel[level];
			}
			if (this.valuesPerLevel.Count > 0)
			{
				return this.valuesPerLevel[this.valuesPerLevel.Count - 1];
			}
			return 1f;
		}

		// Token: 0x040022D6 RID: 8918
		public List<float> valuesPerLevel = new List<float>();
	}
}
