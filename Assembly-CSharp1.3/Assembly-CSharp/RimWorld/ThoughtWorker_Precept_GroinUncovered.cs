using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000948 RID: 2376
	public class ThoughtWorker_Precept_GroinUncovered : ThoughtWorker_Precept
	{
		// Token: 0x06003CE9 RID: 15593 RVA: 0x0015096D File Offset: 0x0014EB6D
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			return ThoughtWorker_Precept_GroinUncovered.HasUncoveredGroin(p);
		}

		// Token: 0x06003CEA RID: 15594 RVA: 0x0015097C File Offset: 0x0014EB7C
		public static bool HasUncoveredGroin(Pawn p)
		{
			if (p.apparel == null)
			{
				return false;
			}
			List<Apparel> wornApparel = p.apparel.WornApparel;
			for (int i = 0; i < wornApparel.Count; i++)
			{
				Apparel apparel = wornApparel[i];
				for (int j = 0; j < apparel.def.apparel.bodyPartGroups.Count; j++)
				{
					if (apparel.def.apparel.bodyPartGroups[j] == BodyPartGroupDefOf.Legs)
					{
						return false;
					}
				}
			}
			return true;
		}
	}
}
