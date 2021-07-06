using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x02001FD8 RID: 8152
	public class QuestNode_Root_ShuttleCrash_Rescue : QuestNode
	{
		// Token: 0x17001985 RID: 6533
		// (get) Token: 0x0600ACF1 RID: 44273 RVA: 0x0032551C File Offset: 0x0032371C
		private static QuestGen_Pawns.GetPawnParms CivilianPawnParams
		{
			get
			{
				return new QuestGen_Pawns.GetPawnParms
				{
					mustBeOfFaction = Faction.Empire,
					canGeneratePawn = true,
					mustBeWorldPawn = true,
					mustBeOfKind = PawnKindDefOf.Empire_Common_Lodger
				};
			}
		}

		// Token: 0x0600ACF2 RID: 44274 RVA: 0x0032555C File Offset: 0x0032375C
		protected override void RunInt()
		{
			if (!ModLister.RoyaltyInstalled)
			{
				Log.ErrorOnce("Shuttle crash rescue is a Royalty-specific game system. If you want to use this code please check ModLister.RoyaltyInstalled before calling it. See rules on the Ludeon forum for more info.", 8811221, false);
				return;
			}
			Quest quest = QuestGen.quest;
			Slate slate = QuestGen.slate;
			Map map = QuestGen_Get.GetMap(false, null);
			float questPoints = slate.Get<float>("points", 0f, false);
			slate.Set<Map>("map", map, false);
			slate.Set<int>("rescueDelay", 20000, false);
			slate.Set<int>("leaveDelay", 30000, false);
			slate.Set<int>("rescueShuttleAfterRaidDelay", 10000, false);
			int max = Mathf.FloorToInt(QuestNode_Root_ShuttleCrash_Rescue.MaxCiviliansByPointsCurve.Evaluate(questPoints));
			int num = Rand.Range(1, max);
			int max2 = Mathf.FloorToInt(QuestNode_Root_ShuttleCrash_Rescue.MaxSoldiersByPointsCurve.Evaluate(questPoints));
			int num2 = Rand.Range(1, max2);
			Faction enemyFaction;
			this.TryFindEnemyFaction(out enemyFaction);
			Thing crashedShuttle = ThingMaker.MakeThing(ThingDefOf.ShuttleCrashed, null);
			IntVec3 shuttleCrashPosition;
			this.TryFindShuttleCrashPosition(map, Faction.Empire, crashedShuttle.def.size, out shuttleCrashPosition);
			List<Pawn> civilians = new List<Pawn>();
			List<Pawn> list = new List<Pawn>();
			for (int i = 0; i < num - 1; i++)
			{
				Pawn pawn = quest.GetPawn(QuestNode_Root_ShuttleCrash_Rescue.CivilianPawnParams);
				civilians.Add(pawn);
				list.Add(pawn);
			}
			Pawn asker = quest.GetPawn(new QuestGen_Pawns.GetPawnParms
			{
				mustBeOfFaction = Faction.Empire,
				canGeneratePawn = true,
				mustBeWorldPawn = true,
				seniorityRange = new FloatRange(100f, QuestNode_Root_ShuttleCrash_Rescue.MaxAskerSeniorityByPointsCurve.Evaluate(questPoints)),
				mustHaveRoyalTitleInCurrentFaction = true
			});
			civilians.Add(asker);
			PawnKindDef mustBeOfKind = new PawnKindDef[]
			{
				PawnKindDefOf.Empire_Fighter_Trooper,
				PawnKindDefOf.Empire_Fighter_Janissary,
				PawnKindDefOf.Empire_Fighter_Cataphract
			}.RandomElement<PawnKindDef>();
			List<Pawn> soldiers = new List<Pawn>();
			for (int j = 0; j < num2; j++)
			{
				Pawn pawn2 = quest.GetPawn(new QuestGen_Pawns.GetPawnParms
				{
					mustBeOfFaction = Faction.Empire,
					canGeneratePawn = true,
					mustBeOfKind = mustBeOfKind,
					mustBeWorldPawn = true
				});
				soldiers.Add(pawn2);
			}
			List<Pawn> allPassengers = new List<Pawn>();
			allPassengers.AddRange(soldiers);
			allPassengers.AddRange(civilians);
			quest.BiocodeWeapons(allPassengers, null);
			Thing rescueShuttle = QuestGen_Shuttle.GenerateShuttle(Faction.Empire, allPassengers, null, false, false, false, -1, false, true, false, false, null, null, -1, null, false, true, allPassengers.Cast<Thing>().ToList<Thing>());
			Action <>9__6;
			Action <>9__4;
			quest.Delay(120, delegate
			{
				Quest quest = quest;
				LetterDef negativeEvent = LetterDefOf.NegativeEvent;
				string inSignal3 = null;
				string chosenPawnSignal = null;
				Faction relatedFaction = null;
				MapParent useColonistsOnMap = null;
				bool useColonistsFromCaravanArg = false;
				QuestPart.SignalListenMode signalListenMode = QuestPart.SignalListenMode.OngoingOnly;
				string label = "LetterLabelShuttleCrashed".Translate();
				string text4 = "LetterTextShuttleCrashed".Translate();
				quest.Letter(negativeEvent, inSignal3, chosenPawnSignal, relatedFaction, useColonistsOnMap, useColonistsFromCaravanArg, signalListenMode, Gen.YieldSingle<Thing>(crashedShuttle), false, text4, null, label, null, null);
				quest.SpawnSkyfaller(map, ThingDefOf.ShuttleCrashing, Gen.YieldSingle<Thing>(crashedShuttle), Faction.OfPlayer, new IntVec3?(shuttleCrashPosition), null, false, false, null, null);
				Quest quest2 = quest;
				MapParent parent = map.Parent;
				IEnumerable<Thing> allPassengers = allPassengers;
				string customLetterLabel = null;
				RulePack customLetterLabelRules = null;
				string customLetterText = null;
				RulePack customLetterTextRules = null;
				IntVec3? dropSpot = new IntVec3?(shuttleCrashPosition);
				quest2.DropPods(parent, allPassengers, customLetterLabel, customLetterLabelRules, customLetterText, customLetterTextRules, new bool?(false), false, false, false, null, null, QuestPart.SignalListenMode.OngoingOnly, dropSpot, true);
				quest.DefendPoint(map.Parent, shuttleCrashPosition, soldiers, Faction.Empire, null, null, new float?((float)12), false, false);
				IntVec3 position = shuttleCrashPosition + IntVec3.South;
				IEnumerable<Pawn> civilians;
				quest.WaitForEscort(map.Parent, civilians, Faction.Empire, position, null, false);
				string inSignal4 = QuestGenUtility.HardcodedSignalWithQuestID("rescueShuttle.Spawned");
				quest.ExitOnShuttle(map.Parent, allPassengers, Faction.Empire, rescueShuttle, inSignal4, false);
				quest.SendShuttleAwayOnCleanup(rescueShuttle, false);
				Quest quest3 = quest;
				int delayTicks = 20000;
				civilians = civilians;
				Action complete;
				if ((complete = <>9__4) == null)
				{
					complete = (<>9__4 = delegate()
					{
						Thing rescueShuttle;
						quest.Letter(LetterDefOf.NeutralEvent, null, null, Faction.Empire, null, false, QuestPart.SignalListenMode.OngoingOnly, Gen.YieldSingle<Thing>(rescueShuttle), false, "[rescueShuttleArrivedLetterText]", null, "[rescueShuttleArrivedLetterLabel]", null, null);
						quest.SpawnSkyfaller(map, ThingDefOf.ShuttleIncoming, Gen.YieldSingle<Thing>(rescueShuttle), Faction.Empire, null, null, false, false, crashedShuttle, null);
						Quest quest4 = quest;
						rescueShuttle = rescueShuttle;
						int delayTicks2 = 30000;
						string inSignalEnable = null;
						IEnumerable<string> inSignalsDisable = null;
						string outSignalComplete = null;
						Action complete2;
						if ((complete2 = <>9__6) == null)
						{
							complete2 = (<>9__6 = delegate()
							{
								quest.SendShuttleAway(rescueShuttle, false, null);
								quest.End(QuestEndOutcome.Fail, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, true);
							});
						}
						quest4.ShuttleLeaveDelay(rescueShuttle, delayTicks2, inSignalEnable, inSignalsDisable, outSignalComplete, complete2);
					});
				}
				quest3.ShuttleDelay(delayTicks, civilians, complete, null, null, true);
				IntVec3 walkIntSpot;
				this.TryFindRaidWalkInPosition(map, shuttleCrashPosition, out walkIntSpot);
				float soldiersTotalCombatPower = 0f;
				for (int k = 0; k < soldiers.Count; k++)
				{
					soldiersTotalCombatPower += soldiers[k].kindDef.combatPower;
				}
				quest.Delay(10000, delegate
				{
					List<Pawn> list2 = PawnGroupMakerUtility.GeneratePawns(new PawnGroupMakerParms
					{
						faction = enemyFaction,
						groupKind = PawnGroupKindDefOf.Combat,
						points = (questPoints + soldiersTotalCombatPower) * 0.37f,
						tile = map.Tile
					}, true).ToList<Pawn>();
					for (int l = 0; l < list2.Count; l++)
					{
						Find.WorldPawns.PassToWorld(list2[l], PawnDiscardDecideMode.Decide);
						QuestGen.AddToGeneratedPawns(list2[l]);
					}
					QuestPart_PawnsArrive questPart_PawnsArrive = new QuestPart_PawnsArrive();
					questPart_PawnsArrive.pawns.AddRange(list2);
					questPart_PawnsArrive.arrivalMode = PawnsArrivalModeDefOf.EdgeWalkIn;
					questPart_PawnsArrive.joinPlayer = false;
					questPart_PawnsArrive.mapParent = map.Parent;
					questPart_PawnsArrive.spawnNear = walkIntSpot;
					questPart_PawnsArrive.inSignal = QuestGen.slate.Get<string>("inSignal", null, false);
					questPart_PawnsArrive.sendStandardLetter = false;
					quest.AddPart(questPart_PawnsArrive);
					quest.AssaultThings(map.Parent, list2, enemyFaction, allPassengers, null, null, true);
					quest.Letter(LetterDefOf.ThreatBig, null, null, enemyFaction, null, false, QuestPart.SignalListenMode.OngoingOnly, list2, false, "[raidArrivedLetterText]", null, "[raidArrivedLetterLabel]", null, null);
				}, null, null, null, false, null, null, false, null, null, "RaidDelay", false, QuestPart.SignalListenMode.OngoingOnly);
			}, null, null, null, false, null, null, false, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly);
			string text = QuestGenUtility.HardcodedSignalWithQuestID("rescueShuttle.SentSatisfied");
			string text2 = QuestGenUtility.HardcodedSignalWithQuestID("rescueShuttle.SentUnsatisfied");
			string[] inSignalsShuttleSent = new string[]
			{
				text,
				text2
			};
			string text3 = QuestGenUtility.HardcodedSignalWithQuestID("rescueShuttle.Destroyed");
			string inSignal = QuestGenUtility.HardcodedSignalWithQuestID("asker.Destroyed");
			quest.GoodwillChangeShuttleSentThings(Faction.Empire, list, -5, null, inSignalsShuttleSent, text3, "GoodwillChangeReason_CiviliansLost".Translate(), true, false, QuestPart.SignalListenMode.Always);
			quest.GoodwillChangeShuttleSentThings(Faction.Empire, Gen.YieldSingle<Pawn>(asker), -10, null, inSignalsShuttleSent, text3, "GoodwillChangeReason_CommanderLost".Translate(), true, false, QuestPart.SignalListenMode.Always);
			quest.Leave(allPassengers, "", false, true, null);
			quest.Letter(LetterDefOf.PositiveEvent, text, null, Faction.Empire, null, false, QuestPart.SignalListenMode.OngoingOnly, null, false, "[questCompletedSuccessLetterText]", null, "[questCompletedSuccessLetterLabel]", null, null);
			string questSuccess = QuestGen.GenerateNewSignal("QuestSuccess", true);
			quest.SignalPass(delegate
			{
				RewardsGeneratorParams parms = new RewardsGeneratorParams
				{
					rewardValue = questPoints,
					allowGoodwill = true,
					allowRoyalFavor = true
				};
				quest.GiveRewards(parms, null, null, null, null, null, null, null, null, false, asker, true, false);
				quest.End(QuestEndOutcome.Success, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false);
			}, questSuccess, null);
			quest.SignalPass(null, text, questSuccess);
			Action <>9__7;
			Action <>9__8;
			quest.AnyOnTransporter(allPassengers, rescueShuttle, delegate
			{
				Quest quest = quest;
				IEnumerable<Pawn> pawns = Gen.YieldSingle<Pawn>(asker);
				Thing rescueShuttle = rescueShuttle;
				Action action;
				if ((action = <>9__7) == null)
				{
					action = (<>9__7 = delegate()
					{
						quest.Letter(LetterDefOf.PositiveEvent, null, null, Faction.Empire, null, false, QuestPart.SignalListenMode.OngoingOnly, null, false, "[questCompletedCiviliansLostSuccessLetterText]", null, "[questCompletedCiviliansLostSuccessLetterLabel]", null, null);
						quest.SignalPass(null, null, questSuccess);
					});
				}
				Action elseAction;
				if ((elseAction = <>9__8) == null)
				{
					elseAction = (<>9__8 = delegate()
					{
						quest.Letter(LetterDefOf.NegativeEvent, null, null, Faction.Empire, null, false, QuestPart.SignalListenMode.OngoingOnly, null, false, "[askerLostLetterText]", null, "[askerLostLetterLabel]", null, null);
						quest.End(QuestEndOutcome.Fail, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false);
					});
				}
				quest.AnyOnTransporter(pawns, rescueShuttle, action, elseAction, null, null, null, null, QuestPart.SignalListenMode.OngoingOnly);
			}, delegate
			{
				quest.Letter(LetterDefOf.NegativeEvent, null, null, Faction.Empire, null, false, QuestPart.SignalListenMode.OngoingOnly, null, false, "[allLostLetterText]", null, "[allLostLetterLabel]", null, null);
				quest.End(QuestEndOutcome.Fail, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false);
			}, text2, null, null, null, QuestPart.SignalListenMode.OngoingOnly);
			quest.Letter(LetterDefOf.NegativeEvent, inSignal, null, Faction.Empire, null, false, QuestPart.SignalListenMode.OngoingOnly, Gen.YieldSingle<Pawn>(asker), false, "[askerDiedLetterText]", null, "[askerDiedLetterLabel]", null, null);
			quest.End(QuestEndOutcome.Fail, 0, null, inSignal, QuestPart.SignalListenMode.OngoingOnly, false);
			quest.Letter(LetterDefOf.NegativeEvent, text3, null, Faction.Empire, null, false, QuestPart.SignalListenMode.OngoingOnly, Gen.YieldSingle<Thing>(rescueShuttle), false, "[shuttleDestroyedLetterText]", null, "[shuttleDestroyedLetterLabel]", null, null);
			quest.End(QuestEndOutcome.Fail, 0, null, text3, QuestPart.SignalListenMode.OngoingOnly, false);
			quest.End(QuestEndOutcome.Fail, 0, null, QuestGenUtility.HardcodedSignalWithQuestID("asker.LeftMap"), QuestPart.SignalListenMode.OngoingOnly, true);
			string inSignal2 = QuestGenUtility.HardcodedSignalWithQuestID("askerFaction.BecameHostileToPlayer");
			quest.End(QuestEndOutcome.Fail, 0, null, inSignal2, QuestPart.SignalListenMode.OngoingOnly, true);
			quest.End(QuestEndOutcome.InvalidPreAcceptance, 0, null, inSignal2, QuestPart.SignalListenMode.NotYetAcceptedOnly, false);
			slate.Set<Pawn>("asker", asker, false);
			slate.Set<Faction>("askerFaction", asker.Faction, false);
			slate.Set<Faction>("enemyFaction", enemyFaction, false);
			slate.Set<List<Pawn>>("soldiers", soldiers, false);
			slate.Set<List<Pawn>>("civilians", civilians, false);
			slate.Set<int>("civilianCountMinusOne", civilians.Count - 1, false);
			slate.Set<Thing>("rescueShuttle", rescueShuttle, false);
		}

		// Token: 0x0600ACF3 RID: 44275 RVA: 0x00070BF2 File Offset: 0x0006EDF2
		private bool TryFindEnemyFaction(out Faction enemyFaction)
		{
			return (from f in Find.FactionManager.AllFactionsVisible
			where f.HostileTo(Faction.Empire) && f.HostileTo(Faction.OfPlayer)
			select f).TryRandomElement(out enemyFaction);
		}

		// Token: 0x0600ACF4 RID: 44276 RVA: 0x00070C28 File Offset: 0x0006EE28
		private bool TryFindShuttleCrashPosition(Map map, Faction faction, IntVec2 size, out IntVec3 spot)
		{
			return DropCellFinder.FindSafeLandingSpot(out spot, faction, map, 35, 15, 25, new IntVec2?(size));
		}

		// Token: 0x0600ACF5 RID: 44277 RVA: 0x00325B40 File Offset: 0x00323D40
		private bool TryFindRaidWalkInPosition(Map map, IntVec3 shuttleCrashSpot, out IntVec3 spawnSpot)
		{
			Predicate<IntVec3> predicate = (IntVec3 p) => !map.roofGrid.Roofed(p) && p.Walkable(map) && map.reachability.CanReach(p, shuttleCrashSpot, PathEndMode.OnCell, TraverseMode.NoPassClosedDoors, Danger.Some);
			if (RCellFinder.TryFindEdgeCellFromTargetAvoidingColony(shuttleCrashSpot, map, predicate, out spawnSpot))
			{
				return true;
			}
			if (CellFinder.TryFindRandomEdgeCellWith(predicate, map, CellFinder.EdgeRoadChance_Hostile, out spawnSpot))
			{
				return true;
			}
			spawnSpot = IntVec3.Invalid;
			return false;
		}

		// Token: 0x0600ACF6 RID: 44278 RVA: 0x00325BA8 File Offset: 0x00323DA8
		protected override bool TestRunInt(Slate slate)
		{
			if (!Find.Storyteller.difficultyValues.allowViolentQuests)
			{
				return false;
			}
			Pawn pawn;
			if (!QuestGen_Pawns.GetPawnTest(QuestNode_Root_ShuttleCrash_Rescue.CivilianPawnParams, out pawn))
			{
				return false;
			}
			if (Faction.Empire.PlayerRelationKind == FactionRelationKind.Hostile)
			{
				return false;
			}
			Faction faction;
			if (!this.TryFindEnemyFaction(out faction))
			{
				return false;
			}
			Map map = QuestGen_Get.GetMap(false, null);
			IntVec3 shuttleCrashSpot;
			IntVec3 intVec;
			return this.TryFindShuttleCrashPosition(map, Faction.Empire, ThingDefOf.ShuttleCrashed.size, out shuttleCrashSpot) && this.TryFindRaidWalkInPosition(map, shuttleCrashSpot, out intVec);
		}

		// Token: 0x0400766C RID: 30316
		private const int WanderRadius_Soldiers = 12;

		// Token: 0x0400766D RID: 30317
		private const int QuestStartDelay = 120;

		// Token: 0x0400766E RID: 30318
		private const int RescueShuttle_Delay = 20000;

		// Token: 0x0400766F RID: 30319
		private const int RescueShuttle_LeaveDelay = 30000;

		// Token: 0x04007670 RID: 30320
		private const int RaidDelay = 10000;

		// Token: 0x04007671 RID: 30321
		private const float MinRaidDistance_Colony = 15f;

		// Token: 0x04007672 RID: 30322
		private const float MinRaidDistance_ShuttleCrash = 15f;

		// Token: 0x04007673 RID: 30323
		private const int FactionGoodwillChange_AskerLost = -10;

		// Token: 0x04007674 RID: 30324
		private const int FactionGoodwillChange_CivilianLost = -5;

		// Token: 0x04007675 RID: 30325
		private const float ThreatPointsFactor = 0.37f;

		// Token: 0x04007676 RID: 30326
		private const float MinAskerSeniority = 100f;

		// Token: 0x04007677 RID: 30327
		private static readonly SimpleCurve MaxCiviliansByPointsCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(100f, 2f),
				true
			},
			{
				new CurvePoint(500f, 4f),
				true
			}
		};

		// Token: 0x04007678 RID: 30328
		private static readonly SimpleCurve MaxSoldiersByPointsCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(100f, 3f),
				true
			},
			{
				new CurvePoint(500f, 6f),
				true
			}
		};

		// Token: 0x04007679 RID: 30329
		private static readonly SimpleCurve MaxAskerSeniorityByPointsCurve = new SimpleCurve
		{
			{
				new CurvePoint(300f, 100f),
				true
			},
			{
				new CurvePoint(1500f, 850f),
				true
			}
		};
	}
}
