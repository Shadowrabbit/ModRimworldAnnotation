using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020016D3 RID: 5843
	public class QuestNode_Notify_PlayerRaidedSomeone : QuestNode
	{
		// Token: 0x06008735 RID: 34613 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x06008736 RID: 34614 RVA: 0x003065C0 File Offset: 0x003047C0
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_Notify_PlayerRaidedSomeone questPart_Notify_PlayerRaidedSomeone = new QuestPart_Notify_PlayerRaidedSomeone();
			questPart_Notify_PlayerRaidedSomeone.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_Notify_PlayerRaidedSomeone.getRaidersFromMap = this.getRaidersFromMap.GetValue(slate);
			questPart_Notify_PlayerRaidedSomeone.getRaidersFromMapParent = this.getRaidersFromMapParent.GetValue(slate);
			QuestGen.quest.AddPart(questPart_Notify_PlayerRaidedSomeone);
		}

		// Token: 0x04005542 RID: 21826
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04005543 RID: 21827
		public SlateRef<Map> getRaidersFromMap;

		// Token: 0x04005544 RID: 21828
		public SlateRef<MapParent> getRaidersFromMapParent;
	}
}
