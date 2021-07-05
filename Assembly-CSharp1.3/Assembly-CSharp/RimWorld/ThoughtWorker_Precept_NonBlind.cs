using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200096C RID: 2412
	public class ThoughtWorker_Precept_NonBlind : ThoughtWorker_Precept
	{
		// Token: 0x06003D4F RID: 15695 RVA: 0x00151C2C File Offset: 0x0014FE2C
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			return ThoughtWorker_Precept_NonBlind.IsNotBlind(p, true);
		}

		// Token: 0x06003D50 RID: 15696 RVA: 0x00151C3C File Offset: 0x0014FE3C
		public static bool IsNotBlind(Pawn p, bool checkApparel = true)
		{
			if (checkApparel && p.apparel != null)
			{
				using (List<Apparel>.Enumerator enumerator = p.apparel.WornApparel.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.def.apparel.blocksVision)
						{
							return false;
						}
					}
				}
			}
			return p.health.capacities.CapableOf(PawnCapacityDefOf.Sight) && !HealthUtility.IsMissingSightBodyPart(p);
		}
	}
}
