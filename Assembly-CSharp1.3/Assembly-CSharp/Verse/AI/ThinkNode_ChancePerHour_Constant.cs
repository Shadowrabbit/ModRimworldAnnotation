using System;

namespace Verse.AI
{
	// Token: 0x02000614 RID: 1556
	public class ThinkNode_ChancePerHour_Constant : ThinkNode_ChancePerHour
	{
		// Token: 0x06002D0D RID: 11533 RVA: 0x0010F02F File Offset: 0x0010D22F
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ChancePerHour_Constant thinkNode_ChancePerHour_Constant = (ThinkNode_ChancePerHour_Constant)base.DeepCopy(resolve);
			thinkNode_ChancePerHour_Constant.mtbHours = this.mtbHours;
			thinkNode_ChancePerHour_Constant.mtbDays = this.mtbDays;
			return thinkNode_ChancePerHour_Constant;
		}

		// Token: 0x06002D0E RID: 11534 RVA: 0x0010F055 File Offset: 0x0010D255
		protected override float MtbHours(Pawn Pawn)
		{
			if (this.mtbDays > 0f)
			{
				return this.mtbDays * 24f;
			}
			return this.mtbHours;
		}

		// Token: 0x04001BA9 RID: 7081
		private float mtbHours = -1f;

		// Token: 0x04001BAA RID: 7082
		private float mtbDays = -1f;
	}
}
