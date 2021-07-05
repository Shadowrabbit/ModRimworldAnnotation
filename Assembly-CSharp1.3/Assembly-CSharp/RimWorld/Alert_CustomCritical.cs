using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001247 RID: 4679
	public class Alert_CustomCritical : Alert_Critical
	{
		// Token: 0x06007048 RID: 28744 RVA: 0x00256428 File Offset: 0x00254628
		public override string GetLabel()
		{
			return this.label;
		}

		// Token: 0x06007049 RID: 28745 RVA: 0x00256430 File Offset: 0x00254630
		public override TaggedString GetExplanation()
		{
			return this.explanation;
		}

		// Token: 0x0600704A RID: 28746 RVA: 0x0025643D File Offset: 0x0025463D
		public override AlertReport GetReport()
		{
			return this.report;
		}

		// Token: 0x04003DFC RID: 15868
		public string label;

		// Token: 0x04003DFD RID: 15869
		public string explanation;

		// Token: 0x04003DFE RID: 15870
		public AlertReport report;
	}
}
