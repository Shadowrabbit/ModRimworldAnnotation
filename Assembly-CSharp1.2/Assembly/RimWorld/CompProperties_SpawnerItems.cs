using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001856 RID: 6230
	public class CompProperties_SpawnerItems : CompProperties
	{
		// Token: 0x170015B2 RID: 5554
		// (get) Token: 0x06008A37 RID: 35383 RVA: 0x0005CC09 File Offset: 0x0005AE09
		public IEnumerable<ThingDef> MatchingItems
		{
			get
			{
				return from def in DefDatabase<ThingDef>.AllDefsListForReading
				where def.BaseMarketValue <= this.approxMarketValuePerDay && ((def.IsStuff && this.stuffCategories.Any((StuffCategoryDef c) => def.stuffProps.categories.Contains(c))) || this.categories.Any((ThingCategoryDef c) => def.IsWithinCategory(c)))
				select def;
			}
		}

		// Token: 0x06008A38 RID: 35384 RVA: 0x0005CC21 File Offset: 0x0005AE21
		public CompProperties_SpawnerItems()
		{
			this.compClass = typeof(CompSpawnerItems);
		}

		// Token: 0x06008A39 RID: 35385 RVA: 0x0005CC5A File Offset: 0x0005AE5A
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			if (this.MatchingItems.Count<ThingDef>() <= 0)
			{
				yield return "Could not find any item that would be spawnable by " + parentDef.defName + " (CompSpawnerItems)!";
			}
			yield break;
		}

		// Token: 0x040058A5 RID: 22693
		public float approxMarketValuePerDay;

		// Token: 0x040058A6 RID: 22694
		public int spawnInterval = 60000;

		// Token: 0x040058A7 RID: 22695
		public List<StuffCategoryDef> stuffCategories = new List<StuffCategoryDef>();

		// Token: 0x040058A8 RID: 22696
		public List<ThingCategoryDef> categories = new List<ThingCategoryDef>();
	}
}
