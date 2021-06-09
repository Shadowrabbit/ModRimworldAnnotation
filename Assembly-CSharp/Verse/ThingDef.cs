using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x0200019C RID: 412
	public class ThingDef : BuildableDef
	{
		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x06000A3C RID: 2620 RVA: 0x0000DF0A File Offset: 0x0000C10A
		public bool EverHaulable
		{
			get
			{
				return this.alwaysHaulable || this.designateHaulable;
			}
		}

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x06000A3D RID: 2621 RVA: 0x0000DF1C File Offset: 0x0000C11C
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

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x06000A3E RID: 2622 RVA: 0x0000DF31 File Offset: 0x0000C131
		public override IntVec2 Size
		{
			get
			{
				return this.size;
			}
		}

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x06000A3F RID: 2623 RVA: 0x0000DF39 File Offset: 0x0000C139
		public bool DiscardOnDestroyed
		{
			get
			{
				return this.race == null;
			}
		}

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x06000A40 RID: 2624 RVA: 0x0000DF44 File Offset: 0x0000C144
		public int BaseMaxHitPoints
		{
			get
			{
				return Mathf.RoundToInt(this.GetStatValueAbstract(StatDefOf.MaxHitPoints, null));
			}
		}

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x06000A41 RID: 2625 RVA: 0x0000DF57 File Offset: 0x0000C157
		public float BaseFlammability
		{
			get
			{
				return this.GetStatValueAbstract(StatDefOf.Flammability, null);
			}
		}

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x06000A42 RID: 2626 RVA: 0x0000DF65 File Offset: 0x0000C165
		// (set) Token: 0x06000A43 RID: 2627 RVA: 0x0000DF73 File Offset: 0x0000C173
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

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x06000A44 RID: 2628 RVA: 0x0000DF81 File Offset: 0x0000C181
		public float BaseMass
		{
			get
			{
				return this.GetStatValueAbstract(StatDefOf.Mass, null);
			}
		}

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x06000A45 RID: 2629 RVA: 0x0000DF8F File Offset: 0x0000C18F
		public bool PlayerAcquirable
		{
			get
			{
				return !this.destroyOnDrop;
			}
		}

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x06000A46 RID: 2630 RVA: 0x0009B66C File Offset: 0x0009986C
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

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x06000A47 RID: 2631 RVA: 0x0000DF9A File Offset: 0x0000C19A
		public bool Minifiable
		{
			get
			{
				return this.minifiedDef != null;
			}
		}

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x06000A48 RID: 2632 RVA: 0x0000DFA5 File Offset: 0x0000C1A5
		public bool HasThingIDNumber
		{
			get
			{
				return this.category != ThingCategory.Mote;
			}
		}

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x06000A49 RID: 2633 RVA: 0x0009B6B0 File Offset: 0x000998B0
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

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x06000A4A RID: 2634 RVA: 0x0009B75C File Offset: 0x0009995C
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

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x06000A4B RID: 2635 RVA: 0x0000DFB4 File Offset: 0x0000C1B4
		public bool CoexistsWithFloors
		{
			get
			{
				return !this.neverOverlapFloors && !this.coversFloor;
			}
		}

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x06000A4C RID: 2636 RVA: 0x0000DFC9 File Offset: 0x0000C1C9
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

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x06000A4D RID: 2637 RVA: 0x0000DFEA File Offset: 0x0000C1EA
		public bool MakeFog
		{
			get
			{
				return this.Fillage == FillCategory.Full;
			}
		}

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x06000A4E RID: 2638 RVA: 0x0009B7D4 File Offset: 0x000999D4
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

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x06000A4F RID: 2639 RVA: 0x0000DFF5 File Offset: 0x0000C1F5
		public bool CountAsResource
		{
			get
			{
				return this.resourceReadoutPriority > ResourceCountPriority.Uncounted;
			}
		}

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x06000A50 RID: 2640 RVA: 0x0000E000 File Offset: 0x0000C200
		[Obsolete("Will be removed in a future version.")]
		public bool BlockPlanting
		{
			get
			{
				return this.BlocksPlanting(false);
			}
		}

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x06000A51 RID: 2641 RVA: 0x0000E009 File Offset: 0x0000C209
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

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x06000A52 RID: 2642 RVA: 0x0009B864 File Offset: 0x00099A64
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

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x06000A53 RID: 2643 RVA: 0x0000E01F File Offset: 0x0000C21F
		public bool Claimable
		{
			get
			{
				return this.building != null && this.building.claimable && !this.building.isNaturalRock;
			}
		}

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x06000A54 RID: 2644 RVA: 0x0000E046 File Offset: 0x0000C246
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

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x06000A55 RID: 2645 RVA: 0x0000E063 File Offset: 0x0000C263
		public float MedicineTendXpGainFactor
		{
			get
			{
				return Mathf.Clamp(this.GetStatValueAbstract(StatDefOf.MedicalPotency, null) * 0.7f, 0.5f, 1f);
			}
		}

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x06000A56 RID: 2646 RVA: 0x0000E086 File Offset: 0x0000C286
		public bool CanEverDeteriorate
		{
			get
			{
				return this.useHitPoints && (this.category == ThingCategory.Item || this == ThingDefOf.BurnedTree);
			}
		}

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x06000A57 RID: 2647 RVA: 0x0000E0A5 File Offset: 0x0000C2A5
		public bool CanInteractThroughCorners
		{
			get
			{
				return this.category == ThingCategory.Building && this.holdsRoof && (this.building == null || !this.building.isNaturalRock || this.IsSmoothed);
			}
		}

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x06000A58 RID: 2648 RVA: 0x0000E0DC File Offset: 0x0000C2DC
		public bool AffectsRegions
		{
			get
			{
				return this.passability == Traversability.Impassable || this.IsDoor;
			}
		}

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x06000A59 RID: 2649 RVA: 0x0000E0EF File Offset: 0x0000C2EF
		public bool AffectsReachability
		{
			get
			{
				return this.AffectsRegions || (this.passability == Traversability.Impassable || this.IsDoor) || TouchPathEndModeUtility.MakesOccupiedCellsAlwaysReachableDiagonally(this);
			}
		}

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x06000A5A RID: 2650 RVA: 0x0009B89C File Offset: 0x00099A9C
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

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x06000A5B RID: 2651 RVA: 0x0000E119 File Offset: 0x0000C319
		public bool IsApparel
		{
			get
			{
				return this.apparel != null;
			}
		}

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x06000A5C RID: 2652 RVA: 0x0000E124 File Offset: 0x0000C324
		public bool IsBed
		{
			get
			{
				return typeof(Building_Bed).IsAssignableFrom(this.thingClass);
			}
		}

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x06000A5D RID: 2653 RVA: 0x0000E13B File Offset: 0x0000C33B
		public bool IsCorpse
		{
			get
			{
				return typeof(Corpse).IsAssignableFrom(this.thingClass);
			}
		}

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x06000A5E RID: 2654 RVA: 0x0000E152 File Offset: 0x0000C352
		public bool IsFrame
		{
			get
			{
				return this.isFrameInt;
			}
		}

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x06000A5F RID: 2655 RVA: 0x0000E15A File Offset: 0x0000C35A
		public bool IsBlueprint
		{
			get
			{
				return this.entityDefToBuild != null && this.category == ThingCategory.Ethereal;
			}
		}

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x06000A60 RID: 2656 RVA: 0x0000E170 File Offset: 0x0000C370
		public bool IsStuff
		{
			get
			{
				return this.stuffProps != null;
			}
		}

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x06000A61 RID: 2657 RVA: 0x0000E17B File Offset: 0x0000C37B
		public bool IsMedicine
		{
			get
			{
				return this.statBases.StatListContains(StatDefOf.MedicalPotency);
			}
		}

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x06000A62 RID: 2658 RVA: 0x0000E18D File Offset: 0x0000C38D
		public bool IsDoor
		{
			get
			{
				return typeof(Building_Door).IsAssignableFrom(this.thingClass);
			}
		}

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x06000A63 RID: 2659 RVA: 0x0000E1A4 File Offset: 0x0000C3A4
		public bool IsFilth
		{
			get
			{
				return this.filth != null;
			}
		}

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x06000A64 RID: 2660 RVA: 0x0000E1AF File Offset: 0x0000C3AF
		public bool IsIngestible
		{
			get
			{
				return this.ingestible != null;
			}
		}

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x06000A65 RID: 2661 RVA: 0x0000E1BA File Offset: 0x0000C3BA
		public bool IsNutritionGivingIngestible
		{
			get
			{
				return this.IsIngestible && this.ingestible.CachedNutrition > 0f;
			}
		}

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x06000A66 RID: 2662 RVA: 0x0000E1D8 File Offset: 0x0000C3D8
		public bool IsWeapon
		{
			get
			{
				return this.category == ThingCategory.Item && (!this.verbs.NullOrEmpty<VerbProperties>() || !this.tools.NullOrEmpty<Tool>()) && !this.IsApparel;
			}
		}

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x06000A67 RID: 2663 RVA: 0x0000E208 File Offset: 0x0000C408
		public bool IsCommsConsole
		{
			get
			{
				return typeof(Building_CommsConsole).IsAssignableFrom(this.thingClass);
			}
		}

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x06000A68 RID: 2664 RVA: 0x0000E21F File Offset: 0x0000C41F
		public bool IsOrbitalTradeBeacon
		{
			get
			{
				return typeof(Building_OrbitalTradeBeacon).IsAssignableFrom(this.thingClass);
			}
		}

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x06000A69 RID: 2665 RVA: 0x0000E236 File Offset: 0x0000C436
		public bool IsFoodDispenser
		{
			get
			{
				return typeof(Building_NutrientPasteDispenser).IsAssignableFrom(this.thingClass);
			}
		}

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x06000A6A RID: 2666 RVA: 0x0000E24D File Offset: 0x0000C44D
		public bool IsDrug
		{
			get
			{
				return this.ingestible != null && this.ingestible.drugCategory > DrugCategory.None;
			}
		}

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x06000A6B RID: 2667 RVA: 0x0000E267 File Offset: 0x0000C467
		public bool IsPleasureDrug
		{
			get
			{
				return this.IsDrug && this.ingestible.joy > 0f;
			}
		}

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x06000A6C RID: 2668 RVA: 0x0000E285 File Offset: 0x0000C485
		public bool IsNonMedicalDrug
		{
			get
			{
				return this.IsDrug && this.ingestible.drugCategory != DrugCategory.Medical;
			}
		}

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x06000A6D RID: 2669 RVA: 0x0000E2A2 File Offset: 0x0000C4A2
		public bool IsTable
		{
			get
			{
				return this.surfaceType == SurfaceType.Eat && this.HasComp(typeof(CompGatherSpot));
			}
		}

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x06000A6E RID: 2670 RVA: 0x0000E2BF File Offset: 0x0000C4BF
		public bool IsWorkTable
		{
			get
			{
				return typeof(Building_WorkTable).IsAssignableFrom(this.thingClass);
			}
		}

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x06000A6F RID: 2671 RVA: 0x0000E2D6 File Offset: 0x0000C4D6
		public bool IsShell
		{
			get
			{
				return this.projectileWhenLoaded != null;
			}
		}

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x06000A70 RID: 2672 RVA: 0x0000E2E1 File Offset: 0x0000C4E1
		public bool IsArt
		{
			get
			{
				return this.IsWithinCategory(ThingCategoryDefOf.BuildingsArt);
			}
		}

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x06000A71 RID: 2673 RVA: 0x0000E2EE File Offset: 0x0000C4EE
		public bool IsSmoothable
		{
			get
			{
				return this.building != null && this.building.smoothedThing != null;
			}
		}

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x06000A72 RID: 2674 RVA: 0x0000E308 File Offset: 0x0000C508
		public bool IsSmoothed
		{
			get
			{
				return this.building != null && this.building.unsmoothedThing != null;
			}
		}

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x06000A73 RID: 2675 RVA: 0x0000E322 File Offset: 0x0000C522
		public bool IsMetal
		{
			get
			{
				return this.stuffProps != null && this.stuffProps.categories.Contains(StuffCategoryDefOf.Metallic);
			}
		}

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x06000A74 RID: 2676 RVA: 0x0009B9C8 File Offset: 0x00099BC8
		public bool IsAddictiveDrug
		{
			get
			{
				CompProperties_Drug compProperties = this.GetCompProperties<CompProperties_Drug>();
				return compProperties != null && compProperties.addictiveness > 0f;
			}
		}

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x06000A75 RID: 2677 RVA: 0x0000E343 File Offset: 0x0000C543
		public bool IsMeat
		{
			get
			{
				return this.category == ThingCategory.Item && this.thingCategories != null && this.thingCategories.Contains(ThingCategoryDefOf.MeatRaw);
			}
		}

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x06000A76 RID: 2678 RVA: 0x0000E368 File Offset: 0x0000C568
		public bool IsLeather
		{
			get
			{
				return this.category == ThingCategory.Item && this.thingCategories != null && this.thingCategories.Contains(ThingCategoryDefOf.Leathers);
			}
		}

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x06000A77 RID: 2679 RVA: 0x0009B9F0 File Offset: 0x00099BF0
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

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x06000A78 RID: 2680 RVA: 0x0000E38D File Offset: 0x0000C58D
		public bool IsMeleeWeapon
		{
			get
			{
				return this.IsWeapon && !this.IsRangedWeapon;
			}
		}

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x06000A79 RID: 2681 RVA: 0x0009BA40 File Offset: 0x00099C40
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

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x06000A7A RID: 2682 RVA: 0x0000E3A2 File Offset: 0x0000C5A2
		public bool IsBuildingArtificial
		{
			get
			{
				return (this.category == ThingCategory.Building || this.IsFrame) && (this.building == null || (!this.building.isNaturalRock && !this.building.isResourceRock));
			}
		}

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x06000A7B RID: 2683 RVA: 0x0000E3DE File Offset: 0x0000C5DE
		public bool IsNonResourceNaturalRock
		{
			get
			{
				return this.category == ThingCategory.Building && this.building.isNaturalRock && !this.building.isResourceRock && !this.IsSmoothed;
			}
		}

		// Token: 0x06000A7C RID: 2684 RVA: 0x0009BA90 File Offset: 0x00099C90
		public bool BlocksPlanting(bool canWipePlants = false)
		{
			return (this.building == null || !this.building.SupportsPlants) && (this.blockPlants || (!canWipePlants && this.category == ThingCategory.Plant) || this.Fillage > FillCategory.None || this.IsEdifice());
		}

		// Token: 0x06000A7D RID: 2685 RVA: 0x0009BAE4 File Offset: 0x00099CE4
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

		// Token: 0x06000A7E RID: 2686 RVA: 0x0009BB30 File Offset: 0x00099D30
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

		// Token: 0x06000A7F RID: 2687 RVA: 0x0000E40E File Offset: 0x0000C60E
		public CompProperties CompDefFor<T>() where T : ThingComp
		{
			return this.comps.FirstOrDefault((CompProperties c) => c.compClass == typeof(T));
		}

		// Token: 0x06000A80 RID: 2688 RVA: 0x0000E43A File Offset: 0x0000C63A
		public CompProperties CompDefForAssignableFrom<T>() where T : ThingComp
		{
			return this.comps.FirstOrDefault((CompProperties c) => typeof(T).IsAssignableFrom(c.compClass));
		}

		// Token: 0x06000A81 RID: 2689 RVA: 0x0009BBCC File Offset: 0x00099DCC
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

		// Token: 0x06000A82 RID: 2690 RVA: 0x0009BC0C File Offset: 0x00099E0C
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

		// Token: 0x06000A83 RID: 2691 RVA: 0x0009BC4C File Offset: 0x00099E4C
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

		// Token: 0x06000A84 RID: 2692 RVA: 0x0009BC9C File Offset: 0x00099E9C
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

		// Token: 0x06000A85 RID: 2693 RVA: 0x0009BDE8 File Offset: 0x00099FE8
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

		// Token: 0x06000A86 RID: 2694 RVA: 0x0009BF18 File Offset: 0x0009A118
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
						}), false);
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

		// Token: 0x06000A87 RID: 2695 RVA: 0x0000E466 File Offset: 0x0000C666
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.label.NullOrEmpty())
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
			if (this.comps.Any((CompProperties c) => c.compClass == typeof(CompPowerTrader)) && this.drawerType == DrawerType.MapMeshOnly)
			{
				yield return "has PowerTrader comp but does not draw real time. It won't draw a needs-power overlay.";
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
			if (this.destroyOnDrop)
			{
				if (!this.menuHidden)
				{
					yield return "destroyOnDrop but not menuHidden.";
				}
				if (this.tradeability != Tradeability.None)
				{
					yield return "destroyOnDrop but tradeability is " + this.tradeability;
				}
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
			if (this.drawerType == DrawerType.MapMeshOnly)
			{
				if (this.comps.Any((CompProperties c) => c.compClass == typeof(CompForbiddable)))
				{
					yield return "drawerType=MapMeshOnly but has a CompForbiddable, which must draw in real time.";
				}
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
						goto IL_A96;
					}
				}
				yield return "is haulable, but does not have an authored mass value";
			}
			IL_A96:
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
				foreach (string text7 in this.race.ConfigErrors())
				{
					yield return text7;
				}
				enumerator = null;
			}
			if (this.race != null && this.tools != null)
			{
				ThingDef.<>c__DisplayClass271_3 CS$<>8__locals3 = new ThingDef.<>c__DisplayClass271_3();
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
			yield break;
			yield break;
		}

		// Token: 0x06000A88 RID: 2696 RVA: 0x0000E476 File Offset: 0x0000C676
		public static ThingDef Named(string defName)
		{
			return DefDatabase<ThingDef>.GetNamed(defName, true);
		}

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x06000A89 RID: 2697 RVA: 0x0000E47F File Offset: 0x0000C67F
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

		// Token: 0x06000A8A RID: 2698 RVA: 0x0009C0BC File Offset: 0x0009A2BC
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

		// Token: 0x06000A8B RID: 2699 RVA: 0x0000E4A5 File Offset: 0x0000C6A5
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
				string valueString = array.ToCommaList(false).CapitalizeFirst();
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
				if (verb.forcedMissRadius > 0f)
				{
					yield return new StatDrawEntry(verbStatCategory, "MissRadius".Translate(), verb.forcedMissRadius.ToString("0.#"), "Stat_Thing_Weapon_MissRadius_Desc".Translate(), 3557, null, null, false);
					yield return new StatDrawEntry(verbStatCategory, "DirectHitChance".Translate(), (1f / (float)GenRadial.NumCellsInRadius(verb.forcedMissRadius)).ToStringPercent(), "Stat_Thing_Weapon_DirectHitChance_Desc".Translate(), 3560, null, null, false);
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
					ThingDef.<>c__DisplayClass276_0 CS$<>8__locals1 = new ThingDef.<>c__DisplayClass276_0();
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
					stringBuilder4.AppendLine("StatsReport_DifficultyMultiplier".Translate(Find.Storyteller.difficulty.label) + ": " + Find.Storyteller.difficultyValues.mineYieldFactor.ToStringByStyle(ToStringStyle.PercentZero, ToStringNumberSense.Factor));
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

		// Token: 0x040008F0 RID: 2288
		public Type thingClass;

		// Token: 0x040008F1 RID: 2289
		public ThingCategory category;

		// Token: 0x040008F2 RID: 2290
		public TickerType tickerType;

		// Token: 0x040008F3 RID: 2291
		public int stackLimit = 1;

		// Token: 0x040008F4 RID: 2292
		public IntVec2 size = new IntVec2(1, 1);

		// Token: 0x040008F5 RID: 2293
		public bool destroyable = true;

		// Token: 0x040008F6 RID: 2294
		public bool rotatable = true;

		// Token: 0x040008F7 RID: 2295
		public bool smallVolume;

		// Token: 0x040008F8 RID: 2296
		public bool useHitPoints = true;

		// Token: 0x040008F9 RID: 2297
		public bool receivesSignals;

		// Token: 0x040008FA RID: 2298
		public List<CompProperties> comps = new List<CompProperties>();

		// Token: 0x040008FB RID: 2299
		public List<ThingDefCountClass> killedLeavings;

		// Token: 0x040008FC RID: 2300
		public List<ThingDefCountClass> butcherProducts;

		// Token: 0x040008FD RID: 2301
		public List<ThingDefCountClass> smeltProducts;

		// Token: 0x040008FE RID: 2302
		public bool smeltable;

		// Token: 0x040008FF RID: 2303
		public bool burnableByRecipe;

		// Token: 0x04000900 RID: 2304
		public bool randomizeRotationOnSpawn;

		// Token: 0x04000901 RID: 2305
		public List<DamageMultiplier> damageMultipliers;

		// Token: 0x04000902 RID: 2306
		public bool isTechHediff;

		// Token: 0x04000903 RID: 2307
		public RecipeMakerProperties recipeMaker;

		// Token: 0x04000904 RID: 2308
		public ThingDef minifiedDef;

		// Token: 0x04000905 RID: 2309
		public bool isUnfinishedThing;

		// Token: 0x04000906 RID: 2310
		public bool leaveResourcesWhenKilled;

		// Token: 0x04000907 RID: 2311
		public ThingDef slagDef;

		// Token: 0x04000908 RID: 2312
		public bool isFrameInt;

		// Token: 0x04000909 RID: 2313
		public IntVec3 interactionCellOffset = IntVec3.Zero;

		// Token: 0x0400090A RID: 2314
		public bool hasInteractionCell;

		// Token: 0x0400090B RID: 2315
		public ThingDef interactionCellIcon;

		// Token: 0x0400090C RID: 2316
		public bool interactionCellIconReverse;

		// Token: 0x0400090D RID: 2317
		public ThingDef filthLeaving;

		// Token: 0x0400090E RID: 2318
		public bool forceDebugSpawnable;

		// Token: 0x0400090F RID: 2319
		public bool intricate;

		// Token: 0x04000910 RID: 2320
		public bool scatterableOnMapGen = true;

		// Token: 0x04000911 RID: 2321
		public float deepCommonality;

		// Token: 0x04000912 RID: 2322
		public int deepCountPerCell = 300;

		// Token: 0x04000913 RID: 2323
		public int deepCountPerPortion = -1;

		// Token: 0x04000914 RID: 2324
		public IntRange deepLumpSizeRange = IntRange.zero;

		// Token: 0x04000915 RID: 2325
		public float generateCommonality = 1f;

		// Token: 0x04000916 RID: 2326
		public float generateAllowChance = 1f;

		// Token: 0x04000917 RID: 2327
		private bool canOverlapZones = true;

		// Token: 0x04000918 RID: 2328
		public FloatRange startingHpRange = FloatRange.One;

		// Token: 0x04000919 RID: 2329
		[NoTranslate]
		public List<string> thingSetMakerTags;

		// Token: 0x0400091A RID: 2330
		public bool alwaysFlee;

		// Token: 0x0400091B RID: 2331
		public List<RecipeDef> recipes;

		// Token: 0x0400091C RID: 2332
		public bool messageOnDeteriorateInStorage = true;

		// Token: 0x0400091D RID: 2333
		public bool canLoadIntoCaravan = true;

		// Token: 0x0400091E RID: 2334
		public bool isMechClusterThreat;

		// Token: 0x0400091F RID: 2335
		public FloatRange displayNumbersBetweenSameDefDistRange = FloatRange.Zero;

		// Token: 0x04000920 RID: 2336
		public int minRewardCount = 1;

		// Token: 0x04000921 RID: 2337
		public bool preventSkyfallersLandingOn;

		// Token: 0x04000922 RID: 2338
		public GraphicData graphicData;

		// Token: 0x04000923 RID: 2339
		public DrawerType drawerType = DrawerType.RealtimeOnly;

		// Token: 0x04000924 RID: 2340
		public bool drawOffscreen;

		// Token: 0x04000925 RID: 2341
		public ColorGenerator colorGenerator;

		// Token: 0x04000926 RID: 2342
		public float hideAtSnowDepth = 99999f;

		// Token: 0x04000927 RID: 2343
		public bool drawDamagedOverlay = true;

		// Token: 0x04000928 RID: 2344
		public bool castEdgeShadows;

		// Token: 0x04000929 RID: 2345
		public float staticSunShadowHeight;

		// Token: 0x0400092A RID: 2346
		public bool useSameGraphicForGhost;

		// Token: 0x0400092B RID: 2347
		public bool selectable;

		// Token: 0x0400092C RID: 2348
		public bool neverMultiSelect;

		// Token: 0x0400092D RID: 2349
		public bool isAutoAttackableMapObject;

		// Token: 0x0400092E RID: 2350
		public bool hasTooltip;

		// Token: 0x0400092F RID: 2351
		public List<Type> inspectorTabs;

		// Token: 0x04000930 RID: 2352
		[Unsaved(false)]
		public List<InspectTabBase> inspectorTabsResolved;

		// Token: 0x04000931 RID: 2353
		public bool seeThroughFog;

		// Token: 0x04000932 RID: 2354
		public bool drawGUIOverlay;

		// Token: 0x04000933 RID: 2355
		public bool drawGUIOverlayQuality = true;

		// Token: 0x04000934 RID: 2356
		public ResourceCountPriority resourceReadoutPriority;

		// Token: 0x04000935 RID: 2357
		public bool resourceReadoutAlwaysShow;

		// Token: 0x04000936 RID: 2358
		public bool drawPlaceWorkersWhileSelected;

		// Token: 0x04000937 RID: 2359
		public bool drawPlaceWorkersWhileInstallBlueprintSelected;

		// Token: 0x04000938 RID: 2360
		public ConceptDef storedConceptLearnOpportunity;

		// Token: 0x04000939 RID: 2361
		public float uiIconScale = 1f;

		// Token: 0x0400093A RID: 2362
		public bool hasCustomRectForSelector;

		// Token: 0x0400093B RID: 2363
		public bool alwaysHaulable;

		// Token: 0x0400093C RID: 2364
		public bool designateHaulable;

		// Token: 0x0400093D RID: 2365
		public List<ThingCategoryDef> thingCategories;

		// Token: 0x0400093E RID: 2366
		public bool mineable;

		// Token: 0x0400093F RID: 2367
		public bool socialPropernessMatters;

		// Token: 0x04000940 RID: 2368
		public bool stealable = true;

		// Token: 0x04000941 RID: 2369
		public SoundDef soundDrop;

		// Token: 0x04000942 RID: 2370
		public SoundDef soundPickup;

		// Token: 0x04000943 RID: 2371
		public SoundDef soundInteract;

		// Token: 0x04000944 RID: 2372
		public SoundDef soundImpactDefault;

		// Token: 0x04000945 RID: 2373
		public SoundDef soundPlayInstrument;

		// Token: 0x04000946 RID: 2374
		public bool saveCompressible;

		// Token: 0x04000947 RID: 2375
		public bool isSaveable = true;

		// Token: 0x04000948 RID: 2376
		public bool holdsRoof;

		// Token: 0x04000949 RID: 2377
		public float fillPercent;

		// Token: 0x0400094A RID: 2378
		public bool coversFloor;

		// Token: 0x0400094B RID: 2379
		public bool neverOverlapFloors;

		// Token: 0x0400094C RID: 2380
		public SurfaceType surfaceType;

		// Token: 0x0400094D RID: 2381
		public bool blockPlants;

		// Token: 0x0400094E RID: 2382
		public bool blockLight;

		// Token: 0x0400094F RID: 2383
		public bool blockWind;

		// Token: 0x04000950 RID: 2384
		public Tradeability tradeability = Tradeability.All;

		// Token: 0x04000951 RID: 2385
		[NoTranslate]
		public List<string> tradeTags;

		// Token: 0x04000952 RID: 2386
		public bool tradeNeverStack;

		// Token: 0x04000953 RID: 2387
		public bool healthAffectsPrice = true;

		// Token: 0x04000954 RID: 2388
		public ColorGenerator colorGeneratorInTraderStock;

		// Token: 0x04000955 RID: 2389
		private List<VerbProperties> verbs;

		// Token: 0x04000956 RID: 2390
		public List<Tool> tools;

		// Token: 0x04000957 RID: 2391
		public float equippedAngleOffset;

		// Token: 0x04000958 RID: 2392
		public EquipmentType equipmentType;

		// Token: 0x04000959 RID: 2393
		public TechLevel techLevel;

		// Token: 0x0400095A RID: 2394
		[NoTranslate]
		public List<string> weaponTags;

		// Token: 0x0400095B RID: 2395
		[NoTranslate]
		public List<string> techHediffsTags;

		// Token: 0x0400095C RID: 2396
		public bool destroyOnDrop;

		// Token: 0x0400095D RID: 2397
		public List<StatModifier> equippedStatOffsets;

		// Token: 0x0400095E RID: 2398
		public SoundDef meleeHitSound;

		// Token: 0x0400095F RID: 2399
		public BuildableDef entityDefToBuild;

		// Token: 0x04000960 RID: 2400
		public ThingDef projectileWhenLoaded;

		// Token: 0x04000961 RID: 2401
		public IngestibleProperties ingestible;

		// Token: 0x04000962 RID: 2402
		public FilthProperties filth;

		// Token: 0x04000963 RID: 2403
		public GasProperties gas;

		// Token: 0x04000964 RID: 2404
		public BuildingProperties building;

		// Token: 0x04000965 RID: 2405
		public RaceProperties race;

		// Token: 0x04000966 RID: 2406
		public ApparelProperties apparel;

		// Token: 0x04000967 RID: 2407
		public MoteProperties mote;

		// Token: 0x04000968 RID: 2408
		public PlantProperties plant;

		// Token: 0x04000969 RID: 2409
		public ProjectileProperties projectile;

		// Token: 0x0400096A RID: 2410
		public StuffProperties stuffProps;

		// Token: 0x0400096B RID: 2411
		public SkyfallerProperties skyfaller;

		// Token: 0x0400096C RID: 2412
		public PawnFlyerProperties pawnFlyer;

		// Token: 0x0400096D RID: 2413
		public bool canBeUsedUnderRoof = true;

		// Token: 0x0400096E RID: 2414
		[Unsaved(false)]
		private string descriptionDetailedCached;

		// Token: 0x0400096F RID: 2415
		[Unsaved(false)]
		public Graphic interactionCellGraphic;

		// Token: 0x04000970 RID: 2416
		public const int SmallUnitPerVolume = 10;

		// Token: 0x04000971 RID: 2417
		public const float SmallVolumePerUnit = 0.1f;

		// Token: 0x04000972 RID: 2418
		private List<RecipeDef> allRecipesCached;

		// Token: 0x04000973 RID: 2419
		private static List<VerbProperties> EmptyVerbPropertiesList = new List<VerbProperties>();

		// Token: 0x04000974 RID: 2420
		private Dictionary<ThingDef, Thing> concreteExamplesInt;
	}
}
