using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.AI.Group;

namespace Verse
{
	// Token: 0x02000464 RID: 1124
	public class BackCompatibilityConverter_1_0 : BackCompatibilityConverter
	{
		// Token: 0x06002224 RID: 8740 RVA: 0x000D79AB File Offset: 0x000D5BAB
		public override bool AppliesToVersion(int majorVer, int minorVer)
		{
			return majorVer <= 1 && (majorVer == 0 || minorVer == 0);
		}

		// Token: 0x06002225 RID: 8741 RVA: 0x000D79BC File Offset: 0x000D5BBC
		public override string BackCompatibleDefName(Type defType, string defName, bool forDefInjections = false, XmlNode node = null)
		{
			if (defType == typeof(ThingDef))
			{
				if (defName == "CrashedPoisonShipPart" || defName == "CrashedPsychicEmanatorShipPart")
				{
					return "MechCapsule";
				}
				if (defName == "PoisonSpreader")
				{
					return "Defoliator";
				}
				if (defName == "PoisonSpreaderShipPart")
				{
					return "DefoliatorShipPart";
				}
				if (defName == "MechSerumNeurotrainer")
				{
					XmlNode xmlNode = (node != null) ? node.ParentNode : null;
					if (xmlNode != null && xmlNode.HasChildNodes)
					{
						foreach (object obj in xmlNode.ChildNodes)
						{
							XmlNode xmlNode2 = (XmlNode)obj;
							if (xmlNode2.Name == "skill")
							{
								return ThingDefGenerator_Neurotrainer.NeurotrainerDefPrefix + "_" + xmlNode2.InnerText;
							}
						}
					}
					ThingDef thingDef = (from def in DefDatabase<ThingDef>.AllDefsListForReading
					where def.thingCategories != null && def.thingCategories.Contains(ThingCategoryDefOf.Neurotrainers)
					select def).RandomElementWithFallback(null);
					if (thingDef == null)
					{
						return null;
					}
					return thingDef.defName;
				}
			}
			else if ((defType == typeof(QuestScriptDef) || defType == typeof(TaleDef)) && defName == "JourneyOffer")
			{
				return "EndGame_ShipEscape";
			}
			return null;
		}

		// Token: 0x06002226 RID: 8742 RVA: 0x000D7B38 File Offset: 0x000D5D38
		public override Type GetBackCompatibleType(Type baseType, string providedClassName, XmlNode node)
		{
			if (baseType == typeof(Thing))
			{
				if (providedClassName == "Building_CrashedShipPart" && node["def"] != null)
				{
					BackCompatibilityConverter_1_0.oldCrashedShipParts.Add(node);
					return ThingDefOf.MechCapsule.thingClass;
				}
			}
			else if (baseType == typeof(LordJob) && providedClassName == "LordJob_MechanoidsDefendShip")
			{
				XmlElement xmlElement = node["shipPart"];
				if (xmlElement != null)
				{
					xmlElement.InnerText = xmlElement.InnerText.Replace("Thing_CrashedPsychicEmanatorShipPart", "Thing_MechCapsule").Replace("Thing_CrashedPoisonShipPart", "Thing_MechCapsule");
				}
				return typeof(LordJob_MechanoidsDefend);
			}
			return null;
		}

		// Token: 0x06002227 RID: 8743 RVA: 0x000D7BF0 File Offset: 0x000D5DF0
		public override int GetBackCompatibleBodyPartIndex(BodyDef body, int index)
		{
			if (body != BodyDefOf.MechanicalCentipede)
			{
				return index;
			}
			switch (index)
			{
			case 9:
				return 10;
			case 10:
				return 12;
			case 11:
				return 14;
			case 12:
				return 15;
			case 13:
				return 9;
			case 14:
				return 11;
			case 15:
				return 13;
			default:
				return index;
			}
		}

