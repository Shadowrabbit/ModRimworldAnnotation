using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DEE RID: 7662
	public struct ApparelGraphicRecord
	{
		// Token: 0x0600A61B RID: 42523 RVA: 0x0006DD91 File Offset: 0x0006BF91
		public ApparelGraphicRecord(Graphic graphic, Apparel sourceApparel)
		{
			this.graphic = graphic;
			this.sourceApparel = sourceApparel;
		}

		// Token: 0x0400709A RID: 28826
		public Graphic graphic;

		// Token: 0x0400709B RID: 28827
		public Apparel sourceApparel;
	}
}
