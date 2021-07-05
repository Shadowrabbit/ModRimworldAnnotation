using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000944 RID: 2372
	public class ThoughtWorker_Precept_AnyBodyPartButHairOrFaceCovered : ThoughtWorker_Precept
	{
		// Token: 0x06003CDF RID: 15583 RVA: 0x001507D6 File Offset: 0x0014E9D6
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			return ThoughtWorker_Precept_AnyBodyPartButHairOrFaceCovered.HasCoveredBodyPartsButHairOrFace(p);
		}

		// Token: 0x06003CE0 RID: 15584 RVA: 0x001507E4 File Offset: 0x0014E9E4
		public static bool HasCoveredBodyPartsButHairOrFace(Pawn p)
		{
			if (p.apparel == null)
			{
				return false;
			}
			List<Apparel> wornApparel = p.apparel.WornApparel;
			for (int i = 0; i < wornApparel.Count; i++)
			{
				Apparel apparel = wornApparel[i];
				bool flag = false;
				bool flag2 = false;
				bool flag3 = false;
				for (int j = 0; j < apparel.def.apparel.bodyPartGroups.Count; j++)
				{
					if (apparel.def.apparel.bodyPartGroups[j] == BodyPartGroupDefOf.Torso)
					{
						flag = true;
					}
					else if (apparel.def.apparel.bodyPartGroups[j] == BodyPartGroupDefOf.UpperHead)
					{
						flag2 = true;
					}
					else if (apparel.def.apparel.bodyPartGroups[j] == BodyPartGroupDefOf.FullHead)
					{
						flag3 = true;
					}
				}
				if (flag || (!flag2 && !flag3))
				{
					return true;
				}
			}
			return false;
		}
	}
}
