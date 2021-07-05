using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200193F RID: 6463
	public class Alert_CustomCritical : Alert_Critical
	{
		// Token: 0x06008F3E RID: 36670 RVA: 0x0005FE81 File Offset: 0x0005E081
		public override string GetLabel()
		{
			return this.label;
		}

		// Token: 0x06008F3F RID: 36671 RVA: 0x0005FE89 File Offset: 0x0005E089
		public override TaggedString GetExplanation()
		{
			return this.explanation;
		}

		// Token: 0x06008F40 RID: 36672 RVA: 0x0005FE96 File Offset: 0x0005E096
		public override AlertReport GetReport()
		{
			return this.report;
		}

		// Token: 0x04005B59 RID: 23385
		public string label;

		// Token: 0x04005B5A RID: 23386
		public string explanation;

		// Token: 0x04005B5B RID: 23387
		public AlertReport report;
	}
}
