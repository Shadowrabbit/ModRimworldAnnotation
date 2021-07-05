using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F0E RID: 3854
	public class CompProperties_Glower : CompProperties
	{
		// Token: 0x06005542 RID: 21826 RVA: 0x001C7E44 File Offset: 0x001C6044
		public CompProperties_Glower()
		{
			this.compClass = typeof(CompGlower);
		}

		// Token: 0x0400365D RID: 13917
		public float overlightRadius;

		// Token: 0x0400365E RID: 13918
		public float glowRadius = 14f;

		// Token: 0x0400365F RID: 13919
		public ColorInt glowColor = new ColorInt(255, 255, 255, 0) * 1.45f;
	}
}
