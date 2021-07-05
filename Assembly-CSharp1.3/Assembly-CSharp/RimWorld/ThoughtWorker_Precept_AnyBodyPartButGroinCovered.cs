using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000942 RID: 2370
	public class ThoughtWorker_Precept_AnyBodyPartButGroinCovered : ThoughtWorker_Precept
	{
		// Token: 0x06003CDA RID: 15578 RVA: 0x001506B4 File Offset: 0x0014E8B4
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			return ThoughtWorker_Precept_AnyBodyPartButGroinCovered.HasCoveredBodyPartsButGroin(p) && GenTemperature.SafeTemperatureRange(p.def, null).Includes(p.AmbientTemperature);
		}

		// Token: 0x06003CDB RID: 15579 RVA: 0x001506EC File Offset: 0x0014E8EC
		public static bool HasCoveredBodyPartsButGroin(Pawn p)
		{
			if (p.apparel == null)
			{
				return false;
			}
			List<Apparel> wornApparel = p.apparel.WornApparel;
			for (int i = 0; i < wornApparel.Count; i++)
			{
				Apparel apparel = wornApparel[i];
				if (p.kindDef.apparelRequired == null || !p.kindDef.apparelRequired.Contains(apparel.def))
				{
					bool flag = false;
					bool flag2 = false;
					for (int j = 0; j < apparel.def.apparel.bodyPartGroups.Count; j++)
					{
						if (apparel.def.apparel.bodyPartGroups[j] == BodyPartGroupDefOf.Torso)
						{
							flag = true;
						}
						else if (apparel.def.apparel.bodyPartGroups[j] == BodyPartGroupDefOf.Legs)
						{
							flag2 = true;
						}
					}
					if (flag || !flag2)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
