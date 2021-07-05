using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F49 RID: 8009
	public class QuestNode_GetRandomElement : QuestNode
	{
		// Token: 0x0600AAFF RID: 43775 RVA: 0x0006FEA8 File Offset: 0x0006E0A8
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x0600AB00 RID: 43776 RVA: 0x0006FEB2 File Offset: 0x0006E0B2
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x0600AB01 RID: 43777 RVA: 0x0031DED4 File Offset: 0x0031C0D4
		private void SetVars(Slate slate)
		{
			SlateRef<object> slateRef;
			if (this.options.TryRandomElement(out slateRef))
			{
				slate.Set<object>(this.storeAs.GetValue(slate), slateRef.GetValue(slate), false);
			}
		}

		// Token: 0x0400745B RID: 29787
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x0400745C RID: 29788
		public List<SlateRef<object>> options;
	}
}
