using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F2A RID: 7978
	public class QuestNode_GetMapWealth : QuestNode
	{
		// Token: 0x0600AA8E RID: 43662 RVA: 0x0006FB9A File Offset: 0x0006DD9A
		protected override bool TestRunInt(Slate slate)
		{
			slate.Set<float>(this.storeAs.GetValue(slate), this.map.GetValue(slate).wealthWatcher.WealthTotal, false);
			return true;
		}

		// Token: 0x0600AA8F RID: 43663 RVA: 0x0031BE2C File Offset: 0x0031A02C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			slate.Set<float>(this.storeAs.GetValue(slate), this.map.GetValue(slate).wealthWatcher.WealthTotal, false);
		}

		// Token: 0x040073E7 RID: 29671
		public SlateRef<Map> map;

		// Token: 0x040073E8 RID: 29672
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
