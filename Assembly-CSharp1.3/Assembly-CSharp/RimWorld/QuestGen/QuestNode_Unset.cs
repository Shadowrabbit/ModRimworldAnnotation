using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001648 RID: 5704
	public class QuestNode_Unset : QuestNode
	{
		// Token: 0x06008540 RID: 34112 RVA: 0x002FDBC6 File Offset: 0x002FBDC6
		protected override bool TestRunInt(Slate slate)
		{
			slate.Remove(this.name.GetValue(slate), false);
			return true;
		}

		// Token: 0x06008541 RID: 34113 RVA: 0x002FDBE0 File Offset: 0x002FBDE0
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestGen.slate.Remove(this.name.GetValue(slate), false);
		}

		// Token: 0x0400530E RID: 21262
		[NoTranslate]
		public SlateRef<string> name;
	}
}
