using System;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200168E RID: 5774
	public class QuestNode_GetSameQuestsCount : QuestNode
	{
		// Token: 0x06008647 RID: 34375 RVA: 0x0030271C File Offset: 0x0030091C
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x06008648 RID: 34376 RVA: 0x00302726 File Offset: 0x00300926
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x06008649 RID: 34377 RVA: 0x00302734 File Offset: 0x00300934
		private void SetVars(Slate slate)
		{
			int var = Find.QuestManager.QuestsListForReading.Count((Quest x) => x.root == QuestGen.Root);
			slate.Set<int>("sameQuestsCount", var, false);
		}

		// Token: 0x04005414 RID: 21524
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
