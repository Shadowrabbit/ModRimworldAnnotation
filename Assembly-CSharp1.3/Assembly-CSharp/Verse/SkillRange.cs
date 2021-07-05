using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020000EC RID: 236
	public class SkillRange
	{
		// Token: 0x17000121 RID: 289
		// (get) Token: 0x06000669 RID: 1641 RVA: 0x0001F76F File Offset: 0x0001D96F
		public SkillDef Skill
		{
			get
			{
				return this.skill;
			}
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x0600066A RID: 1642 RVA: 0x0001F777 File Offset: 0x0001D977
		public IntRange Range
		{
			get
			{
				return this.range;
			}
		}

		// Token: 0x0400059C RID: 1436
		private SkillDef skill;

		// Token: 0x0400059D RID: 1437
		private IntRange range = IntRange.one;
	}
}
