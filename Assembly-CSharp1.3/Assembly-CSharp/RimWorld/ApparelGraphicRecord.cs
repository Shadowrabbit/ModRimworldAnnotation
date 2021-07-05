using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001568 RID: 5480
	public struct ApparelGraphicRecord
	{
		// Token: 0x060081BD RID: 33213 RVA: 0x002DDC16 File Offset: 0x002DBE16
		public ApparelGraphicRecord(Graphic graphic, Apparel sourceApparel)
		{
			this.graphic = graphic;
			this.sourceApparel = sourceApparel;
		}

		// Token: 0x040050BB RID: 20667
		public Graphic graphic;

		// Token: 0x040050BC RID: 20668
		public Apparel sourceApparel;
	}
}
