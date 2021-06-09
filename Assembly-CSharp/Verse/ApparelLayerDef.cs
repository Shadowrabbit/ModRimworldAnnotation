using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020000D8 RID: 216
	public class ApparelLayerDef : Def
	{
		// Token: 0x17000116 RID: 278
		// (get) Token: 0x06000668 RID: 1640 RVA: 0x0000B56F File Offset: 0x0000976F
		public bool IsUtilityLayer
		{
			get
			{
				return this == ApparelLayerDefOf.Belt;
			}
		}

		// Token: 0x04000346 RID: 838
		public int drawOrder;
	}
}
