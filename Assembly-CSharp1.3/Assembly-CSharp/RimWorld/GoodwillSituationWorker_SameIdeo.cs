using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EC8 RID: 3784
	public class GoodwillSituationWorker_SameIdeo : GoodwillSituationWorker
	{
		// Token: 0x06005947 RID: 22855 RVA: 0x001E7202 File Offset: 0x001E5402
		public override string GetPostProcessedLabel(Faction other)
		{
			return this.def.label.Formatted(Faction.OfPlayer.ideos.PrimaryIdeo);
		}

		// Token: 0x06005948 RID: 22856 RVA: 0x001E7230 File Offset: 0x001E5430
		public override int GetNaturalGoodwillOffset(Faction other)
		{
			Ideo primaryIdeo = Faction.OfPlayer.ideos.PrimaryIdeo;
			if (primaryIdeo == null || primaryIdeo != other.ideos.PrimaryIdeo)
			{
				return 0;
			}
			return this.def.naturalGoodwillOffset;
		}
	}
}
