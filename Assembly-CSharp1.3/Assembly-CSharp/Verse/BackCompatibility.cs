using System;
using System.Collections.Generic;
using System.Xml;
using RimWorld;
using Verse.AI.Group;

namespace Verse
{
	// Token: 0x0200045F RID: 1119
	public static class BackCompatibility
	{
		// Token: 0x060021F2 RID: 8690 RVA: 0x000D3EF4 File Offset: 0x000D20F4
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

		// Token: 0x060021F3 RID: 8691 RVA: 0x000D3F9C File Offset: 0x000D219C
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
						}));
					}
				}
			}
		}

		// Token: 0x060021F4 RID: 8692 RVA: 0x000D4030 File Offset: 0x000D2230
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
						}));
					}
				}
			}
		}

		// Token: 0x060021F5 RID: 8693 RVA: 0x000D40C4 File Offset: 0x000D22C4
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
						}));
					}
				}
			}
			return text;
		}

		// Token: 0x060021F6 RID: 8694 RVA: 0x000D417C File Offset: 0x000D237C
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

		// Token: 0x060021F7 RID: 8695 RVA: 0x000D41BC File Offset: 0x000D23BC
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
						}));
					}
				}
			}
			return GenTypes.GetTypeInAnyAssembly(providedClassName, null);
		}

		// Token: 0x060021F8 RID: 8696 RVA: 0x000D426C File Offset: 0x000D246C
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
						}));
					}
				}
			}
			return index;
		}

		// Token: 0x060021F9 RID: 8697 RVA: 0x000D42F4 File Offset: 0x000D24F4
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

		// Token: 0x060021FA RID: 8698 RVA: 0x000D4364 File Offset: 0x000D2564
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
						}));
					}
				}
			}
		}

		// Token: 0x060021FB RID: 8699 RVA: 0x000D4400 File Offset: 0x000D2600
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
						}));
					}
				}
			}
		}

		// Token: 0x060021FC RID: 8700 RVA: 0x000D4494 File Offset: 0x000D2694
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
				if (!tracker.CanAssignToTrain(trainableDef).Accepted)
				{
					wantedTrainables[trainableDef] = false;
					learned[trainableDef] = false;
					steps[trainableDef] = 0;
					if (trainableDef == TrainableDefOf.Obedience && tracker.pawn.playerSettings != null)
					{
						tracker.pawn.playerSettings.Master = null;
						tracker.pawn.playerSettings.followDrafted = false;
						tracker.pawn.playerSettings.followFieldwork = false;
					}
				}
				if (tracker.GetSteps(trainableDef) == trainableDef.steps)
				{
					tracker.Train(trainableDef, null, true);
				}
			}
		}

		// Token: 0x060021FD RID: 8701 RVA: 0x000D4600 File Offset: 0x000D2800
		public static void TriggerDataFractionColonyDamageTakenNull(Trigger_FractionColonyDamageTaken trigger, Map map)
		{
			trigger.data = new TriggerData_FractionColonyDamageTaken();
			((TriggerData_FractionColonyDamageTaken)trigger.data).startColonyDamage = map.damageWatcher.DamageTakenEver;
		}

		// Token: 0x060021FE RID: 8702 RVA: 0x000D4628 File Offset: 0x000D2828
		public static void TriggerDataPawnCycleIndNull(Trigger_KidnapVictimPresent trigger)
		{
			trigger.data = new TriggerData_PawnCycleInd();
		}

		// Token: 0x060021FF RID: 8703 RVA: 0x000D4635 File Offset: 0x000D2835
		public static void TriggerDataTicksPassedNull(Trigger_TicksPassed trigger)
		{
			trigger.data = new TriggerData_TicksPassed();
		}

		// Token: 0x06002200 RID: 8704 RVA: 0x000D4642 File Offset: 0x000D2842
		public static TerrainDef BackCompatibleTerrainWithShortHash(ushort hash)
		{
			if (hash == 16442)
			{
				return TerrainDefOf.WaterMovingChestDeep;
			}
			return null;
		}

		// Token: 0x06002201 RID: 8705 RVA: 0x000D4653 File Offset: 0x000D2853
		public static ThingDef BackCompatibleThingDefWithShortHash(ushort hash)
		{
			if (hash == 62520)
			{
				return ThingDefOf.MineableComponentsIndustrial;
			}
			return null;
		}

		// Token: 0x06002202 RID: 8706 RVA: 0x000D4664 File Offset: 0x000D2864
		public static ThingDef BackCompatibleThingDefWithShortHash_Force(ushort hash, int major, int minor)
		{
			if (major == 0 && minor <= 18 && hash == 27292)
			{
				return ThingDefOf.MineableSteel;
			}
			return null;
		}

		// Token: 0x06002203 RID: 8707 RVA: 0x000D4680 File Offset: 0x000D2880
		public static bool CheckSpawnBackCompatibleThingAfterLoading(Thing thing, Map map)
		{
			if (VersionControl.MajorFromVersionString(ScribeMetaHeaderUtility.loadedGameVersion) == 0 && VersionControl.MinorFromVersionString(ScribeMetaHeaderUtility.loadedGameVersion) <= 18 && thing.stackCount > thing.def.stackLimit && thing.stackCount != 1 && thing.def.stackLimit != 1)
			{
				BackCompatibility.tmpThingsToSpawnLater.Add(thing);
				return true;
			}
			return false;
		}

		// Token: 0x06002204 RID: 8708 RVA: 0x000D46DF File Offset: 0x000D28DF
		public static void PreCheckSpawnBackCompatibleThingAfterLoading(Map map)
		{
			BackCompatibility.tmpThingsToSpawnLater.Clear();
		}

		// Token: 0x06002205 RID: 8709 RVA: 0x000D46EC File Offset: 0x000D28EC
		public static void PostCheckSpawnBackCompatibleThingAfterLoading(Map map)
		{
			for (int i = 0; i < BackCompatibility.tmpThingsToSpawnLater.Count; i++)
			{
				GenPlace.TryPlaceThing(BackCompatibility.tmpThingsToSpawnLater[i], BackCompatibility.tmpThingsToSpawnLater[i].Position, map, ThingPlaceMode.Near, null, null, default(Rot4));
			}
			BackCompatibility.tmpThingsToSpawnLater.Clear();
		}

		// Token: 0x06002206 RID: 8710 RVA: 0x000D4748 File Offset: 0x000D2948
		public static void FactionManagerPostLoadInit()
		{
			if (ModsConfig.RoyaltyActive && Find.FactionManager.FirstFactionOfDef(FactionDefOf.Empire) == null && Find.World.info.factionCounts == null)
			{
				Faction faction = FactionGenerator.NewGeneratedFaction(new FactionGeneratorParms(FactionDefOf.Empire, default(IdeoGenerationParms), null));
				Find.FactionManager.Add(faction);
			}
		}

		// Token: 0x06002207 RID: 8711 RVA: 0x000D47AC File Offset: 0x000D29AC
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

		// Token: 0x06002208 RID: 8712 RVA: 0x000D4835 File Offset: 0x000D2A35
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

		// Token: 0x0400154C RID: 5452
		public static readonly Pair<int, int>[] SaveCompatibleMinorVersions = new Pair<int, int>[]
		{
			new Pair<int, int>(17, 18)
		};

		// Token: 0x0400154D RID: 5453
		private static List<BackCompatibilityConverter> conversionChain = new List<BackCompatibilityConverter>
		{
			new BackCompatibilityConverter_0_17_AndLower(),
			new BackCompatibilityConverter_0_18(),
			new BackCompatibilityConverter_0_19(),
			new BackCompatibilityConverter_1_0(),
			new BackCompatibilityConverter_1_2(),
			new BackCompatibilityConverter_Universal()
		};

		// Token: 0x0400154E RID: 5454
		private static readonly List<Tuple<string, Type>> RemovedDefs = new List<Tuple<string, Type>>
		{
			new Tuple<string, Type>("PsychicSilencer", typeof(ThingDef)),
			new Tuple<string, Type>("PsychicSilencer", typeof(HediffDef))
		};

		// Token: 0x0400154F RID: 5455
		private static List<Thing> tmpThingsToSpawnLater = new List<Thing>();
	}
}
