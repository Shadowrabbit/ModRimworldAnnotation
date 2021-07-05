using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000115 RID: 277
	public class ThingDef : BuildableDef
	{
		// Token: 0x17000164 RID: 356
		// (get) Token: 0x0600072F RID: 1839 RVA: 0x00022174 File Offset: 0x00020374
		public bool EverHaulable
		{
			get
			{
				return this.alwaysHaulable || this.designateHaulable;
			}
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x06000730 RID: 1840 RVA: 0x00022186 File Offset: 0x00020386
		public float VolumePerUnit
		{
			get
			{
				if (this.smallVolume)
				{
					return 0.1f;
				}
				return 1f;
			}
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x06000731 RID: 1841 RVA: 0x0002219B File Offset: 0x0002039B
		public override IntVec2 Size
		{
			get
			{
				return this.size;
			}
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x06000732 RID: 1842 RVA: 0x000221A3 File Offset: 0x000203A3
		public bool DiscardOnDestroyed
		{
			get
			{
				return this.race == null;
			}
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x06000733 RID: 1843 RVA: 0x000221AE File Offset: 0x000203AE
		public int BaseMaxHitPoints
		{
			get
			{
				return Mathf.RoundToInt(this.GetStatValueAbstract(StatDefOf.MaxHitPoints, null));
			}
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x06000734 RID: 1844 RVA: 0x000221C1 File Offset: 0x000203C1
		public float BaseFlammability
		{
			get
			{
				return this.GetStatValueAbstract(StatDefOf.Flammability, null);
			}
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x06000735 RID: 1845 RVA: 0x000221CF File Offset: 0x000203CF
		// (set) Token: 0x06000736 RID: 1846 RVA: 0x000221DD File Offset: 0x000203DD
		public float BaseMarketValue
		{
			get
			{
				return this.GetStatValueAbstract(StatDefOf.MarketValue, null);
			}
			set
			{
				this.SetStatBaseValue(StatDefOf.MarketValue, value);
			}
		}

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x06000737 RID: 1847 RVA: 0x000221EB File Offset: 0x000203EB
		public float BaseMass
		{
			get
			{
				return this.GetStatValueAbstract(StatDefOf.Mass, null);
			}
		}

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x06000738 RID: 1848 RVA: 0x000221FC File Offset: 0x000203FC
		public bool PlayerAcquirable
		{
			get
			{
				return !this.destroyOnDrop && (this != ThingDefOf.ReinforcedBarrel || Find.Storyteller == null || !Find.Storyteller.difficulty.classicMortars) && (this.requiresFactionToAcquire == null || Find.World == null || Find.World.factionManager == null || Find.FactionManager.FirstFactionOfDef(this.requiresFactionToAcquire) != null);
			}
		}

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x06000739 RID: 1849 RVA: 0x00022268 File Offset: 0x00020468
		public bool EverTransmitsPower
		{
			get
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					CompProperties_Power compProperties_Power = this.comps[i] as CompProperties_Power;
					if (compProperties_Power != null && compProperties_Power.transmitsPower)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x0600073A RID: 1850 RVA: 0x000222AB File Offset: 0x000204AB
		public bool Minifiable
		{
			get
			{
				return this.minifiedDef != null;
			}
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x0600073B RID: 1851 RVA: 0x000222B6 File Offset: 0x000204B6
		public bool HasThingIDNumber
		{
			get
			{
				return this.category != ThingCategory.Mote;
			}
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x0600073C RID: 1852 RVA: 0x000222C8 File Offset: 0x000204C8
		public List<RecipeDef> AllRecipes
		{
			get
			{
				if (this.allRecipesCached == null)
				{
					this.allRecipesCached = new List<RecipeDef>();
					if (this.recipes != null)
					{
						for (int i = 0; i < this.recipes.Count; i++)
						{
							this.allRecipesCached.Add(this.recipes[i]);
						}
					}
					List<RecipeDef> allDefsListForReading = DefDatabase<RecipeDef>.AllDefsListForReading;
					for (int j = 0; j < allDefsListForReading.Count; j++)
					{
						if (allDefsListForReading[j].recipeUsers != null && allDefsListForReading[j].recipeUsers.Contains(this))
						{
							this.allRecipesCached.Add(allDefsListForReading[j]);
						}
					}
				}
				return this.allRecipesCached;
			}
		}

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x0600073D RID: 1853 RVA: 0x00022374 File Offset: 0x00020574
		public bool ConnectToPower
		{
			get
			{
				if (this.EverTransmitsPower)
				{
					return false;
				}
				for (int i = 0; i < this.comps.Count; i++)
				{
					if (this.comps[i].compClass == typeof(CompPowerBattery))
					{
						return true;
					}
					if (this.comps[i].compClass == typeof(CompPowerTrader))
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x0600073E RID: 1854 RVA: 0x000223EA File Offset: 0x000205EA
		public bool CoexistsWithFloors
		{
			get
			{
				return !this.neverOverlapFloors && !this.coversFloor;
			}
		}

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x0600073F RID: 1855 RVA: 0x000223FF File Offset: 0x000205FF
		public FillCategory Fillage
		{
			get
			{
				if (this.fillPercent < 0.01f)
				{
					return FillCategory.None;
				}
				if (this.fillPercent > 0.99f)
				{
					return FillCategory.Full;
				}
				return FillCategory.Partial;
			}
		}

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x06000740 RID: 1856 RVA: 0x00022420 File Offset: 0x00020620
		public bool MakeFog
		{
			get
			{
				return this.Fillage == FillCategory.Full;
			}
		}

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x06000741 RID: 1857 RVA: 0x0002242C File Offset: 0x0002062C
		public bool CanOverlapZones
		{
			get
			{
				if (this.building != null && this.building.SupportsPlants)
				{
					return false;
				}
				if (this.passability == Traversability.Impassable && this.category != ThingCategory.Plant)
				{
					return false;
				}
				if (this.surfaceType >= SurfaceType.Item)
				{
					return false;
				}
				if (typeof(ISlotGroupParent).IsAssignableFrom(this.thingClass))
				{
					return false;
				}
				if (!this.canOverlapZones)
				{
					return false;
				}
				if (this.IsBlueprint || this.IsFrame)
				{
					ThingDef thingDef = this.entityDefToBuild as ThingDef;
					if (thingDef != null)
					{
						return thingDef.CanOverlapZones;
					}
				}
				return true;
			}
		}

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x06000742 RID: 1858 RVA: 0x000224B9 File Offset: 0x000206B9
		public bool CountAsResource
		{
			get
			{
				return this.resourceReadoutPriority > ResourceCountPriority.Uncounted;
			}
		}

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x06000743 RID: 1859 RVA: 0x000224C4 File Offset: 0x000206C4
		public List<VerbProperties> Verbs
		{
			get
			{
				if (this.verbs != null)
				{
					return this.verbs;
				}
				return ThingDef.EmptyVerbPropertiesList;
			}
		}

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x06000744 RID: 1860 RVA: 0x000224DC File Offset: 0x000206DC
		public bool CanHaveFaction
		{
			get
			{
				if (this.IsBlueprint || this.IsFrame)
				{
					return true;
				}
				ThingCategory thingCategory = this.category;
				return thingCategory == ThingCategory.Pawn || thingCategory == ThingCategory.Building;
			}
		}

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x06000745 RID: 1861 RVA: 0x00022511 File Offset: 0x00020711
		public bool Claimable
		{
			get
			{
				return this.building != null && this.building.claimable && !this.building.isNaturalRock;
			}
		}

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x06000746 RID: 1862 RVA: 0x00022538 File Offset: 0x00020738
		public ThingCategoryDef FirstThingCategory
		{
			get
			{
				if (this.thingCategories.NullOrEmpty<ThingCategoryDef>())
				{
					return null;
				}
				return this.thingCategories[0];
			}
		}

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x06000747 RID: 1863 RVA: 0x00022555 File Offset: 0x00020755
		public float MedicineTendXpGainFactor
		{
			get
			{
				return Mathf.Clamp(this.GetStatValueAbstract(StatDefOf.MedicalPotency, null) * 0.7f, 0.5f, 1f);
			}
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x06000748 RID: 1864 RVA: 0x00022578 File Offset: 0x00020778
		public bool CanEverDeteriorate
		{
			get
			{
				return this.useHitPoints && (this.category == ThingCategory.Item || this == ThingDefOf.BurnedTree);
			}
		}

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x06000749 RID: 1865 RVA: 0x00022597 File Offset: 0x00020797
		public bool CanInteractThroughCorners
		{
			get
			{
				return this.category == ThingCategory.Building && this.holdsRoof && (this.building == null || !this.building.isNaturalRock || this.IsSmoothed);
			}
		}

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x0600074A RID: 1866 RVA: 0x000225CE File Offset: 0x000207CE
		public bool AffectsRegions
		{
			get
			{
				return this.passability == Traversability.Impassable || this.IsDoor || this.IsFence;
			}
		}

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x0600074B RID: 1867 RVA: 0x000225E9 File Offset: 0x000207E9
		public bool AffectsReachability
		{
			get
			{
				return this.AffectsRegions || (this.passability == Traversability.Impassable || this.IsDoor) || TouchPathEndModeUtility.MakesOccupiedCellsAlwaysReachableDiagonally(this);
			}
		}

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x0600074C RID: 1868 RVA: 0x00022614 File Offset: 0x00020814
		public string DescriptionDetailed
		{
			get
			{
				if (this.descriptionDetailedCached == null)
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append(this.description);
					if (this.IsApparel)
					{
						stringBuilder.AppendLine();
						stringBuilder.AppendLine();
						stringBuilder.AppendLine(string.Format("{0}: {1}", "Layer".Translate(), this.apparel.GetLayersString()));
						stringBuilder.Append(string.Format("{0}: {1}", "Covers".Translate(), this.apparel.GetCoveredOuterPartsString(BodyDefOf.Human)));
						if (this.equippedStatOffsets != null && this.equippedStatOffsets.Count > 0)
						{
							stringBuilder.AppendLine();
							stringBuilder.AppendLine();
							for (int i = 0; i < this.equippedStatOffsets.Count; i++)
							{
								if (i > 0)
								{
									stringBuilder.AppendLine();
								}
								StatModifier statModifier = this.equippedStatOffsets[i];
								stringBuilder.Append(string.Format("{0}: {1}", statModifier.stat.LabelCap, statModifier.ValueToStringAsOffset));
							}
						}
					}
					this.descriptionDetailedCached = stringBuilder.ToString();
				}
				return this.descriptionDetailedCached;
			}
		}

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x0600074D RID: 1869 RVA: 0x0002273E File Offset: 0x0002093E
		public bool IsApparel
		{
			get
			{
				return this.apparel != null;
			}
		}

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x0600074E RID: 1870 RVA: 0x00022749 File Offset: 0x00020949
		public bool IsBed
		{
			get
			{
				return typeof(Building_Bed).IsAssignableFrom(this.thingClass);
			}
		}

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x0600074F RID: 1871 RVA: 0x00022760 File Offset: 0x00020960
		public bool IsCorpse
		{
			get
			{
				return typeof(Corpse).IsAssignableFrom(this.thingClass);
			}
		}

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x06000750 RID: 1872 RVA: 0x00022777 File Offset: 0x00020977
		public bool IsFrame
		{
			get
			{
				return this.isFrameInt;
			}
		}

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x06000751 RID: 1873 RVA: 0x0002277F File Offset: 0x0002097F
		public bool IsBlueprint
		{
			get
			{
				return this.entityDefToBuild != null && this.category == ThingCategory.Ethereal;
			}
		}

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x06000752 RID: 1874 RVA: 0x00022795 File Offset: 0x00020995
		public bool IsStuff
		{
			get
			{
				return this.stuffProps != null;
			}
		}

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x06000753 RID: 1875 RVA: 0x000227A0 File Offset: 0x000209A0
		public bool IsMedicine
		{
			get
			{
				return this.statBases.StatListContains(StatDefOf.MedicalPotency);
			}
		}

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x06000754 RID: 1876 RVA: 0x000227B2 File Offset: 0x000209B2
		public bool IsDoor
		{
			get
			{
				return typeof(Building_Door).IsAssignableFrom(this.thingClass);
			}
		}

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x06000755 RID: 1877 RVA: 0x000227C9 File Offset: 0x000209C9
		public bool IsFence
		{
			get
			{
				return this.building != null && this.building.isFence;
			}
		}

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x06000756 RID: 1878 RVA: 0x000227E0 File Offset: 0x000209E0
		public bool IsFilth
		{
			get
			{
				return this.filth != null;
			}
		}

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x06000757 RID: 1879 RVA: 0x000227EB File Offset: 0x000209EB
		public bool IsIngestible
		{
			get
			{
				return this.ingestible != null;
			}
		}

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x06000758 RID: 1880 RVA: 0x000227F6 File Offset: 0x000209F6
		public bool IsNutritionGivingIngestible
		{
			get
			{
				return this.IsIngestible && this.ingestible.CachedNutrition > 0f;
			}
		}

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x06000759 RID: 1881 RVA: 0x00022814 File Offset: 0x00020A14
		public bool IsWeapon
		{
			get
			{
				return this.category == ThingCategory.Item && (!this.verbs.NullOrEmpty<VerbProperties>() || !this.tools.NullOrEmpty<Tool>()) && !this.IsApparel;
			}
		}

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x0600075A RID: 1882 RVA: 0x00022844 File Offset: 0x00020A44
		public bool IsCommsConsole
		{
			get
			{
				return typeof(Building_CommsConsole).IsAssignableFrom(this.thingClass);
			}
		}

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x0600075B RID: 1883 RVA: 0x0002285B File Offset: 0x00020A5B
		public bool IsOrbitalTradeBeacon
		{
			get
			{
				return typeof(Building_OrbitalTradeBeacon).IsAssignableFrom(this.thingClass);
			}
		}

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x0600075C RID: 1884 RVA: 0x00022872 File Offset: 0x00020A72
		public bool IsFoodDispenser
		{
			get
			{
				return typeof(Building_NutrientPasteDispenser).IsAssignableFrom(this.thingClass);
			}
		}

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x0600075D RID: 1885 RVA: 0x00022889 File Offset: 0x00020A89
		public bool IsDrug
		{
			get
			{
				return this.ingestible != null && this.ingestible.drugCategory > DrugCategory.None;
			}
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x0600075E RID: 1886 RVA: 0x000228A3 File Offset: 0x00020AA3
		public bool IsPleasureDrug
		{
			get
			{
				return this.IsDrug && this.ingestible.joy > 0f;
			}
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x0600075F RID: 1887 RVA: 0x000228C1 File Offset: 0x00020AC1
		public bool IsNonMedicalDrug
		{
			get
			{
				return this.IsDrug && this.ingestible.drugCategory != DrugCategory.Medical;
			}
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x06000760 RID: 1888 RVA: 0x000228DE File Offset: 0x00020ADE
		public bool IsTable
		{
			get
			{
				return this.surfaceType == SurfaceType.Eat && this.HasComp(typeof(CompGatherSpot));
			}
		}

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x06000761 RID: 1889 RVA: 0x000228FB File Offset: 0x00020AFB
		public bool IsWorkTable
		{
			get
			{
				return typeof(Building_WorkTable).IsAssignableFrom(this.thingClass);
			}
		}

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x06000762 RID: 1890 RVA: 0x00022912 File Offset: 0x00020B12
		public bool IsShell
		{
			get
			{
				return this.projectileWhenLoaded != null;
			}
		}

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x06000763 RID: 1891 RVA: 0x0002291D File Offset: 0x00020B1D
		public bool IsArt
		{
			get
			{
				return this.IsWithinCategory(ThingCategoryDefOf.BuildingsArt);
			}
		}

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x06000764 RID: 1892 RVA: 0x0002292A File Offset: 0x00020B2A
		public bool IsSmoothable
		{
			get
			{
				return this.building != null && this.building.smoothedThing != null;
			}
		}

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x06000765 RID: 1893 RVA: 0x00022944 File Offset: 0x00020B44
		public bool IsSmoothed
		{
			get
			{
				return this.building != null && this.building.unsmoothedThing != null;
			}
		}

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x06000766 RID: 1894 RVA: 0x0002295E File Offset: 0x00020B5E
		public bool IsMetal
		{
			get
			{
				return this.stuffProps != null && this.stuffProps.categories.Contains(StuffCategoryDefOf.Metallic);
			}
		}

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x06000767 RID: 1895 RVA: 0x0002297F File Offset: 0x00020B7F
		public bool IsCryptosleepCasket
		{
			get
			{
				return typeof(Building_CryptosleepCasket).IsAssignableFrom(this.thingClass);
			}
		}

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x06000768 RID: 1896 RVA: 0x00022996 File Offset: 0x00020B96
		public bool IsGibbetCage
		{
			get
			{
				return typeof(Building_GibbetCage).IsAssignableFrom(this.thingClass);
			}
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x06000769 RID: 1897 RVA: 0x000229B0 File Offset: 0x00020BB0
		public bool IsAddictiveDrug
		{
			get
			{
				CompProperties_Drug compProperties = this.GetCompProperties<CompProperties_Drug>();
				return compProperties != null && compProperties.addictiveness > 0f;
			}
		}

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x0600076A RID: 1898 RVA: 0x000229D6 File Offset: 0x00020BD6
		public bool IsMeat
		{
			get
			{
				return this.category == ThingCategory.Item && this.thingCategories != null && this.thingCategories.Contains(ThingCategoryDefOf.MeatRaw);
			}
		}

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x0600076B RID: 1899 RVA: 0x000229FB File Offset: 0x00020BFB
		public bool IsEgg
		{
			get
			{
				return this.category == ThingCategory.Item && this.thingCategories != null && (this.thingCategories.Contains(ThingCategoryDefOf.EggsFertilized) || this.thingCategories.Contains(ThingCategoryDefOf.EggsUnfertilized));
			}
		}

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x0600076C RID: 1900 RVA: 0x00022A34 File Offset: 0x00020C34
		public bool IsLeather
		{
			get
			{
				return this.category == ThingCategory.Item && this.thingCategories != null && this.thingCategories.Contains(ThingCategoryDefOf.Leathers);
			}
		}

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x0600076D RID: 1901 RVA: 0x00022A59 File Offset: 0x00020C59
		public bool IsWool
		{
			get
			{
				return this.category == ThingCategory.Item && this.thingCategories != null && this.thingCategories.Contains(ThingCategoryDefOf.Wools);
			}
		}

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x0600076E RID: 1902 RVA: 0x00022A80 File Offset: 0x00020C80
		public bool IsRangedWeapon
		{
			get
			{
				if (!this.IsWeapon)
				{
					return false;
				}
				if (!this.verbs.NullOrEmpty<VerbProperties>())
				{
					for (int i = 0; i < this.verbs.Count; i++)
					{
						if (!this.verbs[i].IsMeleeAttack)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x0600076F RID: 1903 RVA: 0x00022AD0 File Offset: 0x00020CD0
		public bool IsMeleeWeapon
		{
			get
			{
				return this.IsWeapon && !this.IsRangedWeapon;
			}
		}

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x06000770 RID: 1904 RVA: 0x00022AE8 File Offset: 0x00020CE8
		public bool IsWeaponUsingProjectiles
		{
			get
			{
				if (!this.IsWeapon)
				{
					return false;
				}
				if (!this.verbs.NullOrEmpty<VerbProperties>())
				{
					for (int i = 0; i < this.verbs.Count; i++)
					{
						if (this.verbs[i].LaunchesProjectile)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x06000771 RID: 1905 RVA: 0x00022B38 File Offset: 0x00020D38
		public bool IsBuildingArtificial
		{
			get
			{
				return (this.category == ThingCategory.Building || this.IsFrame) && (this.building == null || (!this.building.isNaturalRock && !this.building.isResourceRock));
			}
		}

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x06000772 RID: 1906 RVA: 0x00022B74 File Offset: 0x00020D74
		public bool IsNonResourceNaturalRock
		{
			get
			{
				return this.category == ThingCategory.Building && this.building.isNaturalRock && !this.building.isResourceRock && !this.IsSmoothed;
			}
		}

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x06000773 RID: 1907 RVA: 0x00022BA4 File Offset: 0x00020DA4
		public bool IsNaturalOrgan
		{
			get
			{
				if (this.isNaturalOrganCached == null)
				{
					if (this.category != ThingCategory.Item)
					{
						this.isNaturalOrganCached = new bool?(false);
					}
					else
					{
						List<BodyPartDef> allDefsListForReading = DefDatabase<BodyPartDef>.AllDefsListForReading;
						this.isNaturalOrganCached = new bool?(false);
						for (int i = 0; i < allDefsListForReading.Count; i++)
						{
							if (allDefsListForReading[i].spawnThingOnRemoved == this)
							{
								this.isNaturalOrganCached = new bool?(true);
								break;
							}
						}
					}
				}
				return this.isNaturalOrganCached.Value;
			}
		}

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x06000774 RID: 1908 RVA: 0x00022C20 File Offset: 0x00020E20
		public bool IsFungus
		{
			get
			{
				return this.ingestible != null && this.ingestible.foodType.HasFlag(FoodTypeFlags.Fungus);
			}
		}

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x06000775 RID: 1909 RVA: 0x00022C4B File Offset: 0x00020E4B
		public bool CanAffectLinker
		{
			get
			{
				return (this.graphicData != null && this.graphicData.Linked) || this.IsDoor;
			}
		}

		// Token: 0x06000776 RID: 1910 RVA: 0x00022C6C File Offset: 0x00020E6C
		public bool BlocksPlanting(bool canWipePlants = false)
		{
			return (this.building == null || !this.building.SupportsPlants) && (this.blockPlants || (!canWipePlants && this.category == ThingCategory.Plant) || this.Fillage > FillCategory.None || this.IsEdifice());
		}

		// Token: 0x06000777 RID: 1911 RVA: 0x00022CC0 File Offset: 0x00020EC0
		public bool EverStorable(bool willMinifyIfPossible)
		{
			if (typeof(MinifiedThing).IsAssignableFrom(this.thingClass))
			{
				return true;
			}
			if (!this.thingCategories.NullOrEmpty<ThingCategoryDef>())
			{
				if (this.category == ThingCategory.Item)
				{
					return true;
				}
				if (willMinifyIfPossible && this.Minifiable)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000778 RID: 1912 RVA: 0x00022D0C File Offset: 0x00020F0C
		public Thing GetConcreteExample(ThingDef stuff = null)
		{
			if (this.concreteExamplesInt == null)
			{
				this.concreteExamplesInt = new Dictionary<ThingDef, Thing>();
			}
			if (stuff == null)
			{
				stuff = ThingDefOf.Steel;
			}
			if (!this.concreteExamplesInt.ContainsKey(stuff))
			{
				if (this.race == null)
				{
					this.concreteExamplesInt[stuff] = ThingMaker.MakeThing(this, base.MadeFromStuff ? stuff : null);
				}
				else
				{
					this.concreteExamplesInt[stuff] = PawnGenerator.GeneratePawn((from pkd in DefDatabase<PawnKindDef>.AllDefsListForReading
					where pkd.race == this
					select pkd).FirstOrDefault<PawnKindDef>(), null);
				}
			}
			return this.concreteExamplesInt[stuff];
		}

		// Token: 0x06000779 RID: 1913 RVA: 0x00022DA8 File Offset: 0x00020FA8
		public CompProperties CompDefFor<T>() where T : ThingComp
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				if (this.comps[i].compClass == typeof(T))
				{
					return this.comps[i];
				}
			}
			return null;
		}

		// Token: 0x0600077A RID: 1914 RVA: 0x00022DFC File Offset: 0x00020FFC
		public CompProperties CompDefForAssignableFrom<T>() where T : ThingComp
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				if (typeof(T).IsAssignableFrom(this.comps[i].compClass))
				{
					return this.comps[i];
				}
			}
			return null;
		}

		// Token: 0x0600077B RID: 1915 RVA: 0x00022E50 File Offset: 0x00021050
		public bool HasComp(Type compType)
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				if (this.comps[i].compClass == compType)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600077C RID: 1916 RVA: 0x00022E90 File Offset: 0x00021090
		public bool HasAssignableCompFrom(Type compType)
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				if (compType.IsAssignableFrom(this.comps[i].compClass))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600077D RID: 1917 RVA: 0x00022ED0 File Offset: 0x000210D0
		public T GetCompProperties<T>() where T : CompProperties
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				T t = this.comps[i] as T;
				if (t != null)
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x0600077E RID: 1918 RVA: 0x00022F20 File Offset: 0x00021120
		public override void PostLoad()
		{
			if (this.graphicData != null)
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					if (this.graphicData.shaderType == null)
					{
						this.graphicData.shaderType = ShaderTypeDefOf.Cutout;
					}
					this.graphic = this.graphicData.Graphic;
					if (this.drawerType != DrawerType.RealtimeOnly)
					{
						TextureAtlasGroup textureAtlasGroup = this.category.ToAtlasGroup();
						this.graphic.TryInsertIntoAtlas(textureAtlasGroup);
						if (textureAtlasGroup == TextureAtlasGroup.Building && this.Minifiable)
						{
							this.graphic.TryInsertIntoAtlas(TextureAtlasGroup.Item);
						}
					}
				});
			}
			if (this.tools != null)
			{
				for (int i = 0; i < this.tools.Count; i++)
				{
					this.tools[i].id = i.ToString();
				}
			}
			if (this.verbs != null && this.verbs.Count == 1 && this.verbs[0].label.NullOrEmpty())
			{
				this.verbs[0].label = this.label;
			}
			base.PostLoad();
			if (this.category == ThingCategory.Building && this.building == null)
			{
				this.building = new BuildingProperties();
			}
			if (this.building != null)
			{
				this.building.PostLoadSpecial(this);
			}
			if (this.apparel != null)
			{
				this.apparel.PostLoadSpecial(this);
			}
			if (this.plant != null)
			{
				this.plant.PostLoadSpecial(this);
			}
			if (this.comps != null)
			{
				foreach (CompProperties compProperties in this.comps)
				{
					compProperties.PostLoadSpecial(this);
				}
			}
		}

		// Token: 0x0600077F RID: 1919 RVA: 0x0002306C File Offset: 0x0002126C
		protected override void ResolveIcon()
		{
			base.ResolveIcon();
			if (this.category == ThingCategory.Pawn)
			{
				if (!this.race.Humanlike)
				{
					PawnKindDef anyPawnKind = this.race.AnyPawnKind;
					if (anyPawnKind != null)
					{
						Material material = anyPawnKind.lifeStages.Last<PawnKindLifeStage>().bodyGraphicData.Graphic.MatAt(Rot4.East, null);
						this.uiIcon = (Texture2D)material.mainTexture;
						this.uiIconColor = material.color;
						return;
					}
				}
			}
			else
			{
				ThingDef thingDef = GenStuff.DefaultStuffFor(this);
				if (this.colorGenerator != null && (thingDef == null || thingDef.stuffProps.allowColorGenerators))
				{
					this.uiIconColor = this.colorGenerator.ExemplaryColor;
				}
				else if (thingDef != null)
				{
					this.uiIconColor = base.GetColorForStuff(thingDef);
				}
				else if (this.graphicData != null)
				{
					this.uiIconColor = this.graphicData.color;
				}
				if (this.rotatable && this.graphic != null && this.graphic != BaseContent.BadGraphic && this.graphic.ShouldDrawRotated && this.defaultPlacingRot == Rot4.South)
				{
					this.uiIconAngle = 180f + this.graphic.DrawRotatedExtraAngleOffset;
				}
			}
		}

		// Token: 0x06000780 RID: 1920 RVA: 0x0002319C File Offset: 0x0002139C
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			if (this.ingestible != null)
			{
				this.ingestible.parent = this;
			}
			if (this.stuffProps != null)
			{
				this.stuffProps.parent = this;
			}
			if (this.building != null)
			{
				this.building.ResolveReferencesSpecial();
			}
			if (this.graphicData != null)
			{
				this.graphicData.ResolveReferencesSpecial();
			}
			if (this.race != null)
			{
				this.race.ResolveReferencesSpecial();
			}
			if (this.stuffProps != null)
			{
				this.stuffProps.ResolveReferencesSpecial();
			}
			if (this.soundImpactDefault == null)
			{
				this.soundImpactDefault = SoundDefOf.BulletImpact_Ground;
			}
			if (this.soundDrop == null)
			{
				this.soundDrop = SoundDefOf.Standard_Drop;
			}
			if (this.soundPickup == null)
			{
				this.soundPickup = SoundDefOf.Standard_Pickup;
			}
			if (this.soundInteract == null)
			{
				this.soundInteract = SoundDefOf.Standard_Pickup;
			}
			if (this.inspectorTabs != null && this.inspectorTabs.Any<Type>())
			{
				this.inspectorTabsResolved = new List<InspectTabBase>();
				for (int i = 0; i < this.inspectorTabs.Count; i++)
				{
					try
					{
						this.inspectorTabsResolved.Add(InspectTabManager.GetSharedInstance(this.inspectorTabs[i]));
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Could not instantiate inspector tab of type ",
							this.inspectorTabs[i],
							": ",
							ex
						}));
					}
				}
			}
			if (this.comps != null)
			{
				for (int j = 0; j < this.comps.Count; j++)
				{
					this.comps[j].ResolveReferences(this);
				}
			}
		}

		// Token: 0x06000781 RID: 1921 RVA: 0x00023340 File Offset: 0x00021540
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.category != ThingCategory.Ethereal && this.label.NullOrEmpty())
			{
				yield return "no label";
			}
			if (this.graphicData != null)
			{
				foreach (string text2 in this.graphicData.ConfigErrors(this))
				{
					yield return text2;
				}
				enumerator = null;
			}
			if (this.projectile != null)
			{
				foreach (string text3 in this.projectile.ConfigErrors(this))
				{
					yield return text3;
				}
				enumerator = null;
			}
			if (this.statBases != null)
			{
				using (List<StatModifier>.Enumerator enumerator2 = this.statBases.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						StatModifier statBase = enumerator2.Current;
						if ((from st in this.statBases
						where st.stat == statBase.stat
						select st).Count<StatModifier>() > 1)
						{
							yield return "defines the stat base " + statBase.stat + " more than once.";
						}
					}
				}
				List<StatModifier>.Enumerator enumerator2 = default(List<StatModifier>.Enumerator);
			}
			if (!BeautyUtility.BeautyRelevant(this.category) && this.StatBaseDefined(StatDefOf.Beauty))
			{
				yield return "Beauty stat base is defined, but Things of category " + this.category + " cannot have beauty.";
			}
			if (char.IsNumber(this.defName[this.defName.Length - 1]))
			{
				yield return "ends with a numerical digit, which is not allowed on ThingDefs.";
			}
			if (this.thingClass == null)
			{
				yield return "has null thingClass.";
			}
			if (this.comps.Count > 0 && !typeof(ThingWithComps).IsAssignableFrom(this.thingClass))
			{
				yield return "has components but it's thingClass is not a ThingWithComps";
			}
			if (this.ConnectToPower && this.drawerType == DrawerType.RealtimeOnly && this.IsFrame)
			{
				yield return "connects to power but does not add to map mesh. Will not create wire meshes.";
			}
			if (this.costList != null)
			{
				foreach (ThingDefCountClass thingDefCountClass in this.costList)
				{
					if (thingDefCountClass.count == 0)
					{
						yield return "cost in " + thingDefCountClass.thingDef + " is zero.";
					}
				}
				List<ThingDefCountClass>.Enumerator enumerator3 = default(List<ThingDefCountClass>.Enumerator);
			}
			if (this.thingCategories != null)
			{
				ThingCategoryDef thingCategoryDef = this.thingCategories.FirstOrDefault((ThingCategoryDef cat) => this.thingCategories.Count((ThingCategoryDef c) => c == cat) > 1);
				if (thingCategoryDef != null)
				{
					yield return "has duplicate thingCategory " + thingCategoryDef + ".";
				}
			}
			if (this.Fillage == FillCategory.Full && this.category != ThingCategory.Building)
			{
				yield return "gives full cover but is not a building.";
			}
			if (this.equipmentType != EquipmentType.None)
			{
				if (this.techLevel == TechLevel.Undefined && !this.destroyOnDrop)
				{
					yield return "is equipment but has no tech level.";
				}
				if (!this.comps.Any((CompProperties c) => c.compClass == typeof(CompEquippable)))
				{
					yield return "is equipment but has no CompEquippable";
				}
			}
			if (this.thingClass == typeof(Bullet) && this.projectile.damageDef == null)
			{
				yield return " is a bullet but has no damageDef.";
			}
			if (this.destroyOnDrop && this.tradeability != Tradeability.None)
			{
				yield return "destroyOnDrop but tradeability is " + this.tradeability;
			}
			if (this.stackLimit > 1 && !this.drawGUIOverlay)
			{
				yield return "has stackLimit > 1 but also has drawGUIOverlay = false.";
			}
			if (this.damageMultipliers != null)
			{
				using (List<DamageMultiplier>.Enumerator enumerator4 = this.damageMultipliers.GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						DamageMultiplier mult = enumerator4.Current;
						if ((from m in this.damageMultipliers
						where m.damageDef == mult.damageDef
						select m).Count<DamageMultiplier>() > 1)
						{
							yield return "has multiple damage multipliers for damageDef " + mult.damageDef;
							break;
						}
					}
				}
				List<DamageMultiplier>.Enumerator enumerator4 = default(List<DamageMultiplier>.Enumerator);
			}
			if (this.Fillage == FillCategory.Full && !this.IsEdifice())
			{
				yield return "fillPercent is 1.00 but is not edifice";
			}
			if (base.MadeFromStuff && this.constructEffect != null)
			{
				yield return "madeFromStuff but has a defined constructEffect (which will always be overridden by stuff's construct animation).";
			}
			if (base.MadeFromStuff && this.stuffCategories.NullOrEmpty<StuffCategoryDef>())
			{
				yield return "madeFromStuff but has no stuffCategories.";
			}
			if (this.costList.NullOrEmpty<ThingDefCountClass>() && this.costStuffCount <= 0 && this.recipeMaker != null)
			{
				yield return "has a recipeMaker but no costList or costStuffCount.";
			}
			if (this.GetStatValueAbstract(StatDefOf.DeteriorationRate, null) > 1E-05f && !this.CanEverDeteriorate && !this.destroyOnDrop)
			{
				yield return "has >0 DeteriorationRate but can't deteriorate.";
			}
			if (this.smeltProducts != null && this.smeltable)
			{
				yield return "has smeltProducts but has smeltable=false";
			}
			if (this.equipmentType != EquipmentType.None && this.verbs.NullOrEmpty<VerbProperties>() && this.tools.NullOrEmpty<Tool>())
			{
				yield return "is equipment but has no verbs or tools";
			}
			if (this.Minifiable && this.thingCategories.NullOrEmpty<ThingCategoryDef>())
			{
				yield return "is minifiable but not in any thing category";
			}
			if (this.category == ThingCategory.Building && !this.Minifiable && !this.thingCategories.NullOrEmpty<ThingCategoryDef>())
			{
				yield return "is not minifiable yet has thing categories (could be confusing in thing filters because it can't be moved/stored anyway)";
			}
			if (!this.destroyOnDrop && this != ThingDefOf.MinifiedThing && (this.EverHaulable || this.Minifiable))
			{
				if (!this.statBases.NullOrEmpty<StatModifier>())
				{
					if (this.statBases.Any((StatModifier s) => s.stat == StatDefOf.Mass))
					{
						goto IL_9CB;
					}
				}
				yield return "is haulable, but does not have an authored mass value";
			}
			IL_9CB:
			if (this.ingestible == null && this.GetStatValueAbstract(StatDefOf.Nutrition, null) != 0f)
			{
				yield return "has nutrition but ingestible properties are null";
			}
			if (this.BaseFlammability != 0f && !this.useHitPoints && this.category != ThingCategory.Pawn && !this.destroyOnDrop)
			{
				yield return "flammable but has no hitpoints (will burn indefinitely)";
			}
			if (this.graphicData != null && this.graphicData.shadowData != null && this.staticSunShadowHeight > 0f)
			{
				yield return "graphicData defines a shadowInfo but staticSunShadowHeight > 0";
			}
			if (this.saveCompressible && this.Claimable)
			{
				yield return "claimable item is compressible; faction will be unset after load";
			}
			if (this.deepCommonality > 0f != this.deepLumpSizeRange.TrueMax > 0)
			{
				yield return "if deepCommonality or deepLumpSizeRange is set, the other also must be set";
			}
			if (this.deepCommonality > 0f && this.deepCountPerPortion <= 0)
			{
				yield return "deepCommonality > 0 but deepCountPerPortion is not set";
			}
			if (this.verbs != null)
			{
				int num;
				for (int i = 0; i < this.verbs.Count; i = num + 1)
				{
					foreach (string arg in this.verbs[i].ConfigErrors(this))
					{
						yield return string.Format("verb {0}: {1}", i, arg);
					}
					enumerator = null;
					num = i;
				}
			}
			if (this.building != null)
			{
				foreach (string text4 in this.building.ConfigErrors(this))
				{
					yield return text4;
				}
				enumerator = null;
			}
			if (this.apparel != null)
			{
				foreach (string text5 in this.apparel.ConfigErrors(this))
				{
					yield return text5;
				}
				enumerator = null;
			}
			if (this.comps != null)
			{
				int num;
				for (int i = 0; i < this.comps.Count; i = num + 1)
				{
					foreach (string text6 in this.comps[i].ConfigErrors(this))
					{
						yield return text6;
					}
					enumerator = null;
					num = i;
				}
			}
			if (this.race != null)
			{
				foreach (string text7 in this.race.ConfigErrors(this))
				{
					yield return text7;
				}
				enumerator = null;
			}
			if (this.race != null && this.tools != null)
			{
				ThingDef.<>c__DisplayClass301_3 CS$<>8__locals3 = new ThingDef.<>c__DisplayClass301_3();
				CS$<>8__locals3.<>4__this = this;
				CS$<>8__locals3.i = 0;
				while (CS$<>8__locals3.i < this.tools.Count)
				{
					if (this.tools[CS$<>8__locals3.i].linkedBodyPartsGroup != null && !this.race.body.AllParts.Any((BodyPartRecord part) => part.groups.Contains(CS$<>8__locals3.<>4__this.tools[CS$<>8__locals3.i].linkedBodyPartsGroup)))
					{
						yield return string.Concat(new object[]
						{
							"has tool with linkedBodyPartsGroup ",
							this.tools[CS$<>8__locals3.i].linkedBodyPartsGroup,
							" but body ",
							this.race.body,
							" has no parts with that group."
						});
					}
					int num = CS$<>8__locals3.i;
					CS$<>8__locals3.i = num + 1;
				}
				CS$<>8__locals3 = null;
			}
			if (this.ingestible != null)
			{
				foreach (string text8 in this.ingestible.ConfigErrors())
				{
					yield return text8;
				}
				enumerator = null;
			}
			if (this.plant != null)
			{
				foreach (string text9 in this.plant.ConfigErrors())
				{
					yield return text9;
				}
				enumerator = null;
			}
			if (this.tools != null)
			{
				Tool tool = this.tools.SelectMany((Tool lhs) => from rhs in this.tools
				where lhs != rhs && lhs.id == rhs.id
				select rhs).FirstOrDefault<Tool>();
				if (tool != null)
				{
					yield return string.Format("duplicate thingdef tool id {0}", tool.id);
				}
				foreach (Tool tool2 in this.tools)
				{
					foreach (string text10 in tool2.ConfigErrors())
					{
						yield return text10;
					}
					enumerator = null;
				}
				List<Tool>.Enumerator enumerator5 = default(List<Tool>.Enumerator);
			}
			if (!this.randomStyle.NullOrEmpty<ThingStyleChance>())
			{
				using (List<ThingStyleChance>.Enumerator enumerator6 = this.randomStyle.GetEnumerator())
				{
					while (enumerator6.MoveNext())
					{
						if (enumerator6.Current.Chance <= 0f)
						{
							yield return "style chance <= 0.";
						}
					}
				}
				List<ThingStyleChance>.Enumerator enumerator6 = default(List<ThingStyleChance>.Enumerator);
				if (!this.comps.Any((CompProperties c) => c.compClass == typeof(CompStyleable)))
				{
					yield return "random style assigned, but missing CompStyleable!";
				}
			}
			if (this.relicChance > 0f && this.category != ThingCategory.Item)
			{
				yield return "relic chance > 0 but category != item";
			}
			yield break;
			yield break;
		}

		// Token: 0x06000782 RID: 1922 RVA: 0x00023350 File Offset: 0x00021550
		public static ThingDef Named(string defName)
		{
			return DefDatabase<ThingDef>.GetNamed(defName, true);
		}

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x06000783 RID: 1923 RVA: 0x00023359 File Offset: 0x00021559
		public string LabelAsStuff
		{
			get
			{
				if (!this.stuffProps.stuffAdjective.NullOrEmpty())
				{
					return this.stuffProps.stuffAdjective;
				}
				return this.label;
			}
		}

		// Token: 0x06000784 RID: 1924 RVA: 0x00023380 File Offset: 0x00021580
		public bool IsWithinCategory(ThingCategoryDef category)
		{
			if (this.thingCategories == null)
			{
				return false;
			}
			for (int i = 0; i < this.thingCategories.Count; i++)
			{
				for (ThingCategoryDef thingCategoryDef = this.thingCategories[i]; thingCategoryDef != null; thingCategoryDef = thingCategoryDef.parent)
				{
					if (thingCategoryDef == category)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000785 RID: 1925 RVA: 0x000233CD File Offset: 0x000215CD
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			foreach (StatDrawEntry statDrawEntry in base.SpecialDisplayStats(req))
			{
				yield return statDrawEntry;
			}
			IEnumerator<StatDrawEntry> enumerator = null;
			if (this.apparel != null)
			{
				string coveredOuterPartsString = this.apparel.GetCoveredOuterPartsString(BodyDefOf.Human);
				yield return new StatDrawEntry(StatCategoryDefOf.Apparel, "Covers".Translate(), coveredOuterPartsString, "Stat_Thing_Apparel_Covers_Desc".Translate(), 2750, null, null, false);
				yield return new StatDrawEntry(StatCategoryDefOf.Apparel, "Layer".Translate(), this.apparel.GetLayersString(), "Stat_Thing_Apparel_Layer_Desc".Translate(), 2751, null, null, false);
				yield return new StatDrawEntry(StatCategoryDefOf.Apparel, "Stat_Thing_Apparel_CountsAsClothingNudity_Name".Translate(), this.apparel.countsAsClothingForNudity ? "Yes".Translate() : "No".Translate(), "Stat_Thing_Apparel_CountsAsClothingNudity_Desc".Translate(), 2753, null, null, false);
			}
			if (this.IsMedicine && this.MedicineTendXpGainFactor != 1f)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "MedicineXpGainFactor".Translate(), this.MedicineTendXpGainFactor.ToStringPercent(), "Stat_Thing_Drug_MedicineXpGainFactor_Desc".Translate(), 1000, null, null, false);
			}
			if (this.fillPercent > 0f && (this.category == ThingCategory.Item || this.category == ThingCategory.Building || this.category == ThingCategory.Plant))
			{
				StatDrawEntry statDrawEntry2 = new StatDrawEntry(StatCategoryDefOf.Basics, "CoverEffectiveness".Translate(), this.BaseBlockChance().ToStringPercent(), "CoverEffectivenessExplanation".Translate(), 2000, null, null, false);
				yield return statDrawEntry2;
			}
			if (this.constructionSkillPrerequisite > 0)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "SkillRequiredToBuild".Translate(SkillDefOf.Construction.LabelCap), this.constructionSkillPrerequisite.ToString(), "SkillRequiredToBuildExplanation".Translate(SkillDefOf.Construction.LabelCap), 1100, null, null, false);
			}
			if (this.artisticSkillPrerequisite > 0)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "SkillRequiredToBuild".Translate(SkillDefOf.Artistic.LabelCap), this.artisticSkillPrerequisite.ToString(), "SkillRequiredToBuildExplanation".Translate(SkillDefOf.Artistic.LabelCap), 1100, null, null, false);
			}
			string[] array = (from u in (from r in DefDatabase<RecipeDef>.AllDefsListForReading
			where r.recipeUsers != null && r.products.Count == 1 && r.products.Any((ThingDefCountClass p) => p.thingDef == this) && !r.IsSurgery
			select r).SelectMany((RecipeDef r) => r.recipeUsers)
			select u.label).Distinct<string>().ToArray<string>();
			if (array.Any<string>())
			{
				string valueString = array.ToCommaList(false, false).CapitalizeFirst();
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "CreatedAt".Translate(), valueString, "Stat_Thing_CreatedAt_Desc".Translate(), 1103, null, null, false);
			}
			if (this.thingClass != null && typeof(Building_Bed).IsAssignableFrom(this.thingClass) && !this.statBases.StatListContains(StatDefOf.BedRestEffectiveness))
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Building, StatDefOf.BedRestEffectiveness, StatDefOf.BedRestEffectiveness.valueIfMissing, StatRequest.ForEmpty(), ToStringNumberSense.Undefined, null, false);
			}
			if (!this.verbs.NullOrEmpty<VerbProperties>())
			{
				VerbProperties verb = this.verbs.First((VerbProperties x) => x.isPrimary);
				StatCategoryDef verbStatCategory = (this.category == ThingCategory.Pawn) ? (verbStatCategory = StatCategoryDefOf.PawnCombat) : (verbStatCategory = StatCategoryDefOf.Weapon);
				float warmupTime = verb.warmupTime;
				if (warmupTime > 0f)
				{
					TaggedString taggedString = (this.category == ThingCategory.Pawn) ? "MeleeWarmupTime".Translate() : "WarmupTime".Translate();
					yield return new StatDrawEntry(verbStatCategory, taggedString, warmupTime.ToString("0.##") + " " + "LetterSecond".Translate(), "Stat_Thing_Weapon_MeleeWarmupTime_Desc".Translate(), 3555, null, null, false);
				}
				if (verb.defaultProjectile != null)
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.AppendLine("Stat_Thing_Damage_Desc".Translate());
					stringBuilder.AppendLine();
					float num = (float)verb.defaultProjectile.projectile.GetDamageAmount(req.Thing, stringBuilder);
					yield return new StatDrawEntry(verbStatCategory, "Damage".Translate(), num.ToString(), stringBuilder.ToString(), 5500, null, null, false);
					if (verb.defaultProjectile.projectile.damageDef.armorCategory != null)
					{
						StringBuilder stringBuilder2 = new StringBuilder();
						float armorPenetration = verb.defaultProjectile.projectile.GetArmorPenetration(req.Thing, stringBuilder2);
						TaggedString taggedString2 = "ArmorPenetrationExplanation".Translate();
						if (stringBuilder2.Length != 0)
						{
							taggedString2 += "\n\n" + stringBuilder2;
						}
						yield return new StatDrawEntry(verbStatCategory, "ArmorPenetration".Translate(), armorPenetration.ToStringPercent(), taggedString2, 5400, null, null, false);
					}
				}
				if (verb.LaunchesProjectile)
				{
					int burstShotCount = verb.burstShotCount;
					float burstShotFireRate = 60f / verb.ticksBetweenBurstShots.TicksToSeconds();
					float range = verb.range;
					if (burstShotCount > 1)
					{
						yield return new StatDrawEntry(verbStatCategory, "BurstShotCount".Translate(), burstShotCount.ToString(), "Stat_Thing_Weapon_BurstShotCount_Desc".Translate(), 5391, null, null, false);
						yield return new StatDrawEntry(verbStatCategory, "BurstShotFireRate".Translate(), burstShotFireRate.ToString("0.##") + " rpm", "Stat_Thing_Weapon_BurstShotFireRate_Desc".Translate(), 5392, null, null, false);
					}
					yield return new StatDrawEntry(verbStatCategory, "Range".Translate(), range.ToString("F0"), "Stat_Thing_Weapon_Range_Desc".Translate(), 5390, null, null, false);
					if (verb.defaultProjectile != null && verb.defaultProjectile.projectile != null && verb.defaultProjectile.projectile.stoppingPower != 0f)
					{
						yield return new StatDrawEntry(verbStatCategory, "StoppingPower".Translate(), verb.defaultProjectile.projectile.stoppingPower.ToString("F1"), "StoppingPowerExplanation".Translate(), 5402, null, null, false);
					}
				}
				if (verb.ForcedMissRadius > 0f)
				{
					yield return new StatDrawEntry(verbStatCategory, "MissRadius".Translate(), verb.ForcedMissRadius.ToString("0.#"), "Stat_Thing_Weapon_MissRadius_Desc".Translate(), 3557, null, null, false);
					yield return new StatDrawEntry(verbStatCategory, "DirectHitChance".Translate(), (1f / (float)GenRadial.NumCellsInRadius(verb.ForcedMissRadius)).ToStringPercent(), "Stat_Thing_Weapon_DirectHitChance_Desc".Translate(), 3560, null, null, false);
				}
				verb = null;
				verbStatCategory = null;
			}
			if (this.plant != null)
			{
				foreach (StatDrawEntry statDrawEntry3 in this.plant.SpecialDisplayStats())
				{
					yield return statDrawEntry3;
				}
				enumerator = null;
			}
			if (this.ingestible != null)
			{
				foreach (StatDrawEntry statDrawEntry4 in this.ingestible.SpecialDisplayStats())
				{
					yield return statDrawEntry4;
				}
				enumerator = null;
			}
			if (this.race != null)
			{
				foreach (StatDrawEntry statDrawEntry5 in this.race.SpecialDisplayStats(this, req))
				{
					yield return statDrawEntry5;
				}
				enumerator = null;
			}
			if (this.building != null)
			{
				foreach (StatDrawEntry statDrawEntry6 in this.building.SpecialDisplayStats(this, req))
				{
					yield return statDrawEntry6;
				}
				enumerator = null;
			}
			if (this.isTechHediff)
			{
				IEnumerable<RecipeDef> enumerable = from x in DefDatabase<RecipeDef>.AllDefs
				where x.addsHediff != null && x.IsIngredient(this)
				select x;
				bool multiple = enumerable.Count<RecipeDef>() >= 2;
				foreach (RecipeDef recipeDef in enumerable)
				{
					ThingDef.<>c__DisplayClass306_0 CS$<>8__locals1 = new ThingDef.<>c__DisplayClass306_0();
					string extraLabelPart = multiple ? (" (" + recipeDef.addsHediff.label + ")") : "";
					CS$<>8__locals1.diff = recipeDef.addsHediff;
					if (CS$<>8__locals1.diff.addedPartProps != null)
					{
						yield return new StatDrawEntry(StatCategoryDefOf.Basics, "BodyPartEfficiency".Translate() + extraLabelPart, CS$<>8__locals1.diff.addedPartProps.partEfficiency.ToStringByStyle(ToStringStyle.PercentZero, ToStringNumberSense.Absolute), "Stat_Thing_BodyPartEfficiency_Desc".Translate(), 4000, null, null, false);
					}
					foreach (StatDrawEntry statDrawEntry7 in CS$<>8__locals1.diff.SpecialDisplayStats(StatRequest.ForEmpty()))
					{
						statDrawEntry7.category = StatCategoryDefOf.Implant;
						yield return statDrawEntry7;
					}
					enumerator = null;
					HediffCompProperties_VerbGiver hediffCompProperties_VerbGiver = CS$<>8__locals1.diff.CompProps<HediffCompProperties_VerbGiver>();
					if (hediffCompProperties_VerbGiver != null)
					{
						if (!hediffCompProperties_VerbGiver.verbs.NullOrEmpty<VerbProperties>())
						{
							VerbProperties verb = hediffCompProperties_VerbGiver.verbs[0];
							if (!verb.IsMeleeAttack)
							{
								if (verb.defaultProjectile != null)
								{
									StringBuilder stringBuilder3 = new StringBuilder();
									stringBuilder3.AppendLine("Stat_Thing_Damage_Desc".Translate());
									stringBuilder3.AppendLine();
									int damageAmount = verb.defaultProjectile.projectile.GetDamageAmount(null, stringBuilder3);
									yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Damage".Translate() + extraLabelPart, damageAmount.ToString(), stringBuilder3.ToString(), 5500, null, null, false);
									if (verb.defaultProjectile.projectile.damageDef.armorCategory != null)
									{
										float armorPenetration2 = verb.defaultProjectile.projectile.GetArmorPenetration(null, null);
										yield return new StatDrawEntry(StatCategoryDefOf.Basics, "ArmorPenetration".Translate() + extraLabelPart, armorPenetration2.ToStringPercent(), "ArmorPenetrationExplanation".Translate(), 5400, null, null, false);
									}
								}
							}
							else
							{
								int meleeDamageBaseAmount = verb.meleeDamageBaseAmount;
								if (verb.meleeDamageDef.armorCategory != null)
								{
									float num2 = verb.meleeArmorPenetrationBase;
									if (num2 < 0f)
									{
										num2 = (float)meleeDamageBaseAmount * 0.015f;
									}
									yield return new StatDrawEntry(StatCategoryDefOf.Weapon, "ArmorPenetration".Translate() + extraLabelPart, num2.ToStringPercent(), "ArmorPenetrationExplanation".Translate(), 5400, null, null, false);
								}
							}
							verb = null;
						}
						else if (!hediffCompProperties_VerbGiver.tools.NullOrEmpty<Tool>())
						{
							Tool tool = hediffCompProperties_VerbGiver.tools[0];
							if (ThingUtility.PrimaryMeleeWeaponDamageType(hediffCompProperties_VerbGiver.tools).armorCategory != null)
							{
								float num3 = tool.armorPenetration;
								if (num3 < 0f)
								{
									num3 = tool.power * 0.015f;
								}
								yield return new StatDrawEntry(StatCategoryDefOf.Weapon, "ArmorPenetration".Translate() + extraLabelPart, num3.ToStringPercent(), "ArmorPenetrationExplanation".Translate(), 5400, null, null, false);
							}
						}
					}
					ThoughtDef thoughtDef = DefDatabase<ThoughtDef>.AllDefs.FirstOrDefault((ThoughtDef x) => x.hediff == CS$<>8__locals1.diff);
					if (thoughtDef != null && thoughtDef.stages != null && thoughtDef.stages.Any<ThoughtStage>())
					{
						yield return new StatDrawEntry(StatCategoryDefOf.Basics, "MoodChange".Translate() + extraLabelPart, thoughtDef.stages.First<ThoughtStage>().baseMoodEffect.ToStringByStyle(ToStringStyle.Integer, ToStringNumberSense.Offset), "Stat_Thing_MoodChange_Desc".Translate(), 3500, null, null, false);
					}
					CS$<>8__locals1 = null;
					extraLabelPart = null;
				}
				IEnumerator<RecipeDef> enumerator2 = null;
			}
			int num4;
			for (int i = 0; i < this.comps.Count; i = num4 + 1)
			{
				foreach (StatDrawEntry statDrawEntry8 in this.comps[i].SpecialDisplayStats(req))
				{
					yield return statDrawEntry8;
				}
				enumerator = null;
				num4 = i;
			}
			if (this.building != null)
			{
				if (this.building.mineableThing != null)
				{
					Dialog_InfoCard.Hyperlink[] hyperlinks = new Dialog_InfoCard.Hyperlink[]
					{
						new Dialog_InfoCard.Hyperlink(this.building.mineableThing, -1)
					};
					yield return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, "Stat_MineableThing_Name".Translate(), this.building.mineableThing.LabelCap, "Stat_MineableThing_Desc".Translate(), 2200, null, hyperlinks, false);
					StringBuilder stringBuilder4 = new StringBuilder();
					stringBuilder4.AppendLine("Stat_MiningYield_Desc".Translate());
					stringBuilder4.AppendLine();
					stringBuilder4.AppendLine("StatsReport_DifficultyMultiplier".Translate(Find.Storyteller.difficultyDef.label) + ": " + Find.Storyteller.difficulty.mineYieldFactor.ToStringByStyle(ToStringStyle.PercentZero, ToStringNumberSense.Factor));
					yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Stat_MiningYield_Name".Translate(), Mathf.CeilToInt((float)this.building.EffectiveMineableYield).ToString("F0"), stringBuilder4.ToString(), 2200, null, hyperlinks, false);
					hyperlinks = null;
				}
				if (this.building.IsTurret)
				{
					ThingDef turret = this.building.turretGunDef;
					yield return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, "Stat_Weapon_Name".Translate(), turret.LabelCap, "Stat_Weapon_Desc".Translate(), 5389, null, new Dialog_InfoCard.Hyperlink[]
					{
						new Dialog_InfoCard.Hyperlink(turret, -1)
					}, false);
					StatRequest request = StatRequest.For(turret, null, QualityCategory.Normal);
					foreach (StatDrawEntry statDrawEntry9 in turret.SpecialDisplayStats(request))
					{
						if (statDrawEntry9.category == StatCategoryDefOf.Weapon)
						{
							yield return statDrawEntry9;
						}
					}
					enumerator = null;
					for (int i = 0; i < turret.statBases.Count; i = num4 + 1)
					{
						StatModifier statModifier = turret.statBases[i];
						if (statModifier.stat.category == StatCategoryDefOf.Weapon)
						{
							yield return new StatDrawEntry(StatCategoryDefOf.Weapon, statModifier.stat, statModifier.value, request, ToStringNumberSense.Undefined, null, false);
						}
						num4 = i;
					}
					turret = null;
					request = default(StatRequest);
				}
			}
			if (this.IsMeat)
			{
				List<ThingDef> list = new List<ThingDef>();
				foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
				{
					if (thingDef.race != null && thingDef.race.meatDef == this)
					{
						list.Add(thingDef);
					}
				}
				string valueString2 = string.Join(", ", (from p in list
				select p.label).ToArray<string>()).CapitalizeFirst();
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "Stat_SourceSpecies_Name".Translate(), valueString2, "Stat_SourceSpecies_Desc".Translate(), 1200, null, Dialog_InfoCard.DefsToHyperlinks(list), false);
			}
			if (this.IsLeather)
			{
				List<ThingDef> list2 = new List<ThingDef>();
				foreach (ThingDef thingDef2 in DefDatabase<ThingDef>.AllDefs)
				{
					if (thingDef2.race != null && thingDef2.race.leatherDef == this)
					{
						list2.Add(thingDef2);
					}
				}
				string valueString3 = string.Join(", ", (from p in list2
				select p.label).ToArray<string>()).CapitalizeFirst();
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "Stat_SourceSpecies_Name".Translate(), valueString3, "Stat_SourceSpecies_Desc".Translate(), 1200, null, Dialog_InfoCard.DefsToHyperlinks(list2), false);
			}
			if (!this.equippedStatOffsets.NullOrEmpty<StatModifier>())
			{
				for (int i = 0; i < this.equippedStatOffsets.Count; i = num4 + 1)
				{
					StatDef stat = this.equippedStatOffsets[i].stat;
					float num5 = this.equippedStatOffsets[i].value;
					StringBuilder stringBuilder5 = new StringBuilder(stat.description);
					if (req.HasThing && stat.Worker != null)
					{
						stringBuilder5.AppendLine();
						stringBuilder5.AppendLine();
						stringBuilder5.AppendLine("StatsReport_BaseValue".Translate() + ": " + stat.ValueToString(num5, ToStringNumberSense.Offset, stat.finalizeEquippedStatOffset));
						num5 = StatWorker.StatOffsetFromGear(req.Thing, stat);
						if (!stat.parts.NullOrEmpty<StatPart>())
						{
							stringBuilder5.AppendLine();
							for (int j = 0; j < stat.parts.Count; j++)
							{
								string text = stat.parts[j].ExplanationPart(req);
								if (!text.NullOrEmpty())
								{
									stringBuilder5.AppendLine(text);
								}
							}
						}
						stringBuilder5.AppendLine();
						stringBuilder5.AppendLine("StatsReport_FinalValue".Translate() + ": " + stat.ValueToString(num5, ToStringNumberSense.Offset, !stat.formatString.NullOrEmpty()));
					}
					yield return new StatDrawEntry(StatCategoryDefOf.EquippedStatOffsets, this.equippedStatOffsets[i].stat, num5, StatRequest.ForEmpty(), ToStringNumberSense.Offset, null, true).SetReportText(stringBuilder5.ToString());
					num4 = i;
				}
			}
			if (this.IsDrug)
			{
				foreach (StatDrawEntry statDrawEntry10 in DrugStatsUtility.SpecialDisplayStats(this))
				{
					yield return statDrawEntry10;
				}
				enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x0400069D RID: 1693
		public Type thingClass;

		// Token: 0x0400069E RID: 1694
		public ThingCategory category;

		// Token: 0x0400069F RID: 1695
		public TickerType tickerType;

		// Token: 0x040006A0 RID: 1696
		public int stackLimit = 1;

		// Token: 0x040006A1 RID: 1697
		public IntVec2 size = new IntVec2(1, 1);

		// Token: 0x040006A2 RID: 1698
		public bool destroyable = true;

		// Token: 0x040006A3 RID: 1699
		public bool rotatable = true;

		// Token: 0x040006A4 RID: 1700
		public bool smallVolume;

		// Token: 0x040006A5 RID: 1701
		public bool useHitPoints = true;

		// Token: 0x040006A6 RID: 1702
		public bool receivesSignals;

		// Token: 0x040006A7 RID: 1703
		public List<CompProperties> comps = new List<CompProperties>();

		// Token: 0x040006A8 RID: 1704
		[NoTranslate]
		public string devNote;

		// Token: 0x040006A9 RID: 1705
		public List<ThingDefCountClass> killedLeavings;

		// Token: 0x040006AA RID: 1706
		public List<ThingDefCountClass> butcherProducts;

		// Token: 0x040006AB RID: 1707
		public List<ThingDefCountClass> smeltProducts;

		// Token: 0x040006AC RID: 1708
		public bool smeltable;

		// Token: 0x040006AD RID: 1709
		public bool burnableByRecipe;

		// Token: 0x040006AE RID: 1710
		public bool randomizeRotationOnSpawn;

		// Token: 0x040006AF RID: 1711
		public List<DamageMultiplier> damageMultipliers;

		// Token: 0x040006B0 RID: 1712
		public bool isTechHediff;

		// Token: 0x040006B1 RID: 1713
		public RecipeMakerProperties recipeMaker;

		// Token: 0x040006B2 RID: 1714
		public ThingDef minifiedDef;

		// Token: 0x040006B3 RID: 1715
		public bool isUnfinishedThing;

		// Token: 0x040006B4 RID: 1716
		public bool leaveResourcesWhenKilled;

		// Token: 0x040006B5 RID: 1717
		public ThingDef slagDef;

		// Token: 0x040006B6 RID: 1718
		public bool isFrameInt;

		// Token: 0x040006B7 RID: 1719
		public IntVec3 interactionCellOffset = IntVec3.Zero;

		// Token: 0x040006B8 RID: 1720
		public bool hasInteractionCell;

		// Token: 0x040006B9 RID: 1721
		public ThingDef interactionCellIcon;

		// Token: 0x040006BA RID: 1722
		public bool interactionCellIconReverse;

		// Token: 0x040006BB RID: 1723
		public ThingDef filthLeaving;

		// Token: 0x040006BC RID: 1724
		public bool forceDebugSpawnable;

		// Token: 0x040006BD RID: 1725
		public bool intricate;

		// Token: 0x040006BE RID: 1726
		public bool scatterableOnMapGen = true;

		// Token: 0x040006BF RID: 1727
		public float deepCommonality;

		// Token: 0x040006C0 RID: 1728
		public int deepCountPerCell = 300;

		// Token: 0x040006C1 RID: 1729
		public int deepCountPerPortion = -1;

		// Token: 0x040006C2 RID: 1730
		public IntRange deepLumpSizeRange = IntRange.zero;

		// Token: 0x040006C3 RID: 1731
		public float generateCommonality = 1f;

		// Token: 0x040006C4 RID: 1732
		public float generateAllowChance = 1f;

		// Token: 0x040006C5 RID: 1733
		private bool canOverlapZones = true;

		// Token: 0x040006C6 RID: 1734
		public FloatRange startingHpRange = FloatRange.One;

		// Token: 0x040006C7 RID: 1735
		[NoTranslate]
		public List<string> thingSetMakerTags;

		// Token: 0x040006C8 RID: 1736
		public bool alwaysFlee;

		// Token: 0x040006C9 RID: 1737
		public List<RecipeDef> recipes;

		// Token: 0x040006CA RID: 1738
		public bool messageOnDeteriorateInStorage = true;

		// Token: 0x040006CB RID: 1739
		public bool canLoadIntoCaravan = true;

		// Token: 0x040006CC RID: 1740
		public bool isMechClusterThreat;

		// Token: 0x040006CD RID: 1741
		public FloatRange displayNumbersBetweenSameDefDistRange = FloatRange.Zero;

		// Token: 0x040006CE RID: 1742
		public int minRewardCount = 1;

		// Token: 0x040006CF RID: 1743
		public bool preventSkyfallersLandingOn;

		// Token: 0x040006D0 RID: 1744
		public FactionDef requiresFactionToAcquire;

		// Token: 0x040006D1 RID: 1745
		public float relicChance;

		// Token: 0x040006D2 RID: 1746
		public OrderedTakeGroupDef orderedTakeGroup;

		// Token: 0x040006D3 RID: 1747
		public GraphicData graphicData;

		// Token: 0x040006D4 RID: 1748
		public DrawerType drawerType = DrawerType.RealtimeOnly;

		// Token: 0x040006D5 RID: 1749
		public bool drawOffscreen;

		// Token: 0x040006D6 RID: 1750
		public ColorGenerator colorGenerator;

		// Token: 0x040006D7 RID: 1751
		public float hideAtSnowDepth = 99999f;

		// Token: 0x040006D8 RID: 1752
		public bool drawDamagedOverlay = true;

		// Token: 0x040006D9 RID: 1753
		public bool castEdgeShadows;

		// Token: 0x040006DA RID: 1754
		public float staticSunShadowHeight;

		// Token: 0x040006DB RID: 1755
		public bool useSameGraphicForGhost;

		// Token: 0x040006DC RID: 1756
		public List<ThingStyleChance> randomStyle;

		// Token: 0x040006DD RID: 1757
		public float randomStyleChance;

		// Token: 0x040006DE RID: 1758
		public bool canEditAnyStyle;

		// Token: 0x040006DF RID: 1759
		public ThingDef defaultStuff;

		// Token: 0x040006E0 RID: 1760
		public bool selectable;

		// Token: 0x040006E1 RID: 1761
		public bool neverMultiSelect;

		// Token: 0x040006E2 RID: 1762
		public bool isAutoAttackableMapObject;

		// Token: 0x040006E3 RID: 1763
		public bool hasTooltip;

		// Token: 0x040006E4 RID: 1764
		public List<Type> inspectorTabs;

		// Token: 0x040006E5 RID: 1765
		[Unsaved(false)]
		public List<InspectTabBase> inspectorTabsResolved;

		// Token: 0x040006E6 RID: 1766
		public bool seeThroughFog;

		// Token: 0x040006E7 RID: 1767
		public bool drawGUIOverlay;

		// Token: 0x040006E8 RID: 1768
		public bool drawGUIOverlayQuality = true;

		// Token: 0x040006E9 RID: 1769
		public ResourceCountPriority resourceReadoutPriority;

		// Token: 0x040006EA RID: 1770
		public bool resourceReadoutAlwaysShow;

		// Token: 0x040006EB RID: 1771
		public bool drawPlaceWorkersWhileSelected;

		// Token: 0x040006EC RID: 1772
		public bool drawPlaceWorkersWhileInstallBlueprintSelected;

		// Token: 0x040006ED RID: 1773
		public ConceptDef storedConceptLearnOpportunity;

		// Token: 0x040006EE RID: 1774
		public float uiIconScale = 1f;

		// Token: 0x040006EF RID: 1775
		public bool hasCustomRectForSelector;

		// Token: 0x040006F0 RID: 1776
		public bool alwaysHaulable;

		// Token: 0x040006F1 RID: 1777
		public bool designateHaulable;

		// Token: 0x040006F2 RID: 1778
		public List<ThingCategoryDef> thingCategories;

		// Token: 0x040006F3 RID: 1779
		public bool mineable;

		// Token: 0x040006F4 RID: 1780
		public bool socialPropernessMatters;

		// Token: 0x040006F5 RID: 1781
		public bool stealable = true;

		// Token: 0x040006F6 RID: 1782
		public SoundDef soundDrop;

		// Token: 0x040006F7 RID: 1783
		public SoundDef soundPickup;

		// Token: 0x040006F8 RID: 1784
		public SoundDef soundInteract;

		// Token: 0x040006F9 RID: 1785
		public SoundDef soundImpactDefault;

		// Token: 0x040006FA RID: 1786
		public SoundDef soundPlayInstrument;

		// Token: 0x040006FB RID: 1787
		public bool saveCompressible;

		// Token: 0x040006FC RID: 1788
		public bool isSaveable = true;

		// Token: 0x040006FD RID: 1789
		public bool holdsRoof;

		// Token: 0x040006FE RID: 1790
		public float fillPercent;

		// Token: 0x040006FF RID: 1791
		public bool coversFloor;

		// Token: 0x04000700 RID: 1792
		public bool neverOverlapFloors;

		// Token: 0x04000701 RID: 1793
		public SurfaceType surfaceType;

		// Token: 0x04000702 RID: 1794
		public bool blockPlants;

		// Token: 0x04000703 RID: 1795
		public bool blockLight;

		// Token: 0x04000704 RID: 1796
		public bool blockWind;

		// Token: 0x04000705 RID: 1797
		public Tradeability tradeability = Tradeability.All;

		// Token: 0x04000706 RID: 1798
		[NoTranslate]
		public List<string> tradeTags;

		// Token: 0x04000707 RID: 1799
		public bool tradeNeverStack;

		// Token: 0x04000708 RID: 1800
		public bool healthAffectsPrice = true;

		// Token: 0x04000709 RID: 1801
		public ColorGenerator colorGeneratorInTraderStock;

		// Token: 0x0400070A RID: 1802
		private List<VerbProperties> verbs;

		// Token: 0x0400070B RID: 1803
		public List<Tool> tools;

		// Token: 0x0400070C RID: 1804
		public float equippedAngleOffset;

		// Token: 0x0400070D RID: 1805
		public EquipmentType equipmentType;

		// Token: 0x0400070E RID: 1806
		public TechLevel techLevel;

		// Token: 0x0400070F RID: 1807
		public List<WeaponClassDef> weaponClasses;

		// Token: 0x04000710 RID: 1808
		[NoTranslate]
		public List<string> weaponTags;

		// Token: 0x04000711 RID: 1809
		[NoTranslate]
		public List<string> techHediffsTags;

		// Token: 0x04000712 RID: 1810
		public bool violentTechHediff;

		// Token: 0x04000713 RID: 1811
		public bool destroyOnDrop;

		// Token: 0x04000714 RID: 1812
		public List<StatModifier> equippedStatOffsets;

		// Token: 0x04000715 RID: 1813
		public SoundDef meleeHitSound;

		// Token: 0x04000716 RID: 1814
		public float recoilPower = 1f;

		// Token: 0x04000717 RID: 1815
		public float recoilRelaxation = 10f;

		// Token: 0x04000718 RID: 1816
		public BuildableDef entityDefToBuild;

		// Token: 0x04000719 RID: 1817
		public ThingDef projectileWhenLoaded;

		// Token: 0x0400071A RID: 1818
		public RulePackDef ideoBuildingNamerBase;

		// Token: 0x0400071B RID: 1819
		public IngestibleProperties ingestible;

		// Token: 0x0400071C RID: 1820
		public FilthProperties filth;

		// Token: 0x0400071D RID: 1821
		public GasProperties gas;

		// Token: 0x0400071E RID: 1822
		public BuildingProperties building;

		// Token: 0x0400071F RID: 1823
		public RaceProperties race;

		// Token: 0x04000720 RID: 1824
		public ApparelProperties apparel;

		// Token: 0x04000721 RID: 1825
		public MoteProperties mote;

		// Token: 0x04000722 RID: 1826
		public PlantProperties plant;

		// Token: 0x04000723 RID: 1827
		public ProjectileProperties projectile;

		// Token: 0x04000724 RID: 1828
		public StuffProperties stuffProps;

		// Token: 0x04000725 RID: 1829
		public SkyfallerProperties skyfaller;

		// Token: 0x04000726 RID: 1830
		public PawnFlyerProperties pawnFlyer;

		// Token: 0x04000727 RID: 1831
		public RitualFocusProperties ritualFocus;

		// Token: 0x04000728 RID: 1832
		public IngredientProperties ingredient;

		// Token: 0x04000729 RID: 1833
		public bool canBeUsedUnderRoof = true;

		// Token: 0x0400072A RID: 1834
		[Unsaved(false)]
		private string descriptionDetailedCached;

		// Token: 0x0400072B RID: 1835
		[Unsaved(false)]
		public Graphic interactionCellGraphic;

		// Token: 0x0400072C RID: 1836
		[Unsaved(false)]
		private bool? isNaturalOrganCached;

		// Token: 0x0400072D RID: 1837
		public const int SmallUnitPerVolume = 10;

		// Token: 0x0400072E RID: 1838
		public const float SmallVolumePerUnit = 0.1f;

		// Token: 0x0400072F RID: 1839
		private List<RecipeDef> allRecipesCached;

		// Token: 0x04000730 RID: 1840
		private static List<VerbProperties> EmptyVerbPropertiesList = new List<VerbProperties>();

		// Token: 0x04000731 RID: 1841
		private Dictionary<ThingDef, Thing> concreteExamplesInt;
	}
}
