using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C9A RID: 7322
	[DefOf]
	public static class ApparelLayerDefOf
	{
		// Token: 0x06009F9D RID: 40861 RVA: 0x0006A6DA File Offset: 0x000688DA
		static ApparelLayerDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ApparelLayerDefOf));
		}

		// Token: 0x04006C39 RID: 27705
		public static ApparelLayerDef OnSkin;

		// Token: 0x04006C3A RID: 27706
		public static ApparelLayerDef Shell;

		// Token: 0x04006C3B RID: 27707
		public static ApparelLayerDef Middle;

		// Token: 0x04006C3C RID: 27708
		public static ApparelLayerDef Belt;

		// Token: 0x04006C3D RID: 27709
		public static ApparelLayerDef Overhead;
	}
}
