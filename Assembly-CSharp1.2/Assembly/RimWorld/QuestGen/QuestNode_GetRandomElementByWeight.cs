using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F4A RID: 8010
	public class QuestNode_GetRandomElementByWeight : QuestNode
	{
		// Token: 0x0600AB03 RID: 43779 RVA: 0x0006FEBF File Offset: 0x0006E0BF
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x0600AB04 RID: 43780 RVA: 0x0006FEC9 File Offset: 0x0006E0C9
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x0600AB05 RID: 43781 RVA: 0x0031DF0C File Offset: 0x0031C10C
		private void SetVars(Slate slate)
		{
			QuestNode_GetRandomElementByWeight.Option option;
			if (this.options.TryRandomElementByWeight((QuestNode_GetRandomElementByWeight.Option x) => x.weight, out option))
			{
				slate.Set<object>(this.storeAs.GetValue(slate), option.element.GetValue(slate), false);
			}
		}

		// Token: 0x0400745D RID: 29789
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x0400745E RID: 29790
		public List<QuestNode_GetRandomElementByWeight.Option> options = new List<QuestNode_GetRandomElementByWeight.Option>();

		// Token: 0x02001F4B RID: 8011
		public class Option
		{
			// Token: 0x0400745F RID: 29791
			public SlateRef<object> element;

			// Token: 0x04007460 RID: 29792
			public float weight;
		}
	}
}
