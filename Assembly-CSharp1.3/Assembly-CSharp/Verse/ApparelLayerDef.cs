using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000081 RID: 129
	public class ApparelLayerDef : Def
	{
		// Token: 0x170000BA RID: 186
		// (get) Token: 0x060004C5 RID: 1221 RVA: 0x00019821 File Offset: 0x00017A21
		public bool IsUtilityLayer
		{
			get
			{
				return this == ApparelLayerDefOf.Belt;
			}
		}

		// Token: 0x040001AB RID: 427
		public int drawOrder;
	}
}
