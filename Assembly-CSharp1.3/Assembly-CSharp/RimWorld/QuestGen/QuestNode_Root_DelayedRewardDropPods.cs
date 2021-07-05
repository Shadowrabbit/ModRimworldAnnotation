using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020016F9 RID: 5881
	public class QuestNode_Root_DelayedRewardDropPods : QuestNode
	{
		// Token: 0x060087BF RID: 34751 RVA: 0x003095B0 File Offset: 0x003077B0
		protected override void RunInt()
		{
			Quest quest = QuestGen.quest;
			Slate slate = QuestGen.slate;
			Map map = QuestGen_Get.GetMap(false, null);
			List<Thing> rewards = slate.Get<List<Thing>>("rewards", null, false);
			Faction faction = slate.Get<Faction>("faction", null, false);
			Pawn pawn = slate.Get<Pawn>("giver", null, false);
			int delayTicks = slate.Get<int>("delayTicks", 0, false);
			string customerLetterLabel = slate.Get<string>("customLetterLabel", null, false);
			string customerLetterText = slate.Get<string>("customLetterText", null, false);
			if (pawn != null)
			{
				quest.ReservePawns(Gen.YieldSingle<Pawn>(pawn));
			}
			if (faction != null)
			{
				quest.ReserveFaction(faction);
			}
			quest.Delay(delayTicks, delegate
			{
				quest.DropPods(map.Parent, rewards, customerLetterLabel, null, customerLetterText, null, new bool?(true), false, false, false, null, null, QuestPart.SignalListenMode.OngoingOnly, null, true);
				quest.End(QuestEndOutcome.Unknown, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false);
			}, null, null, null, false, null, null, false, null, null, "RewardDelay", false, QuestPart.SignalListenMode.OngoingOnly);
		}

		// Token: 0x060087C0 RID: 34752 RVA: 0x0030969D File Offset: 0x0030789D
		protected override bool TestRunInt(Slate slate)
		{
			return slate.Get<List<Thing>>("rewards", null, false) != null;
		}
	}
}
