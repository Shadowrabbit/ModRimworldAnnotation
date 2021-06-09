using System;
using System.Collections.Generic;
using System.Xml;
using RimWorld;
using Verse.AI.Group;

namespace Verse
{
	// Token: 0x020007B1 RID: 1969
	public static class BackCompatibility
	{
		// Token: 0x060031A6 RID: 12710 RVA: 0x00146EA0 File Offset: 0x001450A0
		public static bool IsSaveCompatibleWith(string version)
		{
			if (VersionControl.MajorFromVersionString(version) == VersionControl.CurrentMajor && VersionControl.MinorFromVersionString(version) == VersionControl.CurrentMinor)
			{
				return true;
			}
			if (VersionControl.MajorFromVersionString(version) >= 1 && VersionControl.MajorFromVersionString(version) == VersionControl.CurrentMajor && VersionControl.MinorFromVersionString(version) <= VersionControl.CurrentMinor)
			{
				return true;
			}
			if (VersionControl.MajorFromVersionString(version) == 0 && VersionControl.CurrentMajor == 0)
			{
				int num = VersionControl.MinorFromVersionString(version);
				int currentMinor = VersionControl.CurrentMinor;
				for (int i = 0; i < BackCompatibility.SaveCompatibleMinorVersions.Length; i++)
				{
					if (BackCompatibility.SaveCompatibleMinorVersions[i].First == num && BackCompatibility.SaveCompatibleMinorVersions[i].Second == currentMinor)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060031A7 RID: 12711 RVA: 0x00146F48 File Offset: 0x00145148
		public static void PreLoadSavegame(string loadingVersion)
		{
			for (int i = 0; i < BackCompatibility.conversionChain.Count; i++)
			{
				if (BackCompatibility.conversionChain[i].AppliesToLoadedGameVersion(true))
				{
					try
					{
						BackCompatibility.conversionChain[i].PreLoadSavegame(loadingVersion);
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Error in PreLoadSavegame of ",
							BackCompatibility.conversionChain[i].GetType(),
							"\n",
							ex
						}), false);
					}
				}
			}
		}

