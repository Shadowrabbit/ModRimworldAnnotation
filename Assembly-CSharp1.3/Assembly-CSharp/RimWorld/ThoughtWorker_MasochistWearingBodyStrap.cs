using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020009D7 RID: 2519
	public class ThoughtWorker_MasochistWearingBodyStrap : ThoughtWorker
	{
		// Token: 0x06003E5B RID: 15963 RVA: 0x00155024 File Offset: 0x00153224
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (ModsConfig.IdeologyActive)
			{
				List<Apparel> wornApparel = p.apparel.WornApparel;
				for (int i = 0; i < wornApparel.Count; i++)
				{
					if (wornApparel[i].def == ThingDefOf.Apparel_BodyStrap)
					{
						return true;
					}
				}
			}
			return ThoughtState.Inactive;
		}
	}
}
