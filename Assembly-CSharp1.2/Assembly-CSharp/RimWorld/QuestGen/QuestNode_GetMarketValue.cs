using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F2B RID: 7979
	public class QuestNode_GetMarketValue : QuestNode
	{
		// Token: 0x0600AA91 RID: 43665 RVA: 0x0006FBC6 File Offset: 0x0006DDC6
		protected override bool TestRunInt(Slate slate)
		{
			return this.DoWork(slate);
		}

		// Token: 0x0600AA92 RID: 43666 RVA: 0x0006FBCF File Offset: 0x0006DDCF
		protected override void RunInt()
		{
			this.DoWork(QuestGen.slate);
		}

		// Token: 0x0600AA93 RID: 43667 RVA: 0x0031BE68 File Offset: 0x0031A068
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

		// Token: 0x040073E9 RID: 29673
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x040073EA RID: 29674
		public SlateRef<IEnumerable<Thing>> things;
	}
}
