using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020018C7 RID: 6343
	public class CompTargetEffect_GoodwillImpact : CompTargetEffect
	{
		// Token: 0x1700161A RID: 5658
		// (get) Token: 0x06008CB2 RID: 36018 RVA: 0x0005E4DE File Offset: 0x0005C6DE
		protected CompProperties_TargetEffect_GoodwillImpact PropsGoodwillImpact
		{
			get
			{
				return (CompProperties_TargetEffect_GoodwillImpact)this.props;
			}
		}

		// Token: 0x06008CB3 RID: 36019 RVA: 0x0028D82C File Offset: 0x0028BA2C
		public override void DoEffectOn(Pawn user, Thing target)
		{
			Pawn pawn = target as Pawn;
			Faction faction = (pawn != null) ? pawn.FactionOrExtraMiniOrHomeFaction : target.Faction;
			if (user.Faction != null && faction != null && !faction.HostileTo(user.Faction))
			{
				faction.TryAffectGoodwillWith(user.Faction, this.PropsGoodwillImpact.goodwillImpact, true, true, "GoodwillChangedReason_UsedItem".Translate(this.parent.LabelShort, target.LabelShort, this.parent.Named("ITEM"), target.Named("TARGET")), new GlobalTargetInfo?(target));
			}
		}
	}
}
