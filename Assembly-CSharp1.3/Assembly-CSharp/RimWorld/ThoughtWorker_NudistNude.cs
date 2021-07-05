using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020009AB RID: 2475
	public class ThoughtWorker_NudistNude : ThoughtWorker
	{
		// Token: 0x06003DE4 RID: 15844 RVA: 0x00153AD0 File Offset: 0x00151CD0
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			List<Apparel> wornApparel = p.apparel.WornApparel;
			for (int i = 0; i < wornApparel.Count; i++)
			{
				Apparel apparel = wornApparel[i];
				for (int j = 0; j < apparel.def.apparel.bodyPartGroups.Count; j++)
				{
					if (apparel.def.apparel.bodyPartGroups[j] == BodyPartGroupDefOf.Torso)
					{
						return false;
					}
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
