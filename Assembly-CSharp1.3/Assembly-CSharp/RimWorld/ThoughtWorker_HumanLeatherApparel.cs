using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020009A4 RID: 2468
	public class ThoughtWorker_HumanLeatherApparel : ThoughtWorker
	{
		// Token: 0x06003DD3 RID: 15827 RVA: 0x00153760 File Offset: 0x00151960
		public static ThoughtState CurrentThoughtState(Pawn p)
		{
			string text = null;
			int num = 0;
			List<Apparel> wornApparel = p.apparel.WornApparel;
			for (int i = 0; i < wornApparel.Count; i++)
			{
				if (wornApparel[i].Stuff == ThingDefOf.Human.race.leatherDef)
				{
					if (text == null)
					{
						text = wornApparel[i].def.label;
					}
					num++;
				}
			}
			if (num == 0)
			{
				return ThoughtState.Inactive;
			}
			if (num >= 5)
			{
				return ThoughtState.ActiveAtStage(4, text);
			}
			return ThoughtState.ActiveAtStage(num - 1, text);
		}

		// Token: 0x06003DD4 RID: 15828 RVA: 0x00150272 File Offset: 0x0014E472
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return ThoughtWorker_HumanLeatherApparel.CurrentThoughtState(p);
		}
	}
}
