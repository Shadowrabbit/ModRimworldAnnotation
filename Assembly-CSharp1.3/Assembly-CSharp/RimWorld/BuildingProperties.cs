using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020009EE RID: 2542
	public class BuildingProperties
	{
		// Token: 0x17000AEB RID: 2795
		// (get) Token: 0x06003EA2 RID: 16034 RVA: 0x00156AA7 File Offset: 0x00154CA7
		public bool SupportsPlants
		{
			get
			{
				return this.sowTag != null;
			}
		}

		// Token: 0x17000AEC RID: 2796
		// (get) Token: 0x06003EA3 RID: 16035 RVA: 0x00156AB2 File Offset: 0x00154CB2
		public int EffectiveMineableYield
		{
			get
			{
				return Mathf.RoundToInt((float)this.mineableYield * Find.Storyteller.difficulty.mineYieldFactor);
			}
		}

		// Token: 0x17000AED RID: 2797
		// (get) Token: 0x06003EA4 RID: 16036 RVA: 0x00156AD0 File Offset: 0x00154CD0
		public bool IsTurret
		{
			get
			{
				return this.turretGunDef != null;
			}
		}

		// Token: 0x17000AEE RID: 2798
		// (get) Token: 0x06003EA5 RID: 16037 RVA: 0x00156ADB File Offset: 0x00154CDB
		public bool IsDeconstructible
		{
			get
			{
				return this.alwaysDeconstructible || (!this.isNaturalRock && this.deconstructible);
			}
		}

		// Token: 0x17000AEF RID: 2799
		// (get) Token: 0x06003EA6 RID: 16038 RVA: 0x00156AF8 File Offset: 0x00154CF8
		public bool IsMortar
		{
			get
			{
				if (!this.IsTurret)
				{
					return false;
				}
				List<VerbProperties> verbs = this.turretGunDef.Verbs;
				for (int i = 0; i < verbs.Count; i++)
				{
					if (verbs[i].isPrimary && verbs[i].defaultProjectile != null && verbs[i].defaultProjectile.projectile.flyOverhead)
					{
						return true;
					}
				}
				if (this.turretGunDef.HasComp(typeof(CompChangeableProjectile)))
				{
					if (this.turretGunDef.building.fixedStorageSettings.filter.Allows(ThingDefOf.Shell_HighExplosive))
					{
						return true;
					}
					foreach (ThingDef thingDef in this.turretGunDef.building.fixedStorageSettings.filter.AllowedThingDefs)
					{
						if (thingDef.projectileWhenLoaded != null && thingDef.projectileWhenLoaded.projectile.flyOverhead)
						{
							return true;
						}
					}
					return false;
				}
				return false;
			}
		}

		// Token: 0x06003EA7 RID: 16039 RVA: 0x00156C0C File Offset: 0x00154E0C
		public IEnumerable<string> ConfigErrors(ThingDef parent)
		{
			if (this.isTrap && !this.isEdifice)
			{
				yield return "isTrap but is not edifice. Code will break.";
			}
			if (this.alwaysDeconstructible && !this.deconstructible)
			{
				yield return "alwaysDeconstructible=true but deconstructible=false";
			}
			if (parent.holdsRoof && !this.isEdifice)
			{
				yield return "holds roof but is not an edifice.";
			}
			if (this.buildingTags.Contains("MechClusterCombatThreat") && this.combatPower <= 0f)
			{
				yield return "has MechClusterCombatThreat tag but 0 combatPower and thus no points cost; this will make an infinite loop during mech cluster building selection";
			}
			if (parent.IsGibbetCage && this.gibbetCageTopGraphicData == null)
			{
				yield return "Gibbet cage has no graphic data for gibbet cage top set.";
			}
			yield break;
		}

		// Token: 0x06003EA8 RID: 16040 RVA: 0x0000313F File Offset: 0x0000133F
		public void PostLoadSpecial(ThingDef parent)
		{
		}

		// Token: 0x06003EA9 RID: 16041 RVA: 0x00156C24 File Offset: 0x00154E24
		public void ResolveReferencesSpecial()
		{
			if (this.soundDoorOpenPowered == null)
			{
				this.soundDoorOpenPowered = SoundDefOf.Door_OpenPowered;
			}
			if (this.soundDoorClosePowered == null)
			{
				this.soundDoorClosePowered = SoundDefOf.Door_ClosePowered;
			}
			if (this.soundDoorOpenManual == null)
			{
				this.soundDoorOpenManual = SoundDefOf.Door_OpenManual;
			}
			if (this.soundDoorCloseManual == null)
			{
				this.soundDoorCloseManual = SoundDefOf.Door_CloseManual;
			}
			if (this.turretGunDef != null)
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					this.turretTopMat = MaterialPool.MatFrom(this.turretGunDef.graphicData.texPath);
				});
			}
			if (this.fixedStorageSettings != null)
			{
				this.fixedStorageSettings.filter.ResolveReferences();
			}
			if (this.defaultStorageSettings == null && this.fixedStorageSettings != null)
			{
				this.defaultStorageSettings = new StorageSettings();
				this.defaultStorageSettings.CopyFrom(this.fixedStorageSettings);
			}
			if (this.defaultStorageSettings != null)
			{
				this.defaultStorageSettings.filter.ResolveReferences();
			}
		}

		// Token: 0x06003EAA RID: 16042 RVA: 0x00156CF4 File Offset: 0x00154EF4
		public static void FinalizeInit()
		{
			List<ThingDef> allDefsListForReading = DefDatabase<ThingDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				ThingDef thingDef = allDefsListForReading[i];
				if (thingDef.building != null && thingDef.building.smoothedThing != null)
				{
					ThingDef thingDef2 = thingDef.building.smoothedThing;
					if (thingDef2.building == null)
					{
						Log.Error(string.Format("{0} is smoothable to non-building {1}", thingDef, thingDef2));
					}
					else if (thingDef2.building.unsmoothedThing == null || thingDef2.building.unsmoothedThing == thingDef)
					{
						thingDef2.building.unsmoothedThing = thingDef;
					}
					else
					{
						Log.Error(string.Format("{0} and {1} both smooth to {2}", thingDef, thingDef2.building.unsmoothedThing, thingDef2));
					}
				}
			}
		}

		// Token: 0x06003EAB RID: 16043 RVA: 0x00156DA7 File Offset: 0x00154FA7
		public IEnumerable<StatDrawEntry> SpecialDisplayStats(ThingDef parentDef, StatRequest req)
		{
			if (this.joyKind != null)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("Stat_RecreationType_Desc".Translate());
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("Stat_JoyKind_AllTypes".Translate() + ":");
				foreach (JoyKindDef joyKindDef in DefDatabase<JoyKindDef>.AllDefs)
				{
					stringBuilder.AppendLine("  - " + joyKindDef.LabelCap);
				}
				yield return new StatDrawEntry(StatCategoryDefOf.Building, "StatsReport_JoyKind".Translate(), this.joyKind.LabelCap, stringBuilder.ToString(), 4750, this.joyKind.LabelCap, null, false);
			}
			if (parentDef.Minifiable)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Building, "StatsReport_WorkToUninstall".Translate(), this.uninstallWork.ToStringWorkAmount(), "Stat_Thing_WorkToUninstall_Desc".Translate(), 1102, null, null, false);
			}
			if (typeof(Building_TrapDamager).IsAssignableFrom(parentDef.thingClass))
			{
				float f = StatDefOf.TrapMeleeDamage.Worker.GetValue(req, true) * 0.015f;
				yield return new StatDrawEntry(StatCategoryDefOf.Building, "TrapArmorPenetration".Translate(), f.ToStringPercent(), "ArmorPenetrationExplanation".Translate(), 3000, null, null, false);
			}
			if (this.isFence)
			{
				TaggedString taggedString = "Stat_Thing_Fence_Desc".Translate();
				BuildingProperties.tmpFenceBlockedAnimals.Clear();
				BuildingProperties.tmpFenceBlockedAnimals.AddRange(from k in DefDatabase<PawnKindDef>.AllDefs
				where k.RaceProps.Animal && k.RaceProps.FenceBlocked
				select k into g
				select g.LabelCap.Resolve() into s
				orderby s
				select s);
				taggedString += ":\n\n";
				taggedString += BuildingProperties.tmpFenceBlockedAnimals.ToLineList("- ");
				yield return new StatDrawEntry(StatCategoryDefOf.Building, "StatsReport_Fence".Translate(), "Yes".Translate(), taggedString, 4800, null, null, false);
				BuildingProperties.tmpFenceBlockedAnimals.Clear();
			}
			if (ModsConfig.IdeologyActive)
			{
				foreach (Ideo ideo in Faction.OfPlayer.ideos.AllIdeos)
				{
					int num;
					for (int i = 0; i < ideo.PreceptsListForReading.Count; i = num + 1)
					{
						Precept_Building precept_Building;
						if ((precept_Building = (ideo.PreceptsListForReading[i] as Precept_Building)) != null && precept_Building.ThingDef == parentDef && precept_Building.presenceDemand != null)
						{
							IdeoBuildingPresenceDemand presenceDemand = precept_Building.presenceDemand;
							if (!presenceDemand.roomRequirements.NullOrEmpty<RoomRequirement>())
							{
								string valueString = (from r in presenceDemand.roomRequirements
								select r.Label(null)).ToCommaList(false, false).CapitalizeFirst();
								string reportText = (from r in presenceDemand.roomRequirements
								select r.LabelCap(null)).ToLineList("  - ", false);
								yield return new StatDrawEntry(StatCategoryDefOf.Building, "RoomRequirements".Translate(), valueString, reportText, 2101, null, null, false);
							}
						}
						num = i;
					}
					ideo = null;
				}
				IEnumerator<Ideo> enumerator2 = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x0400211B RID: 8475
		public bool isEdifice = true;

		// Token: 0x0400211C RID: 8476
		[NoTranslate]
		public List<string> buildingTags = new List<string>();

		// Token: 0x0400211D RID: 8477
		public bool isInert;

		// Token: 0x0400211E RID: 8478
		private bool deconstructible = true;

		// Token: 0x0400211F RID: 8479
		public bool alwaysDeconstructible;

		// Token: 0x04002120 RID: 8480
		public bool claimable = true;

		// Token: 0x04002121 RID: 8481
		public bool isSittable;

		// Token: 0x04002122 RID: 8482
		public bool multiSittable;

		// Token: 0x04002123 RID: 8483
		public bool sitIgnoreOrientation;

		// Token: 0x04002124 RID: 8484
		public SoundDef soundAmbient;

		// Token: 0x04002125 RID: 8485
		public ConceptDef spawnedConceptLearnOpportunity;

		// Token: 0x04002126 RID: 8486
		public ConceptDef boughtConceptLearnOpportunity;

		// Token: 0x04002127 RID: 8487
		public bool expandHomeArea = true;

		// Token: 0x04002128 RID: 8488
		public Type blueprintClass = typeof(Blueprint_Build);

		// Token: 0x04002129 RID: 8489
		public GraphicData blueprintGraphicData;

		// Token: 0x0400212A RID: 8490
		public float uninstallWork = 200f;

		// Token: 0x0400212B RID: 8491
		public bool forceShowRoomStats;

		// Token: 0x0400212C RID: 8492
		public bool wantsHopperAdjacent;

		// Token: 0x0400212D RID: 8493
		public bool allowWireConnection = true;

		// Token: 0x0400212E RID: 8494
		public bool shipPart;

		// Token: 0x0400212F RID: 8495
		public bool canPlaceOverImpassablePlant = true;

		// Token: 0x04002130 RID: 8496
		public float heatPerTickWhileWorking;

		// Token: 0x04002131 RID: 8497
		public bool canBuildNonEdificesUnder = true;

		// Token: 0x04002132 RID: 8498
		public bool canPlaceOverWall;

		// Token: 0x04002133 RID: 8499
		public bool isPlaceOverableWall;

		// Token: 0x04002134 RID: 8500
		public bool allowAutoroof = true;

		// Token: 0x04002135 RID: 8501
		public bool preventDeteriorationOnTop;

		// Token: 0x04002136 RID: 8502
		public bool preventDeteriorationInside;

		// Token: 0x04002137 RID: 8503
		public bool isMealSource;

		// Token: 0x04002138 RID: 8504
		public bool isNaturalRock;

		// Token: 0x04002139 RID: 8505
		public bool isResourceRock;

		// Token: 0x0400213A RID: 8506
		public bool repairable = true;

		// Token: 0x0400213B RID: 8507
		public float roofCollapseDamageMultiplier = 1f;

		// Token: 0x0400213C RID: 8508
		public bool hasFuelingPort;

		// Token: 0x0400213D RID: 8509
		public ThingDef smoothedThing;

		// Token: 0x0400213E RID: 8510
		[Unsaved(false)]
		public ThingDef unsmoothedThing;

		// Token: 0x0400213F RID: 8511
		public TerrainDef naturalTerrain;

		// Token: 0x04002140 RID: 8512
		public TerrainDef leaveTerrain;

		// Token: 0x04002141 RID: 8513
		public float combatPower;

		// Token: 0x04002142 RID: 8514
		public int minMechClusterPoints;

		// Token: 0x04002143 RID: 8515
		public float destroyShakeAmount = -1f;

		// Token: 0x04002144 RID: 8516
		public SoundDef destroySound;

		// Token: 0x04002145 RID: 8517
		public EffecterDef destroyEffecter;

		// Token: 0x04002146 RID: 8518
		public BuildingSizeCategory buildingSizeCategory;

		// Token: 0x04002147 RID: 8519
		public bool isFence;

		// Token: 0x04002148 RID: 8520
		public List<ThingDef> relatedBuildCommands;

		// Token: 0x04002149 RID: 8521
		public bool useIdeoColor;

		// Token: 0x0400214A RID: 8522
		public bool isPlayerEjectable;

		// Token: 0x0400214B RID: 8523
		public SoundDef openingStartedSound;

		// Token: 0x0400214C RID: 8524
		public GraphicData fullGraveGraphicData;

		// Token: 0x0400214D RID: 8525
		public float bed_healPerDay;

		// Token: 0x0400214E RID: 8526
		public bool bed_defaultMedical;

		// Token: 0x0400214F RID: 8527
		public bool bed_showSleeperBody;

		// Token: 0x04002150 RID: 8528
		public bool bed_humanlike = true;

		// Token: 0x04002151 RID: 8529
		public float bed_maxBodySize = 9999f;

		// Token: 0x04002152 RID: 8530
		public bool bed_caravansCanUse;

		// Token: 0x04002153 RID: 8531
		public bool bed_slabBed;

		// Token: 0x04002154 RID: 8532
		public float nutritionCostPerDispense;

		// Token: 0x04002155 RID: 8533
		public SoundDef soundDispense;

		// Token: 0x04002156 RID: 8534
		public ThingDef turretGunDef;

		// Token: 0x04002157 RID: 8535
		public float turretBurstWarmupTime;

		// Token: 0x04002158 RID: 8536
		public float turretBurstCooldownTime = -1f;

		// Token: 0x04002159 RID: 8537
		public float turretInitialCooldownTime;

		// Token: 0x0400215A RID: 8538
		[Unsaved(false)]
		public Material turretTopMat;

		// Token: 0x0400215B RID: 8539
		public float turretTopDrawSize = 2f;

		// Token: 0x0400215C RID: 8540
		public Vector2 turretTopOffset;

		// Token: 0x0400215D RID: 8541
		public bool ai_combatDangerous;

		// Token: 0x0400215E RID: 8542
		public bool ai_chillDestination = true;

		// Token: 0x0400215F RID: 8543
		public bool ai_neverTrashThis;

		// Token: 0x04002160 RID: 8544
		public bool roamerCanOpen;

		// Token: 0x04002161 RID: 8545
		public SoundDef soundDoorOpenPowered;

		// Token: 0x04002162 RID: 8546
		public SoundDef soundDoorClosePowered;

		// Token: 0x04002163 RID: 8547
		public SoundDef soundDoorOpenManual;

		// Token: 0x04002164 RID: 8548
		public SoundDef soundDoorCloseManual;

		// Token: 0x04002165 RID: 8549
		[NoTranslate]
		public string sowTag;

		// Token: 0x04002166 RID: 8550
		public ThingDef defaultPlantToGrow;

		// Token: 0x04002167 RID: 8551
		public ThingDef mineableThing;

		// Token: 0x04002168 RID: 8552
		public int mineableYield = 1;

		// Token: 0x04002169 RID: 8553
		public float mineableNonMinedEfficiency = 0.7f;

		// Token: 0x0400216A RID: 8554
		public float mineableDropChance = 1f;

		// Token: 0x0400216B RID: 8555
		public bool mineableYieldWasteable = true;

		// Token: 0x0400216C RID: 8556
		public float mineableScatterCommonality;

		// Token: 0x0400216D RID: 8557
		public IntRange mineableScatterLumpSizeRange = new IntRange(20, 40);

		// Token: 0x0400216E RID: 8558
		public StorageSettings fixedStorageSettings;

		// Token: 0x0400216F RID: 8559
		public StorageSettings defaultStorageSettings;

		// Token: 0x04002170 RID: 8560
		public bool ignoreStoredThingsBeauty;

		// Token: 0x04002171 RID: 8561
		public bool isTrap;

		// Token: 0x04002172 RID: 8562
		public bool trapDestroyOnSpring;

		// Token: 0x04002173 RID: 8563
		public float trapPeacefulWildAnimalsSpringChanceFactor = 1f;

		// Token: 0x04002174 RID: 8564
		public DamageArmorCategoryDef trapDamageCategory;

		// Token: 0x04002175 RID: 8565
		public GraphicData trapUnarmedGraphicData;

		// Token: 0x04002176 RID: 8566
		[Unsaved(false)]
		public Graphic trapUnarmedGraphic;

		// Token: 0x04002177 RID: 8567
		public float unpoweredWorkTableWorkSpeedFactor;

		// Token: 0x04002178 RID: 8568
		public IntRange watchBuildingStandDistanceRange = IntRange.one;

		// Token: 0x04002179 RID: 8569
		public int watchBuildingStandRectWidth = 3;

		// Token: 0x0400217A RID: 8570
		public bool watchBuildingInSameRoom;

		// Token: 0x0400217B RID: 8571
		public JoyKindDef joyKind;

		// Token: 0x0400217C RID: 8572
		public int haulToContainerDuration;

		// Token: 0x0400217D RID: 8573
		public float instrumentRange;

		// Token: 0x0400217E RID: 8574
		public int minDistanceToSameTypeOfBuilding;

		// Token: 0x0400217F RID: 8575
		public bool artificialForMeditationPurposes = true;

		// Token: 0x04002180 RID: 8576
		public EffecterDef effectWatching;

		// Token: 0x04002181 RID: 8577
		public GraphicData gibbetCageTopGraphicData;

		// Token: 0x04002182 RID: 8578
		public Vector3 gibbetCorposeDrawOffset;

		// Token: 0x04002183 RID: 8579
		public EffecterDef gibbetCagePlaceCorpseEffecter;

		// Token: 0x04002184 RID: 8580
		public EffecterDef openingEffect;

		// Token: 0x04002185 RID: 8581
		private static List<string> tmpFenceBlockedAnimals = new List<string>();
	}
}
