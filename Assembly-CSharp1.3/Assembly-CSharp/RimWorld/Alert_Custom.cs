using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001246 RID: 4678
	public class Alert_Custom : Alert
	{
		// Token: 0x06007044 RID: 28740 RVA: 0x00256403 File Offset: 0x00254603
		public override string GetLabel()
		{
			return this.label;
		}

		// Token: 0x06007045 RID: 28741 RVA: 0x0025640B File Offset: 0x0025460B
		public override TaggedString GetExplanation()
		{
			return this.explanation;
		}

		// Token: 0x06007046 RID: 28742 RVA: 0x00256418 File Offset: 0x00254618
		public override AlertReport GetReport()
		{
			return this.report;
		}

		// Token: 0x04003DF9 RID: 15865
		public string label;

		// Token: 0x04003DFA RID: 15866
		public string explanation;

		// Token: 0x04003DFB RID: 15867
		public AlertReport report;
	}
}
