using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001686 RID: 5766
	public class QuestNode_GetRandomElement : QuestNode
	{
		// Token: 0x06008626 RID: 34342 RVA: 0x00301F27 File Offset: 0x00300127
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x06008627 RID: 34343 RVA: 0x00301F31 File Offset: 0x00300131
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x06008628 RID: 34344 RVA: 0x00301F40 File Offset: 0x00300140
		private void SetVars(Slate slate)
		{
			SlateRef<object> slateRef;
			if (this.options.TryRandomElement(out slateRef))
			{
				slate.Set<object>(this.storeAs.GetValue(slate), slateRef.GetValue(slate), false);
			}
		}

		// Token: 0x040053FA RID: 21498
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x040053FB RID: 21499
		public List<SlateRef<object>> options;
	}
}
