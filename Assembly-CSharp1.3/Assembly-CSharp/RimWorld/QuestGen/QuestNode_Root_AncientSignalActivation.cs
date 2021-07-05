using System;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x020016F1 RID: 5873
	public class QuestNode_Root_AncientSignalActivation : QuestNode
	{
		// Token: 0x06008796 RID: 34710 RVA: 0x00307DF8 File Offset: 0x00305FF8
		protected override void RunInt()
		{
			QuestNode_Root_AncientSignalActivation.<>c__DisplayClass4_0 CS$<>8__locals1 = new QuestNode_Root_AncientSignalActivation.<>c__DisplayClass4_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.quest = QuestGen.quest;
			Slate slate = QuestGen.slate;
			QuestNode_Root_AncientSignalActivation.<>c__DisplayClass4_0 CS$<>8__locals2 = CS$<>8__locals1;
			Map map;
			if ((map = slate.Get<Map>("map", null, false)) == null)
			{
				map = QuestGen_Get.GetMap(false, null);
			}
			CS$<>8__locals2.map = map;
			CS$<>8__locals1.dropSpot = null;
			LookTargets lookTargets = null;
			IntVec3 intVec;
			if (this.TryFindRandomDropSpot(CS$<>8__locals1.map, out intVec))
			{
				lookTargets = new LookTargets(intVec, CS$<>8__locals1.map);
				CS$<>8__locals1.dropSpot = new IntVec3?(intVec);
			}
			Quest quest = CS$<>8__locals1.quest;
			string message = "MessageAncientSignalActivated".Translate();
			MessageTypeDef negativeEvent = MessageTypeDefOf.NegativeEvent;
			bool getLookTargetsFromSignal = false;
			RulePack rules = null;
			string initiateSignal = CS$<>8__locals1.quest.InitiateSignal;
			quest.Message(message, negativeEvent, getLookTargetsFromSignal, rules, lookTargets, initiateSignal);
			ThingSetMakerParams parms = default(ThingSetMakerParams);
			parms.qualityGenerator = new QualityGenerator?(QualityGenerator.Reward);
			parms.makingFaction = Faction.OfAncients;
			CS$<>8__locals1.rewards = ThingSetMakerDefOf.ResourcePod.root.Generate(parms);
			CS$<>8__locals1.quest.Delay(180, delegate
			{
				CS$<>8__locals1.quest.DropPods(CS$<>8__locals1.map.Parent, CS$<>8__locals1.rewards, null, null, null, null, new bool?(true), false, false, false, null, null, QuestPart.SignalListenMode.OngoingOnly, CS$<>8__locals1.dropSpot, true);
				Faction faction;
				if (Rand.Chance(QuestNode_Root_AncientSignalActivation.RaidChance) && Find.Storyteller.difficulty.allowViolentQuests && CS$<>8__locals1.<>4__this.TryFindRandomEnemyFaction(out faction))
				{
					CS$<>8__locals1.quest.Delay(QuestNode_Root_AncientSignalActivation.RaidDelayTicks, delegate
					{
						CS$<>8__locals1.quest.Raid(CS$<>8__locals1.map, QuestNode_Root_AncientSignalActivation.RaidRewardPoints.min, faction, null, null, "MessageAncientSignalHostileDetected".Translate(faction.def.pawnsPlural.CapitalizeFirst(), faction), null, null, null, null, null, "root", PawnsArrivalModeDefOf.EdgeWalkIn, RaidStrategyDefOf.ImmediateAttack);
						CS$<>8__locals1.quest.End(QuestEndOutcome.Unknown, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false);
					}, null, null, null, false, null, null, false, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly);
					return;
				}
				CS$<>8__locals1.quest.End(QuestEndOutcome.Unknown, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false);
			}, null, null, null, false, null, null, false, null, null, "RewardDelay", false, QuestPart.SignalListenMode.OngoingOnly);
		}

		// Token: 0x06008797 RID: 34711 RVA: 0x00307F18 File Offset: 0x00306118
		private bool TryFindRandomEnemyFaction(out Faction faction)
		{
			faction = Find.FactionManager.RandomEnemyFaction(false, false, true, TechLevel.Industrial);
			return faction != null;
		}

		// Token: 0x06008798 RID: 34712 RVA: 0x00307F30 File Offset: 0x00306130
		private bool TryFindRandomDropSpot(Map map, out IntVec3 result)
		{
			return CellFinderLoose.TryGetRandomCellWith((IntVec3 x) => x.Standable(map) && !x.Roofed(map) && !x.Fogged(map), map, 1000, out result);
		}

		// Token: 0x06008799 RID: 34713 RVA: 0x00307F67 File Offset: 0x00306167
		protected override bool TestRunInt(Slate slate)
		{
			return ModLister.CheckIdeology("Ancient signal activation quest");
		}

		// Token: 0x040055B0 RID: 21936
		private const int DropPodDelayTicks = 180;

		// Token: 0x040055B1 RID: 21937
		private static int RaidDelayTicks = 60 * Rand.Range(5, 10);

		// Token: 0x040055B2 RID: 21938
		private static FloatRange RaidRewardPoints = new FloatRange(150f, 200f);

		// Token: 0x040055B3 RID: 21939
		private static float RaidChance = 0.5f;
	}
}
