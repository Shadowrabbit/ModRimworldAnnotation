using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001FA6 RID: 8102
	public class QuestNode_MakeMinified : QuestNode
	{
		// Token: 0x0600AC32 RID: 44082 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600AC33 RID: 44083 RVA: 0x003215C0 File Offset: 0x0031F7C0
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			MinifiedThing var = this.thing.GetValue(slate).MakeMinified();
			QuestGen.slate.Set<MinifiedThing>(this.storeAs.GetValue(slate), var, false);
		}

		// Token: 0x0400759D RID: 30109
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x0400759E RID: 30110
		public SlateRef<Thing> thing;
	}
}
