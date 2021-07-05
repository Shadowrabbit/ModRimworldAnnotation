using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000BA RID: 186
	public class StuffProperties
	{
		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x060005A4 RID: 1444 RVA: 0x0001D054 File Offset: 0x0001B254
		public ThingDef SourceNaturalRock
		{
			get
			{
				if (!this.sourceNaturalRockCached)
				{
					List<ThingDef> allDefsListForReading = DefDatabase<ThingDef>.AllDefsListForReading;
					List<RecipeDef> allDefsListForReading2 = DefDatabase<RecipeDef>.AllDefsListForReading;
					for (int i = 0; i < allDefsListForReading.Count; i++)
					{
						if (allDefsListForReading[i].category == ThingCategory.Building && allDefsListForReading[i].building.isNaturalRock && allDefsListForReading[i].building.mineableThing != null && !allDefsListForReading[i].IsSmoothed)
						{
							if (allDefsListForReading[i].building.mineableThing == this.parent)
							{
								this.sourceNaturalRockCachedValue = allDefsListForReading[i];
								break;
							}
							for (int j = 0; j < allDefsListForReading2.Count; j++)
							{
								if (allDefsListForReading2[j].IsIngredient(allDefsListForReading[i].building.mineableThing))
								{
									bool flag = false;
									for (int k = 0; k < allDefsListForReading2[j].products.Count; k++)
									{
										if (allDefsListForReading2[j].products[k].thingDef == this.parent)
										{
											flag = true;
											break;
										}
									}
									if (flag)
									{
										this.sourceNaturalRockCachedValue = allDefsListForReading[i];
										break;
									}
								}
							}
						}
					}
					this.sourceNaturalRockCached = true;
				}
				return this.sourceNaturalRockCachedValue;
			}
		}

		// Token: 0x060005A5 RID: 1445 RVA: 0x0001D1A4 File Offset: 0x0001B3A4
		public bool CanMake(BuildableDef t)
		{
			if (!t.MadeFromStuff)
			{
				return false;
			}
			for (int i = 0; i < t.stuffCategories.Count; i++)
			{
				for (int j = 0; j < this.categories.Count; j++)
				{
					if (t.stuffCategories[i] == this.categories[j])
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060005A6 RID: 1446 RVA: 0x0001D204 File Offset: 0x0001B404
		public void ResolveReferencesSpecial()
		{
			if (this.appearance == null)
			{
				this.appearance = StuffAppearanceDefOf.Smooth;
			}
		}

		// Token: 0x0400038E RID: 910
		[Unsaved(false)]
		public ThingDef parent;

		// Token: 0x0400038F RID: 911
		public string stuffAdjective;

		// Token: 0x04000390 RID: 912
		public float commonality = 1f;

		// Token: 0x04000391 RID: 913
		public List<StuffCategoryDef> categories = new List<StuffCategoryDef>();

		// Token: 0x04000392 RID: 914
		public List<StatModifier> statOffsets;

		// Token: 0x04000393 RID: 915
		public List<StatModifier> statFactors;

		// Token: 0x04000394 RID: 916
		public Color color = new Color(0.8f, 0.8f, 0.8f);

		// Token: 0x04000395 RID: 917
		public EffecterDef constructEffect;

		// Token: 0x04000396 RID: 918
		public StuffAppearanceDef appearance;

		// Token: 0x04000397 RID: 919
		public bool allowColorGenerators;

		// Token: 0x04000398 RID: 920
		public bool canSuggestUseDefaultStuff;

		// Token: 0x04000399 RID: 921
		public SoundDef soundImpactStuff;

		// Token: 0x0400039A RID: 922
		public SoundDef soundMeleeHitSharp;

		// Token: 0x0400039B RID: 923
		public SoundDef soundMeleeHitBlunt;

		// Token: 0x0400039C RID: 924
		[Unsaved(false)]
		private bool sourceNaturalRockCached;

		// Token: 0x0400039D RID: 925
		[Unsaved(false)]
		private ThingDef sourceNaturalRockCachedValue;
	}
}
