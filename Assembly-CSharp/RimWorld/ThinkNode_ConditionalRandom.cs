using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000E6B RID: 3691
	public class ThinkNode_ConditionalRandom : ThinkNode_Conditional
	{
		// Token: 0x0600530D RID: 21261 RVA: 0x0003A071 File Offset: 0x00038271
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalRandom thinkNode_ConditionalRandom = (ThinkNode_ConditionalRandom)base.DeepCopy(resolve);
			thinkNode_ConditionalRandom.chance = this.chance;
			return thinkNode_ConditionalRandom;
		}

		// Token: 0x0600530E RID: 21262 RVA: 0x0003A08B File Offset: 0x0003828B
		protected override bool Satisfied(Pawn pawn)
		{
			return Rand.Value < this.chance;
		}

		// Token: 0x040034FE RID: 13566
		public float chance = 0.5f;
	}
}
