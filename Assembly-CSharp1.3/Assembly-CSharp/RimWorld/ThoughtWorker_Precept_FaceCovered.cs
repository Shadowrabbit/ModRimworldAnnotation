using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000946 RID: 2374
	public class ThoughtWorker_Precept_FaceCovered : ThoughtWorker_Precept
	{
		// Token: 0x06003CE4 RID: 15588 RVA: 0x001508D6 File Offset: 0x0014EAD6
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			return ThoughtWorker_Precept_FaceCovered.HasCoveredFace(p);
		}

		// Token: 0x06003CE5 RID: 15589 RVA: 0x001508E4 File Offset: 0x0014EAE4
		public static bool HasCoveredFace(Pawn p)
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
					if (apparel.def.apparel.bodyPartGroups[j] == BodyPartGroupDefOf.UpperHead)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
