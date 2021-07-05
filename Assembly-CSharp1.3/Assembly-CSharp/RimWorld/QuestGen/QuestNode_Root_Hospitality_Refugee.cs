using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020016FD RID: 5885
	public class QuestNode_Root_Hospitality_Refugee : QuestNode
	{
		// Token: 0x060087D2 RID: 34770 RVA: 0x0030A350 File Offset: 0x00308550
		protected override void RunInt()
		{
			if (!ModLister.CheckRoyalty("Hospitality refugee"))
			{
				return;
			}
			Quest quest = QuestGen.quest;
			Slate slate = QuestGen.slate;
			Map map = QuestGen_Get.GetMap(false, null);
			int num = slate.Exists("population", false) ? slate.Get<int>("population", 0, false) : map.mapPawns.FreeColonistsSpawnedCount;
			int lodgerCount = Mathf.Max(Mathf.RoundToInt(QuestNode_Root_Hospitality_Refugee.LodgerCountBasedOnColonyPopulationFactorRange.RandomInRange * (float)num), 1);
			int questDurationDays = QuestNode_Root_Hospitality_Refugee.QuestDurationDaysRange.RandomInRange;
			int questDurationTicks = questDurationDays * 60000;
			List<FactionRelation> list = new List<FactionRelation>();
			foreach (Faction faction3 in Find.FactionManager.AllFactionsListForReading)
			{
				if (!faction3.def.permanentEnemy)
				{
					list.Add(new FactionRelation
					{
						other = faction3,
						kind = FactionRelationKind.Neutral
					});
				}
			}
			FactionGeneratorParms factionGeneratorParms = new FactionGeneratorParms(FactionDefOf.OutlanderRefugee, default(IdeoGenerationParms), new bool?(true));
			if (ModsConfig.IdeologyActive)
			{
				factionGeneratorParms.ideoGenerationParms = new IdeoGenerationParms(factionGeneratorParms.factionDef, false, (from p in DefDatabase<PreceptDef>.AllDefs
				where p.proselytizes || p.approvesOfCharity
				select p).ToList<PreceptDef>(), null);
			}
			Faction faction = FactionGenerator.NewGeneratedFactionWithRelations(factionGeneratorParms, list);
			faction.temporary = true;
			Find.FactionManager.Add(faction);
			string lodgerRecruitedSignal = QuestGenUtility.HardcodedSignalWithQuestID("lodgers.Recruited");
			string text = QuestGenUtility.HardcodedSignalWithQuestID("lodgers.Arrested");
			string lodgerDestroyedSignal = QuestGenUtility.HardcodedSignalWithQuestID("lodgers.Destroyed");
			string lodgerKidnapped = QuestGenUtility.HardcodedSignalWithQuestID("lodgers.Kidnapped");
			string lodgerLeftMap = QuestGenUtility.HardcodedSignalWithQuestID("lodgers.LeftMap");
			string lodgerBanished = QuestGenUtility.HardcodedSignalWithQuestID("lodgers.Banished");
			List<Pawn> pawns = new List<Pawn>();
			for (int i = 0; i < lodgerCount; i++)
			{
				Pawn pawn = quest.GeneratePawn(PawnKindDefOf.Refugee, faction, true, null, 0f, true, null, 0f, 0f, false, true);
				pawns.Add(pawn);
				quest.PawnJoinOffer(pawn, "LetterJoinOfferLabel".Translate(pawn.Named("PAWN")), "LetterJoinOfferTitle".Translate(pawn.Named("PAWN")), "LetterJoinOfferText".Translate(pawn.Named("PAWN"), map.Parent.Named("MAP")), delegate
				{
					Quest quest = quest;
					LetterDef positiveEvent = LetterDefOf.PositiveEvent;
					string inSignal = null;
					string chosenPawnSignal = null;
					Faction relatedFaction = null;
					MapParent useColonistsOnMap = null;
					bool useColonistsFromCaravanArg = false;
					QuestPart.SignalListenMode signalListenMode = QuestPart.SignalListenMode.OngoingOnly;
					IEnumerable<object> lookTargets = null;
					bool filterDeadPawnsFromLookTargets = false;
					string label = "LetterLabelMessageRecruitSuccess".Translate() + ": " + pawn.LabelShortCap;
					quest.Letter(positiveEvent, inSignal, chosenPawnSignal, relatedFaction, useColonistsOnMap, useColonistsFromCaravanArg, signalListenMode, lookTargets, filterDeadPawnsFromLookTargets, "MessageRecruitJoinOfferAccepted".Translate(pawn.Named("RECRUITEE")), null, label, null, null);
					quest.SignalPass(null, null, lodgerRecruitedSignal);
				}, null, null, null, null, true);
			}
			slate.Set<List<Pawn>>("lodgers", pawns, false);
			faction.leader = pawns.First<Pawn>();
			Pawn var = pawns.First<Pawn>();
			quest.SetFactionHidden(faction, false, null);
			QuestPart_ExtraFaction extraFactionPart = quest.ExtraFaction(faction, pawns, ExtraFactionType.MiniFaction, false, lodgerRecruitedSignal);
			quest.PawnsArrive(pawns, null, map.Parent, null, true, null, "[lodgersArriveLetterLabel]", "[lodgersArriveLetterText]", null, null, false, false, true);
			QuestPart_Choice questPart_Choice = quest.RewardChoice(null, null);
			QuestPart_Choice.Choice choice = new QuestPart_Choice.Choice();
			choice.rewards.Add(new Reward_VisitorsHelp());
			choice.rewards.Add(new Reward_PossibleFutureReward());
			questPart_Choice.choices.Add(choice);
			quest.SetAllApparelLocked(pawns, null);
			string assaultColonySignal = QuestGen.GenerateNewSignal("AssaultColony", true);
			Action <>9__6;
			Action item = delegate()
			{
				int num2 = Mathf.FloorToInt(QuestNode_Root_Hospitality_Refugee.MutinyTimeRange.RandomInRange * (float)questDurationTicks);
				Quest quest = quest;
				int delayTicks = num2;
				Action inner;
				if ((inner = <>9__6) == null)
				{
					inner = (<>9__6 = delegate()
					{
						quest.Letter(LetterDefOf.ThreatBig, null, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, null, false, "[mutinyLetterText]", null, "[mutinyLetterLabel]", null, null);
						quest.SignalPass(null, null, assaultColonySignal);
						quest.End(QuestEndOutcome.Unknown, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false);
					});
				}
				quest.Delay(delayTicks, inner, null, null, null, false, null, null, false, null, null, "Mutiny (" + num2.ToStringTicksToDays("F1") + ")", false, QuestPart.SignalListenMode.OngoingOnly);
			};
			Action item2 = delegate()
			{
				Pawn factionOpponent = quest.GetPawn(QuestNode_Root_Hospitality_Refugee.FactionOpponentPawnParams);
				slate.Set<Pawn>("factionOpponent", factionOpponent, false);
				int num2 = Mathf.FloorToInt(QuestNode_Root_Hospitality_Refugee.BetrayalOfferTimeRange.RandomInRange * (float)questDurationTicks);
				Action <>9__10;
				quest.Delay(num2, delegate
				{
					float val = (float)lodgerCount * 300f;
					FloatRange value = new FloatRange(0.7f, 1.3f) * val * Find.Storyteller.difficulty.EffectiveQuestRewardValueFactor;
					ThingSetMakerParams parms = default(ThingSetMakerParams);
					parms.totalMarketValueRange = new FloatRange?(value);
					parms.qualityGenerator = new QualityGenerator?(QualityGenerator.Reward);
					parms.makingFaction = faction;
					List<Thing> betrayalRewardThings = ThingSetMakerDefOf.Reward_ItemsStandard.root.Generate(parms);
					Quest quest = quest;
					IEnumerable<Pawn> pawns = pawns;
					ExtraFaction extraFaction = extraFactionPart.extraFaction;
					Pawn factionOpponent = factionOpponent;
					Action success = delegate()
					{
						float num3 = 0f;
						for (int j = 0; j < betrayalRewardThings.Count; j++)
						{
							num3 += betrayalRewardThings[j].MarketValue * (float)betrayalRewardThings[j].stackCount;
						}
						slate.Set<string>("betrayalRewards", GenLabel.ThingsLabel(betrayalRewardThings, "  - "), false);
						slate.Set<float>("betrayalRewardMarketValue", num3, false);
						quest.DropPods(map.Parent, betrayalRewardThings, null, null, null, null, new bool?(true), false, false, false, null, null, QuestPart.SignalListenMode.Always, null, false);
						quest.FactionGoodwillChange(factionOpponent.Faction, 10, null, true, true, true, null, QuestPart.SignalListenMode.Always, false);
						quest.Letter(LetterDefOf.PositiveEvent, null, null, null, null, false, QuestPart.SignalListenMode.Always, betrayalRewardThings, false, "[betrayalOfferRewardLetterText]", null, "[betrayalOfferRewardLetterLabel]", null, null);
					};
					Action failure = delegate()
					{
						quest.DestroyThingsOrPassToWorld(betrayalRewardThings, null, true, QuestPart.SignalListenMode.Always);
						quest.Letter(LetterDefOf.NegativeEvent, null, null, null, null, false, QuestPart.SignalListenMode.Always, null, false, "[betrayalOfferFailedLetterText]", null, "[betrayalOfferFailedLetterLabel]", null, null);
					};
					Action enabled;
					if ((enabled = <>9__10) == null)
					{
						enabled = (<>9__10 = delegate()
						{
							ChoiceLetter_BetrayVisitors choiceLetter_BetrayVisitors = quest.Letter(LetterDefOf.BetrayVisitors, null, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, null, false, "[betrayalOffserLetterText]", null, "[betrayalOfferLetterLabel]", null, null).letter as ChoiceLetter_BetrayVisitors;
							choiceLetter_BetrayVisitors.pawns.AddRange(pawns);
							choiceLetter_BetrayVisitors.asker = factionOpponent;
							choiceLetter_BetrayVisitors.requiresAliveAsker = true;
						});
					}
					quest.BetrayalOffer(pawns, extraFaction, factionOpponent, success, failure, enabled, new List<string>
					{
						lodgerDestroyedSignal,
						lodgerKidnapped,
						lodgerLeftMap,
						lodgerBanished
					}, null, QuestPart.SignalListenMode.Always);
				}, null, null, null, false, null, null, false, null, null, "BetrayalOffer (" + num2.ToStringTicksToDays("F1") + ")", false, QuestPart.SignalListenMode.OngoingOnly);
			};
			if (Find.Storyteller.difficulty.allowViolentQuests)
			{
				List<Tuple<float, Action>> list2 = new List<Tuple<float, Action>>();
				list2.Add(Tuple.Create<float, Action>(0.25f, item));
				Pawn pawn2;
				if (QuestGen_Pawns.GetPawnTest(QuestNode_Root_Hospitality_Refugee.FactionOpponentPawnParams, out pawn2))
				{
					list2.Add(Tuple.Create<float, Action>(0.25f, item2));
				}
				list2.Add(Tuple.Create<float, Action>(0.5f, delegate()
				{
				}));
				Tuple<float, Action> tuple;
				if (list2.TryRandomElementByWeight((Tuple<float, Action> t) => t.Item1, out tuple))
				{
					tuple.Item2();
				}
			}
			QuestPart_RefugeeInteractions questPart_RefugeeInteractions = new QuestPart_RefugeeInteractions();
			questPart_RefugeeInteractions.inSignalEnable = QuestGen.slate.Get<string>("inSignal", null, false);
			questPart_RefugeeInteractions.inSignalDestroyed = lodgerDestroyedSignal;
			questPart_RefugeeInteractions.inSignalArrested = text;
			questPart_RefugeeInteractions.inSignalSurgeryViolation = QuestGenUtility.HardcodedSignalWithQuestID("lodgers.SurgeryViolation");
			questPart_RefugeeInteractions.inSignalKidnapped = lodgerKidnapped;
			questPart_RefugeeInteractions.inSignalRecruited = lodgerRecruitedSignal;
			questPart_RefugeeInteractions.inSignalAssaultColony = assaultColonySignal;
			questPart_RefugeeInteractions.inSignalLeftMap = lodgerLeftMap;
			questPart_RefugeeInteractions.inSignalBanished = lodgerBanished;
			questPart_RefugeeInteractions.outSignalDestroyed_AssaultColony = QuestGen.GenerateNewSignal("LodgerDestroyed_AssaultColony", true);
			questPart_RefugeeInteractions.outSignalDestroyed_LeaveColony = QuestGen.GenerateNewSignal("LodgerDestroyed_LeaveColony", true);
			questPart_RefugeeInteractions.outSignalDestroyed_BadThought = QuestGen.GenerateNewSignal("LodgerDestroyed_BadThought", true);
			questPart_RefugeeInteractions.outSignalArrested_AssaultColony = QuestGen.GenerateNewSignal("LodgerArrested_AssaultColony", true);
			questPart_RefugeeInteractions.outSignalArrested_LeaveColony = QuestGen.GenerateNewSignal("LodgerArrested_LeaveColony", true);
			questPart_RefugeeInteractions.outSignalArrested_BadThought = QuestGen.GenerateNewSignal("LodgerArrested_BadThought", true);
			questPart_RefugeeInteractions.outSignalSurgeryViolation_AssaultColony = QuestGen.GenerateNewSignal("LodgerSurgeryViolation_AssaultColony", true);
			questPart_RefugeeInteractions.outSignalSurgeryViolation_LeaveColony = QuestGen.GenerateNewSignal("LodgerSurgeryViolation_LeaveColony", true);
			questPart_RefugeeInteractions.outSignalSurgeryViolation_BadThought = QuestGen.GenerateNewSignal("LodgerSurgeryViolation_BadThought", true);
			questPart_RefugeeInteractions.outSignalLast_Destroyed = QuestGen.GenerateNewSignal("LastLodger_Destroyed", true);
			questPart_RefugeeInteractions.outSignalLast_Arrested = QuestGen.GenerateNewSignal("LastLodger_Arrested", true);
			questPart_RefugeeInteractions.outSignalLast_Kidnapped = QuestGen.GenerateNewSignal("LastLodger_Kidnapped", true);
			questPart_RefugeeInteractions.outSignalLast_Recruited = QuestGen.GenerateNewSignal("LastLodger_Recruited", true);
			questPart_RefugeeInteractions.outSignalLast_LeftMapAllHealthy = QuestGen.GenerateNewSignal("LastLodger_LeftMapAllHealthy", true);
			questPart_RefugeeInteractions.outSignalLast_LeftMapAllNotHealthy = QuestGen.GenerateNewSignal("LastLodger_LeftMapAllNotHealthy", true);
			questPart_RefugeeInteractions.outSignalLast_Banished = QuestGen.GenerateNewSignal("LastLodger_Banished", true);
			questPart_RefugeeInteractions.pawns.AddRange(pawns);
			questPart_RefugeeInteractions.faction = faction;
			questPart_RefugeeInteractions.mapParent = map.Parent;
			questPart_RefugeeInteractions.signalListenMode = QuestPart.SignalListenMode.Always;
			quest.AddPart(questPart_RefugeeInteractions);
			string lodgerArrestedOrRecruited = QuestGen.GenerateNewSignal("Lodger_ArrestedOrRecruited", true);
			quest.AnySignal(new List<string>
			{
				lodgerRecruitedSignal,
				text
			}, null, new List<string>
			{
				lodgerArrestedOrRecruited
			}, QuestPart.SignalListenMode.OngoingOnly);
			Action <>9__13;
			quest.Delay(questDurationTicks, delegate
			{
				Quest quest = quest;
				Faction faction2 = faction;
				Action action = null;
				Action outAction;
				if ((outAction = <>9__13) == null)
				{
					outAction = (<>9__13 = delegate()
					{
						quest.Letter(LetterDefOf.PositiveEvent, null, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, null, false, "[lodgersLeavingLetterText]", null, "[lodgersLeavingLetterLabel]", null, null);
					});
				}
				quest.SignalPassWithFaction(faction2, action, outAction, null, null);
				quest.Leave(pawns, null, false, false, lodgerArrestedOrRecruited, true);
			}, null, null, null, false, null, null, false, "GuestsDepartsIn".Translate(), "GuestsDepartsOn".Translate(), "QuestDelay", false, QuestPart.SignalListenMode.OngoingOnly);
			quest.Letter(LetterDefOf.NegativeEvent, questPart_RefugeeInteractions.outSignalDestroyed_BadThought, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, null, false, "[lodgerDiedMemoryThoughtLetterText]", null, "[lodgerDiedMemoryThoughtLetterLabel]", null, null);
			quest.Letter(LetterDefOf.NegativeEvent, questPart_RefugeeInteractions.outSignalDestroyed_AssaultColony, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, null, false, "[lodgerDiedAttackPlayerLetterText]", null, "[lodgerDiedAttackPlayerLetterLabel]", null, null);
			quest.Letter(LetterDefOf.NegativeEvent, questPart_RefugeeInteractions.outSignalDestroyed_LeaveColony, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, null, false, "[lodgerDiedLeaveMapLetterText]", null, "[lodgerDiedLeaveMapLetterLabel]", null, null);
			quest.Letter(LetterDefOf.NegativeEvent, questPart_RefugeeInteractions.outSignalLast_Destroyed, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, null, false, "[lodgersAllDiedLetterText]", null, "[lodgersAllDiedLetterLabel]", null, null);
			quest.Letter(LetterDefOf.NegativeEvent, questPart_RefugeeInteractions.outSignalArrested_BadThought, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, null, false, "[lodgerArrestedMemoryThoughtLetterText]", null, "[lodgerArrestedMemoryThoughtLetterLabel]", null, null);
			quest.Letter(LetterDefOf.NegativeEvent, questPart_RefugeeInteractions.outSignalArrested_AssaultColony, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, null, false, "[lodgerArrestedAttackPlayerLetterText]", null, "[lodgerArrestedAttackPlayerLetterLabel]", null, null);
			quest.Letter(LetterDefOf.NegativeEvent, questPart_RefugeeInteractions.outSignalArrested_LeaveColony, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, null, false, "[lodgerArrestedLeaveMapLetterText]", null, "[lodgerArrestedLeaveMapLetterLabel]", null, null);
			quest.Letter(LetterDefOf.NegativeEvent, questPart_RefugeeInteractions.outSignalLast_Arrested, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, null, false, "[lodgersAllArrestedLetterText]", null, "[lodgersAllArrestedLetterLabel]", null, null);
			quest.Letter(LetterDefOf.NegativeEvent, questPart_RefugeeInteractions.outSignalSurgeryViolation_BadThought, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, null, false, "[lodgerViolatedMemoryThoughtLetterText]", null, "[lodgerViolatedMemoryThoughtLetterLabel]", null, null);
			quest.Letter(LetterDefOf.NegativeEvent, questPart_RefugeeInteractions.outSignalSurgeryViolation_AssaultColony, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, null, false, "[lodgerViolatedAttackPlayerLetterText]", null, "[lodgerViolatedAttackPlayerLetterLabel]", null, null);
			quest.Letter(LetterDefOf.NegativeEvent, questPart_RefugeeInteractions.outSignalSurgeryViolation_LeaveColony, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, null, false, "[lodgerViolatedLeaveMapLetterText]", null, "[lodgerViolatedLeaveMapLetterLabel]", null, null);
			quest.AddMemoryThought(pawns, ThoughtDefOf.OtherTravelerDied, questPart_RefugeeInteractions.outSignalDestroyed_BadThought, null, null);
			quest.AddMemoryThought(pawns, ThoughtDefOf.OtherTravelerArrested, questPart_RefugeeInteractions.outSignalArrested_BadThought, null, null);
			quest.AddMemoryThought(pawns, ThoughtDefOf.OtherTravelerSurgicallyViolated, questPart_RefugeeInteractions.outSignalSurgeryViolation_BadThought, null, null);
			quest.End(QuestEndOutcome.Fail, 0, null, questPart_RefugeeInteractions.outSignalDestroyed_AssaultColony, QuestPart.SignalListenMode.OngoingOnly, true);
			quest.End(QuestEndOutcome.Fail, 0, null, questPart_RefugeeInteractions.outSignalDestroyed_LeaveColony, QuestPart.SignalListenMode.OngoingOnly, true);
			quest.End(QuestEndOutcome.Fail, 0, null, questPart_RefugeeInteractions.outSignalLast_Destroyed, QuestPart.SignalListenMode.OngoingOnly, false);
			quest.End(QuestEndOutcome.Fail, 0, null, questPart_RefugeeInteractions.outSignalArrested_AssaultColony, QuestPart.SignalListenMode.OngoingOnly, true);
			quest.End(QuestEndOutcome.Fail, 0, null, questPart_RefugeeInteractions.outSignalArrested_LeaveColony, QuestPart.SignalListenMode.OngoingOnly, true);
			quest.End(QuestEndOutcome.Fail, 0, null, questPart_RefugeeInteractions.outSignalLast_Arrested, QuestPart.SignalListenMode.OngoingOnly, false);
			quest.End(QuestEndOutcome.Fail, 0, null, questPart_RefugeeInteractions.outSignalSurgeryViolation_AssaultColony, QuestPart.SignalListenMode.OngoingOnly, true);
			quest.End(QuestEndOutcome.Fail, 0, null, questPart_RefugeeInteractions.outSignalSurgeryViolation_LeaveColony, QuestPart.SignalListenMode.OngoingOnly, true);
			quest.End(QuestEndOutcome.Fail, 0, null, questPart_RefugeeInteractions.outSignalLast_Kidnapped, QuestPart.SignalListenMode.OngoingOnly, true);
			quest.End(QuestEndOutcome.Fail, 0, null, questPart_RefugeeInteractions.outSignalLast_Banished, QuestPart.SignalListenMode.OngoingOnly, true);
			quest.End(QuestEndOutcome.Success, 0, null, questPart_RefugeeInteractions.outSignalLast_Recruited, QuestPart.SignalListenMode.OngoingOnly, true);
			quest.End(QuestEndOutcome.Success, 0, null, questPart_RefugeeInteractions.outSignalLast_LeftMapAllNotHealthy, QuestPart.SignalListenMode.OngoingOnly, true);
			quest.SignalPass(delegate
			{
				if (Rand.Chance(0.5f))
				{
					float val = (float)(lodgerCount * questDurationDays) * 55f;
					FloatRange marketValueRange = new FloatRange(0.7f, 1.3f) * val * Find.Storyteller.difficulty.EffectiveQuestRewardValueFactor;
					quest.AddQuestRefugeeDelayedReward(quest.AccepterPawn, faction, pawns, marketValueRange, null);
				}
				quest.End(QuestEndOutcome.Success, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, true);
			}, questPart_RefugeeInteractions.outSignalLast_LeftMapAllHealthy, null);
			slate.Set<int>("lodgerCount", lodgerCount, false);
			slate.Set<int>("lodgersCountMinusOne", lodgerCount - 1, false);
			slate.Set<Pawn>("asker", var, false);
			slate.Set<Map>("map", map, false);
			slate.Set<int>("questDurationTicks", questDurationTicks, false);
			slate.Set<Faction>("faction", faction, false);
		}

		// Token: 0x060087D3 RID: 34771 RVA: 0x0030AF2C File Offset: 0x0030912C
		protected override bool TestRunInt(Slate slate)
		{
			return QuestGen_Get.GetMap(false, null) != null;
		}

		// Token: 0x040055D0 RID: 21968
		private static readonly FloatRange LodgerCountBasedOnColonyPopulationFactorRange = new FloatRange(0.3f, 1f);

		// Token: 0x040055D1 RID: 21969
		private static readonly QuestGen_Pawns.GetPawnParms FactionOpponentPawnParams = new QuestGen_Pawns.GetPawnParms
		{
			mustBeWorldPawn = true,
			mustBeFactionLeader = true,
			mustBeNonHostileToPlayer = true
		};

		// Token: 0x040055D2 RID: 21970
		private const float MidEventSelWeight_None = 0.5f;

		// Token: 0x040055D3 RID: 21971
		private const float MidEventSelWeight_Mutiny = 0.25f;

		// Token: 0x040055D4 RID: 21972
		private const float MidEventSelWeight_BetrayalOffer = 0.25f;

		// Token: 0x040055D5 RID: 21973
		private const float RewardPostLeaveChance = 0.5f;

		// Token: 0x040055D6 RID: 21974
		private const float RewardFactor_Postleave = 55f;

		// Token: 0x040055D7 RID: 21975
		private const float RewardFactor_BetrayalOffer = 300f;

		// Token: 0x040055D8 RID: 21976
		private const int BetrayalOfferGoodwillReward = 10;

		// Token: 0x040055D9 RID: 21977
		private static FloatRange BetrayalOfferTimeRange = new FloatRange(0.25f, 0.5f);

		// Token: 0x040055DA RID: 21978
		private static FloatRange MutinyTimeRange = new FloatRange(0.2f, 1f);

		// Token: 0x040055DB RID: 21979
		private static IntRange QuestDurationDaysRange = new IntRange(5, 20);
	}
}
