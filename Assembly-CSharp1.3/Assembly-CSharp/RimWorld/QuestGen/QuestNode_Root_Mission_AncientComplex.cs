using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001700 RID: 5888
	public class QuestNode_Root_Mission_AncientComplex : QuestNode_Root_AncientComplex
	{
		// Token: 0x060087EB RID: 34795 RVA: 0x0030B814 File Offset: 0x00309A14
		protected override void RunInt()
		{
			if (!ModLister.CheckRoyaltyAndIdeology("Ancient Complex mission"))
			{
				return;
			}
			Slate slate = QuestGen.slate;
			Quest quest = QuestGen.quest;
			float points = slate.Get<float>("points", 0f, false);
			string questTagToAdd = QuestGenUtility.HardcodedTargetQuestTagWithQuestID("AncientComplexMission");
			Precept_Relic precept_Relic = slate.Get<Precept_Relic>("relic", null, false);
			if (precept_Relic == null)
			{
				precept_Relic = Faction.OfPlayer.ideos.PrimaryIdeo.GetAllPreceptsOfType<Precept_Relic>().RandomElement<Precept_Relic>();
				Log.Warning("Ancient Complex quest requires relic from parent quest. None found so picking random player relic");
			}
			Faction faction;
			this.TryFindEnemyFaction(out faction);
			Pawn pawn;
			this.TryFindAsker(faction, out pawn);
			int tile;
			TileFinder.TryFindNewSiteTile(out tile, 5, 85, false, TileFinderMode.Furthest, -1, false);
			Map map;
			this.TryFindColonyMapWithFreeColonists(out map, slate, points);
			int population = slate.Exists("population", false) ? slate.Get<int>("population", 0, false) : map.mapPawns.FreeColonists.Where(new Func<Pawn, bool>(this.IsPawnAllowed)).Count<Pawn>();
			int num;
			this.TryGetRequiredColonistCount(map, points, population, out num);
			string text = QuestGenUtility.HardcodedSignalWithQuestID("shuttle.SentSatisfied");
			string inSignal = QuestGenUtility.HardcodedSignalWithQuestID("askerFaction.BecameHostileToPlayer");
			string inSignal2 = QuestGenUtility.HardcodedSignalWithQuestID("terminals.Destroyed");
			string text2 = QuestGenUtility.HardcodedSignalWithQuestID("site.Destroyed");
			string text3 = QuestGen.GenerateNewSignal("TerminalHacked", true);
			string text4 = QuestGen.GenerateNewSignal("AllTerminalsHacked", true);
			QuestGen.GenerateNewSignal("RaidArrives", true);
			string text5 = QuestGen.GenerateNewSignal("MissionSuccess", true);
			string text6 = QuestGen.GenerateNewSignal("RequiredThingsLoaded", true);
			string text7 = QuestGen.GenerateNewSignal("SendShuttleAway", true);
			string text8 = QuestGen.GenerateNewSignal("EmptyShuttle", true);
			string text9 = QuestGen.GenerateNewSignal("CheckShuttleContents", true);
			string text10 = QuestGen.GenerateNewSignal("ShipThingAdded", true);
			ComplexSketch complexSketch = this.GenerateSketch(points, true);
			complexSketch.thingDiscoveredMessage = "MessageAncientTerminalDiscovered".Translate(precept_Relic.Label);
			List<string> list = new List<string>();
			for (int i = 0; i < complexSketch.thingsToSpawn.Count; i++)
			{
				Thing thing = complexSketch.thingsToSpawn[i];
				string text11 = QuestGenUtility.HardcodedTargetQuestTagWithQuestID("terminal" + i);
				QuestUtility.AddQuestTag(thing, text11);
				string item = QuestGenUtility.HardcodedSignalWithQuestID(text11 + ".Hacked");
				list.Add(item);
				thing.TryGetComp<CompHackable>().defence = (float)(Rand.Chance(0.5f) ? QuestNode_Root_Mission_AncientComplex.HackDefenceRange.min : QuestNode_Root_Mission_AncientComplex.HackDefenceRange.max);
			}
			float num2 = Find.Storyteller.difficulty.allowViolentQuests ? QuestNode_Root_Mission_AncientComplex.GetThreatPoints(map.wealthWatcher.WealthTotal, points, num) : 0f;
			SitePartParams parms = new SitePartParams
			{
				ancientComplexSketch = complexSketch,
				threatPoints = num2,
				ancientComplexRewardMaker = ThingSetMakerDefOf.MapGen_AncientComplexRoomLoot_Default
			};
			SitePartDefWithParams val = new SitePartDefWithParams(SitePartDefOf.AncientComplex, parms);
			Site site = QuestGen_Sites.GenerateSite(Gen.YieldSingle<SitePartDefWithParams>(val), tile, Faction.OfAncients, false, null);
			TimedDetectionRaids component = site.GetComponent<TimedDetectionRaids>();
			if (component != null)
			{
				component.alertRaidsArrivingIn = true;
			}
			QuestPart_PassAllActivable questPart_PassAllActivable = new QuestPart_PassAllActivable();
			questPart_PassAllActivable.inSignalEnable = QuestGen.slate.Get<string>("inSignal", null, false);
			questPart_PassAllActivable.inSignals = list;
			questPart_PassAllActivable.outSignalsCompleted.Add(text4);
			questPart_PassAllActivable.outSignalAny = text3;
			questPart_PassAllActivable.expiryInfoPartKey = "TerminalsHacked";
			quest.AddPart(questPart_PassAllActivable);
			quest.Message("[terminalHackedMessage]", null, true, null, null, text3);
			if (Find.Storyteller.difficulty.allowViolentQuests && Rand.Chance(0.5f))
			{
				quest.RandomRaid(site, QuestNode_Root_Mission_AncientComplex.RandomRaidPointsFactorRange * num2, faction, text4, PawnsArrivalModeDefOf.EdgeWalkIn, RaidStrategyDefOf.ImmediateAttack, null, null);
			}
			Thing shuttle = QuestGen_Shuttle.GenerateShuttle(null, null, null, true, false, false, num, true, false, false, true, site, map.Parent, num, null, false, false, false, false);
			slate.Set<Thing>("shuttle", shuttle, false);
			QuestUtility.AddQuestTag(ref shuttle.questTags, questTagToAdd);
			quest.Message("MessageAncientComplexBackToShuttle".Translate(precept_Relic.Label), MessageTypeDefOf.PositiveEvent, false, null, new LookTargets(shuttle), text4);
			TransportShip transportShip = quest.GenerateTransportShip(TransportShipDefOf.Ship_Shuttle, null, shuttle, null).transportShip;
			slate.Set<TransportShip>("transportShip", transportShip, false);
			QuestUtility.AddQuestTag(ref transportShip.questTags, questTagToAdd);
			quest.SendTransportShipAwayOnCleanup(transportShip, true, TransportShipDropMode.None);
			QuestPart_PassWhileActive questPart_PassWhileActive = new QuestPart_PassWhileActive();
			questPart_PassWhileActive.inSignalEnable = slate.Get<string>("inSignal", null, false);
			questPart_PassWhileActive.inSignal = QuestGenUtility.HardcodedSignalWithQuestID("shuttle.ThingAdded");
			questPart_PassWhileActive.outSignal = text10;
			questPart_PassWhileActive.inSignalDisable = text7;
			quest.AddPart(questPart_PassWhileActive);
			QuestPart_Filter_AllRequiredThingsLoaded questPart_Filter_AllRequiredThingsLoaded = new QuestPart_Filter_AllRequiredThingsLoaded();
			questPart_Filter_AllRequiredThingsLoaded.inSignal = text10;
			questPart_Filter_AllRequiredThingsLoaded.shuttle = shuttle;
			questPart_Filter_AllRequiredThingsLoaded.outSignal = text6;
			quest.AddPart(questPart_Filter_AllRequiredThingsLoaded);
			QuestPart_Filter_AnyOnTransporterCapableOfHacking questPart_Filter_AnyOnTransporterCapableOfHacking = new QuestPart_Filter_AnyOnTransporterCapableOfHacking();
			questPart_Filter_AnyOnTransporterCapableOfHacking.transporter = shuttle;
			questPart_Filter_AnyOnTransporterCapableOfHacking.inSignal = text6;
			questPart_Filter_AnyOnTransporterCapableOfHacking.outSignal = text7;
			questPart_Filter_AnyOnTransporterCapableOfHacking.outSignalElse = text9;
			quest.AddPart(questPart_Filter_AnyOnTransporterCapableOfHacking);
			QuestPart_Dialog.Option option = new QuestPart_Dialog.Option();
			option.text = "SendShuttleAnyway".Translate();
			option.outSignal = text7;
			QuestPart_Dialog.Option option2 = new QuestPart_Dialog.Option();
			option2.text = "ReLoadShuttle".Translate();
			option2.outSignal = text8;
			QuestPart_Dialog dialog = new QuestPart_Dialog();
			dialog.title = "[passengersIncapableOfHackingDialogLabel]";
			dialog.text = "[passengersIncapableOfHackingDialogText]";
			dialog.options.Add(option);
			dialog.options.Add(option2);
			dialog.inSignal = text9;
			QuestGen.AddTextRequest("root", delegate(string x)
			{
				dialog.title = x;
			}, QuestGenUtility.MergeRules(null, dialog.title, "root"));
			QuestGen.AddTextRequest("root", delegate(string x)
			{
				dialog.text = x;
			}, QuestGenUtility.MergeRules(null, dialog.text, "root"));
			quest.AddPart(dialog);
			quest.AddShipJob_Arrive(transportShip, map.Parent, null, ShipJobStartMode.Instant, Faction.OfEmpire, null);
			quest.AddShipJob(transportShip, ShipJobDefOf.Unload, ShipJobStartMode.Force, text8);
			quest.Signal(text7, delegate
			{
				quest.SpawnWorldObject(site, null, null);
				quest.TendPawns(null, shuttle, null);
				quest.AddShipJob_WaitSendable(transportShip, site, true, null);
				quest.AddShipJob(transportShip, ShipJobDefOf.Unload, ShipJobStartMode.Queue, null);
				quest.AddShipJob_WaitSendable(transportShip, map.Parent, true, null);
				quest.AddShipJob(transportShip, ShipJobDefOf.Unload, ShipJobStartMode.Queue, null);
				quest.AddShipJob_FlyAway(transportShip, -1, null, TransportShipDropMode.None, null);
			}, null, QuestPart.SignalListenMode.OngoingOnly);
			quest.RequiredShuttleThings(shuttle, site, QuestGenUtility.HardcodedSignalWithQuestID("transportShip.FlewAway"), true, -1);
			quest.ShuttleLeaveDelay(shuttle, 60000, null, Gen.YieldSingle<string>(text), null, delegate
			{
				quest.End(QuestEndOutcome.Fail, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, true);
			});
			string inSignal3 = QuestGenUtility.HardcodedSignalWithQuestID("shuttle.Destroyed");
			quest.FactionGoodwillChange(pawn.Faction, 0, inSignal3, true, true, true, HistoryEventDefOf.ShuttleDestroyed, QuestPart.SignalListenMode.OngoingOnly, true);
			quest.End(QuestEndOutcome.Fail, 0, null, inSignal3, QuestPart.SignalListenMode.OngoingOnly, true);
			quest.SignalPass(delegate
			{
				quest.End(QuestEndOutcome.Fail, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, true);
			}, inSignal, null);
			quest.FeedPawns(null, shuttle, text);
			quest.TendPawns(null, shuttle, text);
			Reward_RelicInfo reward_RelicInfo = new Reward_RelicInfo();
			reward_RelicInfo.relic = precept_Relic;
			reward_RelicInfo.quest = quest;
			QuestPart_Choice questPart_Choice = quest.RewardChoice(null, null);
			QuestPart_Choice.Choice choice = new QuestPart_Choice.Choice();
			choice.rewards.Add(reward_RelicInfo);
			questPart_Choice.choices.Add(choice);
			int num3 = (int)(this.TimeLimitDays.RandomInRange * 60000f);
			slate.Set<int>("timeoutTicks", num3, false);
			quest.WorldObjectTimeout(site, num3, null, null, false, null, true);
			QuestPart_Filter_Hacked questPart_Filter_Hacked = new QuestPart_Filter_Hacked();
			questPart_Filter_Hacked.inSignal = inSignal2;
			questPart_Filter_Hacked.outSignalElse = QuestGen.GenerateNewSignal("FailQuestTerminalDestroyed", true);
			quest.AddPart(questPart_Filter_Hacked);
			quest.End(QuestEndOutcome.Fail, 0, null, questPart_Filter_Hacked.outSignalElse, QuestPart.SignalListenMode.OngoingOnly, true);
			Quest quest3 = quest;
			Action action = delegate()
			{
				quest.End(QuestEndOutcome.Fail, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, true);
			};
			string inSignalEnable = null;
			string inSignalDisable = text4;
			quest3.SignalPassActivable(action, inSignalEnable, text2, null, null, inSignalDisable, false);
			QuestPart_PassAll questPart_PassAll = new QuestPart_PassAll();
			questPart_PassAll.inSignals.Add(text4);
			questPart_PassAll.inSignals.Add(text2);
			questPart_PassAll.outSignal = text5;
			quest.AddPart(questPart_PassAll);
			quest.End(QuestEndOutcome.Success, 0, null, text5, QuestPart.SignalListenMode.OngoingOnly, true);
			QuestPart_PawnKilled questPart_PawnKilled = new QuestPart_PawnKilled();
			questPart_PawnKilled.mapParent = site;
			questPart_PawnKilled.faction = Faction.OfPlayer;
			questPart_PawnKilled.outSignal = QuestGen.GenerateNewSignal("PawnKilled", true);
			quest.AddPart(questPart_PawnKilled);
			quest.SignalPassActivable(delegate
			{
				QuestPart_Filter_AnyColonistCapableOfHacking questPart_Filter_AnyColonistCapableOfHacking = new QuestPart_Filter_AnyColonistCapableOfHacking();
				questPart_Filter_AnyColonistCapableOfHacking.mapParent = site;
				questPart_Filter_AnyColonistCapableOfHacking.inSignal = slate.Get<string>("inSignal", null, false);
				questPart_Filter_AnyColonistCapableOfHacking.outSignalElse = QuestGen.GenerateNewSignal("NoHackerLetter", true);
				quest.AddPart(questPart_Filter_AnyColonistCapableOfHacking);
				Quest quest2 = quest;
				LetterDef negativeEvent = LetterDefOf.NegativeEvent;
				string outSignalElse = questPart_Filter_AnyColonistCapableOfHacking.outSignalElse;
				string chosenPawnSignal = null;
				Faction relatedFaction = null;
				MapParent useColonistsOnMap = null;
				bool useColonistsFromCaravanArg = false;
				QuestPart.SignalListenMode signalListenMode = QuestPart.SignalListenMode.OngoingOnly;
				IEnumerable<object> lookTargets = null;
				bool filterDeadPawnsFromLookTargets = false;
				string label = "LetterLabelLastHackerLost".Translate();
				quest2.Letter(negativeEvent, outSignalElse, chosenPawnSignal, relatedFaction, useColonistsOnMap, useColonistsFromCaravanArg, signalListenMode, lookTargets, filterDeadPawnsFromLookTargets, "LetterTextLastHackerLost".Translate(), null, label, null, null);
			}, null, questPart_PawnKilled.outSignal, null, null, null, false);
			slate.Set<Precept_Relic>("relic", precept_Relic, false);
			slate.Set<List<Thing>>("terminals", complexSketch.thingsToSpawn, false);
			slate.Set<int>("terminalCount", complexSketch.thingsToSpawn.Count, false);
			slate.Set<Pawn>("asker", pawn, false);
			slate.Set<Faction>("enemyFaction", faction, false);
			slate.Set<int>("colonistCount", num, false);
			slate.Set<Site>("site", site, false);
		}

		// Token: 0x060087EC RID: 34796 RVA: 0x0030C229 File Offset: 0x0030A429
		private static float GetThreatPoints(float colonyWealth, float points, int pawnCount)
		{
			return QuestNode_Root_AncientComplex.ThreatPointsOverPointsCurve.Evaluate(points) * QuestNode_Root_Mission_AncientComplex.ThreatPointsFactorOverColonyWealthCurve.Evaluate(colonyWealth) * QuestNode_Root_Mission_AncientComplex.ThreatPointsFactorOverPawnCountCurve.Evaluate((float)pawnCount);
		}

		// Token: 0x060087ED RID: 34797 RVA: 0x0030C250 File Offset: 0x0030A450
		public int GetRequiredColonistCount(Map map, float points)
		{
			int min = (int)QuestNode_Root_Mission_AncientComplex.MinFreeColonistsOverPointsCurve.Evaluate(points);
			int count = map.mapPawns.FreeColonists.Count;
			return Rand.Range(min, count);
		}

		// Token: 0x060087EE RID: 34798 RVA: 0x0030C280 File Offset: 0x0030A480
		private bool TryFindEnemyFaction(out Faction enemyFaction)
		{
			Faction faction;
			if ((from f in Find.FactionManager.AllFactionsVisible
			where f.HostileTo(Faction.OfPlayer)
			select f).TryRandomElement(out faction))
			{
				enemyFaction = faction;
				return true;
			}
			enemyFaction = null;
			return false;
		}

		// Token: 0x060087EF RID: 34799 RVA: 0x0030C2D0 File Offset: 0x0030A4D0
		private bool TryFindAsker(Faction enemyFaction, out Pawn pawn)
		{
			IEnumerable<Faction> source = from f in Find.FactionManager.AllFactionsVisible
			where !f.IsPlayer && !f.HostileTo(Faction.OfPlayer) && f.HostileTo(enemyFaction) && f.leader != null
			select f;
			Faction faction;
			if ((from f in source
			where f.def.techLevel >= TechLevel.Industrial
			select f).TryRandomElement(out faction))
			{
				pawn = faction.leader;
				return true;
			}
			Faction faction2;
			if (source.TryRandomElement(out faction2))
			{
				pawn = faction2.leader;
				return true;
			}
			pawn = null;
			return false;
		}

		// Token: 0x060087F0 RID: 34800 RVA: 0x0030C358 File Offset: 0x0030A558
		private bool TryFindColonyMapWithFreeColonists(out Map map, Slate slate, float points)
		{
			int value = (int)QuestNode_Root_Mission_AncientComplex.MinFreeColonistsOverPointsCurve.Evaluate(points);
			map = QuestGen_Get.GetMap(false, new int?(value));
			return map != null;
		}

		// Token: 0x060087F1 RID: 34801 RVA: 0x0030C385 File Offset: 0x0030A585
		private bool TryFindSiteTile(out int tile)
		{
			return TileFinder.TryFindNewSiteTile(out tile, 5, 85, false, TileFinderMode.Near, -1, false);
		}

		// Token: 0x060087F2 RID: 34802 RVA: 0x0030C394 File Offset: 0x0030A594
		private bool TryGetRequiredColonistCount(Map map, float points, int population, out int requiredColonistCount)
		{
			requiredColonistCount = -1;
			int num = (int)QuestNode_Root_Mission_AncientComplex.MinFreeColonistsOverPointsCurve.Evaluate(points);
			if (population <= num)
			{
				return false;
			}
			requiredColonistCount = Rand.Range(num, population);
			return true;
		}

		// Token: 0x060087F3 RID: 34803 RVA: 0x0030C3C3 File Offset: 0x0030A5C3
		private bool IsPawnAllowed(Pawn pawn)
		{
			return !pawn.IsSlave && !pawn.IsQuestLodger() && !pawn.Downed && pawn.health.hediffSet.BleedRateTotal <= 0f;
		}

		// Token: 0x060087F4 RID: 34804 RVA: 0x0030C3FC File Offset: 0x0030A5FC
		protected override bool TestRunInt(Slate slate)
		{
			float points = slate.Get<float>("points", 0f, false);
			Faction enemyFaction;
			Pawn pawn;
			if (!this.TryFindEnemyFaction(out enemyFaction) || !this.TryFindAsker(enemyFaction, out pawn))
			{
				return false;
			}
			Map map;
			if (!this.TryFindColonyMapWithFreeColonists(out map, slate, points))
			{
				return false;
			}
			int num;
			if (!TileFinder.TryFindNewSiteTile(out num, 5, 85, false, TileFinderMode.Near, -1, true))
			{
				return false;
			}
			int population = slate.Exists("population", false) ? slate.Get<int>("population", 0, false) : map.mapPawns.FreeColonists.Where(new Func<Pawn, bool>(this.IsPawnAllowed)).Count<Pawn>();
			return this.TryGetRequiredColonistCount(map, points, population, out num);
		}

		// Token: 0x060087F5 RID: 34805 RVA: 0x0030C49C File Offset: 0x0030A69C
		[DebugOutput("Quests", false)]
		public static void MissionAncientComplex()
		{
			List<Tuple<float, int, float>> list = new List<Tuple<float, int, float>>();
			int[] array = new int[]
			{
				1,
				5,
				10,
				20
			};
			float[] array2 = new float[]
			{
				10000f,
				100000f,
				1000000f
			};
			foreach (float item in DebugActionsUtility.PointsOptions(false))
			{
				foreach (int item2 in array)
				{
					foreach (float item3 in array2)
					{
						list.Add(new Tuple<float, int, float>(item, item2, item3));
					}
				}
			}
			IEnumerable<Tuple<float, int, float>> dataSources = list;
			TableDataGetter<Tuple<float, int, float>>[] array5 = new TableDataGetter<Tuple<float, int, float>>[6];
			array5[0] = new TableDataGetter<Tuple<float, int, float>>("points", (Tuple<float, int, float> x) => x.Item1);
			array5[1] = new TableDataGetter<Tuple<float, int, float>>("wealth", (Tuple<float, int, float> x) => x.Item3);
			array5[2] = new TableDataGetter<Tuple<float, int, float>>("wealth threat factor", (Tuple<float, int, float> x) => string.Format("x{0}", QuestNode_Root_Mission_AncientComplex.ThreatPointsFactorOverColonyWealthCurve.Evaluate(x.Item3)));
			array5[3] = new TableDataGetter<Tuple<float, int, float>>("colonists", (Tuple<float, int, float> x) => x.Item2);
			array5[4] = new TableDataGetter<Tuple<float, int, float>>("colonist threat factor", (Tuple<float, int, float> x) => string.Format("x{0}", QuestNode_Root_Mission_AncientComplex.ThreatPointsFactorOverPawnCountCurve.Evaluate((float)x.Item2)));
			array5[5] = new TableDataGetter<Tuple<float, int, float>>("threat points", (Tuple<float, int, float> x) => QuestNode_Root_Mission_AncientComplex.GetThreatPoints(x.Item3, x.Item1, x.Item2));
			DebugTables.MakeTablesDialog<Tuple<float, int, float>>(dataSources, array5);
		}

		// Token: 0x040055E3 RID: 21987
		private const int MinTilesFromColony = 5;

		// Token: 0x040055E4 RID: 21988
		private const int MaxTilesFromColony = 85;

		// Token: 0x040055E5 RID: 21989
		private FloatRange TimeLimitDays = new FloatRange(2f, 5f);

		// Token: 0x040055E6 RID: 21990
		private static IntRange HackDefenceRange = new IntRange(300, 1800);

		// Token: 0x040055E7 RID: 21991
		private const float MinMaxHackDefenceChance = 0.5f;

		// Token: 0x040055E8 RID: 21992
		private static SimpleCurve MinFreeColonistsOverPointsCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 3f),
				true
			},
			{
				new CurvePoint(500f, 3f),
				true
			},
			{
				new CurvePoint(10000f, 8f),
				true
			}
		};

		// Token: 0x040055E9 RID: 21993
		private static SimpleCurve ThreatPointsFactorOverColonyWealthCurve = new SimpleCurve
		{
			{
				new CurvePoint(10000f, 0.5f),
				true
			},
			{
				new CurvePoint(100000f, 1f),
				true
			},
			{
				new CurvePoint(1000000f, 1.5f),
				true
			}
		};

		// Token: 0x040055EA RID: 21994
		private static SimpleCurve ThreatPointsFactorOverPawnCountCurve = new SimpleCurve
		{
			{
				new CurvePoint(1f, 0.5f),
				true
			},
			{
				new CurvePoint(2f, 0.55f),
				true
			},
			{
				new CurvePoint(5f, 0.75f),
				true
			},
			{
				new CurvePoint(20f, 5f),
				true
			}
		};

		// Token: 0x040055EB RID: 21995
		private static readonly FloatRange RandomRaidPointsFactorRange = new FloatRange(0.45f, 0.55f);

		// Token: 0x040055EC RID: 21996
		private const string RootSymbol = "root";

		// Token: 0x040055ED RID: 21997
		private const float ChanceToSpawnAllTerminalsHackedRaid = 0.5f;
	}
}
