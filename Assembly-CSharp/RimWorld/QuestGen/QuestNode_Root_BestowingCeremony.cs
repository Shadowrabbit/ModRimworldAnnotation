using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x02001FCB RID: 8139
	public class QuestNode_Root_BestowingCeremony : QuestNode
	{
		// Token: 0x0600ACAD RID: 44205 RVA: 0x00322F60 File Offset: 0x00321160
		private bool TryGetCeremonyTarget(Slate slate, out Pawn pawn, out Faction bestowingFaction)
		{
			slate.TryGet<Faction>("bestowingFaction", out bestowingFaction, false);
			if (!slate.TryGet<Pawn>("titleHolder", out pawn, false) || pawn.Faction == null || !pawn.Faction.IsPlayer)
			{
				pawn = null;
				foreach (Map map in Find.Maps)
				{
					if (map.IsPlayerHome)
					{
						foreach (Pawn pawn2 in map.mapPawns.AllPawns)
						{
							if (pawn2.Faction != null && pawn2.Faction.IsPlayer)
							{
								if (bestowingFaction != null)
								{
									return RoyalTitleUtility.ShouldGetBestowingCeremonyQuest(pawn2, bestowingFaction);
								}
								return RoyalTitleUtility.ShouldGetBestowingCeremonyQuest(pawn2, out bestowingFaction);
							}
						}
					}
				}
				bestowingFaction = null;
				return false;
			}
			if (bestowingFaction != null)
			{
				return RoyalTitleUtility.ShouldGetBestowingCeremonyQuest(pawn, bestowingFaction);
			}
			return RoyalTitleUtility.ShouldGetBestowingCeremonyQuest(pawn, out bestowingFaction);
		}

		// Token: 0x0600ACAE RID: 44206 RVA: 0x00323078 File Offset: 0x00321278
		protected override void RunInt()
		{
			if (!ModLister.RoyaltyInstalled)
			{
				Log.ErrorOnce("Bestowing ceremony is a Royalty-specific game system. If you want to use this code please check ModLister.RoyaltyInstalled before calling it. See rules on the Ludeon forum for more info.", 3454535, false);
				return;
			}
			Quest quest = QuestGen.quest;
			Slate slate = QuestGen.slate;
			Pawn pawn;
			Faction faction;
			if (!this.TryGetCeremonyTarget(QuestGen.slate, out pawn, out faction))
			{
				return;
			}
			RoyalTitleDef titleAwardedWhenUpdating = pawn.royalty.GetTitleAwardedWhenUpdating(faction, pawn.royalty.GetFavor(faction));
			string text = QuestGenUtility.HardcodedTargetQuestTagWithQuestID("Bestowing");
			string text2 = QuestGenUtility.QuestTagSignal(text, "CeremonyExpired");
			string inSignal = QuestGenUtility.QuestTagSignal(text, "CeremonyFailed");
			string inSignal2 = QuestGenUtility.QuestTagSignal(text, "CeremonyDone");
			string inSignal3 = QuestGenUtility.QuestTagSignal(text, "TitleAwardedWhenUpdatingChanged");
			Thing thing = QuestGen_Shuttle.GenerateShuttle(faction, null, null, false, false, false, 0, false, true, true, true, null, null, -1, null, false, true, null);
			Pawn pawn2 = quest.GetPawn(new QuestGen_Pawns.GetPawnParms
			{
				mustBeOfKind = PawnKindDefOf.Empire_Royal_Bestower,
				canGeneratePawn = true,
				mustBeOfFaction = faction,
				mustBeWorldPawn = true
			});
			QuestUtility.AddQuestTag(ref thing.questTags, text);
			QuestUtility.AddQuestTag(ref pawn.questTags, text);
			ThingOwner<Thing> innerContainer = pawn2.inventory.innerContainer;
			for (int i = innerContainer.Count - 1; i >= 0; i--)
			{
				if (innerContainer[i].def == ThingDefOf.PsychicAmplifier)
				{
					Thing thing2 = innerContainer[i];
					innerContainer.RemoveAt(i);
					thing2.Destroy(DestroyMode.Vanish);
				}
			}
			int num = titleAwardedWhenUpdating.maxPsylinkLevel - pawn.GetPsylinkLevel();
			for (int j = 0; j < num + 1; j++)
			{
				innerContainer.TryAdd(ThingMaker.MakeThing(ThingDefOf.PsychicAmplifier, null), 1, true);
			}
			List<Pawn> list = new List<Pawn>();
			list.Add(pawn2);
			slate.Set<List<Pawn>>("shuttleContents", list, false);
			slate.Set<Thing>("shuttle", thing, false);
			slate.Set<Pawn>("target", pawn, false);
			slate.Set<Faction>("bestowingFaction", faction, false);
			List<Pawn> list2 = new List<Pawn>();
			for (int k = 0; k < 6; k++)
			{
				Pawn item = quest.GeneratePawn(PawnKindDefOf.Empire_Fighter_Janissary, faction, true, null, 0f, true, null, 0f, 0f, false, false);
				list.Add(item);
				list2.Add(item);
			}
			slate.Set<List<Pawn>>("defenders", list2, false);
			CompShuttle compShuttle = thing.TryGetComp<CompShuttle>();
			compShuttle.requiredPawns = list;
			compShuttle.sendAwayIfAllDespawned = list.Cast<Thing>().ToList<Thing>();
			compShuttle.sendAwayIfAllPawnsLeftToLoadAreNotOfFaction = faction;
			quest.AddContentsToShuttle(thing, list, null);
			quest.SpawnSkyfaller(null, ThingDefOf.ShuttleIncoming, Gen.YieldSingle<Thing>(thing), Faction.OfPlayer, null, null, true, true, null, pawn);
			quest.FactionGoodwillChange(faction, -5, QuestGenUtility.HardcodedSignalWithQuestID("defenders.Killed"), true, true, "GoodwillChangeReason_AttackedFaction".Translate(faction), true, QuestPart.SignalListenMode.OngoingOnly);
			QuestPart_BestowingCeremony questPart_BestowingCeremony = new QuestPart_BestowingCeremony();
			questPart_BestowingCeremony.inSignal = QuestGen.slate.Get<string>("inSignal", null, false);
			questPart_BestowingCeremony.pawns.Add(pawn2);
			questPart_BestowingCeremony.mapOfPawn = pawn;
			questPart_BestowingCeremony.faction = pawn2.Faction;
			questPart_BestowingCeremony.bestower = pawn2;
			questPart_BestowingCeremony.target = pawn;
			questPart_BestowingCeremony.shuttle = thing;
			questPart_BestowingCeremony.questTag = text;
			quest.AddPart(questPart_BestowingCeremony);
			QuestPart_EscortPawn questPart_EscortPawn = new QuestPart_EscortPawn();
			questPart_EscortPawn.inSignal = QuestGen.slate.Get<string>("inSignal", null, false);
			questPart_EscortPawn.escortee = pawn2;
			questPart_EscortPawn.pawns.AddRange(list2);
			questPart_EscortPawn.mapOfPawn = pawn;
			questPart_EscortPawn.faction = pawn2.Faction;
			questPart_EscortPawn.shuttle = thing;
			quest.AddPart(questPart_EscortPawn);
			string inSignal4 = QuestGenUtility.HardcodedSignalWithQuestID("shuttle.Killed");
			quest.SetFactionRelations(faction, FactionRelationKind.Hostile, inSignal4, null);
			quest.End(QuestEndOutcome.Fail, 0, null, inSignal4, QuestPart.SignalListenMode.OngoingOnly, true);
			QuestPart_RequirementsToAcceptThroneRoom questPart_RequirementsToAcceptThroneRoom = new QuestPart_RequirementsToAcceptThroneRoom();
			questPart_RequirementsToAcceptThroneRoom.faction = faction;
			questPart_RequirementsToAcceptThroneRoom.forPawn = pawn;
			questPart_RequirementsToAcceptThroneRoom.forTitle = titleAwardedWhenUpdating;
			quest.AddPart(questPart_RequirementsToAcceptThroneRoom);
			QuestPart_RequirementsToAcceptPawnOnColonyMap questPart_RequirementsToAcceptPawnOnColonyMap = new QuestPart_RequirementsToAcceptPawnOnColonyMap();
			questPart_RequirementsToAcceptPawnOnColonyMap.pawn = pawn;
			quest.AddPart(questPart_RequirementsToAcceptPawnOnColonyMap);
			QuestPart_RequirementsToAcceptNoDanger questPart_RequirementsToAcceptNoDanger = new QuestPart_RequirementsToAcceptNoDanger();
			questPart_RequirementsToAcceptNoDanger.map = pawn.Map;
			questPart_RequirementsToAcceptNoDanger.dangerTo = faction;
			quest.AddPart(questPart_RequirementsToAcceptNoDanger);
			quest.AddPart(new QuestPart_RequirementsToAcceptNoOngoingBestowingCeremony());
			string inSignal5 = QuestGenUtility.HardcodedSignalWithQuestID("shuttleContents.Recruited");
			string inSignal6 = QuestGenUtility.HardcodedSignalWithQuestID("bestowingFaction.BecameHostileToPlayer");
			quest.Signal(inSignal5, delegate
			{
				quest.End(QuestEndOutcome.Fail, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, true);
			}, null, QuestPart.SignalListenMode.OngoingOnly);
			quest.End(QuestEndOutcome.Fail, 0, null, QuestGenUtility.HardcodedSignalWithQuestID("target.Killed"), QuestPart.SignalListenMode.OngoingOrNotYetAccepted, true);
			Quest quest2 = quest;
			LetterDef negativeEvent = LetterDefOf.NegativeEvent;
			string inSignal7 = text2;
			string chosenPawnSignal = null;
			Faction relatedFaction = null;
			MapParent useColonistsOnMap = null;
			bool useColonistsFromCaravanArg = false;
			QuestPart.SignalListenMode signalListenMode = QuestPart.SignalListenMode.OngoingOnly;
			IEnumerable<object> lookTargets = null;
			bool filterDeadPawnsFromLookTargets = false;
			string label = "LetterLabelBestowingCeremonyExpired".Translate();
			quest2.Letter(negativeEvent, inSignal7, chosenPawnSignal, relatedFaction, useColonistsOnMap, useColonistsFromCaravanArg, signalListenMode, lookTargets, filterDeadPawnsFromLookTargets, "LetterTextBestowingCeremonyExpired".Translate(pawn.Named("TARGET")), null, label, null, null);
			quest.End(QuestEndOutcome.Fail, 0, null, text2, QuestPart.SignalListenMode.OngoingOnly, false);
			quest.End(QuestEndOutcome.Fail, 0, null, inSignal6, QuestPart.SignalListenMode.OngoingOrNotYetAccepted, true);
			quest.End(QuestEndOutcome.Fail, 0, null, inSignal, QuestPart.SignalListenMode.OngoingOnly, true);
			quest.End(QuestEndOutcome.Fail, 0, null, inSignal3, QuestPart.SignalListenMode.OngoingOrNotYetAccepted, true);
			quest.End(QuestEndOutcome.Success, 0, null, inSignal2, QuestPart.SignalListenMode.OngoingOnly, false);
			QuestPart_Choice questPart_Choice = quest.RewardChoice(null, null);
			QuestPart_Choice.Choice choice = new QuestPart_Choice.Choice();
			choice.rewards.Add(new Reward_BestowingCeremony
			{
				targetPawnName = pawn.NameShortColored.Resolve(),
				titleName = titleAwardedWhenUpdating.GetLabelCapFor(pawn),
				awardingFaction = faction,
				givePsylink = (titleAwardedWhenUpdating.maxPsylinkLevel > pawn.GetPsylinkLevel()),
				royalTitle = titleAwardedWhenUpdating
			});
			questPart_Choice.choices.Add(choice);
			List<Rule> list3 = new List<Rule>();
			list3.AddRange(GrammarUtility.RulesForPawn("pawn", pawn, null, true, true));
			list3.Add(new Rule_String("newTitle", titleAwardedWhenUpdating.GetLabelCapFor(pawn)));
			QuestGen.AddQuestNameRules(list3);
			List<Rule> list4 = new List<Rule>();
			list4.AddRange(GrammarUtility.RulesForFaction("faction", faction, true));
			list4.AddRange(GrammarUtility.RulesForPawn("pawn", pawn, null, true, true));
			list4.Add(new Rule_String("newTitle", pawn.royalty.GetTitleAwardedWhenUpdating(faction, pawn.royalty.GetFavor(faction)).GetLabelFor(pawn)));
			list4.Add(new Rule_String("psylinkLevel", titleAwardedWhenUpdating.maxPsylinkLevel.ToString()));
			QuestGen.AddQuestDescriptionRules(list4);
		}

		// Token: 0x0600ACAF RID: 44207 RVA: 0x00323710 File Offset: 0x00321910
		protected override bool TestRunInt(Slate slate)
		{
			Pawn pawn;
			Faction faction;
			Pawn pawn2;
			return this.TryGetCeremonyTarget(slate, out pawn, out faction) && !faction.HostileTo(Faction.OfPlayer) && QuestGen_Pawns.GetPawnTest(new QuestGen_Pawns.GetPawnParms
			{
				mustBeOfKind = PawnKindDefOf.Empire_Royal_Bestower,
				canGeneratePawn = true,
				mustBeOfFaction = faction
			}, out pawn2);
		}

		// Token: 0x0400762C RID: 30252
		public const string QuestTag = "Bestowing";
	}
}
