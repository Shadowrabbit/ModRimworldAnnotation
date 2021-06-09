using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F68 RID: 8040
	public class QuestNode_SetFactionRelations : QuestNode
	{
		// Token: 0x0600AB65 RID: 43877 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600AB66 RID: 43878 RVA: 0x0031F418 File Offset: 0x0031D618
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_SetFactionRelations questPart_SetFactionRelations = new QuestPart_SetFactionRelations();
			questPart_SetFactionRelations.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_SetFactionRelations.faction = this.faction.GetValue(slate);
			questPart_SetFactionRelations.relationKind = this.relationKind.GetValue(slate);
			questPart_SetFactionRelations.canSendLetter = (this.sendLetter.GetValue(slate) ?? true);
			QuestGen.quest.AddPart(questPart_SetFactionRelations);
		}

		// Token: 0x040074B5 RID: 29877
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x040074B6 RID: 29878
		public SlateRef<Faction> faction;

		// Token: 0x040074B7 RID: 29879
		public SlateRef<FactionRelationKind> relationKind;

		// Token: 0x040074B8 RID: 29880
		public SlateRef<bool?> sendLetter;

		// Token: 0x040074B9 RID: 29881
		private const string RootSymbol = "root";
	}
}