		// Token: 0x060031A8 RID: 12712 RVA: 0x00146FDC File Offset: 0x001451DC
		public static void PostLoadSavegame(string loadingVersion)
		{
			for (int i = 0; i < BackCompatibility.conversionChain.Count; i++)
			{
				if (BackCompatibility.conversionChain[i].AppliesToLoadedGameVersion(true))
				{
					try
					{
						BackCompatibility.conversionChain[i].PostLoadSavegame(loadingVersion);
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Error in PostLoadSavegame of ",
							BackCompatibility.conversionChain[i].GetType(),
							"\n",
							ex
						}), false);
					}
				}
			}
		}

		// Token: 0x060031A9 RID: 12713 RVA: 0x00147070 File Offset: 0x00145270
		public static string BackCompatibleDefName(Type defType, string defName, bool forDefInjections = false, XmlNode node = null)
		{
			if (GenDefDatabase.GetDefSilentFail(defType, defName, false) != null)
			{
				return defName;
			}
			string text = defName;
			for (int i = 0; i < BackCompatibility.conversionChain.Count; i++)
			{
				if (Scribe.mode == LoadSaveMode.Inactive || BackCompatibility.conversionChain[i].AppliesToLoadedGameVersion(false))
				{
					try
					{
						string text2 = BackCompatibility.conversionChain[i].BackCompatibleDefName(defType, text, forDefInjections, node);
						if (text2 != null)
						{
							text = text2;
						}
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Error in BackCompatibleDefName of ",
							BackCompatibility.conversionChain[i].GetType(),
							"\n",
							ex
						}), false);
					}
				}
			}
			return text;
		}

		// Token: 0x060031AA RID: 12714 RVA: 0x000270D3 File Offset: 0x000252D3
		public static object BackCompatibleEnum(Type enumType, string enumName)
		{
			if (enumType == typeof(QualityCategory))
			{
				if (enumName == "Shoddy")
				{
					return QualityCategory.Poor;
				}
				if (enumName == "Superior")
				{
					return QualityCategory.Excellent;
				}
			}
			return null;
		}

		// Token: 0x060031AB RID: 12715 RVA: 0x00147128 File Offset: 0x00145328
		public static Type GetBackCompatibleType(Type baseType, string providedClassName, XmlNode node)
		{
			for (int i = 0; i < BackCompatibility.conversionChain.Count; i++)
			{
				if (BackCompatibility.conversionChain[i].AppliesToLoadedGameVersion(false))
				{
					try
					{
						Type backCompatibleType = BackCompatibility.conversionChain[i].GetBackCompatibleType(baseType, providedClassName, node);
						if (backCompatibleType != null)
						{
							return backCompatibleType;
						}
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Error in GetBackCompatibleType of ",
							BackCompatibility.conversionChain[i].GetType(),
							"\n",
							ex
						}), false);
					}
				}
			}
			return GenTypes.GetTypeInAnyAssembly(providedClassName, null);
		}

		// Token: 0x060031AC RID: 12716 RVA: 0x001471D8 File Offset: 0x001453D8
		public static int GetBackCompatibleBodyPartIndex(BodyDef body, int index)
		{
			for (int i = 0; i < BackCompatibility.conversionChain.Count; i++)
			{
				if (BackCompatibility.conversionChain[i].AppliesToLoadedGameVersion(false))
				{
					try
					{
						index = BackCompatibility.conversionChain[i].GetBackCompatibleBodyPartIndex(body, index);
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Error in GetBackCompatibleBodyPartIndex of ",
							body,
							"\n",
							ex
						}), false);
					}
				}
			}
			return index;
		}

		// Token: 0x060031AD RID: 12717 RVA: 0x00147264 File Offset: 0x00145464
		public static bool WasDefRemoved(string defName, Type type)
		{
			foreach (Tuple<string, Type> tuple in BackCompatibility.RemovedDefs)
			{
				if (tuple.Item1 == defName && tuple.Item2 == type)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060031AE RID: 12718 RVA: 0x001472D4 File Offset: 0x001454D4
		public static void PostExposeData(object obj)
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				return;
			}
			for (int i = 0; i < BackCompatibility.conversionChain.Count; i++)
			{
				if (BackCompatibility.conversionChain[i].AppliesToLoadedGameVersion(false))
				{
					try
					{
						BackCompatibility.conversionChain[i].PostExposeData(obj);
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Error in PostExposeData of ",
							BackCompatibility.conversionChain[i].GetType(),
							"\n",
							ex
						}), false);
					}
				}
			}
		}

		// Token: 0x060031AF RID: 12719 RVA: 0x00147374 File Offset: 0x00145574
		public static void PostCouldntLoadDef(string defName)
		{
			for (int i = 0; i < BackCompatibility.conversionChain.Count; i++)
			{
				if (BackCompatibility.conversionChain[i].AppliesToLoadedGameVersion(false))
				{
					try
					{
						BackCompatibility.conversionChain[i].PostCouldntLoadDef(defName);
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Error in PostCouldntLoadDef of ",
							BackCompatibility.conversionChain[i].GetType(),
							"\n",
							ex
						}), false);
					}
				}
			}
		}

		// Token: 0x060031B0 RID: 12720 RVA: 0x00147408 File Offset: 0x00145608
		public static void PawnTrainingTrackerPostLoadInit(Pawn_TrainingTracker tracker, ref DefMap<TrainableDef, bool> wantedTrainables, ref DefMap<TrainableDef, int> steps, ref DefMap<TrainableDef, bool> learned)
		{
			if (wantedTrainables == null)
			{
				wantedTrainables = new DefMap<TrainableDef, bool>();
			}
			if (steps == null)
			{
				steps = new DefMap<TrainableDef, int>();
			}
			if (learned == null)
			{
				learned = new DefMap<TrainableDef, bool>();
			}
			if (tracker.GetSteps(TrainableDefOf.Tameness) == 0 && DefDatabase<TrainableDef>.AllDefsListForReading.Any((TrainableDef td) => tracker.GetSteps(td) != 0))
			{
				tracker.Train(TrainableDefOf.Tameness, null, true);
			}
			foreach (TrainableDef trainableDef in DefDatabase<TrainableDef>.AllDefsListForReading)
			{
				if (tracker.GetSteps(trainableDef) == trainableDef.steps)
				{
					tracker.Train(trainableDef, null, true);
				}
			}
		}

		// Token: 0x060031B1 RID: 12721 RVA: 0x00027110 File Offset: 0x00025310
		public static void TriggerDataFractionColonyDamageTakenNull(Trigger_FractionColonyDamageTaken trigger, Map map)
		{
			trigger.data = new TriggerData_FractionColonyDamageTaken();
			((TriggerData_FractionColonyDamageTaken)trigger.data).startColonyDamage = map.damageWatcher.DamageTakenEver;
		}

		// Token: 0x060031B2 RID: 12722 RVA: 0x00027138 File Offset: 0x00025338
		public static void TriggerDataPawnCycleIndNull(Trigger_KidnapVictimPresent trigger)
		{
			trigger.data = new TriggerData_PawnCycleInd();
		}

		// Token: 0x060031B3 RID: 12723 RVA: 0x00027145 File Offset: 0x00025345
		public static void TriggerDataTicksPassedNull(Trigger_TicksPassed trigger)
		{
			trigger.data = new TriggerData_TicksPassed();
		}

		// Token: 0x060031B4 RID: 12724 RVA: 0x00027152 File Offset: 0x00025352
		public static TerrainDef BackCompatibleTerrainWithShortHash(ushort hash)
		{
			if (hash == 16442)
			{
				return TerrainDefOf.WaterMovingChestDeep;
			}
			return null;
		}

		// Token: 0x060031B5 RID: 12725 RVA: 0x00027163 File Offset: 0x00025363
		public static ThingDef BackCompatibleThingDefWithShortHash(ushort hash)
		{
			if (hash == 62520)
			{
				return ThingDefOf.MineableComponentsIndustrial;
			}
			return null;
		}

		// Token: 0x060031B6 RID: 12726 RVA: 0x00027174 File Offset: 0x00025374
		public static ThingDef BackCompatibleThingDefWithShortHash_Force(ushort hash, int major, int minor)
		{
			if (major == 0 && minor <= 18 && hash == 27292)
			{
				return ThingDefOf.MineableSteel;
			}
			return null;
		}

		// Token: 0x060031B7 RID: 12727 RVA: 0x001474E0 File Offset: 0x001456E0
		public static bool CheckSpawnBackCompatibleThingAfterLoading(Thing thing, Map map)
		{
			if (VersionControl.MajorFromVersionString(ScribeMetaHeaderUtility.loadedGameVersion) == 0 && VersionControl.MinorFromVersionString(ScribeMetaHeaderUtility.loadedGameVersion) <= 18 && thing.stackCount > thing.def.stackLimit && thing.stackCount != 1 && thing.def.stackLimit != 1)
			{
				BackCompatibility.tmpThingsToSpawnLater.Add(thing);
				return true;
			}
			return false;
		}

		// Token: 0x060031B8 RID: 12728 RVA: 0x0002718D File Offset: 0x0002538D
		public static void PreCheckSpawnBackCompatibleThingAfterLoading(Map map)
		{
			BackCompatibility.tmpThingsToSpawnLater.Clear();
		}

		// Token: 0x060031B9 RID: 12729 RVA: 0x00147540 File Offset: 0x00145740
		public static void PostCheckSpawnBackCompatibleThingAfterLoading(Map map)
		{
			for (int i = 0; i < BackCompatibility.tmpThingsToSpawnLater.Count; i++)
			{
				GenPlace.TryPlaceThing(BackCompatibility.tmpThingsToSpawnLater[i], BackCompatibility.tmpThingsToSpawnLater[i].Position, map, ThingPlaceMode.Near, null, null, default(Rot4));
			}
			BackCompatibility.tmpThingsToSpawnLater.Clear();
		}

		// Token: 0x060031BA RID: 12730 RVA: 0x0014759C File Offset: 0x0014579C
		public static void FactionManagerPostLoadInit()
		{
			if (ModsConfig.RoyaltyActive && Find.FactionManager.FirstFactionOfDef(FactionDefOf.Empire) == null)
			{
				Faction faction = FactionGenerator.NewGeneratedFaction(FactionDefOf.Empire);
				Find.FactionManager.Add(faction);
			}
		}

		// Token: 0x060031BB RID: 12731 RVA: 0x001475D8 File Offset: 0x001457D8
		public static void ResearchManagerPostLoadInit()
		{
			List<ResearchProjectDef> allDefsListForReading = DefDatabase<ResearchProjectDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				if ((allDefsListForReading[i].IsFinished || allDefsListForReading[i].ProgressReal > 0f) && allDefsListForReading[i].TechprintsApplied < allDefsListForReading[i].TechprintCount)
				{
					Find.ResearchManager.AddTechprints(allDefsListForReading[i], allDefsListForReading[i].TechprintCount - allDefsListForReading[i].TechprintsApplied);
				}
			}
		}

		// Token: 0x060031BC RID: 12732 RVA: 0x00027199 File Offset: 0x00025399
		public static void PrefsDataPostLoad(PrefsData prefsData)
		{
			if (prefsData.pauseOnUrgentLetter != null)
			{
				if (prefsData.pauseOnUrgentLetter.Value)
				{
					prefsData.automaticPauseMode = AutomaticPauseMode.MajorThreat;
					return;
				}
				prefsData.automaticPauseMode = AutomaticPauseMode.Never;
			}
		}

		// Token: 0x0400225F RID: 8799
		public static readonly Pair<int, int>[] SaveCompatibleMinorVersions = new Pair<int, int>[]
		{
			new Pair<int, int>(17, 18)
		};

		// Token: 0x04002260 RID: 8800
		private static List<BackCompatibilityConverter> conversionChain = new List<BackCompatibilityConverter>
		{
			new BackCompatibilityConverter_0_17_AndLower(),
			new BackCompatibilityConverter_0_18(),
			new BackCompatibilityConverter_0_19(),
			new BackCompatibilityConverter_1_0(),
			new BackCompatibilityConverter_Universal()
		};

		// Token: 0x04002261 RID: 8801
		private static readonly List<Tuple<string, Type>> RemovedDefs = new List<Tuple<string, Type>>
		{
			new Tuple<string, Type>("PsychicSilencer", typeof(ThingDef)),
			new Tuple<string, Type>("PsychicSilencer", typeof(HediffDef))
		};

		// Token: 0x04002262 RID: 8802
		private static List<Thing> tmpThingsToSpawnLater = new List<Thing>();
	}
}
