using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200138A RID: 5002
	public class CompAbilityEffect_Smokepop : CompAbilityEffect
	{
		// Token: 0x170010C5 RID: 4293
		// (get) Token: 0x06006C98 RID: 27800 RVA: 0x00049DB9 File Offset: 0x00047FB9
		public new CompProperties_AbilitySmokepop Props
		{
			get
			{
				return (CompProperties_AbilitySmokepop)this.props;
			}
		}

		// Token: 0x06006C99 RID: 27801 RVA: 0x00215BC0 File Offset: 0x00213DC0
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			GenExplosion.DoExplosion(target.Cell, this.parent.pawn.MapHeld, this.Props.smokeRadius, DamageDefOf.Smoke, null, -1, -1f, null, null, null, null, ThingDefOf.Gas_Smoke, 1f, 1, false, null, 0f, 1, 0f, false, null, null);
		}

		// Token: 0x06006C9A RID: 27802 RVA: 0x00049DC6 File Offset: 0x00047FC6
		public override void DrawEffectPreview(LocalTargetInfo target)
		{
			GenDraw.DrawRadiusRing(target.Cell, this.Props.smokeRadius);
		}
	}
}
