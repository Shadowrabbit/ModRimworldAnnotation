using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020016B5 RID: 5813
	public class QuestNode_ChangeHeir : QuestNode
	{
		// Token: 0x060086D2 RID: 34514 RVA: 0x00304AA8 File Offset: 0x00302CA8
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

		// Token: 0x060086D3 RID: 34515 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0400549E RID: 21662
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x0400549F RID: 21663
		public SlateRef<Faction> faction;

		// Token: 0x040054A0 RID: 21664
		public SlateRef<Thing> factionOf;

		// Token: 0x040054A1 RID: 21665
		public SlateRef<Pawn> holder;

		// Token: 0x040054A2 RID: 21666
		public SlateRef<Pawn> heir;
	}
}
