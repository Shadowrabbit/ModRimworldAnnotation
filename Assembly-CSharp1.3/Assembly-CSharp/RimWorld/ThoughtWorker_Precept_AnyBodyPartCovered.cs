using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000940 RID: 2368
	public class ThoughtWorker_Precept_AnyBodyPartCovered : ThoughtWorker_Precept
	{
		// Token: 0x06003CD5 RID: 15573 RVA: 0x00150650 File Offset: 0x0014E850
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			return ThoughtWorker_Precept_AnyBodyPartCovered.HasUnnecessarilyCoveredBodyParts(p);
		}

		// Token: 0x06003CD6 RID: 15574 RVA: 0x00150660 File Offset: 0x0014E860
		public static bool HasUnnecessarilyCoveredBodyParts(Pawn p)
		{
			return p.apparel != null && p.apparel.AnyClothing && PawnUtility.HasClothingNotRequiredByKind(p) && GenTemperature.SafeTemperatureRange(p.def, null).Includes(p.AmbientTemperature);
		}
	}
}
