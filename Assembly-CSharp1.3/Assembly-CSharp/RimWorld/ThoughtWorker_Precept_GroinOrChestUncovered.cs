using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200094A RID: 2378
	public class ThoughtWorker_Precept_GroinOrChestUncovered : ThoughtWorker_Precept
	{
		// Token: 0x06003CEE RID: 15598 RVA: 0x00150A05 File Offset: 0x0014EC05
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			return ThoughtWorker_Precept_GroinOrChestUncovered.HasUncoveredGroinOrChest(p);
		}

		// Token: 0x06003CEF RID: 15599 RVA: 0x00150A14 File Offset: 0x0014EC14
		public static bool HasUncoveredGroinOrChest(Pawn p)
		{
			if (p.apparel == null)
			{
				return false;
			}
			bool flag = false;
			bool flag2 = false;
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
				}
				if (flag && flag2)
				{
					break;
				}
			}
			return !flag || !flag2;
		}
	}
}
