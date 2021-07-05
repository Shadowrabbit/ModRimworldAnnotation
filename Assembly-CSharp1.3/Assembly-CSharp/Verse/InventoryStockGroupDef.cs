using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x020000D4 RID: 212
	public class InventoryStockGroupDef : Def
	{
		// Token: 0x17000107 RID: 263
		// (get) Token: 0x06000612 RID: 1554 RVA: 0x0001E986 File Offset: 0x0001CB86
		public ThingDef DefaultThingDef
		{
			get
			{
				return this.defaultThingDef ?? this.thingDefs.First<ThingDef>();
			}
		}

		// Token: 0x06000613 RID: 1555 RVA: 0x0001E99D File Offset: 0x0001CB9D
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.defaultThingDef != null && !this.thingDefs.Contains(this.defaultThingDef))
			{
				yield return "Default thing def " + this.defaultThingDef.defName + " should be in thingDefs but not found.";
			}
			if (this.min > this.max)
			{
				yield return "Min should be less than max.";
			}
			if (this.min < 0 || this.max < 0)
			{
				yield return "Min/max should be greater than zero.";
			}
			if (this.thingDefs.NullOrEmpty<ThingDef>())
			{
				yield return "thingDefs cannot be null or empty.";
			}
			yield break;
			yield break;
		}

		// Token: 0x04000497 RID: 1175
		public List<ThingDef> thingDefs;

		// Token: 0x04000498 RID: 1176
		public int min;

		// Token: 0x04000499 RID: 1177
		public int max = 3;

		// Token: 0x0400049A RID: 1178
		public ThingDef defaultThingDef;
	}
}
