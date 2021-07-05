using System;

namespace Verse
{
	// Token: 0x02000109 RID: 265
	public class ThingStyleCategoryWithPriority : IExposable
	{
		// Token: 0x06000702 RID: 1794 RVA: 0x000033AC File Offset: 0x000015AC
		public ThingStyleCategoryWithPriority()
		{
		}

		// Token: 0x06000703 RID: 1795 RVA: 0x000219C8 File Offset: 0x0001FBC8
		public ThingStyleCategoryWithPriority(StyleCategoryDef category, float priority)
		{
			this.category = category;
			this.priority = priority;
		}

		// Token: 0x06000704 RID: 1796 RVA: 0x000219DE File Offset: 0x0001FBDE
		public void ExposeData()
		{
			Scribe_Defs.Look<StyleCategoryDef>(ref this.category, "category");
			Scribe_Values.Look<float>(ref this.priority, "priority", 0f, false);
		}

		// Token: 0x04000649 RID: 1609
		public StyleCategoryDef category;

		// Token: 0x0400064A RID: 1610
		public float priority;
	}
}
