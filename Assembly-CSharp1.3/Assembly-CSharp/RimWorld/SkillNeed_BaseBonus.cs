using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A32 RID: 2610
	public class SkillNeed_BaseBonus : SkillNeed
	{
		// Token: 0x06003F4A RID: 16202 RVA: 0x00158A08 File Offset: 0x00156C08
		public override float ValueFor(Pawn pawn)
		{
			if (pawn.skills == null)
			{
				return 1f;
			}
			int level = pawn.skills.GetSkill(this.skill).Level;
			return this.ValueAtLevel(level);
		}

		// Token: 0x06003F4B RID: 16203 RVA: 0x00158A41 File Offset: 0x00156C41
		private float ValueAtLevel(int level)
		{
			return this.baseValue + this.bonusPerLevel * (float)level;
		}

		// Token: 0x06003F4C RID: 16204 RVA: 0x00158A53 File Offset: 0x00156C53
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

		// Token: 0x040022D4 RID: 8916
		private float baseValue = 0.5f;

		// Token: 0x040022D5 RID: 8917
		private float bonusPerLevel = 0.05f;
	}
}
