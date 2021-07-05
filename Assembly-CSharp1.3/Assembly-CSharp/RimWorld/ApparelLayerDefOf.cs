using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200145A RID: 5210
	[DefOf]
	public static class ApparelLayerDefOf
	{
		// Token: 0x06007D4D RID: 32077 RVA: 0x002C4B39 File Offset: 0x002C2D39
		static ApparelLayerDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ApparelLayerDefOf));
		}

		// Token: 0x04004D2C RID: 19756
		public static ApparelLayerDef OnSkin;

		// Token: 0x04004D2D RID: 19757
		public static ApparelLayerDef Shell;

		// Token: 0x04004D2E RID: 19758
		public static ApparelLayerDef Middle;

		// Token: 0x04004D2F RID: 19759
		public static ApparelLayerDef Belt;

		// Token: 0x04004D30 RID: 19760
		public static ApparelLayerDef Overhead;
	}
}
