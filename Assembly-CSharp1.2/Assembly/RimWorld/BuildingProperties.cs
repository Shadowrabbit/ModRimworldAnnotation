using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EFA RID: 3834
	public class BuildingProperties
	{
		// Token: 0x17000CEE RID: 3310
		// (get) Token: 0x060054E8 RID: 21736 RVA: 0x0003ADDA File Offset: 0x00038FDA
		public bool SupportsPlants
		{
			get
			{
				return this.sowTag != null;
			}
		}

		// Token: 0x17000CEF RID: 3311
		// (get) Token: 0x060054E9 RID: 21737 RVA: 0x0003ADE5 File Offset: 0x00038FE5
		public int EffectiveMineableYield
		{
			get
			{
				return Mathf.RoundToInt((float)this.mineableYield * Find.Storyteller.difficultyValues.mineYieldFactor);
			}
		}

		// Token: 0x17000CF0 RID: 3312
		// (get) Token: 0x060054EA RID: 21738 RVA: 0x0003AE03 File Offset: 0x00039003
		public bool IsTurret
		{
			get
			{
				return this.turretGunDef != null;
			}
		}

		// Token: 0x17000CF1 RID: 3313
		// (get) Token: 0x060054EB RID: 21739 RVA: 0x0003AE0E File Offset: 0x0003900E
		public bool IsDeconstructible
		{
			get
			{
				return this.alwaysDeconstructible || (!this.isNaturalRock && this.deconstructible);
			}
		}

		// Token: 0x17000CF2 RID: 3314
		// (get) Token: 0x060054EC RID: 21740 RVA: 0x001C66C8 File Offset: 0x001C48C8
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

		// Token: 0x060054ED RID: 21741 RVA: 0x0003AE2A File Offset: 0x0003902A
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
			yield break;
		}

		// Token: 0x060054EE RID: 21742 RVA: 0x00006A05 File Offset: 0x00004C05
		public void PostLoadSpecial(ThingDef parent)
		{
		}

		// Token: 0x060054EF RID: 21743 RVA: 0x001C67DC File Offset: 0x001C49DC
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

		// Token: 0x060054F0 RID: 21744 RVA: 0x001C68AC File Offset: 0x001C4AAC
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
						Log.Error(string.Format("{0} is smoothable to non-building {1}", thingDef, thingDef2), false);
					}
					else if (thingDef2.building.unsmoothedThing == null || thingDef2.building.unsmoothedThing == thingDef)
					{
						thingDef2.building.unsmoothedThing = thingDef;
					}
					else
					{
						Log.Error(string.Format("{0} and {1} both smooth to {2}", thingDef, thingDef2.building.unsmoothedThing, thingDef2), false);
					}
				}
			}
		}

		// Token: 0x060054F1 RID: 21745 RVA: 0x0003AE41 File Offset: 0x00039041
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
			yield break;
		}

		// Token: 0x04003598 RID: 13720
		public bool isEdifice = true;

		// Token: 0x04003599 RID: 13721
		[NoTranslate]
		public List<string> buildingTags = new List<string>();

		// Token: 0x0400359A RID: 13722
		public bool isInert;

		// Token: 0x0400359B RID: 13723
		private bool deconstructible = true;

		// Token: 0x0400359C RID: 13724
		public bool alwaysDeconstructible;

		// Token: 0x0400359D RID: 13725
		public bool claimable = true;

		// Token: 0x0400359E RID: 13726
		public bool isSittable;

		// Token: 0x0400359F RID: 13727
		public SoundDef soundAmbient;

		// Token: 0x040035A0 RID: 13728
		public ConceptDef spawnedConceptLearnOpportunity;

		// Token: 0x040035A1 RID: 13729
		public ConceptDef boughtConceptLearnOpportunity;

		// Token: 0x040035A2 RID: 13730
		public bool expandHomeArea = true;

		// Token: 0x040035A3 RID: 13731
		public Type blueprintClass = typeof(Blueprint_Build);

		// Token: 0x040035A4 RID: 13732
		public GraphicData blueprintGraphicData;

		// Token: 0x040035A5 RID: 13733
		public float uninstallWork = 200f;

		// Token: 0x040035A6 RID: 13734
		public bool forceShowRoomStats;

		// Token: 0x040035A7 RID: 13735
		public bool wantsHopperAdjacent;

		// Token: 0x040035A8 RID: 13736
		public bool allowWireConnection = true;

		// Token: 0x040035A9 RID: 13737
		public bool shipPart;

		// Token: 0x040035AA RID: 13738
		public bool canPlaceOverImpassablePlant = true;

		// Token: 0x040035AB RID: 13739
		public float heatPerTickWhileWorking;

		// Token: 0x040035AC RID: 13740
		public bool canBuildNonEdificesUnder = true;

		// Token: 0x040035AD RID: 13741
		public bool canPlaceOverWall;

		// Token: 0x040035AE RID: 13742
		public bool allowAutoroof = true;

		// Token: 0x040035AF RID: 13743
		public bool preventDeteriorationOnTop;

		// Token: 0x040035B0 RID: 13744
		public bool preventDeteriorationInside;

		// Token: 0x040035B1 RID: 13745
		public bool isMealSource;

		// Token: 0x040035B2 RID: 13746
		public bool isNaturalRock;

		// Token: 0x040035B3 RID: 13747
		public bool isResourceRock;

		// Token: 0x040035B4 RID: 13748
		public bool repairable = true;

		// Token: 0x040035B5 RID: 13749
		public float roofCollapseDamageMultiplier = 1f;

		// Token: 0x040035B6 RID: 13750
		public bool hasFuelingPort;

		// Token: 0x040035B7 RID: 13751
		public ThingDef smoothedThing;

		// Token: 0x040035B8 RID: 13752
		[Unsaved(false)]
		public ThingDef unsmoothedThing;

		// Token: 0x040035B9 RID: 13753
		public TerrainDef naturalTerrain;

		// Token: 0x040035BA RID: 13754
		public TerrainDef leaveTerrain;

		// Token: 0x040035BB RID: 13755
		public float combatPower;

		// Token: 0x040035BC RID: 13756
		public int minMechClusterPoints;

		// Token: 0x040035BD RID: 13757
		public float destroyShakeAmount = -1f;

		// Token: 0x040035BE RID: 13758
		public SoundDef destroySound;

		// Token: 0x040035BF RID: 13759
		public EffecterDef destroyEffecter;

		// Token: 0x040035C0 RID: 13760
		public BuildingSizeCategory buildingSizeCategory;

		// Token: 0x040035C1 RID: 13761
		public bool isPlayerEjectable;

		// Token: 0x040035C2 RID: 13762
		public GraphicData fullGraveGraphicData;

		// Token: 0x040035C3 RID: 13763
		public float bed_healPerDay;

		// Token: 0x040035C4 RID: 13764
		public bool bed_defaultMedical;

		// Token: 0x040035C5 RID: 13765
		public bool bed_showSleeperBody;

		// Token: 0x040035C6 RID: 13766
		public bool bed_humanlike = true;

		// Token: 0x040035C7 RID: 13767
		public float bed_maxBodySize = 9999f;

		// Token: 0x040035C8 RID: 13768
		public bool bed_caravansCanUse;

		// Token: 0x040035C9 RID: 13769
		public float nutritionCostPerDispense;

		// Token: 0x040035CA RID: 13770
		public SoundDef soundDispense;

		// Token: 0x040035CB RID: 13771
		public ThingDef turretGunDef;

		// Token: 0x040035CC RID: 13772
		public float turretBurstWarmupTime;

		// Token: 0x040035CD RID: 13773
		public float turretBurstCooldownTime = -1f;

		// Token: 0x040035CE RID: 13774
		public float turretInitialCooldownTime;

		// Token: 0x040035CF RID: 13775
		[Unsaved(false)]
		public Material turretTopMat;

		// Token: 0x040035D0 RID: 13776
		public float turretTopDrawSize = 2f;

		// Token: 0x040035D1 RID: 13777
		public Vector2 turretTopOffset;

		// Token: 0x040035D2 RID: 13778
		public bool ai_combatDangerous;

		// Token: 0x040035D3 RID: 13779
		public bool ai_chillDestination = true;

		// Token: 0x040035D4 RID: 13780
		public bool ai_neverTrashThis;

		// Token: 0x040035D5 RID: 13781
		public SoundDef soundDoorOpenPowered;

		// Token: 0x040035D6 RID: 13782
		public SoundDef soundDoorClosePowered;

		// Token: 0x040035D7 RID: 13783
		public SoundDef soundDoorOpenManual;

		// Token: 0x040035D8 RID: 13784
		public SoundDef soundDoorCloseManual;

		// Token: 0x040035D9 RID: 13785
		[NoTranslate]
		public string sowTag;

		// Token: 0x040035DA RID: 13786
		public ThingDef defaultPlantToGrow;

		// Token: 0x040035DB RID: 13787
		public ThingDef mineableThing;

		// Token: 0x040035DC RID: 13788
		public int mineableYield = 1;

		// Token: 0x040035DD RID: 13789
		public float mineableNonMinedEfficiency = 0.7f;

		// Token: 0x040035DE RID: 13790
		public float mineableDropChance = 1f;

		// Token: 0x040035DF RID: 13791
		public bool mineableYieldWasteable = true;

		// Token: 0x040035E0 RID: 13792
		public float mineableScatterCommonality;

		// Token: 0x040035E1 RID: 13793
		public IntRange mineableScatterLumpSizeRange = new IntRange(20, 40);

		// Token: 0x040035E2 RID: 13794
		public StorageSettings fixedStorageSettings;

		// Token: 0x040035E3 RID: 13795
		public StorageSettings defaultStorageSettings;

		// Token: 0x040035E4 RID: 13796
		public bool ignoreStoredThingsBeauty;

		// Token: 0x040035E5 RID: 13797
		public bool isTrap;

		// Token: 0x040035E6 RID: 13798
		public bool trapDestroyOnSpring;

		// Token: 0x040035E7 RID: 13799
		public float trapPeacefulWildAnimalsSpringChanceFactor = 1f;

		// Token: 0x040035E8 RID: 13800
		public DamageArmorCategoryDef trapDamageCategory;

		// Token: 0x040035E9 RID: 13801
		public GraphicData trapUnarmedGraphicData;

		// Token: 0x040035EA RID: 13802
		[Unsaved(false)]
		public Graphic trapUnarmedGraphic;

		// Token: 0x040035EB RID: 13803
		public float unpoweredWorkTableWorkSpeedFactor;

		// Token: 0x040035EC RID: 13804
		public IntRange watchBuildingStandDistanceRange = IntRange.one;

		// Token: 0x040035ED RID: 13805
		public int watchBuildingStandRectWidth = 3;

		// Token: 0x040035EE RID: 13806
		public bool watchBuildingInSameRoom;

		// Token: 0x040035EF RID: 13807
		public JoyKindDef joyKind;

		// Token: 0x040035F0 RID: 13808
		public int haulToContainerDuration;

		// Token: 0x040035F1 RID: 13809
		public float instrumentRange;

		// Token: 0x040035F2 RID: 13810
		public int minDistanceToSameTypeOfBuilding;

		// Token: 0x040035F3 RID: 13811
		public bool artificialForMeditationPurposes = true;

		// Token: 0x040035F4 RID: 13812
		public EffecterDef effectWatching;
	}
}
