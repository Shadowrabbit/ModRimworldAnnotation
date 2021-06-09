using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200193E RID: 6462
	public class Alert_Custom : Alert
	{
		// Token: 0x06008F3A RID: 36666 RVA: 0x0005FE5C File Offset: 0x0005E05C
		public override string GetLabel()
		{
			return this.label;
		}

		// Token: 0x06008F3B RID: 36667 RVA: 0x0005FE64 File Offset: 0x0005E064
		public override TaggedString GetExplanation()
		{
			return this.explanation;
		}

		// Token: 0x06008F3C RID: 36668 RVA: 0x0005FE71 File Offset: 0x0005E071
		public override AlertReport GetReport()
		{
			return this.report;
		}

		// Token: 0x04005B56 RID: 23382
		public string label;

		// Token: 0x04005B57 RID: 23383
		public string explanation;

		// Token: 0x04005B58 RID: 23384
		public AlertReport report;
	}
}
