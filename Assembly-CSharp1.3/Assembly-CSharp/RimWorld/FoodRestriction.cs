using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E2F RID: 3631
	public class FoodRestriction : IExposable, ILoadReferenceable
	{
		// Token: 0x060053FC RID: 21500 RVA: 0x001C6D1F File Offset: 0x001C4F1F
		public FoodRestriction(int id, string label)
		{
			this.id = id;
			this.label = label;
		}

		// Token: 0x060053FD RID: 21501 RVA: 0x001C6D40 File Offset: 0x001C4F40
		public FoodRestriction()
		{
		}

		// Token: 0x060053FE RID: 21502 RVA: 0x001C6D53 File Offset: 0x001C4F53
		public bool Allows(ThingDef def)
		{
			return this.filter.Allows(def);
		}

		// Token: 0x060053FF RID: 21503 RVA: 0x001C6D61 File Offset: 0x001C4F61
		public bool Allows(Thing thing)
		{
			return this.filter.Allows(thing);
		}

		// Token: 0x06005400 RID: 21504 RVA: 0x001C6D6F File Offset: 0x001C4F6F
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.id, "id", 0, false);
			Scribe_Values.Look<string>(ref this.label, "label", null, false);
			Scribe_Deep.Look<ThingFilter>(ref this.filter, "filter", Array.Empty<object>());
		}

		// Token: 0x06005401 RID: 21505 RVA: 0x001C6DAA File Offset: 0x001C4FAA
		public string GetUniqueLoadID()
		{
			return "FoodRestriction_" + this.label + this.id;
		}

		// Token: 0x0400316C RID: 12652
		public int id;

		// Token: 0x0400316D RID: 12653
		public string label;

		// Token: 0x0400316E RID: 12654
		public ThingFilter filter = new ThingFilter();
	}
}
