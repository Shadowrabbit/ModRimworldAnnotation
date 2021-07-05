using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001677 RID: 5751
	public class QuestNode_GetMapWealth : QuestNode
	{
		// Token: 0x060085E4 RID: 34276 RVA: 0x00300044 File Offset: 0x002FE244
		protected override bool TestRunInt(Slate slate)
		{
			slate.Set<float>(this.storeAs.GetValue(slate), this.map.GetValue(slate).wealthWatcher.WealthTotal, false);
			return true;
		}

		// Token: 0x060085E5 RID: 34277 RVA: 0x00300070 File Offset: 0x002FE270
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			slate.Set<float>(this.storeAs.GetValue(slate), this.map.GetValue(slate).wealthWatcher.WealthTotal, false);
		}

		// Token: 0x040053B0 RID: 21424
		public SlateRef<Map> map;

		// Token: 0x040053B1 RID: 21425
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
