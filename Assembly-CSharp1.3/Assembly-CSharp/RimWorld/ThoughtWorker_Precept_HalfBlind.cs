using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200096A RID: 2410
	public class ThoughtWorker_Precept_HalfBlind : ThoughtWorker_Precept
	{
		// Token: 0x06003D49 RID: 15689 RVA: 0x00151B69 File Offset: 0x0014FD69
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			return ThoughtWorker_Precept_HalfBlind.IsHalfBlind(p);
		}

		// Token: 0x06003D4A RID: 15690 RVA: 0x00151B76 File Offset: 0x0014FD76
		public static bool IsHalfBlind(Pawn p)
		{
			return !ThoughtWorker_Precept_ArtificialBlind.IsArtificiallyBlind(p) && p.health.capacities.CapableOf(PawnCapacityDefOf.Sight) && HealthUtility.IsMissingSightBodyPart(p);
		}
	}
}
