using System;

namespace Verse.Grammar
{
	// Token: 0x0200053C RID: 1340
	public class Rule_Number : Rule
	{
		// Token: 0x06002861 RID: 10337 RVA: 0x000F6700 File Offset: 0x000F4900
		public override Rule DeepCopy()
		{
			Rule_Number rule_Number = (Rule_Number)base.DeepCopy();
			rule_Number.range = this.range;
			rule_Number.selectionWeight = this.selectionWeight;
			return rule_Number;
		}

		// Token: 0x170007ED RID: 2029
		// (get) Token: 0x06002862 RID: 10338 RVA: 0x000F6725 File Offset: 0x000F4925
		public override float BaseSelectionWeight
		{
			get
			{
				return (float)this.selectionWeight;
			}
		}

		// Token: 0x06002863 RID: 10339 RVA: 0x000F6730 File Offset: 0x000F4930
		public override string Generate()
		{
			return this.range.RandomInRange.ToString();
		}

		// Token: 0x06002864 RID: 10340 RVA: 0x000F6750 File Offset: 0x000F4950
		public override string ToString()
		{
			return this.keyword + "->(number: " + this.range.ToString() + ")";
		}

		// Token: 0x040018EB RID: 6379
		private IntRange range = IntRange.zero;

		// Token: 0x040018EC RID: 6380
		public int selectionWeight = 1;
	}
}
