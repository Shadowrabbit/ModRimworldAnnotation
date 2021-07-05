using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000E44 RID: 3652
	public class ThinkNode_ConditionalBodySize : ThinkNode_Conditional
	{
		// Token: 0x060052BB RID: 21179 RVA: 0x001BFB48 File Offset: 0x001BDD48
		protected override bool Satisfied(Pawn pawn)
		{
			float bodySize = pawn.BodySize;
			return bodySize >= this.min && bodySize <= this.max;
		}

		// Token: 0x040034F8 RID: 13560
		public float min;

		// Token: 0x040034F9 RID: 13561
		public float max = 99999f;
	}
}
