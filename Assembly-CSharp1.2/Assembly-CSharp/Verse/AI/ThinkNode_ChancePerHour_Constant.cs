using System;

namespace Verse.AI
{
	// Token: 0x02000A74 RID: 2676
	public class ThinkNode_ChancePerHour_Constant : ThinkNode_ChancePerHour
	{
		// Token: 0x06003FF0 RID: 16368 RVA: 0x0002FE35 File Offset: 0x0002E035
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ChancePerHour_Constant thinkNode_ChancePerHour_Constant = (ThinkNode_ChancePerHour_Constant)base.DeepCopy(resolve);
			thinkNode_ChancePerHour_Constant.mtbHours = this.mtbHours;
			thinkNode_ChancePerHour_Constant.mtbDays = this.mtbDays;
			return thinkNode_ChancePerHour_Constant;
		}

		// Token: 0x06003FF1 RID: 16369 RVA: 0x0002FE5B File Offset: 0x0002E05B
		protected override float MtbHours(Pawn Pawn)
		{
			if (this.mtbDays > 0f)
			{
				return this.mtbDays * 24f;
			}
			return this.mtbHours;
		}

		// Token: 0x04002C20 RID: 11296
		private float mtbHours = -1f;

		// Token: 0x04002C21 RID: 11297
		private float mtbDays = -1f;
	}
}
