using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000922 RID: 2338
	public class ThinkNode_ConditionalRandom : ThinkNode_Conditional
	{
		// Token: 0x06003C79 RID: 15481 RVA: 0x0014F83D File Offset: 0x0014DA3D
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalRandom thinkNode_ConditionalRandom = (ThinkNode_ConditionalRandom)base.DeepCopy(resolve);
			thinkNode_ConditionalRandom.chance = this.chance;
			return thinkNode_ConditionalRandom;
		}

		// Token: 0x06003C7A RID: 15482 RVA: 0x0014F857 File Offset: 0x0014DA57
		protected override bool Satisfied(Pawn pawn)
		{
			return Rand.Value < this.chance;
		}

		// Token: 0x040020B0 RID: 8368
		public float chance = 0.5f;
	}
}
