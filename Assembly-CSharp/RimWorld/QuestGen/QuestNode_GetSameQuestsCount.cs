using System;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F56 RID: 8022
	public class QuestNode_GetSameQuestsCount : QuestNode
	{
		// Token: 0x0600AB2C RID: 43820 RVA: 0x0007000A File Offset: 0x0006E20A
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x0600AB2D RID: 43821 RVA: 0x00070014 File Offset: 0x0006E214
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x0600AB2E RID: 43822 RVA: 0x0031E66C File Offset: 0x0031C86C
		private void SetVars(Slate slate)
		{
			int var = Find.QuestManager.QuestsListForReading.Count((Quest x) => x.root == QuestGen.Root);
			slate.Set<int>("sameQuestsCount", var, false);
		}

		// Token: 0x04007481 RID: 29825
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
