using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001FB1 RID: 8113
	public class QuestNode_RemoveMemoryThought : QuestNode
	{
		// Token: 0x0600AC54 RID: 44116 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600AC55 RID: 44117 RVA: 0x00321EBC File Offset: 0x003200BC
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_RemoveMemoryThought questPart_RemoveMemoryThought = new QuestPart_RemoveMemoryThought();
			questPart_RemoveMemoryThought.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_RemoveMemoryThought.def = this.def.GetValue(slate);
			questPart_RemoveMemoryThought.pawn = this.pawn.GetValue(slate);
			questPart_RemoveMemoryThought.count = this.count.GetValue(slate);
			questPart_RemoveMemoryThought.otherPawn = this.otherPawn.GetValue(slate);
			questPart_RemoveMemoryThought.addToLookTargets = (this.addToLookTargets.GetValue(slate) ?? true);
			QuestGen.quest.AddPart(questPart_RemoveMemoryThought);
		}

		// Token: 0x040075CE RID: 30158
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x040075CF RID: 30159
		public SlateRef<ThoughtDef> def;

		// Token: 0x040075D0 RID: 30160
		public SlateRef<Pawn> pawn;

		// Token: 0x040075D1 RID: 30161
		public SlateRef<Pawn> otherPawn;

		// Token: 0x040075D2 RID: 30162
		public SlateRef<int?> count;

		// Token: 0x040075D3 RID: 30163
		public SlateRef<bool?> addToLookTargets;
	}
}
