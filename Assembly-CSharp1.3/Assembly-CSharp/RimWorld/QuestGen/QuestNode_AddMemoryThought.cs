using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020016AB RID: 5803
	public class QuestNode_AddMemoryThought : QuestNode
	{
		// Token: 0x060086B3 RID: 34483 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x060086B4 RID: 34484 RVA: 0x00304430 File Offset: 0x00302630
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.pawns.GetValue(slate) != null)
			{
				foreach (Pawn pawn in this.pawns.GetValue(slate))
				{
					QuestPart_AddMemoryThought questPart_AddMemoryThought = new QuestPart_AddMemoryThought();
					questPart_AddMemoryThought.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
					questPart_AddMemoryThought.def = this.def.GetValue(slate);
					questPart_AddMemoryThought.pawn = pawn;
					questPart_AddMemoryThought.otherPawn = this.otherPawn.GetValue(slate);
					questPart_AddMemoryThought.addToLookTargets = (this.addToLookTargets.GetValue(slate) ?? true);
					QuestGen.quest.AddPart(questPart_AddMemoryThought);
				}
			}
		}

		// Token: 0x0400547C RID: 21628
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x0400547D RID: 21629
		public SlateRef<ThoughtDef> def;

		// Token: 0x0400547E RID: 21630
		public SlateRef<IEnumerable<Pawn>> pawns;

		// Token: 0x0400547F RID: 21631
		public SlateRef<Pawn> otherPawn;

		// Token: 0x04005480 RID: 21632
		public SlateRef<bool?> addToLookTargets;
	}
}
