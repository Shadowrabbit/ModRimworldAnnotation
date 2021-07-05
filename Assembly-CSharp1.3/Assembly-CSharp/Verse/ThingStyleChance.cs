using System;

namespace Verse
{
	// Token: 0x02000117 RID: 279
	public class ThingStyleChance
	{
		// Token: 0x170001AE RID: 430
		// (get) Token: 0x06000799 RID: 1945 RVA: 0x000237DD File Offset: 0x000219DD
		public ThingStyleDef StyleDef
		{
			get
			{
				return this.styleDef;
			}
		}

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x0600079A RID: 1946 RVA: 0x000237E5 File Offset: 0x000219E5
		public float Chance
		{
			get
			{
				return this.chance;
			}
		}

		// Token: 0x0400073B RID: 1851
		private ThingStyleDef styleDef;

		// Token: 0x0400073C RID: 1852
		private float chance = 1f;
	}
}
