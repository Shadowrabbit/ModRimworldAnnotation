using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200094E RID: 2382
	public class ThoughtWorker_Precept_GroinChestHairOrFaceUncovered : ThoughtWorker_Precept
	{
		// Token: 0x06003CF8 RID: 15608 RVA: 0x00150C0B File Offset: 0x0014EE0B
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			return ThoughtWorker_Precept_GroinChestHairOrFaceUncovered.HasUncoveredGroinChestHairOrFace(p);
		}

		// Token: 0x06003CF9 RID: 15609 RVA: 0x00150C18 File Offset: 0x0014EE18
		public static bool HasUncoveredGroinChestHairOrFace(Pawn p)
		{
			if (p.apparel == null)
			{
				return false;
			}
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
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
					else if (apparel.def.apparel.bodyPartGroups[j] == BodyPartGroupDefOf.UpperHead)
					{
						flag3 = true;
					}
					else if (apparel.def.apparel.bodyPartGroups[j] == BodyPartGroupDefOf.Eyes)
					{
						flag4 = true;
					}
					else if (apparel.def.apparel.bodyPartGroups[j] == BodyPartGroupDefOf.FullHead)
					{
						flag3 = true;
						flag4 = true;
					}
				}
				if (flag && flag2 && flag3 && flag4)
				{
					break;
				}
			}
			return !flag || !flag2 || !flag3 || !flag4;
		}
	}
}
