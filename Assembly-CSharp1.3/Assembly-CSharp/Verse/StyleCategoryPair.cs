using System;

namespace Verse
{
	// Token: 0x0200010A RID: 266
	public class StyleCategoryPair : IExposable
	{
		// Token: 0x06000705 RID: 1797 RVA: 0x00021A06 File Offset: 0x0001FC06
		public void ExposeData()
		{
			Scribe_Defs.Look<StyleCategoryDef>(ref this.category, "category");
			Scribe_Defs.Look<ThingStyleDef>(ref this.styleDef, "styleDef");
		}

		// Token: 0x0400064B RID: 1611
		public StyleCategoryDef category;

		// Token: 0x0400064C RID: 1612
		public ThingStyleDef styleDef;
	}
}
