using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014D1 RID: 5329
	public class FoodRestriction : IExposable, ILoadReferenceable
	{
		// Token: 0x060072C5 RID: 29381 RVA: 0x0004D2B8 File Offset: 0x0004B4B8
		public FoodRestriction(int id, string label)
		{
			this.id = id;
			this.label = label;
		}

		// Token: 0x060072C6 RID: 29382 RVA: 0x0004D2D9 File Offset: 0x0004B4D9
		public FoodRestriction()
		{
		}

		// Token: 0x060072C7 RID: 29383 RVA: 0x0004D2EC File Offset: 0x0004B4EC
		public bool Allows(ThingDef def)
		{
			return this.filter.Allows(def);
		}

		// Token: 0x060072C8 RID: 29384 RVA: 0x0004D2FA File Offset: 0x0004B4FA
		public bool Allows(Thing thing)
		{
			return this.filter.Allows(thing);
		}

		// Token: 0x060072C9 RID: 29385 RVA: 0x0004D308 File Offset: 0x0004B508
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.id, "id", 0, false);
			Scribe_Values.Look<string>(ref this.label, "label", null, false);
			Scribe_Deep.Look<ThingFilter>(ref this.filter, "filter", Array.Empty<object>());
		}

		// Token: 0x060072CA RID: 29386 RVA: 0x0004D343 File Offset: 0x0004B543
		public string GetUniqueLoadID()
		{
			return "FoodRestriction_" + this.label + this.id;
		}

		// Token: 0x04004B90 RID: 19344
		public int id;

		// Token: 0x04004B91 RID: 19345
		public string label;

		// Token: 0x04004B92 RID: 19346
		public ThingFilter filter = new ThingFilter();
	}
}
