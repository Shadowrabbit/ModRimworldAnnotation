using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020011E9 RID: 4585
	public class CompTargetEffect_GoodwillImpact : CompTargetEffect
	{
		// Token: 0x1700132C RID: 4908
		// (get) Token: 0x06006E7C RID: 28284 RVA: 0x00250448 File Offset: 0x0024E648
		protected CompProperties_TargetEffect_GoodwillImpact PropsGoodwillImpact
		{
			get
			{
				return (CompProperties_TargetEffect_GoodwillImpact)this.props;
			}
		}

		// Token: 0x06006E7D RID: 28285 RVA: 0x00250458 File Offset: 0x0024E658
		public override void DoEffectOn(Pawn user, Thing target)
		{
			Pawn pawn = target as Pawn;
			Faction faction = (pawn != null) ? pawn.HomeFaction : target.Faction;
			if (user.Faction == Faction.OfPlayer && faction != null && !faction.HostileTo(user.Faction))
			{
				Faction.OfPlayer.TryAffectGoodwillWith(faction, this.PropsGoodwillImpact.goodwillImpact, true, true, HistoryEventDefOf.UsedHarmfulItem, null);
			}
		}
	}
}
