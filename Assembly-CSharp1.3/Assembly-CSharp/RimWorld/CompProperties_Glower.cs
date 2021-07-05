using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A03 RID: 2563
	public class CompProperties_Glower : CompProperties
	{
		// Token: 0x06003EEF RID: 16111 RVA: 0x00157C70 File Offset: 0x00155E70
		public CompProperties_Glower()
		{
			this.compClass = typeof(CompGlower);
		}

		// Token: 0x040021DC RID: 8668
		public float overlightRadius;

		// Token: 0x040021DD RID: 8669
		public float glowRadius = 14f;

		// Token: 0x040021DE RID: 8670
		public ColorInt glowColor = new ColorInt(255, 255, 255, 0) * 1.45f;
	}
}
