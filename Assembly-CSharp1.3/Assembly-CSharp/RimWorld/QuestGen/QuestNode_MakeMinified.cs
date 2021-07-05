using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020016CF RID: 5839
	public class QuestNode_MakeMinified : QuestNode
	{
		// Token: 0x06008729 RID: 34601 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600872A RID: 34602 RVA: 0x00306304 File Offset: 0x00304504
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			MinifiedThing var = this.thing.GetValue(slate).MakeMinified();
			QuestGen.slate.Set<MinifiedThing>(this.storeAs.GetValue(slate), var, false);
		}

		// Token: 0x04005530 RID: 21808
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04005531 RID: 21809
		public SlateRef<Thing> thing;
	}
}
