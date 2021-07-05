using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020008FB RID: 2299
	public class ThinkNode_ConditionalBodySize : ThinkNode_Conditional
	{
		// Token: 0x06003C27 RID: 15399 RVA: 0x0014F230 File Offset: 0x0014D430
		protected override bool Satisfied(Pawn pawn)
		{
			float bodySize = pawn.BodySize;
			return bodySize >= this.min && bodySize <= this.max;
		}

		// Token: 0x040020AA RID: 8362
		public float min;

		// Token: 0x040020AB RID: 8363
		public float max = 99999f;
	}
}
