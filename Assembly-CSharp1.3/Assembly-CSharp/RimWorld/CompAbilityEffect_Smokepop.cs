using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D5F RID: 3423
	public class CompAbilityEffect_Smokepop : CompAbilityEffect
	{
		// Token: 0x17000DC5 RID: 3525
		// (get) Token: 0x06004F9D RID: 20381 RVA: 0x001AA8C8 File Offset: 0x001A8AC8
		public new CompProperties_AbilitySmokepop Props
		{
			get
			{
				return (CompProperties_AbilitySmokepop)this.props;
			}
		}

		// Token: 0x06004F9E RID: 20382 RVA: 0x001AA8D8 File Offset: 0x001A8AD8
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			GenExplosion.DoExplosion(target.Cell, this.parent.pawn.MapHeld, this.Props.smokeRadius, DamageDefOf.Smoke, null, -1, -1f, null, null, null, null, ThingDefOf.Gas_Smoke, 1f, 1, false, null, 0f, 1, 0f, false, null, null);
		}

		// Token: 0x06004F9F RID: 20383 RVA: 0x001AA947 File Offset: 0x001A8B47
		public override void DrawEffectPreview(LocalTargetInfo target)
		{
			GenDraw.DrawRadiusRing(target.Cell, this.Props.smokeRadius);
		}
	}
}
