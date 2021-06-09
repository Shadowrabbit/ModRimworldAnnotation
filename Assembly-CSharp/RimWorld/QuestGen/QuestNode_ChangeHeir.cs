using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F88 RID: 8072
	public class QuestNode_ChangeHeir : QuestNode
	{
		// Token: 0x0600ABD1 RID: 43985 RVA: 0x00320264 File Offset: 0x0031E464
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_ChangeHeir questPart_ChangeHeir = new QuestPart_ChangeHeir();
			questPart_ChangeHeir.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_ChangeHeir.faction = (this.faction.GetValue(slate) ?? this.factionOf.GetValue(slate).Faction);
			questPart_ChangeHeir.holder = this.holder.GetValue(slate);
			questPart_ChangeHeir.heir = this.heir.GetValue(slate);
			QuestGen.quest.AddPart(questPart_ChangeHeir);
		}

		// Token: 0x0600ABD2 RID: 43986 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0400751C RID: 29980
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x0400751D RID: 29981
		public SlateRef<Faction> faction;

		// Token: 0x0400751E RID: 29982
		public SlateRef<Thing> factionOf;

		// Token: 0x0400751F RID: 29983
		public SlateRef<Pawn> holder;

		// Token: 0x04007520 RID: 29984
		public SlateRef<Pawn> heir;
	}
}
