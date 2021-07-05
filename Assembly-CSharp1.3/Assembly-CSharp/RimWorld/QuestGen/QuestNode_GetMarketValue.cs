using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001678 RID: 5752
	public class QuestNode_GetMarketValue : QuestNode
	{
		// Token: 0x060085E7 RID: 34279 RVA: 0x003000AC File Offset: 0x002FE2AC
		protected override bool TestRunInt(Slate slate)
		{
			return this.DoWork(slate);
		}

		// Token: 0x060085E8 RID: 34280 RVA: 0x003000B5 File Offset: 0x002FE2B5
		protected override void RunInt()
		{
			this.DoWork(QuestGen.slate);
		}

		// Token: 0x060085E9 RID: 34281 RVA: 0x003000C4 File Offset: 0x002FE2C4
		private bool DoWork(Slate slate)
		{
			float num = 0f;
			if (this.things.GetValue(slate) != null)
			{
				foreach (Thing thing in this.things.GetValue(slate))
				{
					num += thing.MarketValue * (float)thing.stackCount;
				}
			}
			slate.Set<float>(this.storeAs.GetValue(slate), num, false);
			return true;
		}

		// Token: 0x040053B2 RID: 21426
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x040053B3 RID: 21427
		public SlateRef<IEnumerable<Thing>> things;
	}
}
