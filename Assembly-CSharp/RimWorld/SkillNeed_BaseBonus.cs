using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F4D RID: 3917
	public class SkillNeed_BaseBonus : SkillNeed
	{
		// Token: 0x0600562A RID: 22058 RVA: 0x001C9F08 File Offset: 0x001C8108
		public override float ValueFor(Pawn pawn)
		{
			if (pawn.skills == null)
			{
				return 1f;
			}
			int level = pawn.skills.GetSkill(this.skill).Level;
			return this.ValueAtLevel(level);
		}

		// Token: 0x0600562B RID: 22059 RVA: 0x0003BC6B File Offset: 0x00039E6B
		private float ValueAtLevel(int level)
		{
			return this.baseValue + this.bonusPerLevel * (float)level;
		}

		// Token: 0x0600562C RID: 22060 RVA: 0x0003BC7D File Offset: 0x00039E7D
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			int num;
			for (int i = 1; i <= 20; i = num + 1)
			{
				if (this.ValueAtLevel(i) <= 0f)
				{
					yield return "SkillNeed yields factor < 0 at skill level " + i;
				}
				num = i;
			}
			yield break;
			yield break;
		}

		// Token: 0x04003797 RID: 14231
		private float baseValue = 0.5f;

		// Token: 0x04003798 RID: 14232
		private float bonusPerLevel = 0.05f;
	}
}
