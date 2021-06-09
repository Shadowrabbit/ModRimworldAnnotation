using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x02001FD5 RID: 8149
	public abstract class QuestNode_Root_Mission : QuestNode
	{
		// Token: 0x17001980 RID: 6528
		// (get) Token: 0x0600ACCF RID: 44239
		protected abstract string QuestTag { get; }

		// Token: 0x17001981 RID: 6529
		// (get) Token: 0x0600ACD0 RID: 44240 RVA: 0x0000A2E4 File Offset: 0x000084E4
		protected virtual bool AddCampLootReward
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001982 RID: 6530
		// (get) Token: 0x0600ACD1 RID: 44241 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected virtual bool IsViolent
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600ACD2 RID: 44242
		protected abstract Pawn GetAsker(Quest quest);

		// Token: 0x0600ACD3 RID: 44243 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected virtual bool CanGetAsker()
		{
			return true;
		}

		// Token: 0x0600ACD4 RID: 44244
		protected abstract int GetRequiredPawnCount(int population, float threatPoints);

		// Token: 0x0600ACD5 RID: 44245 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected virtual bool DoesPawnCountAsAvailableForFight(Pawn p)
		{
			return true;
		}

		// Token: 0x0600ACD6 RID: 44246
		protected abstract Site GenerateSite(Pawn asker, float threatPoints, int pawnCount, int population, int tile);

		// Token: 0x0600ACD7 RID: 44247 RVA: 0x00070B0F File Offset: 0x0006ED0F
		protected virtual bool TryFindSiteTile(out int tile)
		{
			return TileFinder.TryFindNewSiteTile(out tile, 80, 85, false, true, -1);
		}

		// Token: 0x0600ACD8 RID: 44248 RVA: 0x00324C30 File Offset: 0x00322E30
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

		// Token: 0x0600ACD9 RID: 44249 RVA: 0x00324D10 File Offset: 0x00322F10
		protected override void RunInt()
		{
			if (!ModLister.RoyaltyInstalled)
			{
				Log.ErrorOnce("Missions are a Royalty-specific game system. If you want to use this code please check ModLister.RoyaltyInstalled before calling it. See rules on the Ludeon forum for more info.", 324345634, false);
				return;
			}
			Quest quest = QuestGen.quest;
			Slate slate = QuestGen.slate;
			string text = QuestGenUtility.HardcodedTargetQuestTagWithQuestID(this.QuestTag);
			int num = slate.Get<int>("points", 0, false);
			Pawn asker = this.GetAsker(quest);
			int num2;
			int population;
			Map map;
			this.ResolveParameters(slate, out num2, out population, out map);
			int tile;
			this.TryFindSiteTile(out tile);
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
				Quest quest3 = quest;
				LetterDef choosePawn = LetterDefOf.ChoosePawn;
				string inSignal3 = null;
				string royalFavorLabel = asker.Faction.def.royalFavorLabel;
				string text4 = "LetterTextHonorAward_BanditCamp".Translate(asker.Faction.def.royalFavorLabel);
				quest3.Letter(choosePawn, inSignal3, signalChosenPawn, null, null, false, QuestPart.SignalListenMode.OngoingOnly, null, false, text4, null, royalFavorLabel, null, signalSentSatisfied);
			}, null, this.AddCampLootReward, asker, false, false);
			Thing shuttle = QuestGen_Shuttle.GenerateShuttle(null, null, null, true, true, false, num2, true, true, false, true, site, map.Parent, num2, null, false, false, null);
			shuttle.TryGetComp<CompShuttle>().sendAwayIfQuestFinished = quest;
			slate.Set<Thing>("shuttle", shuttle, false);
			quest.SpawnWorldObject(site, null, signalSentSatisfied);
			QuestUtility.AddQuestTag(ref shuttle.questTags, text);
			quest.SpawnSkyfaller(map, ThingDefOf.ShuttleIncoming, Gen.YieldSingle<Thing>(shuttle), Faction.OfPlayer, null, null, true, true, null, null);
			quest.ShuttleLeaveDelay(shuttle, 60000, null, Gen.YieldSingle<string>(signalSentSatisfied), null, delegate
			{
				quest.SendShuttleAway(shuttle, true, null);
				quest.End(QuestEndOutcome.Fail, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, true);
			});
			string inSignal2 = QuestGenUtility.HardcodedSignalWithQuestID("shuttle.Killed");
			quest.SetFactionRelations(asker.Faction, FactionRelationKind.Hostile, inSignal2, null);
			quest.End(QuestEndOutcome.Fail, 0, null, inSignal2, QuestPart.SignalListenMode.OngoingOnly, true);
			quest.SignalPass(delegate
			{
				quest.End(QuestEndOutcome.Fail, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, true);
			}, inSignal, null);
			quest.FeedPawns(null, shuttle, signalSentSatisfied);
			Quest quest4 = quest;
			Action action = delegate()
			{
				quest.SendShuttleAway(shuttle, true, null);
			};
			string inSignalEnable = null;
			string inSignalDisable = signalSentSatisfied;
			quest4.SignalPassActivable(action, inSignalEnable, text2, null, null, inSignalDisable, false);
			QuestUtility.AddQuestTag(ref site.questTags, text);
			slate.Set<Site>("site", site, false);
			quest.SendShuttleAwayOnCleanup(shuttle, true);
			quest.SignalPassActivable(delegate
			{
				quest.Message("MessageMissionGetBackToShuttle".Translate(site.Faction.Named("FACTION")), MessageTypeDefOf.PositiveEvent, false, null, new LookTargets(shuttle), null);
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
			Quest quest2 = quest;
			Action action2 = delegate()
			{
				quest.End(QuestEndOutcome.Fail, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, true);
			};
			string inSignalEnable2 = null;
			inSignalDisable = text2;
			quest2.SignalPassActivable(action2, inSignalEnable2, text3, null, null, inSignalDisable, false);
			int num3 = (int)(this.timeLimitDays.RandomInRange * 60000f);
			slate.Set<int>("timeoutTicks", num3, false);
			quest.WorldObjectTimeout(site, num3, null, null, false, null, true);
			List<Rule> list = new List<Rule>();
			list.AddRange(GrammarUtility.RulesForWorldObject("site", site, true));
			QuestGen.AddQuestDescriptionRules(list);
		}

		// Token: 0x0600ACDA RID: 44250 RVA: 0x0032515C File Offset: 0x0032335C
		protected override bool TestRunInt(Slate slate)
		{
			if (!ModLister.RoyaltyInstalled)
			{
				Log.ErrorOnce("Missions are a Royalty-specific game system. If you want to use this code please check ModLister.RoyaltyInstalled before calling it. See rules on the Ludeon forum for more info.", 324345634, false);
				return false;
			}
			if (this.IsViolent && !Find.Storyteller.difficultyValues.allowViolentQuests)
			{
				return false;
			}
			int num;
			int num2;
			Map map;
			this.ResolveParameters(slate, out num, out num2, out map);
			return num != -1 && (this.CanGetAsker() && map != null) && this.TryFindSiteTile(out num2);
		}

		// Token: 0x0400765C RID: 30300
		public const int MinTilesAwayFromColony = 80;

		// Token: 0x0400765D RID: 30301
		public const int MaxTilesAwayFromColony = 85;

		// Token: 0x0400765E RID: 30302
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

		// Token: 0x0400765F RID: 30303
		public FloatRange timeLimitDays = new FloatRange(2f, 5f);

		// Token: 0x04007660 RID: 30304
		private static List<Map> tmpMaps = new List<Map>();
	}
}
