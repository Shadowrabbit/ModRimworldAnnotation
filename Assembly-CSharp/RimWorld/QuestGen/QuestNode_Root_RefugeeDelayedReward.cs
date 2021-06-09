using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001FCD RID: 8141
	public class QuestNode_Root_RefugeeDelayedReward : QuestNode
	{
		// Token: 0x0600ACB3 RID: 44211 RVA: 0x0032376C File Offset: 0x0032196C
		protected override void RunInt()
		{
			Quest quest = QuestGen.quest;
			Slate slate = QuestGen.slate;
			Map map = QuestGen_Get.GetMap(false, null);
			Faction faction = slate.Get<Faction>("faction", null, false);
			FloatRange marketValueRange = slate.Get<FloatRange>("marketValueRange", default(FloatRange), false);
			Pawn val = slate.Get<Pawn>("rewardGiver", null, false);
			quest.ReservePawns(Gen.YieldSingle<Pawn>(val));
			quest.ReserveFaction(faction);
			int num = Rand.Range(5, 20) * 60000;
			slate.Set<int>("rewardDelayTicks", num, false);
			quest.Delay(num, delegate
			{
				ThingSetMakerParams parms = default(ThingSetMakerParams);
				parms.totalMarketValueRange = new FloatRange?(marketValueRange);
				parms.qualityGenerator = new QualityGenerator?(QualityGenerator.Reward);
				parms.makingFaction = faction;
				List<Thing> list = ThingSetMakerDefOf.Reward_ItemsStandard.root.Generate(parms);
				slate.Set<string>("listOfRewards", GenLabel.ThingsLabel(list, "  - "), false);
				quest.DropPods(map.Parent, list, null, null, "[rewardLetterText]", null, new bool?(true), false, false, false, null, null, QuestPart.SignalListenMode.OngoingOnly, null, true);
				quest.End(QuestEndOutcome.Unknown, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false);
			}, null, null, null, false, null, null, false, null, null, "RewardDelay", false, QuestPart.SignalListenMode.OngoingOnly);
		}

		// Token: 0x0600ACB4 RID: 44212 RVA: 0x00323864 File Offset: 0x00321A64
		protected override bool TestRunInt(Slate slate)
		{
			FloatRange floatRange;
			return slate.Get<Pawn>("rewardGiver", null, false) != null && slate.TryGet<FloatRange>("marketValueRange", out floatRange, false) && slate.Get<Faction>("faction", null, false) != null;
		}
	}
}
