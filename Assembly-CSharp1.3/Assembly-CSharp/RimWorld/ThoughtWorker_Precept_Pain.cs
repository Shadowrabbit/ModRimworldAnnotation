using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000939 RID: 2361
	public class ThoughtWorker_Precept_Pain : ThoughtWorker_Precept
	{
		// Token: 0x06003CBF RID: 15551 RVA: 0x001501C3 File Offset: 0x0014E3C3
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			return ThoughtWorker_Pain.CurrentThoughtState(p);
		}
	}
}