		// Token: 0x06002228 RID: 8744 RVA: 0x000D7C44 File Offset: 0x000D5E44
		public override void PostExposeData(object obj)
		{
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				Game game = obj as Game;
				if (game != null && game.questManager == null)
				{
					game.questManager = new QuestManager();
				}
				Zone zone = obj as Zone;
				if (zone != null && zone.ID == -1)
				{
					zone.ID = Find.UniqueIDsManager.GetNextZoneID();
				}
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				Pawn pawn = obj as Pawn;
				if (pawn != null && pawn.royalty == null)
				{
					pawn.royalty = new Pawn_RoyaltyTracker(pawn);
				}
				Pawn_NativeVerbs pawn_NativeVerbs = obj as Pawn_NativeVerbs;
				if (pawn_NativeVerbs != null && pawn_NativeVerbs.verbTracker == null)
				{
					pawn_NativeVerbs.verbTracker = new VerbTracker(pawn_NativeVerbs);
				}
				Thing thing = obj as Thing;
				if (thing != null)
				{
					if (thing.def.defName == "Sandbags" && thing.Stuff == null)
					{
						thing.SetStuffDirect(ThingDefOf.Cloth);
					}
					if (thing.def == ThingDefOf.MechCapsule)
					{
						foreach (XmlNode xmlNode in BackCompatibilityConverter_1_0.oldCrashedShipParts)
						{
							XmlElement xmlElement = xmlNode["def"];
							XmlElement xmlElement2 = xmlNode["id"];
							if (xmlElement != null && xmlElement2 != null && Thing.IDNumberFromThingID(xmlElement2.InnerText) == thing.thingIDNumber)
							{
								BackCompatibilityConverter_1_0.upgradedCrashedShipParts.Add(new BackCompatibilityConverter_1_0.UpgradedCrashedShipPart
								{
									originalDefName = xmlElement.InnerText,
									thing = thing
								});
							}
						}
					}
				}
				StoryWatcher storyWatcher = obj as StoryWatcher;
				if (storyWatcher != null)
				{
					if (storyWatcher.watcherAdaptation == null)
					{
						storyWatcher.watcherAdaptation = new StoryWatcher_Adaptation();
					}
					if (storyWatcher.watcherPopAdaptation == null)
					{
						storyWatcher.watcherPopAdaptation = new StoryWatcher_PopAdaptation();
					}
				}
				FoodRestrictionDatabase foodRestrictionDatabase = obj as FoodRestrictionDatabase;
				if (foodRestrictionDatabase != null && VersionControl.BuildFromVersionString(ScribeMetaHeaderUtility.loadedGameVersion) < 2057)
				{
					List<FoodRestriction> allFoodRestrictions = foodRestrictionDatabase.AllFoodRestrictions;
					for (int i = 0; i < allFoodRestrictions.Count; i++)
					{
						allFoodRestrictions[i].filter.SetAllow(ThingCategoryDefOf.CorpsesHumanlike, true, null, null);
						allFoodRestrictions[i].filter.SetAllow(ThingCategoryDefOf.CorpsesAnimal, true, null, null);
					}
				}
				SitePart sitePart = obj as SitePart;
				if (sitePart != null)
				{
					sitePart.hidden = sitePart.def.defaultHidden;
				}
			}
		}

		// Token: 0x06002229 RID: 8745 RVA: 0x000D7E94 File Offset: 0x000D6094
		public static Quest MakeAndAddWorldObjectQuest(WorldObject destination, string description)
		{
			Quest quest = Quest.MakeRaw();
			quest.SetInitiallyAccepted();
			QuestPartUtility.MakeAndAddEndCondition<QuestPart_NoWorldObject>(quest, quest.InitiateSignal, QuestEndOutcome.Unknown, null).worldObject = destination;
			quest.description = description;
			Find.QuestManager.Add(quest);
			return quest;
		}

		// Token: 0x0600222A RID: 8746 RVA: 0x000D7EDC File Offset: 0x000D60DC
		public static Quest MakeAndAddTradeRequestQuest(WorldObject destination, string description, TradeRequestComp tradeRequest)
		{
			Quest quest = Quest.MakeRaw();
			quest.SetInitiallyAccepted();
			string text = "Quest" + quest.id + ".TradeRequestSite";
			QuestUtility.AddQuestTag(ref destination.questTags, text);
			QuestPartUtility.MakeAndAddEndCondition<QuestPart_NoWorldObject>(quest, quest.InitiateSignal, QuestEndOutcome.Unknown, null).worldObject = destination;
			QuestPartUtility.MakeAndAddEndCondition<QuestPart_NoWorldObject>(quest, text + ".TradeRequestFulfilled", QuestEndOutcome.Success, null);
			if (destination.rewards != null)
			{
				QuestPart_GiveToCaravan questPart_GiveToCaravan = new QuestPart_GiveToCaravan
				{
					inSignal = text + ".TradeRequestFulfilled",
					Things = destination.rewards
				};
				foreach (Thing thing in questPart_GiveToCaravan.Things)
				{
					thing.holdingOwner = null;
				}
				quest.AddPart(questPart_GiveToCaravan);
			}
			quest.description = description;
			Find.QuestManager.Add(quest);
			return quest;
		}

