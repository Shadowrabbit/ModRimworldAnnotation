using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020009A5 RID: 2469
	public class ThoughtWorker_ApparelDamaged : ThoughtWorker
	{
		// Token: 0x06003DD6 RID: 15830 RVA: 0x001537E4 File Offset: 0x001519E4
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			float num = 999f;
			List<Apparel> wornApparel = p.apparel.WornApparel;
			for (int i = 0; i < wornApparel.Count; i++)
			{
				if (wornApparel[i].def.useHitPoints && !p.apparel.IsLocked(wornApparel[i]) && wornApparel[i].def.apparel.careIfDamaged)
				{
					float num2 = (float)wornApparel[i].HitPoints / (float)wornApparel[i].MaxHitPoints;
					if (num2 < num)
					{
						num = num2;
					}
					if (num < 0.2f)
					{
						return ThoughtState.ActiveAtStage(1);
					}
				}
			}
			if (num < 0.5f)
			{
				return ThoughtState.ActiveAtStage(0);
			}
			return ThoughtState.Inactive;
		}

		// Token: 0x040020E3 RID: 8419
		public const float MinForFrayed = 0.5f;

		// Token: 0x040020E4 RID: 8420
		public const float MinForTattered = 0.2f;
	}
}
