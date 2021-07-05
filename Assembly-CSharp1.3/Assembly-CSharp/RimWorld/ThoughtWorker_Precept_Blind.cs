using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000969 RID: 2409
	public class ThoughtWorker_Precept_Blind : ThoughtWorker_Precept
	{
		// Token: 0x06003D46 RID: 15686 RVA: 0x00151B42 File Offset: 0x0014FD42
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			return ThoughtWorker_Precept_Blind.IsBlind(p);
		}

		// Token: 0x06003D47 RID: 15687 RVA: 0x00151B4F File Offset: 0x0014FD4F
		public static bool IsBlind(Pawn p)
		{
			return !p.health.capacities.CapableOf(PawnCapacityDefOf.Sight);
		}
	}
}
