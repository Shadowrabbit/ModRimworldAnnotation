using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200093B RID: 2363
	public class ThoughtWorker_Precept_HumanLeatherApparel : ThoughtWorker_Precept
	{
		// Token: 0x06003CC4 RID: 15556 RVA: 0x00150272 File Offset: 0x0014E472
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			return ThoughtWorker_HumanLeatherApparel.CurrentThoughtState(p);
		}
	}
}
