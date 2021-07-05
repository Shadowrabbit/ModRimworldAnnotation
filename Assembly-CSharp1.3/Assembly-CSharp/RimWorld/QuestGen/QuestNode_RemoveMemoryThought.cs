using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020016DA RID: 5850
	public class QuestNode_RemoveMemoryThought : QuestNode
	{
		// Token: 0x0600874C RID: 34636 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600874D RID: 34637 RVA: 0x00306DB4 File Offset: 0x00304FB4
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

		// Token: 0x04005565 RID: 21861
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04005566 RID: 21862
		public SlateRef<ThoughtDef> def;

		// Token: 0x04005567 RID: 21863
		public SlateRef<Pawn> pawn;

		// Token: 0x04005568 RID: 21864
		public SlateRef<Pawn> otherPawn;

		// Token: 0x04005569 RID: 21865
		public SlateRef<int?> count;

		// Token: 0x0400556A RID: 21866
		public SlateRef<bool?> addToLookTargets;
	}
}
