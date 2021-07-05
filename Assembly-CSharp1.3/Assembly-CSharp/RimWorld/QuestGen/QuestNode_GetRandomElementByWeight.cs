using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001687 RID: 5767
	public class QuestNode_GetRandomElementByWeight : QuestNode
	{
		// Token: 0x0600862A RID: 34346 RVA: 0x00301F77 File Offset: 0x00300177
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x0600862B RID: 34347 RVA: 0x00301F81 File Offset: 0x00300181
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x0600862C RID: 34348 RVA: 0x00301F90 File Offset: 0x00300190
		private void SetVars(Slate slate)
		{
			QuestNode_GetRandomElementByWeight.Option option;
			if (this.options.TryRandomElementByWeight((QuestNode_GetRandomElementByWeight.Option x) => x.weight, out option))
			{
				slate.Set<object>(this.storeAs.GetValue(slate), option.element.GetValue(slate), false);
			}
		}

		// Token: 0x040053FC RID: 21500
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x040053FD RID: 21501
		public List<QuestNode_GetRandomElementByWeight.Option> options = new List<QuestNode_GetRandomElementByWeight.Option>();

		// Token: 0x02002930 RID: 10544
		public class Option
		{
			// Token: 0x04009B19 RID: 39705
			public SlateRef<object> element;

			// Token: 0x04009B1A RID: 39706
			public float weight;
		}
	}
}
