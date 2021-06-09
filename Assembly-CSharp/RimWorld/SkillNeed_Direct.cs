using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F4F RID: 3919
	public class SkillNeed_Direct : SkillNeed
	{
		// Token: 0x06005638 RID: 22072 RVA: 0x001CA0DC File Offset: 0x001C82DC
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

		// Token: 0x0400379F RID: 14239
		public List<float> valuesPerLevel = new List<float>();
	}
}
