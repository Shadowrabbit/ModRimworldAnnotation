using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020011A4 RID: 4516
	public class CompProperties_SpawnerItems : CompProperties
	{
		// Token: 0x170012D5 RID: 4821
		// (get) Token: 0x06006CC2 RID: 27842 RVA: 0x002483E9 File Offset: 0x002465E9
		public IEnumerable<ThingDef> MatchingItems
		{
			get
			{
				return from def in DefDatabase<ThingDef>.AllDefsListForReading
				where def.BaseMarketValue <= this.approxMarketValuePerDay && ((def.IsStuff && this.stuffCategories.Any((StuffCategoryDef c) => def.stuffProps.categories.Contains(c))) || this.categories.Any((ThingCategoryDef c) => def.IsWithinCategory(c)))
				select def;
			}
		}

		// Token: 0x06006CC3 RID: 27843 RVA: 0x00248401 File Offset: 0x00246601
		public CompProperties_SpawnerItems()
		{
			this.compClass = typeof(CompSpawnerItems);
		}

		// Token: 0x06006CC4 RID: 27844 RVA: 0x0024843A File Offset: 0x0024663A
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			if (this.MatchingItems.Count<ThingDef>() <= 0)
			{
				yield return "Could not find any item that would be spawnable by " + parentDef.defName + " (CompSpawnerItems)!";
			}
			yield break;
		}

		// Token: 0x04003C78 RID: 15480
		public float approxMarketValuePerDay;

		// Token: 0x04003C79 RID: 15481
		public int spawnInterval = 60000;

		// Token: 0x04003C7A RID: 15482
		public List<StuffCategoryDef> stuffCategories = new List<StuffCategoryDef>();

		// Token: 0x04003C7B RID: 15483
		public List<ThingCategoryDef> categories = new List<ThingCategoryDef>();
	}
}
