using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E39 RID: 3641
	public class InventoryStockEntry : IExposable
	{
		// Token: 0x0600544F RID: 21583 RVA: 0x001C967D File Offset: 0x001C787D
		public void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
			Scribe_Values.Look<int>(ref this.count, "count", 0, false);
		}

		// Token: 0x040031B1 RID: 12721
		public ThingDef thingDef;

		// Token: 0x040031B2 RID: 12722
		public int count;
	}
}
