using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x02001704 RID: 5892
	public class QuestNode_Root_ReliquaryPilgrims : QuestNode
	{
		// Token: 0x0600880D RID: 34829 RVA: 0x0030D1C0 File Offset: 0x0030B3C0
		protected override void RunInt()
		{
			if (!ModLister.CheckIdeology("Reliquary pilgrims"))
			{
				return;
			}
			Quest quest = QuestGen.quest;
			Slate slate = QuestGen.slate;
			Map map = QuestGen_Get.GetMap(false, null);
			int randomInRange = QuestNode_Root_ReliquaryPilgrims.PilgrimCount.RandomInRange;
			FactionDef facDef;
			PawnKindDef kindDef;
			this.GetFactionAndPawnKind(out facDef, out kindDef);
			Precept_Relic precept_Relic;
			Building building;
			Thing var;
			this.TryFindReliquaryWithRelic(map, out precept_Relic, out building, out var);
			List<FactionRelation> list = new List<FactionRelation>();
			foreach (Faction faction2 in Find.FactionManager.AllFactionsListForReading)
			{
				if (!faction2.def.permanentEnemy)
				{
					list.Add(new FactionRelation
					{
						other = faction2,
						kind = FactionRelationKind.Neutral
					});
				}
			}
			Faction faction = FactionGenerator.NewGeneratedFactionWithRelations(facDef, list, true);
			faction.temporary = true;
			Find.FactionManager.Add(faction);
			List<Pawn> list2 = new List<Pawn>();
			for (int i = 0; i < randomInRange; i++)
			{
				Pawn pawn = quest.GeneratePawn(kindDef, faction, true, null, 0f, true, null, 0f, 0f, false, true);
				pawn.ideo.SetIdeo(precept_Relic.ideo);
				list2.Add(pawn);
			}
			quest.SetFactionHidden(faction, false, null);
			quest.PawnsArrive(list2, null, map.Parent, null, false, null, "[pilgrimsArrivedLetterLabel]", "[pilgrimsArrivedLetterText]", null, null, false, false, true);
			string text = QuestGen.GenerateNewSignal("VenerationCompleted", true);
			string text2 = QuestGenUtility.HardcodedSignalWithQuestID("relicThing.Despawned");
			QuestPart_Venerate questPart_Venerate = new QuestPart_Venerate();
			questPart_Venerate.inSignal = QuestGen.slate.Get<string>("inSignal", null, false);
			questPart_Venerate.inSignalForceExit = text2;
			questPart_Venerate.pawns.AddRange(list2);
			questPart_Venerate.target = building;
			questPart_Venerate.venerateDurationTicks = QuestNode_Root_ReliquaryPilgrims.VisitDurationTicksRange.RandomInRange;
			questPart_Venerate.faction = faction;
			questPart_Venerate.mapParent = map.Parent;
			questPart_Venerate.outSignalVenerationCompleted = text;
			quest.AddPart(questPart_Venerate);
			Quest quest2 = quest;
			string message = "[pilgrimsLeavingMessage]";
			MessageTypeDef neutralEvent = MessageTypeDefOf.NeutralEvent;
			bool getLookTargetsFromSignal = false;
			RulePack rules = null;
			string inSignal = text;
			quest2.Message(message, neutralEvent, getLookTargetsFromSignal, rules, list2, inSignal);
			string text3 = QuestGen.GenerateNewSignal("AllLeftMap", true);
			QuestPart_PassAll questPart_PassAll = new QuestPart_PassAll();
			questPart_PassAll.outSignal = text3;
			quest.AddPart(questPart_PassAll);
			slate.Set<List<Pawn>>("pawns", list2, false);
			slate.Set<Faction>("faction", faction, false);
			foreach (Pawn pawn2 in list2)
			{
				string text4 = QuestGenUtility.HardcodedTargetQuestTagWithQuestID(pawn2.ThingID);
				QuestUtility.AddQuestTag(pawn2, text4);
				string item = QuestGenUtility.HardcodedSignalWithQuestID(text4 + ".LeftMap");
				questPart_PassAll.inSignals.Add(item);
			}
			ThingSetMakerParams parms = default(ThingSetMakerParams);
			parms.totalMarketValueRange = new FloatRange?(QuestNode_Root_ReliquaryPilgrims.RewardMarketValueRange);
			parms.qualityGenerator = new QualityGenerator?(QualityGenerator.Reward);
			parms.makingFaction = faction;
			parms.countRange = new IntRange?(new IntRange(1, 1));
			List<Thing> list3 = ThingSetMakerDefOf.Reward_ReliquaryPilgrims.root.Generate(parms);
			QuestPart_DelayedRewardDropPods delayedReward = new QuestPart_DelayedRewardDropPods();
			delayedReward.inSignal = text3;
			delayedReward.faction = faction;
			delayedReward.giver = list2[0];
			delayedReward.rewards.AddRange(list3);
			delayedReward.delayTicks = QuestNode_Root_ReliquaryPilgrims.RewardDelayRangeTicks.RandomInRange;
			delayedReward.chance = QuestNode_Root_ReliquaryPilgrims.RewardChance;
			QuestGen.AddTextRequest("root", delegate(string x)
			{
				delayedReward.customLetterText = x;
			}, QuestGenUtility.MergeRules(null, "[delayedRewardLetterText]", "root"));
			quest.Message("[pilgrimsLeftMessage]", MessageTypeDefOf.NeutralEvent, false, null, null, text3);
			quest.AddPart(delayedReward);
			string item2 = QuestGenUtility.HardcodedSignalWithQuestID("pawns.Arrested");
			string item3 = QuestGenUtility.HardcodedSignalWithQuestID("pawns.Killed");
			quest.AnySignal(new List<string>
			{
				item2,
				item3
			}, delegate
			{
				QuestPart_FactionRelationChange questPart_FactionRelationChange = new QuestPart_FactionRelationChange();
				questPart_FactionRelationChange.faction = faction;
				questPart_FactionRelationChange.relationKind = FactionRelationKind.Hostile;
				questPart_FactionRelationChange.canSendHostilityLetter = false;
				questPart_FactionRelationChange.inSignal = QuestGen.slate.Get<string>("inSignal", null, false);
				quest.AddPart(questPart_FactionRelationChange);
			}, null, QuestPart.SignalListenMode.OngoingOnly);
			string text5 = QuestGen.GenerateNewSignal("RelicInvalidated", true);
			QuestPart_PassAnyActivable questPart_PassAnyActivable = new QuestPart_PassAnyActivable();
			questPart_PassAnyActivable.inSignalEnable = QuestGen.slate.Get<string>("inSignal", null, false);
			questPart_PassAnyActivable.inSignals.Add(text2);
			questPart_PassAnyActivable.inSignals.Add(QuestGenUtility.HardcodedSignalWithQuestID("reliquary.Destroyed"));
			questPart_PassAnyActivable.outSignalsCompleted.Add(text5);
			questPart_PassAnyActivable.inSignalDisable = text;
			quest.AddPart(questPart_PassAnyActivable);
			quest.End(QuestEndOutcome.Fail, 0, null, text5, QuestPart.SignalListenMode.OngoingOnly, true);
			quest.End(QuestEndOutcome.Fail, 0, null, QuestGenUtility.HardcodedSignalWithQuestID("faction.BecameHostileToPlayer"), QuestPart.SignalListenMode.OngoingOnly, true);
			quest.End(QuestEndOutcome.Success, 0, null, text3, QuestPart.SignalListenMode.OngoingOnly, false);
			slate.Set<Precept_Relic>("relic", precept_Relic, false);
			slate.Set<Thing>("relicThing", var, false);
			slate.Set<Building>("reliquary", building, false);
			slate.Set<int>("pilgrimCount", randomInRange, false);
			slate.Set<string>("rewards", GenLabel.ThingsLabel(list3, "  - "), false);
			slate.Set<float>("rewardsMarketValue", TradeUtility.TotalMarketValue(list3), false);
			slate.Set<string>("venerateDate", GenDate.DateFullStringAt((long)GenDate.TickGameToAbs(quest.acceptanceTick), Find.WorldGrid.LongLatOf(map.Tile)), false);
			slate.Set<bool>("pilgrimFaction", faction.def == FactionDefOf.Pilgrims, false);
		}

		// Token: 0x0600880E RID: 34830 RVA: 0x0030D7A8 File Offset: 0x0030B9A8
		private bool TryFindReliquaryWithRelic(Map map, out Precept_Relic relic, out Building reliquary, out Thing relicThing)
		{
			foreach (Thing thing in map.listerThings.ThingsOfDef(ThingDefOf.Reliquary).InRandomOrder(null))
			{
				CompThingContainer compThingContainer = thing.TryGetComp<CompThingContainer>();
				if (compThingContainer != null)
				{
					foreach (Thing thing2 in ((IEnumerable<Thing>)compThingContainer.GetDirectlyHeldThings()))
					{
						Precept_Relic precept_Relic;
						if ((precept_Relic = (thing2.StyleSourcePrecept as Precept_Relic)) != null)
						{
							reliquary = (Building)thing;
							relic = precept_Relic;
							relicThing = thing2;
							return true;
						}
					}
				}
			}
			reliquary = null;
			relic = null;
			relicThing = null;
			return false;
		}

		// Token: 0x0600880F RID: 34831 RVA: 0x0030D874 File Offset: 0x0030BA74
		private void GetFactionAndPawnKind(out FactionDef factionDef, out PawnKindDef pawnKind)
		{
			if (Rand.Bool)
			{
				factionDef = FactionDefOf.Pilgrims;
				pawnKind = PawnKindDefOf.PovertyPilgrim;
				return;
			}
			factionDef = FactionDefOf.OutlanderCivil;
			pawnKind = PawnKindDefOf.WellEquippedTraveler;
		}

		// Token: 0x06008810 RID: 34832 RVA: 0x0030D89C File Offset: 0x0030BA9C
		protected override bool TestRunInt(Slate slate)
		{
			Map map = QuestGen_Get.GetMap(false, null);
			Precept_Relic precept_Relic;
			Building building;
			Thing thing;
			return this.TryFindReliquaryWithRelic(map, out precept_Relic, out building, out thing);
		}

		// Token: 0x040055FC RID: 22012
		private static IntRange VisitDurationTicksRange = new IntRange(5000, 10000);

		// Token: 0x040055FD RID: 22013
		private static IntRange PilgrimCount = new IntRange(1, 4);

		// Token: 0x040055FE RID: 22014
		private static float RewardChance = 0.5f;

		// Token: 0x040055FF RID: 22015
		private static FloatRange RewardMarketValueRange = new FloatRange(1000f, 2000f);

		// Token: 0x04005600 RID: 22016
		private static IntRange RewardDelayRangeTicks = new IntRange(300000, 600000);

		// Token: 0x04005601 RID: 22017
		private const string RootSymbol = "root";
	}
}
