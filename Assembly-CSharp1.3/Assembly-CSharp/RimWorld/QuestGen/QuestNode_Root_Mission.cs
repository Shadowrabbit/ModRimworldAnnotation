using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x020016FF RID: 5887
	public abstract class QuestNode_Root_Mission : QuestNode
	{
		// Token: 0x17001621 RID: 5665
		// (get) Token: 0x060087DC RID: 34780
		protected abstract string QuestTag { get; }

		// Token: 0x17001622 RID: 5666
		// (get) Token: 0x060087DD RID: 34781 RVA: 0x0001276E File Offset: 0x0001096E
		protected virtual bool AddCampLootReward
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001623 RID: 5667
		// (get) Token: 0x060087DE RID: 34782 RVA: 0x000126F5 File Offset: 0x000108F5
		protected virtual bool IsViolent
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060087DF RID: 34783
		protected abstract Pawn GetAsker(Quest quest);

		// Token: 0x060087E0 RID: 34784 RVA: 0x000126F5 File Offset: 0x000108F5
		protected virtual bool CanGetAsker()
		{
			return true;
		}

		// Token: 0x060087E1 RID: 34785
		protected abstract int GetRequiredPawnCount(int population, float threatPoints);

		// Token: 0x060087E2 RID: 34786 RVA: 0x000126F5 File Offset: 0x000108F5
		protected virtual bool DoesPawnCountAsAvailableForFight(Pawn p)
		{
			return true;
		}

		// Token: 0x060087E3 RID: 34787
		protected abstract Site GenerateSite(Pawn asker, float threatPoints, int pawnCount, int population, int tile);

		// Token: 0x060087E4 RID: 34788 RVA: 0x0030B119 File Offset: 0x00309319
		protected virtual bool TryFindSiteTile(out int tile, bool exitOnFirstTileFound = false)
		{
			return TileFinder.TryFindNewSiteTile(out tile, 80, 85, false, TileFinderMode.Near, -1, exitOnFirstTileFound);
		}

		// Token: 0x060087E5 RID: 34789 RVA: 0x0030B12C File Offset: 0x0030932C
		private void ResolveParameters(Slate slate, out int requiredPawnCount, out int population, out Map colonyMap)
		{
			try
			{
				foreach (Map map in Find.Maps)
				{
					if (map.IsPlayerHome)
					{
						QuestNode_Root_Mission.tmpMaps.Add(map);
					}
				}
				colonyMap = QuestNode_Root_Mission.tmpMaps.RandomElementWithFallback(null);
				population = (slate.Exists("population", false) ? slate.Get<int>("population", 0, false) : (from c in colonyMap.mapPawns.FreeColonists
				where this.DoesPawnCountAsAvailableForFight(c)
				select c).Count<Pawn>());
				requiredPawnCount = this.GetRequiredPawnCount(population, (float)slate.Get<int>("points", 0, false));
			}
			finally
			{
				QuestNode_Root_Mission.tmpMaps.Clear();
			}
		}

		// Token: 0x060087E6 RID: 34790 RVA: 0x0030B20C File Offset: 0x0030940C
		protected override void RunInt()
		{
			if (!ModLister.CheckRoyalty("Mission"))
			{
				return;
			}
			Quest quest = QuestGen.quest;
			Slate slate = QuestGen.slate;
			string text = QuestGenUtility.HardcodedTargetQuestTagWithQuestID(this.QuestTag);
			QuestGenUtility.RunAdjustPointsForDistantFight();
			int num = slate.Get<int>("points", 0, false);
			Pawn asker = this.GetAsker(quest);
			int num2;
			int population;
			Map map;
			this.ResolveParameters(slate, out num2, out population, out map);
			int tile;
			this.TryFindSiteTile(out tile, false);
			slate.Set<Pawn>("asker", asker, false);
			slate.Set<Faction>("askerFaction", asker.Faction, false);
			slate.Set<int>("requiredPawnCount", num2, false);
			slate.Set<Map>("map", map, false);
			Site site = this.GenerateSite(asker, (float)num, num2, population, tile);
			string inSignal = QuestGenUtility.HardcodedSignalWithQuestID("askerFaction.BecameHostileToPlayer");
			string text2 = QuestGenUtility.QuestTagSignal(text, "AllEnemiesDefeated");
			string signalSentSatisfied = QuestGenUtility.HardcodedSignalWithQuestID("shuttle.SentSatisfied");
			QuestGenUtility.HardcodedSignalWithQuestID("shuttle.Spawned");
			string text3 = QuestGenUtility.QuestTagSignal(text, "MapRemoved");
			string signalChosenPawn = QuestGen.GenerateNewSignal("ChosenPawnSignal", true);
			quest.GiveRewards(new RewardsGeneratorParams
			{
				allowGoodwill = true,
				allowRoyalFavor = true,
				giverFaction = asker.Faction,
				rewardValue = QuestNode_Root_Mission.RewardValueCurve.Evaluate((float)num),
				chosenPawnSignal = signalChosenPawn
			}, text2, null, null, null, null, null, delegate
			{
				Quest quest2 = quest;
				LetterDef choosePawn = LetterDefOf.ChoosePawn;
				string inSignal3 = null;
				string royalFavorLabel = asker.Faction.def.royalFavorLabel;
				string text4 = "LetterTextHonorAward_BanditCamp".Translate(asker.Faction.def.royalFavorLabel);
				quest2.Letter(choosePawn, inSignal3, signalChosenPawn, null, null, false, QuestPart.SignalListenMode.OngoingOnly, null, false, text4, null, royalFavorLabel, null, signalSentSatisfied);
			}, null, this.AddCampLootReward, asker, false, false);
			Thing shuttle = QuestGen_Shuttle.GenerateShuttle(null, null, null, true, true, false, num2, true, true, false, true, site, map.Parent, num2, null, false, false, false, false);
			slate.Set<Thing>("shuttle", shuttle, false);
			QuestUtility.AddQuestTag(ref shuttle.questTags, text);
			quest.SpawnWorldObject(site, null, null);
			TransportShip transportShip = quest.GenerateTransportShip(TransportShipDefOf.Ship_Shuttle, null, shuttle, null).transportShip;
			slate.Set<TransportShip>("transportShip", transportShip, false);
			QuestUtility.AddQuestTag(ref transportShip.questTags, text);
			quest.SendTransportShipAwayOnCleanup(transportShip, true, TransportShipDropMode.None);
			quest.AddShipJob_Arrive(transportShip, map.Parent, null, ShipJobStartMode.Instant, Faction.OfEmpire, null);
			quest.AddShipJob_WaitSendable(transportShip, site, true, null);
			quest.AddShipJob(transportShip, ShipJobDefOf.Unload, ShipJobStartMode.Queue, null);
			quest.AddShipJob_WaitSendable(transportShip, map.Parent, true, null);
			quest.AddShipJob(transportShip, ShipJobDefOf.Unload, ShipJobStartMode.Queue, null);
			quest.AddShipJob_FlyAway(transportShip, -1, null, TransportShipDropMode.None, null);
			quest.TendPawns(null, shuttle, signalSentSatisfied);
			quest.RequiredShuttleThings(shuttle, site, QuestGenUtility.HardcodedSignalWithQuestID("transportShip.FlewAway"), true, -1);
			quest.ShuttleLeaveDelay(shuttle, 60000, null, Gen.YieldSingle<string>(signalSentSatisfied), null, delegate
			{
				quest.End(QuestEndOutcome.Fail, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, true);
			});
			string inSignal2 = QuestGenUtility.HardcodedSignalWithQuestID("shuttle.Killed");
			quest.FactionGoodwillChange(asker.Faction, 0, inSignal2, true, true, true, HistoryEventDefOf.ShuttleDestroyed, QuestPart.SignalListenMode.OngoingOnly, true);
			quest.End(QuestEndOutcome.Fail, 0, null, inSignal2, QuestPart.SignalListenMode.OngoingOnly, true);
			quest.SignalPass(delegate
			{
				quest.End(QuestEndOutcome.Fail, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, true);
			}, inSignal, null);
			quest.FeedPawns(null, shuttle, signalSentSatisfied);
			QuestUtility.AddQuestTag(ref site.questTags, text);
			slate.Set<Site>("site", site, false);
			quest.SignalPassActivable(delegate
			{
				quest.Message("MessageMissionGetBackToShuttle".Translate(site.Faction.Named("FACTION")), MessageTypeDefOf.PositiveEvent, false, null, new LookTargets(shuttle), null);
				quest.Notify_PlayerRaidedSomeone(null, site, null);
			}, signalSentSatisfied, text2, null, null, null, false);
			quest.SignalPassAllSequence(delegate
			{
				quest.End(QuestEndOutcome.Success, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, true);
			}, new List<string>
			{
				signalSentSatisfied,
				text2,
				text3
			}, null);
			Quest quest3 = quest;
			Action action = delegate()
			{
				quest.End(QuestEndOutcome.Fail, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, true);
			};
			string inSignalEnable = null;
			string inSignalDisable = text2;
			quest3.SignalPassActivable(action, inSignalEnable, text3, null, null, inSignalDisable, false);
			int num3 = (int)(this.timeLimitDays.RandomInRange * 60000f);
			slate.Set<int>("timeoutTicks", num3, false);
			quest.WorldObjectTimeout(site, num3, null, null, false, null, true);
			List<Rule> list = new List<Rule>();
			list.AddRange(GrammarUtility.RulesForWorldObject("site", site, true));
			QuestGen.AddQuestDescriptionRules(list);
		}

		// Token: 0x060087E7 RID: 34791 RVA: 0x0030B6E0 File Offset: 0x003098E0
		protected override bool TestRunInt(Slate slate)
		{
			QuestGenUtility.TestRunAdjustPointsForDistantFight(slate);
			if (!ModLister.CheckRoyalty("Mission"))
			{
				return false;
			}
			if (this.IsViolent && !Find.Storyteller.difficulty.allowViolentQuests)
			{
				return false;
			}
			int num;
			int num2;
			Map map;
			this.ResolveParameters(slate, out num, out num2, out map);
			return num != -1 && (this.CanGetAsker() && map != null) && this.TryFindSiteTile(out num2, true);
		}

		// Token: 0x040055DE RID: 21982
		public const int MinTilesAwayFromColony = 80;

		// Token: 0x040055DF RID: 21983
		public const int MaxTilesAwayFromColony = 85;

		// Token: 0x040055E0 RID: 21984
		private static readonly SimpleCurve RewardValueCurve = new SimpleCurve
		{
			{
				new CurvePoint(200f, 550f),
				true
			},
			{
				new CurvePoint(400f, 1100f),
				true
			},
			{
				new CurvePoint(800f, 1600f),
				true
			},
			{
				new CurvePoint(1600f, 2600f),
				true
			},
			{
				new CurvePoint(3200f, 3600f),
				true
			},
			{
				new CurvePoint(30000f, 10000f),
				true
			}
		};

		// Token: 0x040055E1 RID: 21985
		public FloatRange timeLimitDays = new FloatRange(2f, 5f);

		// Token: 0x040055E2 RID: 21986
		private static List<Map> tmpMaps = new List<Map>();
	}
}
