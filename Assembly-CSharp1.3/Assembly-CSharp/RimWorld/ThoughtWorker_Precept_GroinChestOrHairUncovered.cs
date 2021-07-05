using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200094C RID: 2380
	public class ThoughtWorker_Precept_GroinChestOrHairUncovered : ThoughtWorker_Precept
	{
		// Token: 0x06003CF3 RID: 15603 RVA: 0x00150ADC File Offset: 0x0014ECDC
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			return ThoughtWorker_Precept_GroinChestOrHairUncovered.HasUncoveredGroinChestOrHair(p);
		}

		// Token: 0x06003CF4 RID: 15604 RVA: 0x00150AEC File Offset: 0x0014ECEC
		public static bool HasUncoveredGroinChestOrHair(Pawn p)
		{
			if (p.apparel == null)
			{
				return false;
			}
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			List<Apparel> wornApparel = p.apparel.WornApparel;
			for (int i = 0; i < wornApparel.Count; i++)
			{
				Apparel apparel = wornApparel[i];
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
					else if (apparel.def.apparel.bodyPartGroups[j] == BodyPartGroupDefOf.UpperHead || apparel.def.apparel.bodyPartGroups[j] == BodyPartGroupDefOf.FullHead)
					{
						flag3 = true;
					}
				}
				if (flag && flag2 && flag3)
				{
					break;
				}
			}
			return !flag || !flag2 || !flag3;
		}
	}
}
