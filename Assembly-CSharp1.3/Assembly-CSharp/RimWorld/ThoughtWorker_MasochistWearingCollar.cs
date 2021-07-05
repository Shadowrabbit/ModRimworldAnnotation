using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020009D6 RID: 2518
	public class ThoughtWorker_MasochistWearingCollar : ThoughtWorker
	{
		// Token: 0x06003E59 RID: 15961 RVA: 0x00154FD4 File Offset: 0x001531D4
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (ModsConfig.IdeologyActive)
			{
				List<Apparel> wornApparel = p.apparel.WornApparel;
				for (int i = 0; i < wornApparel.Count; i++)
				{
					if (wornApparel[i].def == ThingDefOf.Apparel_Collar)
					{
						return true;
					}
				}
			}
			return ThoughtState.Inactive;
		}
	}
}
