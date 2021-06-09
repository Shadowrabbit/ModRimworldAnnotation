using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EAF RID: 3759
	public class ThoughtWorker_ApparelDamaged : ThoughtWorker
	{
		// Token: 0x060053AD RID: 21421 RVA: 0x001C1700 File Offset: 0x001BF900
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

		// Token: 0x0400350C RID: 13580
		public const float MinForFrayed = 0.5f;

		// Token: 0x0400350D RID: 13581
		public const float MinForTattered = 0.2f;
	}
}
