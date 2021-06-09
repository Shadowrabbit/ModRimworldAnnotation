using System;

namespace Verse.Grammar
{
	// Token: 0x0200090B RID: 2315
	public class Rule_Number : Rule
	{
		// Token: 0x0600397E RID: 14718 RVA: 0x0002C608 File Offset: 0x0002A808
		public override Rule DeepCopy()
		{
			Rule_Number rule_Number = (Rule_Number)base.DeepCopy();
			rule_Number.range = this.range;
			rule_Number.selectionWeight = this.selectionWeight;
			return rule_Number;
		}

		// Token: 0x17000923 RID: 2339
		// (get) Token: 0x0600397F RID: 14719 RVA: 0x0002C62D File Offset: 0x0002A82D
		public override float BaseSelectionWeight
		{
			get
			{
				return (float)this.selectionWeight;
			}
		}

		// Token: 0x06003980 RID: 14720 RVA: 0x00167A30 File Offset: 0x00165C30
		public override string Generate()
		{
			return this.range.RandomInRange.ToString();
		}

		// Token: 0x06003981 RID: 14721 RVA: 0x0002C636 File Offset: 0x0002A836
		public override string ToString()
		{
			return this.keyword + "->(number: " + this.range.ToString() + ")";
		}

		// Token: 0x040027D2 RID: 10194
		private IntRange range = IntRange.zero;

		// Token: 0x040027D3 RID: 10195
		public int selectionWeight = 1;
	}
}
