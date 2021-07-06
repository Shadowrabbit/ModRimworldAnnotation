using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000E62 RID: 3682
	public class ThinkNode_ConditionalNeedPercentageAbove : ThinkNode_Conditional
	{
		// Token: 0x060052F9 RID: 21241 RVA: 0x00039F1B File Offset: 0x0003811B
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalNeedPercentageAbove thinkNode_ConditionalNeedPercentageAbove = (ThinkNode_ConditionalNeedPercentageAbove)base.DeepCopy(resolve);
			thinkNode_ConditionalNeedPercentageAbove.need = this.need;
			thinkNode_ConditionalNeedPercentageAbove.threshold = this.threshold;
			return thinkNode_ConditionalNeedPercentageAbove;
		}

		// Token: 0x060052FA RID: 21242 RVA: 0x00039F41 File Offset: 0x00038141
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.needs.TryGetNeed(this.need).CurLevelPercentage > this.threshold;
		}

		// Token: 0x040034FB RID: 13563
		private NeedDef need;

		// Token: 0x040034FC RID: 13564
		private float threshold;
	}
}
