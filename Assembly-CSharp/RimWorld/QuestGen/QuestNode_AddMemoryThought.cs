using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F7F RID: 8063
	public class QuestNode_AddMemoryThought : QuestNode
	{
		// Token: 0x0600ABB5 RID: 43957 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600ABB6 RID: 43958 RVA: 0x0031FD34 File Offset: 0x0031DF34
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

		// Token: 0x040074FF RID: 29951
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04007500 RID: 29952
		public SlateRef<ThoughtDef> def;

		// Token: 0x04007501 RID: 29953
		public SlateRef<IEnumerable<Pawn>> pawns;

		// Token: 0x04007502 RID: 29954
		public SlateRef<Pawn> otherPawn;

		// Token: 0x04007503 RID: 29955
		public SlateRef<bool?> addToLookTargets;
	}
}
