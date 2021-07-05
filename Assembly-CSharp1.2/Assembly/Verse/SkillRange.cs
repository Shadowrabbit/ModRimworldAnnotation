using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000162 RID: 354
	public class SkillRange
	{
		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x060008EC RID: 2284 RVA: 0x0000D08C File Offset: 0x0000B28C
		public SkillDef Skill
		{
			get
			{
				return this.skill;
			}
		}

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x060008ED RID: 2285 RVA: 0x0000D094 File Offset: 0x0000B294
		public IntRange Range
		{
			get
			{
				return this.range;
			}
		}

		// Token: 0x04000791 RID: 1937
		private SkillDef skill;

		// Token: 0x04000792 RID: 1938
		private IntRange range = IntRange.one;
	}
}
