using System;

namespace Verse
{
	// Token: 0x02000108 RID: 264
	public class ThingDefStyle
	{
		// Token: 0x17000151 RID: 337
		// (get) Token: 0x060006FF RID: 1791 RVA: 0x000219B8 File Offset: 0x0001FBB8
		public ThingDef ThingDef
		{
			get
			{
				return this.thingDef;
			}
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x06000700 RID: 1792 RVA: 0x000219C0 File Offset: 0x0001FBC0
		public ThingStyleDef StyleDef
		{
			get
			{
				return this.styleDef;
			}
		}

		// Token: 0x04000647 RID: 1607
		private ThingDef thingDef;

		// Token: 0x04000648 RID: 1608
		private ThingStyleDef styleDef;
	}
}
