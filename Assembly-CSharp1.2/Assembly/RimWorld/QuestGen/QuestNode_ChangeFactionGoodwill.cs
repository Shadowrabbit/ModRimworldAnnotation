using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F84 RID: 8068
	public class QuestNode_ChangeFactionGoodwill : QuestNode
	{
		// Token: 0x0600ABC4 RID: 43972 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600ABC5 RID: 43973 RVA: 0x0031FF88 File Offset: 0x0031E188
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_FactionGoodwillChange questPart_FactionGoodwillChange = new QuestPart_FactionGoodwillChange();
			questPart_FactionGoodwillChange.change = this.change.GetValue(slate);
			questPart_FactionGoodwillChange.faction = (this.faction.GetValue(slate) ?? this.factionOf.GetValue(slate).Faction);
			questPart_FactionGoodwillChange.reason = this.reason.GetValue(slate);
			questPart_FactionGoodwillChange.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			QuestGen.quest.AddPart(questPart_FactionGoodwillChange);
		}

		// Token: 0x0400750B RID: 29963
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x0400750C RID: 29964
		public SlateRef<Faction> faction;

		// Token: 0x0400750D RID: 29965
		public SlateRef<Thing> factionOf;

		// Token: 0x0400750E RID: 29966
		public SlateRef<int> change;

		// Token: 0x0400750F RID: 29967
		public SlateRef<string> reason;
	}
}