		// Token: 0x0600222B RID: 8747 RVA: 0x000D7FCC File Offset: 0x000D61CC
		public override void PreLoadSavegame(string loadingVersion)
		{
			BackCompatibilityConverter_1_0.oldCrashedShipParts.Clear();
			BackCompatibilityConverter_1_0.upgradedCrashedShipParts.Clear();
		}

		// Token: 0x0600222C RID: 8748 RVA: 0x000D7FE4 File Offset: 0x000D61E4
		public override void PostLoadSavegame(string loadingVersion)
		{
			BackCompatibilityConverter_1_0.<>c__DisplayClass11_0 CS$<>8__locals1 = new BackCompatibilityConverter_1_0.<>c__DisplayClass11_0();
			BackCompatibilityConverter_1_0.oldCrashedShipParts.Clear();
			foreach (BackCompatibilityConverter_1_0.UpgradedCrashedShipPart upgradedCrashedShipPart in BackCompatibilityConverter_1_0.upgradedCrashedShipParts)
			{
				Thing thing = upgradedCrashedShipPart.thing;
				IntVec3 intVec = IntVec3.Invalid;
				Map map;
				if (thing.Spawned)
				{
					intVec = thing.Position;
					map = thing.Map;
				}
				else
				{
					Skyfaller skyfaller = thing.ParentHolder as Skyfaller;
					if (skyfaller == null)
					{
						thing.Destroy(DestroyMode.Vanish);
					}
					intVec = skyfaller.Position;
					map = skyfaller.Map;
				}
				if (!(intVec == IntVec3.Invalid))
				{
					intVec = new IntVec3(intVec.x - Mathf.CeilToInt((float)thing.def.size.x / 2f), intVec.y, intVec.z);
					Thing item = null;
					if (upgradedCrashedShipPart.originalDefName == "CrashedPoisonShipPart" || upgradedCrashedShipPart.originalDefName == "PoisonSpreaderShipPart")
					{
						item = ThingMaker.MakeThing(ThingDefOf.DefoliatorShipPart, null);
					}
					else if (upgradedCrashedShipPart.originalDefName == "CrashedPsychicEmanatorShipPart")
					{
						item = ThingMaker.MakeThing(ThingDefOf.PsychicDronerShipPart, null);
					}
					ActiveDropPodInfo activeDropPodInfo = new ActiveDropPodInfo();
					activeDropPodInfo.innerContainer.TryAdd(item, 1, true);
					activeDropPodInfo.openDelay = 60;
					activeDropPodInfo.leaveSlag = false;
					activeDropPodInfo.despawnPodBeforeSpawningThing = true;
					activeDropPodInfo.spawnWipeMode = new WipeMode?(WipeMode.Vanish);
					DropPodUtility.MakeDropPodAt(intVec, map, activeDropPodInfo);
				}
			}
			BackCompatibilityConverter_1_0.upgradedCrashedShipParts.Clear();
			CS$<>8__locals1.sites = Find.WorldObjects.Sites;
			int k;
			int j;
			for (k = 0; k < CS$<>8__locals1.sites.Count; k = j + 1)
			{
				if (!Find.QuestManager.QuestsListForReading.Any((Quest x) => x.QuestLookTargets.Contains(CS$<>8__locals1.sites[k])))
				{
					Quest quest = Quest.MakeRaw();
					QuestUtility.GenerateBackCompatibilityNameFor(quest);
					quest.SetInitiallyAccepted();
					quest.appearanceTick = CS$<>8__locals1.sites[k].creationGameTicks;
					TimeoutComp component = CS$<>8__locals1.sites[k].GetComponent<TimeoutComp>();
					if (component != null && component.Active && !CS$<>8__locals1.sites[k].HasMap)
					{
						QuestPartUtility.MakeAndAddQuestTimeoutDelay(quest, component.TicksLeft, CS$<>8__locals1.sites[k]);
						component.StopTimeout();
					}
					QuestPartUtility.MakeAndAddEndCondition<QuestPart_NoWorldObject>(quest, quest.InitiateSignal, QuestEndOutcome.Unknown, null).worldObject = CS$<>8__locals1.sites[k];
					ChoiceLetter choiceLetter = Find.Archive.ArchivablesListForReading.OfType<ChoiceLetter>().FirstOrDefault((ChoiceLetter x) => x.lookTargets != null && x.lookTargets.targets.Contains(CS$<>8__locals1.sites[k]));
					if (choiceLetter != null)
					{
						quest.description = choiceLetter.text;
					}
					Find.QuestManager.Add(quest);
				}
				j = k;
			}
			CS$<>8__locals1.worldObjects = Find.WorldObjects.AllWorldObjects;
			int l;
			Predicate<QuestPart> <>9__3;
			for (l = 0; l < CS$<>8__locals1.worldObjects.Count; l = j + 1)
			{
				if (CS$<>8__locals1.worldObjects[l].def == WorldObjectDefOf.EscapeShip && !Find.QuestManager.QuestsListForReading.Any(delegate(Quest x)
				{
					List<QuestPart> partsListForReading = x.PartsListForReading;
					Predicate<QuestPart> predicate;
					if ((predicate = <>9__3) == null)
					{
						predicate = (<>9__3 = ((QuestPart y) => y is QuestPart_NoWorldObject && ((QuestPart_NoWorldObject)y).worldObject == CS$<>8__locals1.worldObjects[l]));
					}
					return partsListForReading.Any(predicate);
				}))
				{
					BackCompatibilityConverter_1_0.MakeAndAddWorldObjectQuest(CS$<>8__locals1.worldObjects[l], null);
				}
				j = l;
			}
			int m;
			Predicate<QuestPart> <>9__5;
			for (m = 0; m < CS$<>8__locals1.worldObjects.Count; m = j + 1)
			{
				if (CS$<>8__locals1.worldObjects[m] is PeaceTalks && !Find.QuestManager.QuestsListForReading.Any(delegate(Quest x)
				{
					List<QuestPart> partsListForReading = x.PartsListForReading;
					Predicate<QuestPart> predicate;
					if ((predicate = <>9__5) == null)
					{
						predicate = (<>9__5 = ((QuestPart y) => y is QuestPart_NoWorldObject && ((QuestPart_NoWorldObject)y).worldObject == CS$<>8__locals1.worldObjects[m]));
					}
					return partsListForReading.Any(predicate);
				}))
				{
					Quest quest2 = BackCompatibilityConverter_1_0.MakeAndAddWorldObjectQuest(CS$<>8__locals1.worldObjects[m], null);
					ChoiceLetter choiceLetter2 = Find.Archive.ArchivablesListForReading.OfType<ChoiceLetter>().FirstOrDefault((ChoiceLetter x) => x.lookTargets != null && x.lookTargets.targets.Contains(CS$<>8__locals1.worldObjects[m]));
					if (choiceLetter2 != null)
					{
						quest2.description = choiceLetter2.text;
					}
				}
				j = m;
			}
			int i;
			Predicate<QuestPart> <>9__8;
			for (i = 0; i < CS$<>8__locals1.worldObjects.Count; i = j + 1)
			{
				TradeRequestComp component2 = CS$<>8__locals1.worldObjects[i].GetComponent<TradeRequestComp>();
				if (component2 != null && component2.ActiveRequest && !Find.QuestManager.QuestsListForReading.Any(delegate(Quest x)
				{
					List<QuestPart> partsListForReading = x.PartsListForReading;
					Predicate<QuestPart> predicate;
					if ((predicate = <>9__8) == null)
					{
						predicate = (<>9__8 = ((QuestPart y) => y is QuestPart_NoWorldObject && ((QuestPart_NoWorldObject)y).worldObject == CS$<>8__locals1.worldObjects[i]));
					}
					return partsListForReading.Any(predicate);
				}))
				{
					Quest quest3 = BackCompatibilityConverter_1_0.MakeAndAddTradeRequestQuest(CS$<>8__locals1.worldObjects[i], null, component2);
					ChoiceLetter choiceLetter3 = Find.Archive.ArchivablesListForReading.OfType<ChoiceLetter>().FirstOrDefault((ChoiceLetter x) => x.lookTargets != null && x.lookTargets.targets.Contains(CS$<>8__locals1.worldObjects[i]));
					if (choiceLetter3 != null)
					{
						quest3.description = choiceLetter3.text;
					}
				}
				j = i;
			}
		}

		// Token: 0x04001555 RID: 5461
		private static List<XmlNode> oldCrashedShipParts = new List<XmlNode>();

		// Token: 0x04001556 RID: 5462
		private static List<BackCompatibilityConverter_1_0.UpgradedCrashedShipPart> upgradedCrashedShipParts = new List<BackCompatibilityConverter_1_0.UpgradedCrashedShipPart>();

		// Token: 0x02001C7B RID: 7291
		private struct UpgradedCrashedShipPart
		{
			// Token: 0x04006DFF RID: 28159
			public string originalDefName;

			// Token: 0x04006E00 RID: 28160
			public Thing thing;
		}
	}
}
