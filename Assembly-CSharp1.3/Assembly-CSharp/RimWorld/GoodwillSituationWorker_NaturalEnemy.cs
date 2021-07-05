using System;

namespace RimWorld
{
	// Token: 0x02000EC6 RID: 3782
	public class GoodwillSituationWorker_NaturalEnemy : GoodwillSituationWorker
	{
		// Token: 0x06005942 RID: 22850 RVA: 0x001E7127 File Offset: 0x001E5327
		public override int GetNaturalGoodwillOffset(Faction other)
		{
			if (!other.def.naturalEnemy)
			{
				return 0;
			}
			return -130;
		}
	}
}
