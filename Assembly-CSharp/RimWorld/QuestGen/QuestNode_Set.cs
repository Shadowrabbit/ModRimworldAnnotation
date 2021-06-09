using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001EFF RID: 7935
	public class QuestNode_Set : QuestNode
	{
		// Token: 0x0600A9FD RID: 43517 RVA: 0x0006F807 File Offset: 0x0006DA07
		protected override bool TestRunInt(Slate slate)
		{
			slate.Set<object>(this.name.GetValue(slate), this.value.GetValue(slate), false);
			return true;
		}

		// Token: 0x0600A9FE RID: 43518 RVA: 0x0031A3CC File Offset: 0x003185CC
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestGen.slate.Set<object>(this.name.GetValue(slate), this.value.GetValue(slate), false);
		}

		// Token: 0x0400734B RID: 29515
		[NoTranslate]
		public SlateRef<string> name;

		// Token: 0x0400734C RID: 29516
		public SlateRef<object> value;
	}
}
