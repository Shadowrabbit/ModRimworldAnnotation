using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000122 RID: 290
	public class StuffProperties
	{
		// Token: 0x1700016C RID: 364
		// (get) Token: 0x060007D9 RID: 2009 RVA: 0x00094060 File Offset: 0x00092260
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

		// Token: 0x060007DA RID: 2010 RVA: 0x000941B0 File Offset: 0x000923B0
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

		// Token: 0x060007DB RID: 2011 RVA: 0x0000C3D7 File Offset: 0x0000A5D7
		public void ResolveReferencesSpecial()
		{
			if (this.appearance == null)
			{
				this.appearance = StuffAppearanceDefOf.Smooth;
			}
		}

		// Token: 0x04000572 RID: 1394
		[Unsaved(false)]
		public ThingDef parent;

		// Token: 0x04000573 RID: 1395
		public string stuffAdjective;

		// Token: 0x04000574 RID: 1396
		public float commonality = 1f;

		// Token: 0x04000575 RID: 1397
		public List<StuffCategoryDef> categories = new List<StuffCategoryDef>();

		// Token: 0x04000576 RID: 1398
		public List<StatModifier> statOffsets;

		// Token: 0x04000577 RID: 1399
		public List<StatModifier> statFactors;

		// Token: 0x04000578 RID: 1400
		public Color color = new Color(0.8f, 0.8f, 0.8f);

		// Token: 0x04000579 RID: 1401
		public EffecterDef constructEffect;

		// Token: 0x0400057A RID: 1402
		public StuffAppearanceDef appearance;

		// Token: 0x0400057B RID: 1403
		public bool allowColorGenerators;

		// Token: 0x0400057C RID: 1404
		public SoundDef soundImpactStuff;

		// Token: 0x0400057D RID: 1405
		public SoundDef soundMeleeHitSharp;

		// Token: 0x0400057E RID: 1406
		public SoundDef soundMeleeHitBlunt;

		// Token: 0x0400057F RID: 1407
		[Unsaved(false)]
		private bool sourceNaturalRockCached;

		// Token: 0x04000580 RID: 1408
		[Unsaved(false)]
		private ThingDef sourceNaturalRockCachedValue;
	}
}
